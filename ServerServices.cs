using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;



namespace ServerSide
{
    static class ServerServices
    {
        private static Socket serverSocket;
        private static List<session> sessionsList = new List<session>();
        private static byte[] ServerRole = Encoding.UTF8.GetBytes("%%ServerRelatedMessage%%");




        public static bool IsServerMassage(byte[] data)
        {
            try
            {
                data = Encryption.DecryptToBytes(data);
                if (data.Take(ServerRole.Length).SequenceEqual(ServerRole))
                {
                    return true;
                }
            }
            catch (Exception e){ 
                Console.Write(e.Message);
            }
            return false;
        }
        public static void HandleServerMessages(byte[] buffer, session currentSession)
        {

            string[] message = Encryption.Decrypt(buffer).Split(';');
            
            switch (message[1])
            {
                case "SAVEX":
                    

                    break;
                case "302":

                    FormController.disconnectClient(currentSession);
                    currentSession.disconnectClient();
                    break;

                case "303":
                    
                    FormController.disconnectController(currentSession);
                    currentSession.disconnect();
                    break;


                default: break;
            }
 
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
            foreach (var session in sessionsList)
            {
                session.disconnect();
            }
            sessionsList = null;
            sessionsList = new List<session>();
        }
    }
}
