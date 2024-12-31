using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        private string ClientPublicKey;
        private EndPoint ClientUDP_endpoint;
        




        public session(Socket Controller, string Code, string publicKey)
        {
            
            this.Controller = Controller;
            this.Code = Code;
            this.ControllerPublicKey = publicKey;
            

            byte[] EncryptedCode = Encryption.Encrypt($"CDR: {Code}", ControllerPublicKey);
            byte[] g = Encoding.UTF8.GetBytes(EncryptedCode.Length.ToString());
            Controller.Send(Encoding.UTF8.GetBytes(EncryptedCode.Length.ToString()));
            Controller.Send(EncryptedCode);

        }
        public bool AddClient(Socket Client, string PublicKey, string name)
        {
            if (Client == null)
                return false;
            this.ClientConn = Client;
            this.ClientPublicKey = PublicKey;
            this.ClientKnickname = name;

            byte[] ConnectedMassege = Encryption.Encrypt($"200;{this.ClientConn.RemoteEndPoint.ToString()}", this.ControllerPublicKey);
            Controller.Send(Encoding.UTF8.GetBytes(ConnectedMassege.Length.ToString()));
            Thread.Sleep(200);
            Controller.Send(ConnectedMassege);
            


            return true;
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
            return (  ((IPEndPoint)this.Controller.RemoteEndPoint).Address.ToString(), ((IPEndPoint)this.Controller.RemoteEndPoint).Port.ToString()  );
        }



        public (string, string) GetCLientEndPoint()
        {
            if (ClientConn == null)
                return ("Not Connected", "Not Connected");
            return (((IPEndPoint)this.Controller.RemoteEndPoint).Address.ToString(), ((IPEndPoint)this.Controller.RemoteEndPoint).Port.ToString());
        }
        public string GetClienKnickname()
        {
            if (this.ClientKnickname == null)
                return "Oops...";
            return this.ClientKnickname;
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
                if (ClientConn != null)
                    ClientConn.Close();

                if (UdpClientConn != null)
                    UdpClientConn.Close();
                byte[] EncryptedBytes = Encryption.Encrypt("Shut;", ControllerPublicKey);
                Controller.Send(Encoding.UTF8.GetBytes(EncryptedBytes.Length.ToString()));
                Thread.Sleep(200);
                Controller.Send(EncryptedBytes);
                Controller.Close();


                ClientPublicKey = null;
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






    }
}
