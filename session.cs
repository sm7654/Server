using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

 
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
                        Controller.ReceiveTimeout = 2000;
                        Controller.Receive(buffer);

                        if (ServerServices.IsServerMassageRSA(buffer))
                        {
                            ServerServices.HandleServerMessages(buffer, this, false);

                        }
                        if (ClientConn != null)
                        {
                            
                            ClientConn.Send(Encoding.UTF8.GetBytes(bufferSize.ToString()));
                            Thread.Sleep(200);
                            ClientConn.Send(buffer);
                        }
                    }
                    catch (Exception e) { }
                }
            } catch (Exception e) { }
            
        }
        private void ClientStream()
        {
            try
            {

                while (Controller.Connected && ClientConn.Connected)
                {
                    try
                    {

                        ClientConn.ReceiveTimeout = 0;
                        byte[] buffer = new byte[1024];
                        int byterec = ClientConn.Receive(buffer);
                        int bufferSize = int.Parse(Encoding.UTF8.GetString(buffer, 0, byterec));
                        buffer = new byte[bufferSize];
                        ClientConn.ReceiveTimeout = 2000;
                        ClientConn.Receive(buffer);

                        if (ServerServices.IsServerMassageAES(buffer))
                        {
                            ServerServices.HandleServerMessages(buffer, this, true);
                        }
                        else
                        {

                            Controller.Send(Encoding.UTF8.GetBytes(bufferSize.ToString()));
                            Thread.Sleep(300);
                            Controller.Send(buffer);
                        }
                    }
                    catch (Exception e) { }
                }
            }
            catch (Exception e) { }
            
            

        }




        public bool AddClient(Socket Client, string name)
        {
            if (Client == null)
                return false;
            this.ClientConn = Client;
            this.ClientKnickname = name;


            //////////////////////////////////////////////////////
            byte[] AESkey = new byte[128];
            int bytesread = Client.Receive(AESkey);
            byte[] AESIv = new byte[128];
            bytesread = Client.Receive(AESIv);
            AesEncryption.Addkeys(AESkey, AESIv);
            //////////////////////////////////////////////////////


            // send to conrolller client connected
            byte[] ConnectedMassege = RsaEncryption.Encrypt($"200;{this.ClientConn.RemoteEndPoint.ToString()}", this.ControllerPublicKey);
            Controller.Send(Encoding.UTF8.GetBytes(ConnectedMassege.Length.ToString()));
            Thread.Sleep(200);
            Controller.Send(ConnectedMassege);


            
           
            // send clien the controller's public key
            Client.Send(Encoding.UTF8.GetBytes( Encoding.UTF8.GetBytes(ControllerPublicKey).Length.ToString() ));
            Thread.Sleep(200);
            Client.Send(Encoding.UTF8.GetBytes(ControllerPublicKey));


            // recive from to client the AES keys and send it to controller

            AESIv = new byte[128];
            bytesread = Client.Receive(AESkey);

            AESIv = new byte[1024];
            bytesread = Client.Receive(AESIv);

            Controller.Send(AESkey);
            Thread.Sleep(200);
            Controller.Send(AESIv);


            IsClientConnected = true;

            new Thread(() => ClientStream()).Start();
            return true;
        }

        

        public void SendToClient(byte[] data)
        {
            ClientConn.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));
            Thread.Sleep(200);
            ClientConn.Send(data);
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
                    ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes(";302")).ToArray()
                    , ControllerPublicKey
                );

            Controller.Send(Encoding.UTF8.GetBytes(bytes.Length.ToString()));
            Thread.Sleep(200);
            Controller.Send(bytes);

        }


        


        public void sendVideoBytesToClient(byte[] bytes)
        {
            /*
             * try {
             *  udpConnection.SendTo(bytes, ClientUDP_endpoint);
             * } 
             * catch (Exception e) {   } 
             *
             */
        }




        private void ReadDataFromMicroConntroller()
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

        public bool disconnect()
        {


            try
            {
                ServerServices.removeSession(this);
                FormController.RemoveSession(this);
                if (ClientConn != null)
                    ClientConn.Close();

                if (UdpClientConn != null)
                    UdpClientConn.Close();
                byte[] EncryptedBytes = RsaEncryption.Encrypt("Shut;", ControllerPublicKey);
                if (Controller != null)
                {
                    Controller.Send(Encoding.UTF8.GetBytes(EncryptedBytes.Length.ToString()));
                    Thread.Sleep(200);
                    Controller.Send(EncryptedBytes);
                    Controller.Close();
                }

                if (!IsClientConnected)
                    return true;

                ControllerPublicKey = null;


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





        public string GetCode()
        {
            return this.Code;
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
