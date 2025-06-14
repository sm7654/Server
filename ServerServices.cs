using CredentialManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace ServerSide
{
    static class ServerServices
    {
        private static Socket serverSocket;
        private static List<session> sessionsList = new List<session>();
        private static List<Guest> ConnectionRequests = new List<Guest>();
        private static uint timer = 0;
        private static SessionDisplayForm sessionDisplayForm = null;

        private static byte[] ServerRole = Encoding.UTF8.GetBytes("%%ServerRelatedMessage%%");

        

        public static bool IsBadGuest(string MotherBoard_SN)
        {



            foreach (Guest guest in ConnectionRequests)
            {
                if (MotherBoard_SN == guest.GEt_MotherBoard_SN())
                {
                    if (guest is BadGuest)
                        return true;
                }
            }
            return false;
        }
        public static void RemoveGueast(Guest g)
        {
            if (ConnectionRequests.Contains(g))
                ConnectionRequests.Remove(g);
        }
        
        public static void importBlockedGuestFromSQL()
        {
            List<BadGuest> l = SqlService.GetBlockedGuestFromSQL();
            if (l == null)
                return;
            foreach (BadGuest guest in l)
            {
                ConnectionRequests.Add(guest);
                BlockedClient blockedClient = new BlockedClient(guest);
                FormController.AddBlockedGuest(blockedClient);
            }
        }
        private static object lockedThread = new object();
        public static BadGuest MakeGuestBad(Guest g)
        {
            for (int i = 0; i < ConnectionRequests.Count; i++)
            {
                object item = ConnectionRequests[i];
                if (item is Guest)
                {
                    if (((Guest)item).GEt_MotherBoard_SN() == g.GEt_MotherBoard_SN())
                    {
                        g.StopCount();
                        BadGuest bg = new BadGuest(g);
                        ConnectionRequests[i] = bg;
                        return bg;
                    }
                }
            }
            return null;
        }

        public static void AddGuest(Guest g)
        {
            lock (lockedThread)
            {
                ConnectionRequests.Add(g);
            }
        }

        
        public static bool IsItServerRelatedMessage(byte[] data)
        {
            try
            {

                if (data.Take(ServerRole.Length).SequenceEqual(ServerRole))
                {
                    return true;
                }
            }
            catch (Exception e){
                Console.WriteLine();
            }

            return false;
        }


        public static (byte[], byte[]) GetKeysFromFile()
        {
            try
            {
                byte[] key;
                byte[] iv;
                // קביעת הנתיב של הקובץ בו ישמרו המפתחות המוצפנים
                string filepath = "ServerAppSettingsAESAlg.Encrypted";
                //בדיקה האם הקובץ קיים
                if (File.Exists(filepath))
                {
                    //קריאה של כל המידע מהקובץ
                    byte[] bytes = File.ReadAllBytes(filepath);
                    //פענוח של המידע מהקבוץ
                    byte[] decryptedBytes = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                    //שליפת המפתחות המפוענחים מהקובץ
                    iv = decryptedBytes.Take(16).ToArray();        
                    key = decryptedBytes.Skip(16).ToArray();
                }
                else
                {
                    //יצירת מפתחות הצפנה חדשים
                    (key, iv) = AesEncryption.generate();
                    //שילוב של שני המפתחות למערך בייטים
                    byte[] en = iv.Concat(key).ToArray();
                    //הצפנת המפתחות
                    en = ProtectedData.Protect(en, null, DataProtectionScope.CurrentUser);
                    //כתיבת המפתחות המוצפנים לקובץ
                    File.WriteAllBytes(filepath, en);
                }
                //החזרת המפתחות
                return (key, iv);
            }
            catch (Exception e)
            {
                //החזרת ערכים ריקים או התגלתה שגיאה
                return (null, null);
            }
        }

        public static void HandleServerMessages(byte[] buffer, session curentSession)
        {
            try
            {
                string message = Encoding.UTF8.GetString(buffer);
               
                string tempString = message.Split('&')[1];


                switch (tempString.Split(';')[0])
                {
                    case "EXPERREQUST":

                        string experimentString = SqlService.GetExpererimentOfUser(message);
                        if (experimentString == "")
                            return;
                        byte[] bytes = ServerRole.Concat(Encoding.UTF8.GetBytes("&EXPERIMENT_RESULTS" + experimentString)).ToArray();
                        curentSession.SendToClient(bytes, false);

                        break;
                    case "EXPERIMENT_RESULTS":

                        SqlService.AddExperimentToDatabase(tempString, curentSession.GetClienKnickname());

                        break;
                    case "REQSTHISTORY":
                        // get send from sql 
                        List<string> CS = SqlService.GetAllUserHistoryAndSendToClient(message.Split('&')[2]);
                        if (CS != null)
                            sendCreationStringsToClient(CS, curentSession);

                        break;
                    case "MicroPing":
                        curentSession.SendToMicroFromServer(Encoding.UTF8.GetBytes("&Ping"), false);
                        break;
                    // קבלת הודעת הפינג מצד השרת ושליחת הודעת פינג חזרה
                    case "ClientPing":
                        curentSession.SendToClientFromServer(Encoding.UTF8.GetBytes("&Ping"), false);
                        break;
                    case "ClientGotKeys":
                        curentSession.ClientGotKeys();
                        break;
                    case "MicroGotKeys":
                        curentSession.MicroGotKeys();
                        break;
                    case "303":

                        sessionsList.Remove(curentSession);
                        FormController.RemoveSession(curentSession);
                        curentSession.disconnect();
                        break;


                    default: break;
                }
            }
            catch (Exception e) 
            {
            }
 
        }
        private static void sendCreationStringsToClient(List<string> CS, session curentSession)
        {
            byte[] Experiments = ServerRole.Concat(Encoding.UTF8.GetBytes("&EXPERIMENT_RESULTS")).ToArray();
            foreach (string CreationString in CS)
            {
                Experiments = Experiments.Concat(Encoding.UTF8.GetBytes($"&{CreationString}")).ToArray();
            }
            curentSession.SendToClient(Experiments, false);
        }


        public static string GenerateRandomString(int length)
        {
            string code;
            do
            {
                Random _random = new Random();
                const string chars = "a2QjWz3n0v1p7Gm5kI9oXebVd4yHcL6f8sT";
                char[] stringChars = new char[length];

                for (int i = 0; i < length; i++)
                {
                    stringChars[i] = chars[_random.Next(chars.Length)];
                }
                code = new string(stringChars);
            } while (!Avalible(code));

            return code;
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
            foreach (var session in sessionsList)
                session.disconnect();
            
            
            serverSocket.Close();
        }

        public static void CloseAllConnection()
        {
            foreach (session session in sessionsList.ToList())
            {
                try
                {
                    session.disconnect();
                }
                catch (Exception e) { }
            }
            
            sessionsList.Clear();
        }
        public static void StartTimer()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    timer++;
                }
            }).Start();
        }
        public static uint GetTime()
        {
            return timer;
        }
        public static List<session> GetSessionsList()
        {
            return sessionsList;
        }

        public static string CalcTime(uint timeinsec)
        {
            uint hours;
            uint minutes;
            uint seconds;
            hours = timeinsec / 3600;
            minutes = (timeinsec - hours * 3600) / 60;
            seconds = timeinsec - hours * 3600 - minutes * 60;

            /*
            if (minutes < 0)
                minutes = 0;
            if (seconds < 0)
                seconds = 0;
            */

            return $"{hours}h {minutes}m {seconds}s";
        }

        public static string MakeBytesString(double number)
        {
            string Unit = "";
            double unitClac = (double)number / 1024;

            if (unitClac < 1)
            {
                Unit = "bytes";
            }
            else if (unitClac >= 1 && unitClac < 1000)
            {
                Unit = "Kb";
                number /= 1000;
            }
            else if (unitClac >= 1000 && unitClac < 1000 * 1000)
            {
                Unit = "Mb";
                number /= 1000 * 1000;
            }
            else if (unitClac >= 1000 * 1000)
            {
                Unit = "Gb";
                number /= 1000 * 1000 * 1000;
            }
            return (Math.Round(number, 2)).ToString() + " " + Unit;
        }
        public static void setDisplayFrom(SessionDisplayForm SDF)
        {
            sessionDisplayForm = SDF;
        }
        public static void AddNewSessionToForm(SessionRecord SD)
        {
            if (sessionDisplayForm != null)
                sessionDisplayForm.AddSessoinToFrom(SD);
        }


    }
}
