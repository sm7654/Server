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
        private static int clientsCount = 0;

        public static void AddServerSock(Socket ServerSock) { serverSocket = ServerSock; }

        public static void addSession(session session)
        {
            sessionsList.Add(session);
            clientsCount++;
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
