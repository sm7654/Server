using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class ServerConnectedForm : Form
    {
        Socket ServerSock;
        Socket UdpClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        sessionLayot Layout;



        

        public ServerConnectedForm(Socket serverSock)
        {
            this.ServerSock = serverSock;
            ServerServices.AddServerSock(serverSock);
            UdpClientSocket.Bind(new IPEndPoint(IPAddress.Any, 65000));
            InitializeComponent();
            FormController.SetForm(this);
            ServerServices.StartTimer();
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

            ServerSock.Listen(100);



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

                bool IsMicro = false;
                Guest TempG = null;
                byte[] PublicKeyBytes = new byte[1024];
                int Keylength = Conn.Receive(PublicKeyBytes);
                string PublicKey = Encoding.UTF8.GetString(PublicKeyBytes, 0, Keylength);
                Conn.Send(RsaEncryption.getpublickey());


                byte[] recognition = new byte[128];
                Conn.Receive(recognition);
                string re = RsaEncryption.Decrypt(recognition);
                if (re == "Esp")
                    IsMicro = true;
                //////////////////////////////

                /*(byte[] AesKey, byte[] AesIV) = AesEncryption.GetAesEncryptedTempKeys(PublicKey);
                Conn.Send(AesKey);
                Thread.Sleep(200);
                Conn.Send(AesIV);
*/
                //////////////////////////////

                if (!IsMicro)
                {


                    byte[] bytes = new byte[128];
                    Conn.Receive(bytes);
                    string MotherBoardSerialNumber = "";
                    

                    try
                    {


                        MotherBoardSerialNumber = RsaEncryption.Decrypt(bytes);

                        (bool NC, Guest g) = ServerServices.HasBennHere(MotherBoardSerialNumber);

                        Thread.Sleep(100);

                        if (NC)
                        {
                            if (g is BadGuest)
                            {
                                Conn.Send(Encoding.UTF8.GetBytes("NoOk"));
                                foreach (BlockedClient badGuest in BlockedClients.Controls)
                                    if (badGuest.Get_MotherBoard_SN() == MotherBoardSerialNumber)
                                    {
                                        badGuest.AddAttempt();
                                        break;
                                    }

                                return;
                            }
                            Conn.Send(Encoding.UTF8.GetBytes("Ok"));
                        }
                        else
                        {
                            Conn.Send(Encoding.UTF8.GetBytes("Ok"));
                            ServerServices.AddGuest(g);
                        }
                        TempG = g;

                    }
                    catch (Exception e)
                    {
                    }
                }



                //string[] CodeAndKnickname = reciveIdentifiers(Conn);
                string[] SessionAndMicroName;
                if (IsMicro)
                {
                    AesEncryprionForSession AEFS = new AesEncryprionForSession();
                    (byte[] aeskey, byte[] aesIv) = AEFS.GetMicroKeys(PublicKey);
                    Conn.Send(aeskey);
                    Thread.Sleep(200);
                    Conn.Send(aesIv);

                    do
                    {
                        ////////////////////////////////////////////////////////////////////////////////////////////////////
                        //get micro name and session name
                        byte[] MicroSessionName = new byte[1024];
                        int byterec = Conn.Receive(MicroSessionName);
                        int bufferSize = int.Parse(Encoding.UTF8.GetString(MicroSessionName, 0, byterec));
                        MicroSessionName = new byte[bufferSize];
                        Conn.Receive(MicroSessionName);
                        ////////////////////////////////////////////////////////////////////////////////////////////////////
                        SessionAndMicroName = AEFS.DecryptDataForMicroToString(MicroSessionName).Split('&');

                        if (!IsMicroNameExist(SessionAndMicroName[0]))
                        {
                            break;
                        } else
                        {
                            //send to micro error message
                            byte[] Error = AEFS.EncryptedDataToMicro(Encoding.UTF8.GetBytes($"500"));
                            Conn.Send(Encoding.UTF8.GetBytes(Error.Length.ToString()));
                            Thread.Sleep(250);
                            Conn.Send(Error);
                        }
                    }
                    while (true);

                    /////////////////////////////////////////////////////////////////////////////////////
                    //Send the code to the micro
                    string Code = ServerServices.GenerateRandomString(5);
                    byte[] EncryptesCode = AEFS.EncryptedDataToMicro($"{Code}");
                    Conn.Send(Encoding.UTF8.GetBytes(EncryptesCode.Length.ToString()));
                    Thread.Sleep(250);
                    Conn.Send(EncryptesCode);
                    /////////////////////////////////////////////////////////////////////////////////////
                    
                    session session = new session(Conn, Code, SessionAndMicroName[0],PublicKey ,AEFS);
                    session.SetControllerKnickname(SessionAndMicroName[1]);
                    ServerServices.addSession(session);

                    this.BeginInvoke(new Action(() =>
                    {

                        sessionLayot test = new sessionLayot(session);
                        
                        test.Click += ControllCliked;
                        SessionsViewPanel.Controls.Add(test);

                    }));
                    return;
                }
                else
                {
                    (byte[] key, byte[] iv) = AesEncryption.GetAesEncryptedTempKeys(PublicKey);

                    Conn.Send(key);
                    Thread.Sleep(200);
                    Conn.Send(iv);


                    string[] CodeAndKnickname = reciveIdentifiers(Conn);
                    do
                    {
                        try
                        {
                            string username = CodeAndKnickname[1];
                            bool success = false;
                            bool LoginRequest = false;
                            string personalCode = "";
                            if (CodeAndKnickname[0] == "RESETPASS")
                            {
                                success = SqlService.RestPass(username, CodeAndKnickname[2], CodeAndKnickname[3], false);
                            } else if (CodeAndKnickname[0] == "CONNECTOSESSION")
                            {
                                success = SqlService.LoginSql(username, CodeAndKnickname[2], false);
                                LoginRequest = true;
                            } else if (CodeAndKnickname[0] == "NEWCLIENT")
                            {

                                (success, personalCode) = SqlService.Register(username, CodeAndKnickname[2], false);
                            }

                            if (success)
                            {
                                if (LoginRequest)
                                {
                                    if (sessoinSearch(Conn, CodeAndKnickname, PublicKey))
                                    {
                                        return;
                                    }
                                    else
                                    {

                                        TempG.Log();
                                    }
                                }
                                else
                                {
                                    SendToClient(Conn, $"201&{personalCode}");
                                }
                            }
                            else
                            {
                                TempG.Log();
                                SendToClient(Conn, $"400&");
                            }

                            CodeAndKnickname = reciveIdentifiers(Conn);

                            if (TempG.GetLogs() > 8 && (TempG.IsConssistent() || TempG.AvrageLogTime() < 4))
                            {
                                SendToClient(Conn, $"999&");
                                BlockedClient NewBad = new BlockedClient(ServerServices.MakeGuestBlack(TempG));
                                this.BeginInvoke(new Action(() => {
                                    BlockedClients.Controls.Add(NewBad);
                                }));
                                Conn.Close();
                                return;
                            }
                        
                        
                            

                        }
                        catch (Exception ex) {
                            SendToClient(Conn, $"400&");
                            if (CodeAndKnickname[0] == null)
                            {
                                ServerServices.StopCountToGuest(TempG);
                                Conn.Close();
                                return;
                            }
                            CodeAndKnickname = reciveIdentifiers(Conn);
                            
                        }



                    }

                    while (true);

                }
                
                
            }
            catch (Exception e) { }
        }
        private void SendToClient(Socket Conn, string message)
        {
            byte[] EncryptedMessage = AesEncryption.EncryptedDataWithTempKeys(Encoding.UTF8.GetBytes(message));
            Conn.Send(Encoding.UTF8.GetBytes(EncryptedMessage.Length.ToString()));
            Thread.Sleep(300);
            Conn.Send(EncryptedMessage);


        }

        private string[] reciveIdentifiers(Socket Conn)
        {
            try
            {
                byte[] BufferSizeRecive = new byte[1024];
                Conn.ReceiveTimeout = 0;
                int bytesRead = Conn.Receive(BufferSizeRecive);
                BufferSizeRecive = new byte[int.Parse(Encoding.UTF8.GetString(BufferSizeRecive, 0, bytesRead))];
                Conn.Receive(BufferSizeRecive);
                try
                {
                    string[] ff = AesEncryption.DecryptDataToStringWithTempKeys(BufferSizeRecive).Split('&');
                    return ff;
                }
                catch (Exception e)
                { return new string[7]; }
                
            }
            catch (Exception e) {
                MessageBox.Show($"{e.Message} From Reciever");
                return new string[7]; }
        }



        private bool sessoinSearch(Socket Conn,  string[] CodeAndKnickname, string publickey)
        {
            string username = CodeAndKnickname[1];
            
           
            session DesiredSession = ServerServices.GetSession(CodeAndKnickname[3]);
            if (DesiredSession == null)
            {
                SendToClient(Conn, "400&");
                return false;
            }
            else
            {
                SendToClient(Conn, "200&");

                DesiredSession.AddClient(Conn, username, publickey);
                foreach (var Control in SessionsViewPanel.Controls)
                {
                    if (((sessionLayot)Control).GetSession().GetCode() == CodeAndKnickname[3])
                        ((sessionLayot)Control).SetClientInLayout(DesiredSession);
                }
                return true;

            }

            
        }

        private bool IsMicroNameExist(string name)
        {
            foreach (session s in ServerServices.GetSessionsList())
                if (s.GetControllerKnickname().Equals(name))
                    return true;
            

            return false;
        }


        private void ControllCliked(object sender, EventArgs e)
        {
            if (this.Layout != null)
            {
                this.Layout.BackColor = SystemColors.MenuHighlight;
                this.Layout.ToggleShutButton();
                this.Layout = null;
            }
            else
            {
                this.Layout = (sessionLayot)sender;
                this.Layout.BackColor = Color.DarkOrange;
                this.Layout.ToggleShutButton();
            }
        }     

        private void CloseButton_Click(object sender, EventArgs e)
        {
            SessionsViewPanel.Controls.Clear();
            ServerServices.CloseConnection();
        }


        private void Disconnectbutton_Click(object sender, EventArgs e)
        {
            try
            {
                session s = this.Layout.GetSession();
                s.disconnect();
                FormController.RemoveSession(s);
                ServerServices.removeSession(s);
                

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
        }

        private void SessionSearch_TextChanged(object sender, EventArgs e)
        {
            string Code = SessionSearch.Text;
            if (Code.StartsWith("&"))
            {
                AdvancedSearch(Code);
                return;
            }    

            foreach (var Control in SessionsViewPanel.Controls)
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

        private void AdvancedSearch(string text)
        {
            string[] advSearch = text.Substring(1).Split('&');
            if (advSearch.Length <= 1)
                return;
            switch (advSearch[0].ToLower())
            {
                case "sn":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (!((sessionLayot)Control).GetSession().GetSessionName().StartsWith(advSearch[1]))
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();

                    }
                    break;
                case "code":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (!((sessionLayot)Control).GetSession().GetCode().StartsWith(advSearch[1]))
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();

                    }
                    break;
                case "micro":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (!((sessionLayot)Control).GetSession().GetControllerKnickname().StartsWith(advSearch[1]))
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();
                        
                    }
                    break;
                case "client":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (!((sessionLayot)Control).GetSession().GetClienKnickname().StartsWith(advSearch[1]))
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();

                    }
                    break;
                case "mip":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (!((sessionLayot)Control).GetSession().GetControllerEndPoint().Item1.StartsWith(advSearch[1]))
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();

                    }
                    break;
                case "mport":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (!((sessionLayot)Control).GetSession().GetControllerEndPoint().Item2.StartsWith(advSearch[1]))
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();

                    }
                    break;
                case "cip":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (((sessionLayot)Control).GetSession().GetControllerEndPoint().Item1 != advSearch[1])
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();

                    }
                    break;
                case "cport":
                    foreach (var Control in SessionsViewPanel.Controls)
                    {
                        if (!((sessionLayot)Control).GetSession().GetCLientEndPoint().Item2.StartsWith(advSearch[1]))
                            ((sessionLayot)Control).Hide();
                        else
                            ((sessionLayot)Control).Show();

                    }
                    break;

                default: break;
            }
            
        }


        private void ServerConnectedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosingController.btnExit_Click();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show(
            //ServerServices.EV()
            //); 
            //AesEncryption.ChengeIvAndKey();

        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }


    
}
