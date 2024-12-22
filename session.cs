using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace ServerSide
{

    internal class session
    {
        private string Code;
        private Socket Controller;
        private string ControllerPublicKey;
        private Socket ClientConn = null;
        private string ClientPublicKey;
        




        public session(Socket Controller, string Code, string publicKey)
        {
            
            this.Controller = Controller;
            this.Code = Code;
            this.ControllerPublicKey = publicKey;
            

            byte[] EncryptedCode = Encryption.Encrypt($"CDR: {Code}", ControllerPublicKey);

            Controller.Send(Encoding.UTF8.GetBytes(EncryptedCode.Length.ToString()));
            Controller.Send(EncryptedCode);

        }
        public bool AddClient(Socket Client, string PublicKey)
        {
            if (Client == null)
                return false;
            this.ClientConn = Client;
            this.ClientPublicKey = PublicKey;
            byte[] ConnectedMassege = Encoding.UTF8.GetBytes(200.ToString());
            Controller.Send(ConnectedMassege);


            return true;
        }

        public string GetCode()
        {
            return this.Code;
        }


        public void SetControllerKey(string publicKey)
        {
            this.ControllerPublicKey = publicKey;
        }









        private void ReadDataFromMicroConntroller()
        {
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
                catch (FormatException e) { Console.WriteLine("Did not recived a number....."); }
            }
        }




    }
}
