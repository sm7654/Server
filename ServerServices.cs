﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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

        private static byte[] ServerRole = Encoding.UTF8.GetBytes("%%ServerRelatedMessage%%");

        public static void ChengeIvAndKeyToSessions(byte[] iv, byte[] key)
        {

            byte[] chageIv = AesEncryption.EncryptedData(ServerRole.Concat(Encoding.UTF8.GetBytes("&CHANGEIVANDKEY&").Concat(iv).Concat(key)).ToArray());
            foreach (session Session in sessionsList)
                Session.SendToClient(chageIv);
        }

        public static (bool, Guest) HasBennHere(string MotherBoard_SN)
        {
            foreach (Guest guest in ConnectionRequests)
            {
                if (MotherBoard_SN == guest.GEt_MotherBoard_SN())
                {
                    guest.RestartCount();
                    return (true, guest);
                }
            }
            
            Guest g = new Guest(MotherBoard_SN);
            return (false, g);
        }
        public static void StopCountToGuest(Guest g)
        {
            foreach (Guest guest in ConnectionRequests)
            {
                if (guest == g)
                {
                    guest.StopCount();
                    break;
                }
            }
        }

        private static object lockedThread = new object();
        public static BadGuest MakeGuestBlack(Guest g)
        {
            for (int i = 0; i < ConnectionRequests.Count; i++)
            {
                object item = ConnectionRequests[i];
                if (item is Guest)
                {
                    if (((Guest)item).GEt_MotherBoard_SN() == g.GEt_MotherBoard_SN())
                    {
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
            try
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
                        List<string> CS = SqlService.GetAllUserHistoryAndSendToClient(message.Split('&')[2]);
                        if (CS != null)
                            sendCreationStringsToClient(CS, curentSession);

                        break;
                    case "302":

                        string newCode = ServerServices.GenerateRandomString(5);
                        curentSession.disconnectClient(newCode);
                        FormController.disconnectClient(curentSession, newCode);
                        break;

                    case "303":
                        sessionsList.Remove(curentSession);
                        curentSession.disconnect();
                        FormController.disconnectController(curentSession);

                        break;


                    default: break;
                }
            }
            catch (Exception e) { }
 
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

            if (minutes < 0)
                minutes = 0;
            if (seconds < 0)
                seconds = 0;


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
        


    }
}
