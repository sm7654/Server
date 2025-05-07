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

        private AesEncryprionForSession AEFS;

        private sessionLayot SessionUI = null;


        private List<SessionRecord> SessionsRecoeds = new List<SessionRecord>();

        private SessionRecord SRD = null;
        
        public session(Socket Controller, string Code, string sessionName, string publickeymicro, AesEncryprionForSession AESForSession)
        {

            this.Controller = Controller;
            this.Code = Code;
            this.sessionName = sessionName;
            this.ControllerPublicKey = publickeymicro;
            this.AEFS = AESForSession;



            ///////////////////////////////////////


            new Thread(() => MicroStream()).Start();
        }

        

        private void MicroStream()
        {
            try
            {
                while (Controller.Connected)
                {

                    byte[] buffer = new byte[1024];
                    byte[] bufferToClient = new byte[1024];
                    try
                    {
                        Controller.ReceiveTimeout = 0;
                        int byterec = Controller.Receive(buffer);
                        int bufferSize = int.Parse(Encoding.UTF8.GetString(buffer, 0, byterec));
                        buffer = new byte[bufferSize];
                        Controller.Receive(buffer);
                        bufferToClient = buffer;

                        AddToBytesToMicro((uint)byterec + (uint)bufferSize);
                        buffer = AEFS.DecryptDataForMicro(buffer);
                        if (ServerServices.IsItServerRelatedMessage(buffer))
                        {
                            ServerServices.HandleServerMessages(buffer, this);
                        }
                        else if (ClientConn != null)
                        {
                            SendToClient(bufferToClient, true);
                        }
                    }
                    catch (SocketException SE)
                    {

                        FormController.RemoveSession(this);
                        disconnect();
                    }

                    catch (Exception e)
                    { 
                        SendToClient(bufferToClient, true); }
                }
            }
            catch
            (Exception e)
            { }

        }
        private void ClientStream()
        {
            try
            {

                while (Controller.Connected && ClientConn.Connected)
                {
                    byte[] buffer = new byte[1024];
                    byte[] bufferoMicro = new byte[1024];
                    try
                    {
                        ClientConn.SendTimeout = 0;
                        int byterec = ClientConn.Receive(buffer);
                        int bufferSize = int.Parse(Encoding.UTF8.GetString(buffer, 0, byterec));
                        buffer = new byte[bufferSize];
                        ClientConn.Receive(buffer);
                        bufferoMicro = buffer;
                        buffer = AEFS.DecryptDataForClient(buffer);
                        if (ServerServices.IsItServerRelatedMessage(buffer))
                        {
                            ServerServices.HandleServerMessages(buffer, this);
                        }
                        else
                        {

                            SendToMicro(bufferoMicro, true);
                        }

                        AddToBytesToCLient((uint)byterec + (uint)bufferSize);
                    }
                    catch (SocketException SE)
                    {
                        string newCode = ServerServices.GenerateRandomString(5);
                        disconnectClient(newCode);
                        FormController.disconnectClient(this, newCode);
                        
                    }
                    catch (Exception e) {
                        SendToMicro(bufferoMicro, true);
                    }
                }
            }
            catch (Exception e) {
                 }



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

            (byte[] AESKEYSERVER, byte[] AESIVSERVER) = AEFS.GetClientKeys(publickeyClient);
            Client.Send(AESKEYSERVER);
            Thread.Sleep(200);
            Client.Send(AESIVSERVER);



            byte[] AESIv = new byte[128];
            byte[] AESkey = new byte[128];
            int bytesread = 0;

            // send to conrolller client connected
            byte[] hh = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes($"&200&{this.ClientConn.RemoteEndPoint.ToString()}")).ToArray();


            SendToMicro(hh, false);
            
            SendToClient(Encoding.UTF8.GetBytes(ControllerPublicKey), false);
            


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

            AddToBytesToCLient((uint)AESIVSERVER.Length + (uint)AESKEYSERVER.Length);
            AddToBytesToCLient(128 * 2);

            AddToBytesToMicro(128 * 2);

            



            return true;
        }


        public void SendToClient(byte[] data, bool Encrypted)
        {
            if (ClientConn == null)
                return;
            lock (ClientSendLock) {
                if (!Encrypted)
                    data = AEFS.EncryptedDataToClient(data);
                ClientConn.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));
                Thread.Sleep(200);
                ClientConn.Send(data);


                AddToBytesToCLient((uint)Encoding.UTF8.GetBytes(data.Length.ToString()).Length + (uint)data.Length);



            }
        }
        public void SendToClientFromServer(byte[] data, bool Encrypted)
        {
            SendToClient(ServerServices.GetServerRole().Concat(data).ToArray(), Encrypted);
        }
        public void SendToMicro(byte[] data, bool Encrypted)
        {
            if (Controller == null)
                return;
            try
            {
                lock (ControllerSendLock)
                {
                    if (!Encrypted)
                        data = AEFS.EncryptedDataToMicro(data);
                    Controller.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));
                    Thread.Sleep(200);
                    Controller.Send(data);

                    AddToBytesToMicro((uint)Encoding.UTF8.GetBytes(data.ToString()).Length + (uint)data.Length);

                }
            } catch (Exception e) { 
                return; 
            }
        }
        public void SendToMicroFromServer(byte[] data, bool Encrypted)
        {
            SendToMicro(ServerServices.GetServerRole().Concat(data).ToArray(), Encrypted);
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

            byte[] bytes = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes($"&302&{this.Code}")).ToArray();

            SendToMicro(bytes, false);
            







        }






        public bool disconnect()
        {


            ControllerPublicKey = null;
            if (SRD != null)
            {
                SRD.MakeNotLiveRecored();
                SRD = null;
            }

            SessionsRecoeds = null;
            BytesFromMicroGlobal = 0;
            BytesFromMicro = 0;
            BytesFromClient = 0;
            EnterDate = null;
            EnterTimeGlobal = 0;
            EnterTime = 0;
            UdpClientConn = null;



            Client_endpoint = null;

            try
            {
                byte[] message = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes("&Shut")).ToArray();
                byte[] EncryptedBytes = message;

                if (Controller != null)
                {
                    SendToMicro(EncryptedBytes, false);
                    Controller.Close();
                }

            }
            catch (Exception e) { Controller.Close(); }

            try {
                if (ClientConn != null)
                {
                    byte[] message = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes("&303")).ToArray();
                    byte[] EncryptedBytes = message;

                    SendToClient(EncryptedBytes, false);
                    ClientConn.Close();
                }

            }
            catch (Exception e) { ClientConn.Close(); }
            try
            {
                Code = null;
                
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public int GetRandomDelay()
        {
            Random _random = new Random();
            return _random.Next(10000, 11000); // Random number between 60000 and 180000 (inclusive)
        }

        public void ChengeIvAndKey()
        {
            //while (true)
            //{
            int delay = GetRandomDelay();
            delay = 0;
            Thread.Sleep(delay);
            byte[] AESTempIV;
            byte[] AESTempIKey;
            using (Aes aesServise = Aes.Create())
            {
                aesServise.KeySize = 256;
                AESTempIV = aesServise.IV;
                AESTempIKey = aesServise.Key;
            }

            AEFS.SetNewIvAndKey(AESTempIV, AESTempIKey);

            byte[] chageIv = ServerServices.GetServerRole().Concat(Encoding.UTF8.GetBytes("&CHANGEIVANDKEY&").Concat(AESTempIV).Concat(AESTempIKey)).ToArray();
            //}
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
