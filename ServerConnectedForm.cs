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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ServerSide
{
    public partial class ServerConnectedForm : Form
    {
        Socket ServerSock;
        sessionLayot Layout;
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

            ServerSock.Listen(5);



            while (true)
            {
                Socket Conn = ServerSock.Accept();



                new Thread(() => SelectionOfConnections(Conn)).Start();


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




                byte[] BufferSizeRecive = new byte[1024];
                int bytesRead = Conn.Receive(BufferSizeRecive);

                int IdentifierLength = int.Parse(Encoding.UTF8.GetString(BufferSizeRecive, 0, bytesRead));
                byte[] IdentifierBuffer = new byte[IdentifierLength];

                Conn.Receive(IdentifierBuffer);

                
                string[] CodeAndKnickname = Encryption.Decrypt(IdentifierBuffer).Split(';');
                string Identifier = CodeAndKnickname[0];

                if (Identifier == "Esp")
                {
                    
                    session session = new session(Conn, GenerateRandomString(5), PublicKey);
                    session.SetControllerKnickname(CodeAndKnickname[1]);
                    ServerServices.addSession(session);

                    this.BeginInvoke(new Action(() => {
                        
                        sessionLayot test = new sessionLayot(session);
                        test.Click += ControllCliked;
                        SessionViewPanel.Controls.Add(test);
                        
                    }));

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
                        //
                        DesiredSession.AddClient(Conn, PublicKey, CodeAndKnickname[1]);
                        foreach (var Control in SessionViewPanel.Controls)
                        {
                            if ( ((sessionLayot)Control).GetSession().GetCode() == Identifier ) {

                                ((sessionLayot)Control).SetClientInLayout(DesiredSession);

                                break;
                            }
                        }
                    }
                    return;
                }
                
            }
            catch (Exception e) { }
        }

        private void ControllCliked(object sender, EventArgs e)
        {
            Speedometer.Text = "clicked";
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

            /*
            if (Code == "")
            {
                foreach (var Control in SessionViewPanel.Controls)
                {
                    ((sessionLayot)Control).Show();
                }
            }
            */

            foreach (var Control in SessionViewPanel.Controls)
            {
                if ( !( (sessionLayot)Control ).GetSession().GetCode().StartsWith(Code) )
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
