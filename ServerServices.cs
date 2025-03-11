using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;



namespace ServerSide
{
    static class ServerServices
    {
        private static Socket serverSocket;
        private static List<session> sessionsList = new List<session>();
        private static List<Guest> ConnectionRequests = new List<Guest>();

        private static byte[] ServerRole = Encoding.UTF8.GetBytes("%%ServerRelatedMessage%%");

        public static void ChangeIvToSessions(byte[] iv)
        {
            byte[] chageIv = AesEncryption.EncryptedData(ServerRole.Concat(Encoding.UTF8.GetBytes("&CHANGEIV&").Concat(iv)).ToArray());
            foreach (session Session in sessionsList)
                Session.SendToClient(chageIv);
        }

        public static (bool, Guest) HasBennHere(string MotherBoard_SN)
        {
            foreach (Guest guest in ConnectionRequests)
            {
                if (MotherBoard_SN == guest.GEt_MotherBoard_SN())
                {
                    return (true, guest);
                }
            }
            
            Guest g = new Guest(MotherBoard_SN);
            return (false, g);
        }
        
        private static object lockedThread = new object();
        public static void MakeGuestBlack(Guest g)
        {
            for (int i = 0; i < ConnectionRequests.Count; i++)
            {
                object item = ConnectionRequests[i];
                if (item is Guest)
                {
                    if (((Guest)item).GEt_MotherBoard_SN() == g.GEt_MotherBoard_SN())
                    {
                        BlackGuest bg = new BlackGuest(g);
                        ConnectionRequests[i] = bg;
                        return;
                    }
                }
            }
        }

        public static void AddGuest(Guest g)
        {
            lock (lockedThread)
            {
                ConnectionRequests.Add(g);
            }
        }

        public static bool IsServerMassageFromMicro(byte[] data)
        {
            try
            {
                data = RsaEncryption.DecryptToBytes(data);
                if (data.Take(ServerRole.Length).SequenceEqual(ServerRole))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                
                
            }
            return false;
        }
        public static bool IsServerMassageFromClient(byte[] data)
        {
            try
            {
                data = AesEncryption.DecryptData(data);
                if (data.Take(ServerRole.Length).SequenceEqual(ServerRole))
                {
                    return true;
                }
            }
            catch (Exception e){ 
                
            }
            return false;
        }




        public static void HandleServerMessages(byte[] buffer, session curentSession, bool IsCLient)
        {
            string message = "";
            if (!IsCLient)
                message = RsaEncryption.Decrypt(buffer);
            else
                message = AesEncryption.DecryptDataToString(buffer);

            string tempString = message.Split('&')[1];

            switch (tempString.Split(';')[0])
            {
                case "EXPERREQUST":
                    
                    string experimentString = SqlService.GetExpererimentOfUser(message);
                    if (experimentString == "")
                        return;
                    byte[] bytes = ServerRole.Concat(Encoding.UTF8.GetBytes("&EXPERIMENT_RESULTS" + experimentString)).ToArray();
                    curentSession.SendToClient(AesEncryption.EncryptedData(bytes));

                    break;
                case "EXPERIMENT_RESULTS":
                    
                    SqlService.AddExperimentToDatabase(tempString, curentSession.GetClienKnickname(), tempString.Split(';')[tempString.Split(';').Length - 3]);
                    
                    break;
                case "REQSTHISTORY":
                    // get send from sql 
                    List<string> CS =  SqlService.GetAllUserHistoryAndSendToClient(message.Split('&')[2]);
                    if (CS != null)
                        sendCreationStringsToClient(CS, curentSession);

                    break;
                case "302":
                    curentSession.disconnectClient(); 
                    FormController.disconnectClient(curentSession);
                    
                    break;

                case "303":
                    sessionsList.Remove(curentSession);
                    curentSession.disconnect();
                    FormController.disconnectController(curentSession);
                    
                    break;


                default: break;
            }
 
        }
        private static void sendCreationStringsToClient(List<string> CS, session curentSession)
        {
            byte[] Experiments = ServerRole.Concat(Encoding.UTF8.GetBytes("&EXPERIMENT_RESULTS")).ToArray();
            foreach (string CreationString in CS)
            {
                Experiments = Experiments.Concat(Encoding.UTF8.GetBytes($"&{CreationString}")).ToArray();
            }
            byte[] CreationByteArray = AesEncryption.EncryptedData(Experiments);
            curentSession.SendToClient(CreationByteArray);
        }




        public static void AddServerSock(Socket ServerSock) { serverSocket = ServerSock; }

        public static void addSession(session session)
        {
            sessionsList.Add(session);
        }

        public static void removeSession(session session) 
        {
            sessionsList.Remove(session);
        }

        public static bool Avalible(string x)
        {
            foreach (var session in sessionsList)
            {
                if (session.GetCode().Equals(x))
                    return false;
            }
            return true;
        }

        public static session GetSession(string x)
        {
            try
            {
                foreach (var session in sessionsList)
                {
                    if (session.GetCode().Equals(x))
                        return session;
                }
            }
            catch (Exception e)
            {}
            return null;

        }
        public static byte[] GetServerRole()
        {
            return ServerRole;
        }

        public static void CloseConnection()
        {
            //hell

            //Send To all Sessions to close connection
            foreach (var session in sessionsList)
            {
                session.disconnect();
            }
            
            serverSocket.Close();
            ClosingController.btnExit_Click();
        }

        public static void CloseAllConnection()
        {
            foreach (session session in sessionsList)
            {
                session.disconnect();
            }
            sessionsList = new List<session>();
        }
    }
}
