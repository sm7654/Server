using System;
using System.Collections.Generic;
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
        private string sessionName;
        private string Code;
        private uint EnterTimeGlobal = 0;
        private uint EnterTime = 0;
        private string EnterDate = "";

        private Socket Controller;
        private string ControllerKnickname;
        private string ControllerPublicKey;

        private Socket ClientConn = null;
        private Socket UdpClientConn = null;
        private string ClientKnickname;
        private string Client_endpoint;
        private bool IsClientConnected = false;

        private object ClientSendLock = new object();
        private object ControllerSendLock = new object();

        private uint BytesFromMicroGlobal = 0;
        private uint BytesFromMicro = 0;
        private uint BytesFromClient = 0;


        private sessionLayot SessionUI = null;


        private List<SessionRecord> SessionsRecoeds = new List<SessionRecord>();

        private SessionRecord SRD = null;
        
        public session(Socket Controller, string Code, string publicKey, string sessionname)
        {

            this.Controller = Controller;
            this.Code = Code;
            this.ControllerPublicKey = publicKey;
            this.sessionName = sessionname;

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
                        AddToBytesToMicro((uint)byterec, (uint)bufferSize);
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
                        AddToBytesToCLient((uint)byterec, (uint)bufferSize);
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
            this.Client_endpoint = Client.RemoteEndPoint.ToString();
            EnterTime = ServerServices.GetTime();
            EnterDate = DateTime.Now.ToString();


            SRD = new SessionRecord(Code, Client_endpoint, ClientKnickname, BytesFromClient,  BytesFromMicro,  EnterTime, EnterDate);
            SessionsRecoeds.Add(SRD);

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

            AddToBytesToCLient((uint)AESIVSERVER.Length, (uint)AESKEYSERVER.Length);
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


                AddToBytesToCLient((uint)Encoding.UTF8.GetBytes(data.Length.ToString()).Length, (uint)data.Length);



            }
        }
        public void SendToClientFromServer(byte[] data)
        {
            SendToClient(ServerServices.GetServerRole().Concat(data).ToArray());
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

                AddToBytesToMicro((uint)Encoding.UTF8.GetBytes(data.ToString()).Length, (uint)data.Length);

            }
        }
        public void SendToMicroFromServer(byte[] data)
        {
            SendToMicro(ServerServices.GetServerRole().Concat(data).ToArray());
        }

        private void AddToBytesToCLient(uint bytes)
        {
            lock ((object)BytesFromClient)
            {
                BytesFromClient += bytes;
                
                if (SRD != null)
                {
                    SRD.SetBytesToClient(BytesFromClient);
                }

            }
        }
        private void AddToBytesToCLient(uint First, uint Last)
        {
            lock ((object)BytesFromClient)
            {
                BytesFromClient += First + Last;
                if (SRD != null)
                {
                    SRD.SetBytesToClient(BytesFromClient);
                }

            }
        }
        private void AddToBytesToMicro(uint bytes)
        {
            lock ((object)BytesFromMicro)
            {
                BytesFromMicro += bytes;
                BytesFromMicroGlobal += bytes;
                if (SRD != null)
                {
                    SRD.SetBytesToMicro(BytesFromMicro);
                }

            }
        }
        private void AddToBytesToMicro(uint First, uint Last)
        {
            lock ((object)BytesFromMicro)
            {
                BytesFromMicro += First + Last;
                BytesFromMicroGlobal += First + Last;

                
                if (SRD != null)
                {
                    SRD.SetBytesToMicro(BytesFromMicro);
                }
                
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


        public void disconnectClient(string newcode)
        {
            SRD.MakeNotLiveRecored();
            SRD = null;
            
            EnterTime = 0;
            EnterDate = "";
            BytesFromClient = 0;
            BytesFromMicro = 0;
            ClientConn = null;
            UdpClientConn = null;
            ClientKnickname = null;
            Client_endpoint = "";
            this.Code = newcode;
            byte[] bytes =
                RsaEncryption.EncryptBytes(
                    ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes($"&302&{this.Code}")).ToArray()
                    , ControllerPublicKey
                );

            SendToMicro(bytes);







        }

    




        public bool disconnect()
        {


            try
            {
                FormController.RemoveSession(this);
                SRD.MakeNotLiveRecored();
                SRD = null;
                SessionsRecoeds = null;
                BytesFromMicroGlobal = 0;
                BytesFromMicro = 0;
                BytesFromClient = 0;
                EnterDate = null;
                EnterTimeGlobal = 0;
                EnterTime = 0;

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

                Client_endpoint = null;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public uint GetEnterTime()
        {
            return EnterTime;
        }
        


        public string GetSessionName()
        {
            return sessionName;
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
        
        
        public (string, string) GetControllerEndPoint()
        {
            return (((IPEndPoint)this.Controller.RemoteEndPoint).Address.ToString(), ((IPEndPoint)this.Controller.RemoteEndPoint).Port.ToString());
        }
        
        public List<SessionRecord> GetSessionsRecords()
        {
            return this.SessionsRecoeds;
        }
        public uint GetTotalMicroBytes()
        {
            return BytesFromMicroGlobal;
        }
    }
}
