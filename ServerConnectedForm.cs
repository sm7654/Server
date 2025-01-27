using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ServerSide
{
    public partial class ServerConnectedForm : Form
    {
        Socket ServerSock;
        Socket UdpClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        sessionLayot Layout;


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool consoleRun();

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
            } while (!ServerServices.Avalible(code));

            return code;
        }


        public ServerConnectedForm(Socket serverSock)
        {
            this.ServerSock = serverSock;
            ServerServices.AddServerSock(serverSock);
            UdpClientSocket.Bind(new IPEndPoint(IPAddress.Any, 65000));
            InitializeComponent();
            FormController.SetForm(this);
        }

        public ServerConnectedForm()
        {
            InitializeComponent();
            consoleRun();
        }
        private void ServerConnectedForm_Load(object sender, EventArgs e)
        {
            




            new Thread(() => ServerHandler()).Start();
        }




        public void ServerHandler()
        {

            ServerSock.Listen(5);



            while (true)
            {
                

                Socket Conn = ServerSock.Accept();


                new Thread(() => SelectionOfConnections(Conn)).Start();
                new Thread(() => ListenToUdpClientConnections()).Start();
                new Thread(() => ReadVideoBytesFromMicro()).Start();
                


            }
        }
        public void SelectionOfConnections(Socket Conn)
        {
            

            try
            {


                byte[] PublicKeyBytes = new byte[1024];
                int Keylength = Conn.Receive(PublicKeyBytes);
                string PublicKey = Encoding.UTF8.GetString(PublicKeyBytes, 0, Keylength);
                Conn.Send(Encryption.getpublickey());


                

                string[] CodeAndKnickname = reciveIdentifiers(Conn);
                

                if (CodeAndKnickname[0] == "Esp")
                {

                    if (IsMicroNameExist(CodeAndKnickname[1]))
                    {
                        Conn.Send(Encryption.Encrypt("500", PublicKey));
                        return;
                    }

                    session session = new session(Conn, GenerateRandomString(5), PublicKey);
                    session.SetControllerKnickname(CodeAndKnickname[1]);
                    ServerServices.addSession(session);

                    this.BeginInvoke(new Action(() =>
                    {

                        sessionLayot test = new sessionLayot(session);
                        test.Click += ControllCliked;
                        SessionViewPanel.Controls.Add(test);

                    }));
                    return;
                }
                else
                {
                    
                    do
                    {
                        string username = CodeAndKnickname[0];
                        bool success = false;
                        bool LoginRequest = false;
                        if (CodeAndKnickname.Length > 2)
                        {
                            success = SqlService.LoginSql(username, CodeAndKnickname[1], false);
                            LoginRequest = true;
                        }
                        else
                            success = SqlService.Register(username, CodeAndKnickname[1], false);


                        if (success)
                        {
                            if (LoginRequest)
                            {
                                if (sessoinSearch(Conn, PublicKey, CodeAndKnickname))
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Conn.Send(Encoding.UTF8.GetBytes("201"));
                            }
                        }
                        else
                        {
                            Conn.Send(Encoding.UTF8.GetBytes("400"));
                        }
                        
                        CodeAndKnickname = reciveIdentifiers(Conn);

                    }

                    while (true);

                }
                
                
            }
            catch (Exception e) { }
        }
        private string[] reciveIdentifiers(Socket Conn)
        {

            byte[] BufferSizeRecive = new byte[1024];
            int bytesRead = Conn.Receive(BufferSizeRecive);

            int IdentifierLength = int.Parse(Encoding.UTF8.GetString(BufferSizeRecive, 0, bytesRead));
            byte[] IdentifierBuffer = new byte[IdentifierLength];

            Conn.Receive(IdentifierBuffer);
            return Encryption.Decrypt(IdentifierBuffer).Split(';');
        }



        private bool sessoinSearch(Socket Conn, string PublicKey, string[] CodeAndKnickname)
        {
            string username = CodeAndKnickname[0];
            
           
            session DesiredSession = ServerServices.GetSession(CodeAndKnickname[2]);
            if (DesiredSession == null)
            {
                Conn.Send(Encoding.UTF8.GetBytes("403"));
                return false;
            }
            else
            {
                Conn.Send(Encoding.UTF8.GetBytes("200"));
                DesiredSession.AddClient(Conn, PublicKey, username);
                foreach (var Control in SessionViewPanel.Controls)
                {
                    if (((sessionLayot)Control).GetSession().GetCode() == CodeAndKnickname[2])
                        ((sessionLayot)Control).SetClientInLayout(DesiredSession);
                }
                return true;

            }

            
        }
        private bool IsMicroNameExist(string name)
        {
            foreach (var sessionOb in SessionViewPanel.Controls)
            {
                sessionLayot CurrentSession = (sessionLayot)sessionOb;

                if (CurrentSession.GetSession().GetControllerKnickname() == name)
                    return true;
            }

            return false;
        }



        private void ListenToUdpClientConnections()
        {

            while (true)
            {
                try
                {
                    byte[] bytes = new byte[1024];
                    EndPoint En = new IPEndPoint(IPAddress.Any, 0);
                    int bytesRead = UdpClientSocket.ReceiveFrom(bytes, ref En);
                    string roomCode = Encryption.Decrypt(bytes.Take(bytesRead).ToArray());

                    new Thread(() =>
                    {
                        session DesiredSession = ServerServices.GetSession(roomCode);
                        DesiredSession.SetClientUdpEndPoint(En);
                    }).Start();
                }
                catch (Exception e) { }
            }
            
        }

        private void ReadVideoBytesFromMicro()
        {
            /*
            while (true)
            {
                byte[] bytes = new byte[3072];
                EndPoint en = new IPEndPoint();
                UdpConnection.recieveFrom(bytes, en);

                new Thread(() => sendVideoBytesToSession(bytes) ).Start();
            }
            */
        }

        private void sendVideoBytesToSession(byte[] bytes)
        {

            string sessionCode = Encryption.Decrypt(bytes.Take(1024).ToArray());

            ServerServices.GetSession(sessionCode).sendVideoBytesToClient(bytes);

            return;
        }

        private void ControllCliked(object sender, EventArgs e)
        {
            if (this.Layout != null)
                this.Layout.BorderStyle = BorderStyle.None;
            this.Layout = (sessionLayot)sender;
            this.Layout.BorderStyle = BorderStyle.Fixed3D;
        }


        
        
        private void CloseButton_Click(object sender, EventArgs e)
        {
            ServerServices.CloseConnection();
        }

        
       

        private void Disconnectbutton_Click(object sender, EventArgs e)
        {
            try
            {

                ServerServices.removeSession(this.Layout.GetSession());
                this.Layout.GetSession().disconnect();
                SessionViewPanel.Controls.Remove(this.Layout);

            } catch (Exception t)
            {

            }
        }

        private void DisconnectServerButton_Click(object sender, EventArgs e)
        {
            ServerServices.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerServices.CloseAllConnection();
            SessionViewPanel.Controls.Clear();
        }

        private void SessionSearch_TextChanged(object sender, EventArgs e)
        {
            string Code = SessionSearch.Text;


            foreach (var Control in SessionViewPanel.Controls)
            {
                if ( !( (sessionLayot)Control ).GetSession().GetCode().Contains(Code) )
                {
                    ((sessionLayot)Control).Hide();
                } else
                {
                    ((sessionLayot)Control).Show();
                }
            }
        }
    }


    
}
