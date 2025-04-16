using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;




namespace ServerSide
{


    public class session
    {
        private string Code;

        private Socket Controller;
        private string ControllerKnickname;
        private string ControllerPublicKey;

        private Socket ClientConn = null;
        private Socket UdpClientConn = null;
        private string ClientKnickname;
        private EndPoint ClientUDP_endpoint;
        private bool IsClientConnected = false;

        private object ClientSendLock = new object();
        private object ControllerSendLock = new object();

        private int BytesFromMicro = 0;
        private int BytesFromClient = 0;

        private sessionLayot SessionUI = null;


        public session(Socket Controller, string Code, string publicKey)
        {

            this.Controller = Controller;
            this.Code = Code;
            this.ControllerPublicKey = publicKey;


            byte[] EncryptedCode = RsaEncryption.Encrypt($"{Code}", ControllerPublicKey);

            Controller.Send(EncryptedCode);

            new Thread(() => MicroStream()).Start();



        }

        private void MicroStream()
        {
            try
            {
                while (Controller.Connected)
                {
                    try
                    {
                        Controller.ReceiveTimeout = 0;
                        byte[] buffer = new byte[1024];
                        int byterec = Controller.Receive(buffer);
                        int bufferSize = int.Parse(Encoding.UTF8.GetString(buffer, 0, byterec));
                        buffer = new byte[bufferSize];
                        Controller.Receive(buffer);
                        AddToBytesToMicro(byterec, bufferSize);
                        if (ServerServices.IsServerMassageFromMicro(buffer))
                        {
                            ServerServices.HandleServerMessages(buffer, this, false);
                        }
                        if (ClientConn != null)
                        {
                            /*
                            ClientConn.Send(Encoding.UTF8.GetBytes(bufferSize.ToString()));
                            Thread.Sleep(200);
                            ClientConn.Send(buffer);*/
                            SendToClient(buffer);

                        }
                    }
                    catch (Exception e)
                    { MessageBox.Show(e.Message + "1"); }
                }
            }
            catch
            (Exception e) { MessageBox.Show(e.Message + "1"); }

        }
        private void ClientStream()
        {
            try
            {

                while (Controller.Connected && ClientConn.Connected)
                {
                    try
                    {

                        byte[] buffer = new byte[1024];
                        int byterec = ClientConn.Receive(buffer);
                        int bufferSize = int.Parse(Encoding.UTF8.GetString(buffer, 0, byterec));
                        buffer = new byte[bufferSize];
                        ClientConn.Receive(buffer);
                        AddToBytesToCLient(byterec, bufferSize);
                        if (ServerServices.IsServerMassageFromClient(buffer))
                        {
                            ServerServices.HandleServerMessages(buffer, this, true);
                        }
                        else
                        {

                            SendToMicro(buffer);
                            /*Controller.Send(Encoding.UTF8.GetBytes(bufferSize.ToString()));
                            Thread.Sleep(250);
                            Controller.Send(buffer);*/
                        }
                    }
                    catch (Exception e) { MessageBox.Show(e.Message + "4"); }
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message + "5" +
                ""); }



        }




        public bool AddClient(Socket Client, string name, string publickeyClient)
        {
            if (Client == null)
                return false;
            this.ClientConn = Client;
            this.ClientKnickname = name;

            (byte[] AESKEYSERVER, byte[] AESIVSERVER) = AesEncryption.GetAesEncryptedKeys(publickeyClient);
            Client.Send(AESKEYSERVER);
            Thread.Sleep(100);
            Client.Send(AESIVSERVER);



            byte[] AESIv = new byte[128];
            byte[] AESkey = new byte[128];
            int bytesread = 0;

            // send to conrolller client connected
            byte[] hh = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes($"&200&{this.ClientConn.RemoteEndPoint.ToString()}")).ToArray();
            byte[] ConnectedMassege = RsaEncryption.Encrypt(Encoding.UTF8.GetString(hh), this.ControllerPublicKey);

            SendToMicro(ConnectedMassege);
            /*Controller.Send(Encoding.UTF8.GetBytes(ConnectedMassege.Length.ToString()));
            Thread.Sleep(200);
            Controller.Send(ConnectedMassege);
*/



            // send clien the controller's public key
            /*Client.Send(Encoding.UTF8.GetBytes( Encoding.UTF8.GetBytes(ControllerPublicKey).Length.ToString() ));
            Thread.Sleep(200);
            Client.Send();
            */
            SendToClient(Encoding.UTF8.GetBytes(ControllerPublicKey));
            //SendToClient(Encoding.UTF8.GetBytes(ControllerPublicKey));



            // recive from to client the AES keys and send it to controller

            AESIv = new byte[128];
            bytesread = Client.Receive(AESkey);

            AESIv = new byte[128];
            bytesread = Client.Receive(AESIv);



            Controller.Send(AESkey);
            Thread.Sleep(200);
            Controller.Send(AESIv);



            IsClientConnected = true;

            new Thread(() => ClientStream()).Start();

            AddToBytesToCLient(AESIVSERVER.Length, AESKEYSERVER.Length);
            AddToBytesToCLient(128 * 2);

            AddToBytesToMicro(128 * 2);
            return true;
        }



        public void SendToClient(byte[] data)
        {
            if (ClientConn == null)
                return;
            lock (ClientSendLock) {
                ClientConn.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));
                Thread.Sleep(200);
                ClientConn.Send(data);


                AddToBytesToCLient(Encoding.UTF8.GetBytes(data.Length.ToString()).Length, data.Length);



            }
        }
        public void SendToMicro(byte[] data)
        {
            if (Controller == null)
                return;
            lock (ControllerSendLock)
            {
                Controller.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));
                Thread.Sleep(250);
                Controller.Send(data);

                AddToBytesToMicro(Encoding.UTF8.GetBytes(data.ToString()).Length, data.Length);

            }
        }

        private void AddToBytesToCLient(int bytes)
        {
            lock ((object)BytesFromClient)
            {
                BytesFromClient += bytes;

                UpdateCLientBytes(BytesFromClient);
                if (SessionUI == null)
                    return;
                SessionUI.BeginInvoke(new Action(() => {
                    SessionUI.AddToBytesClient(BytesFromClient);

                }));
            }
        }
        private void AddToBytesToCLient(int First, int Last)
        {
            lock ((object)BytesFromClient)
            {
                BytesFromClient += First + Last;
                UpdateCLientBytes(BytesFromClient);
                if (SessionUI == null)
                    return;
                SessionUI.BeginInvoke(new Action(() => {
                    SessionUI.AddToBytesClient(BytesFromClient);

                }));
            }
        }
        private void AddToBytesToMicro(int bytes)
        {
            lock ((object)BytesFromMicro)
            {
                BytesFromMicro += bytes;

                UpdateMicrotBytes(BytesFromMicro);
                if (SessionUI == null)
                    return;
                SessionUI.BeginInvoke(new Action(() => {
                    SessionUI.AddToBytesMicro(BytesFromMicro);

                }));
            }
        }
        private void AddToBytesToMicro(int First, int Last)
        {
            lock ((object)BytesFromMicro)
            {
                BytesFromMicro += First + Last;

                UpdateMicrotBytes(BytesFromMicro);
                if (SessionUI == null)
                    return;
                SessionUI.BeginInvoke(new Action(() => {
                    SessionUI.AddToBytesMicro(BytesFromMicro);

                }));
            }
        }



        public (string, string) GetCLientEndPoint()
        {
            if (ClientConn == null)
                return ("Not Connected", "Not Connected");
            return (((IPEndPoint)this.ClientConn.RemoteEndPoint).Address.ToString(), ((IPEndPoint)this.ClientConn.RemoteEndPoint).Port.ToString());
        }
        public string GetClienKnickname()
        {
            if (this.ClientKnickname == null)
                return "Oops...";
            return this.ClientKnickname;
        }


        public void disconnectClient()
        {
            ClientConn = null;
            UdpClientConn = null;
            ClientKnickname = null;
            ClientUDP_endpoint = null;
            byte[] bytes =
                RsaEncryption.EncryptBytes(
                    ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes("&302")).ToArray()
                    , ControllerPublicKey
                );

            SendToMicro(bytes);

            /*Controller.Send(Encoding.UTF8.GetBytes(bytes.Length.ToString()));
            Thread.Sleep(200);
            Controller.Send(bytes);
*/

        }

        /*private void ReadDataFromMicroConntroller()
        {
            //ignore*****************
            while (true)
            {
                try
                {
                    byte[] BufferSizeRecive = new byte[1024];
                    int massageLength = ClientConn.Receive(BufferSizeRecive);

                    massageLength = int.Parse(Encoding.UTF8.GetString(BufferSizeRecive, 0, massageLength));
                    byte[] Buffer = new byte[massageLength];

                    ClientConn.Receive(Buffer);
                    string MSG = Encoding.UTF8.GetString(Buffer);

                    // send message to controller
                }
                catch (FormatException e) { }
            }
        }
*/
        public bool disconnect()
        {


            try
            {
                FormController.RemoveSession(this);

                if (UdpClientConn != null)
                    UdpClientConn.Close();
                byte[] message = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes("&Shut")).ToArray();
                byte[] EncryptedBytes = RsaEncryption.EncryptBytes(message, ControllerPublicKey);
                if (Controller != null)
                {
                    SendToMicro(EncryptedBytes);
                    /*Controller.Send(Encoding.UTF8.GetBytes(EncryptedBytes.Length.ToString()));
                    Thread.Sleep(200);
                    Controller.Send(EncryptedBytes);*/
                    Controller.Close();
                }
                ControllerPublicKey = null;

                if (!IsClientConnected)
                    return true;


                if (ClientConn != null)
                {
                    message = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes("&303")).ToArray();
                    EncryptedBytes = AesEncryption.EncryptedData(message);
                    SendToClient(EncryptedBytes);
                }
                ClientConn = null;
                Controller = null;
                UdpClientConn = null;


                Code = null;

                ClientUDP_endpoint = null;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void UpdateCLientBytes(int number)
        {
            
        }

        private void UpdateMicrotBytes(int number)
        {
            
        }



        public string GetCode()
        {
            return this.Code;
        }
        public void SetSessionUi(sessionLayot SL)
        {
            this.SessionUI = SL;
        }
        public void SetControllerKnickname(string knickname)
        {
            this.ControllerKnickname = knickname;
        }
        public string GetControllerKnickname()
        {
            return this.ControllerKnickname;
        }

        public void SetControllerKey(string publicKey)
        {
            this.ControllerPublicKey = publicKey;
        }
        public void SetClientUdpEndPoint(EndPoint En)
        {
            this.ClientUDP_endpoint = En;
        }
        public string GetClientUdpEndPoint()
        {
            return this.ClientUDP_endpoint.ToString();
        }
        public (string, string) GetControllerEndPoint()
        {
            return (((IPEndPoint)this.Controller.RemoteEndPoint).Address.ToString(), ((IPEndPoint)this.Controller.RemoteEndPoint).Port.ToString());
        }
        

    }
}
