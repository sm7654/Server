using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class ServerConnectedForm : Form
    {
        Socket ServerSock;
        public static string GenerateRandomString(int length)
        {
            string code;
            do
            {
                Random _random = new Random();
                const string chars = "qI3cXfL0e9jN5T2hR8pZb4Y7wD1aV6mWkAonKzCuGvFslJit";
                char[] stringChars = new char[length];

                for (int i = 0; i < length; i++)
                {
                    stringChars[i] = chars[_random.Next(chars.Length)];
                }
                code = new string(stringChars);
            } while (!ServerServices.Avalible(code));

            return code;
        }


        public ServerConnectedForm(Socket serverSock)
        {
            this.ServerSock = serverSock;
            ServerServices.AddServerSock(serverSock);
            InitializeComponent();
        }

        public ServerConnectedForm()
        {
            InitializeComponent();
        }
        private void ServerConnectedForm_Load(object sender, EventArgs e)
        {
            new Thread(() => ServerHandler()).Start();
        }




        public void ServerHandler()
        {
            int BoundPort = ((IPEndPoint)ServerSock.LocalEndPoint).Port;
            ServerSock.Listen(1);



            while (true)
            {
                Socket Conn = ServerSock.Accept();



                new Thread(() => SelectionOfConnections(Conn)).Start();


            }
        }
        public void SelectionOfConnections(Socket Conn)
        {
            byte[] PublicKeyBytes = new byte[1024];
            int Keylength = Conn.Receive(PublicKeyBytes);
            string PublicKey = Encoding.UTF8.GetString(PublicKeyBytes, 0, Keylength);

            
            try
            {

                


                
                byte[] BufferSizeRecive = new byte[1024];
                int bytesRead = Conn.Receive(BufferSizeRecive);
                int IdentifierLength = int.Parse(Encoding.UTF8.GetString(BufferSizeRecive, 0, bytesRead));
                byte[] IdentifierBuffer = new byte[IdentifierLength];
                bytesRead = Conn.Receive(IdentifierBuffer);

                string Identifier = Encoding.UTF8.GetString(IdentifierBuffer, 0, bytesRead);
                Speedometer.Text = Identifier;

                if (Identifier == "Esp")
                {
                    session session = new session(Conn, GenerateRandomString(5), PublicKey);
                    ServerServices.addSession(session);
                    return;
                }
                else
                {
                    session DesiredSession = ServerServices.GetSession(Identifier);
                    if (DesiredSession == null)
                    {
                        Conn.Send(Encoding.UTF8.GetBytes("400"));
                    }
                    else
                    {
                        DesiredSession.AddClient(Conn, PublicKey);
                        Conn.Send(Encoding.UTF8.GetBytes("200"));
                    }
                    return;
                }
                
            }
            catch (Exception e) { }
        }

        private void Speedometer_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ServerServices.CloseConnection();
        }
    }


    
}
