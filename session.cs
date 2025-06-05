using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
        private uint EnterTime = 0;
        private string EnterDate = "";

        private Socket Controller = null;
        private string ControllerKnickname;
        private string ControllerPublicKey;
        private bool MicroConnected = false;

        private Socket ClientConn = null;
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
            MicroConnected = true;
            AEFS.setSession(this);

            new Thread(MicroStream).Start();

        }
        public session()
        {
            AEFS = new AesEncryprionForSession();
            AEFS.setSession(this);
            this.ControllerPublicKey = "";
            this.Controller = null;
        }



        private void MicroStream()      
        {
            try
            {
                while (Controller.Connected)
                {

                    byte[] buffer = new byte[1024];
                    byte[] bufferToClient = new byte[1024];
                    int byterec = 0;
                    int bufferSize = 0;
                    try
                    {
                        Controller.ReceiveTimeout = 5000;
                        byterec = Controller.Receive(buffer);
                        bufferSize = int.Parse(Encoding.UTF8.GetString(buffer, 0, byterec));
                        buffer = new byte[bufferSize];
                        Controller.Receive(buffer);
                    }
                    catch (SocketException EX)
                    {
                        ServerServices.removeSession(this);
                        FormController.RemoveSession(this);
                        disconnect();
                    } 
                    

                    new Thread(() =>
                    {
                        try
                        {

                            bufferToClient = buffer;
                            buffer = AEFS.DecryptDataForMicro(buffer);

                            if (ServerServices.IsItServerRelatedMessage(buffer))
                            {
                                new Thread(() => ServerServices.HandleServerMessages(buffer, this)).Start();
                            }
                            else if (ClientConn != null)
                            {

                                new Thread(() => SendToClient(bufferToClient, true)).Start();
                            }

                        }
                        catch (Exception e)
                        {
                            if (bufferToClient != null)
                                SendToClient(bufferToClient, true);
                        }
                        AddToBytesToMicro((uint)byterec + (uint)bufferSize);
                    }).Start();
                    
                    
                }
            }
            catch
            (Exception e)
            {
                ServerServices.removeSession(this);
                FormController.RemoveSession(this);
                disconnect();
            }

        }
        private void ClientStream()
        {
            try
            {

                
                
                
                while (ClientConn.Connected)
                {
                    byte[] buffer = new byte[1024];
                    byte[] bufferoMicro = new byte[1024];
                    int byterec = 0;
                    int bufferSize = 0;
                    try
                    {
                        // קביעת זמן שהייה מקסימאלי לקבלת מידע 
                        ClientConn.ReceiveTimeout = 5000;
                        // קבלת הגודל של המידע לתוך המערך
                        byterec = ClientConn.Receive(buffer);
                        //המרה של תוכן המערך למחרוזת ואז למספר המייצג את גודל המידע 
                        bufferSize = int.Parse(Encoding.UTF8.GetString(buffer, 0, byterec));
                        //ויצירת מערך חדש לפי גודל המידע
                        buffer = new byte[bufferSize];
                        // קבלת המידע עצמו
                        ClientConn.Receive(buffer);
                       
                    }
                    catch (SocketException SE)
                    {
                        //כאשר לא מקבל שום דבר במשך יותר מ 5 שניות נכנס לפה
                        //יצירת קוד חדש בשביל השיחה
                        string newCode = ServerServices.GenerateRandomString(5);
                        //ניתוק הלקוח מהשיחה
                        disconnectClient(newCode);
                        //שינוי הטופס כך שיראה שלקוח התנתק
                        FormController.disconnectClient(this, newCode);

                    }
                    new Thread(() =>
                    {

                        try
                        {
                            //מתקשר עם הלקוח
                            //
                            //שומר את המידע למקרה וההצפנה לא מצליחה ומחזירה ריק
                            bufferoMicro = buffer;
                            //ניסיון לפענח את המידע עם המפתחות של השרת-לקוח
                            buffer = AEFS.DecryptDataForClient(buffer);
                            if (ServerServices.IsItServerRelatedMessage(buffer))
                            {
                                //במידע והשרת הצליח לפענח את המידע עם המפתחות וגם לזאת הוא משייך את ההודעה אליו ומטפל בה
                                new Thread(() => ServerServices.HandleServerMessages(buffer, this)).Start();
                            }
                            else
                            {
                                //במידה והשרת לא מזהה את המזהה הייחודי שלו מתבצעת שליחת המידע לבקר
                                new Thread(() => SendToMicro(bufferoMicro, true)).Start();
                            }
                        }
                        catch (Exception e)
                        {
                            //אם קראה שגיאה שליחת המידע לבקר
                            SendToMicro(bufferoMicro, true);
                        }




                        AddToBytesToCLient((uint)byterec + (uint)bufferSize);
                    }).Start();
                    
                    
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
            if (Controller != null)
            {
                EnterTime = ServerServices.GetTime();
                EnterDate = DateTime.Now.ToString();

                SRD = new SessionRecord(Code, Client_endpoint, ClientKnickname, BytesFromClient,  BytesFromMicro,  EnterTime, EnterDate);
                SessionsRecoeds.Add(SRD);

            }

            
            (byte[] AESKEYSERVER, byte[] AESIVSERVER) = AEFS.GetClientKeys(publickeyClient);
            Client.Send(AESKEYSERVER);
            Thread.Sleep(200);
            Client.Send(AESIVSERVER);



            byte[] AESIv = new byte[128];
            byte[] AESkey = new byte[128];
            int bytesread = 0;

            // send to conrolller client connected
            if (Controller != null)
            {
                lock (ControllerSendLock)
                {
                    byte[] hh = Encoding.UTF8.GetBytes($"&200&{this.ClientConn.RemoteEndPoint.ToString()}");

                    SendToMicroFromServer(hh, false);
                    SendToClient(Encoding.UTF8.GetBytes(ControllerPublicKey), false);

                    AESIv = new byte[128];
                    bytesread = Client.Receive(AESkey);

                    AESIv = new byte[128];
                    bytesread = Client.Receive(AESIv);

                    Controller.Send(AESkey);
                    Thread.Sleep(200);
                    Controller.Send(AESIv);
                }
            }


            IsClientConnected = true;
            
            new Thread(() => ClientStream()).Start();
            new Thread(() => ChangeIvAndKeyForClient()).Start();
            new Thread(() => ChangeIvAndKeyForMicro()).Start();

            AddToBytesToCLient((uint)AESIVSERVER.Length + (uint)AESKEYSERVER.Length);
            AddToBytesToCLient(128 * 2);

            AddToBytesToMicro(128 * 2);




            return true;
        }


        public void SendToClient(byte[] data, bool Encrypted)
        {
            if (ClientConn == null)
                return;

            //כדי לוודא שאין עוד תהליכון ששולח מידע במקביל ClientSendLock מבקש בעלות על האובייקט 
            lock (ClientSendLock) {
                
                //בודק האם יש לבצע הצפנה או לא
                if (!Encrypted)
                    //מבצע הצפנה למידע
                    data = AEFS.EncryptedDataToClient(data);


                ClientConn.Send(Encoding.UTF8.GetBytes(data.Length.ToString()));// שליחת גודל במידע שנרצה לשלוח
                Thread.Sleep(200);// דיליי קטן כדי לוודא שהצד המקבל מוכן לקבל את המידע עצמו
                ClientConn.Send(data);// שליחת המידע עצמו

                //הוספה של כמות המידע שעבר ללקוח לסכום הכללי 
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
                    Controller.Send(Encoding.UTF8.GetBytes(data.Length.ToString())); // שליחת גודל במידע שנרצה לשלוח
                    Thread.Sleep(250); // דיליי קטן כדי לוודא שהצד המקבל מוכן לקבל את המידע עצמו
                    Controller.Send(data); // שליחת המידע עצמו

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
            // ווידוי שעדכון של נתון לא יתבצע במקביל לעדכון אחר (יוביל לנתונים לא אמינים)צ
            lock ((object)BytesFromClient)
            {
                //הוספת כמות מידע חדשה שהועברה מלקוח\ללקוח לכמות מידע שבערה\הועברה עד עכשיו
                BytesFromClient += bytes;
                //מניעת שגיאות
                if (SRD != null)
                {
                    //עדכון האובייקט המייצג את השיחה בכמות המידע שעברה מהלקוח\ללקוח
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
            IsClientConnected = false;
            if (Controller != null)
            {
                SRD.MakeNotLiveRecored();
                SRD = null; 

            }
            EnterTime = 0;
            EnterDate = "";
            BytesFromClient = 0;
            BytesFromMicro = 0;
            ClientConn = null;
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
            EnterTime = 0;
            


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
            catch (Exception e) { Controller.Close(); MessageBox.Show("hoo shit, not good"); }

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
            return _random.Next(1000 * 10, 1000 * 20); // Random number between 60000 and 180000 (inclusive)
        }

        public void ChangeIvAndKeyForClient()
        {
            while (IsClientConnected)
            {
                int delay = GetRandomDelay();
                Thread.Sleep(delay);
                AEFS.regenerateKeysForClient();
            }
        }
        public void ChangeIvAndKeyForMicro()
        {
            while (MicroConnected)
            {
                int delay = GetRandomDelay();
                Thread.Sleep(delay);
                AEFS.regenerateKeysForMicro();
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
        public bool ClientConnected()
        {
            return IsClientConnected;
        }
        public void ClientGotKeys()
        {
            AEFS.ClientGotKeysSuccessfuly();
        }
        public void MicroGotKeys()
        {
            AEFS.MicroGotKeysSuccessfuly();
        }
    }
}
