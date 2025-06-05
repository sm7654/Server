using System;
using System.Collections.Generic;
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
        sessionLayot Layout;



        

        public ServerConnectedForm(Socket serverSock)
        {
            this.ServerSock = serverSock;
            ServerServices.AddServerSock(serverSock);
            InitializeComponent();
            FormController.SetForm(this);
            ServerServices.StartTimer();

        }

        private void ServerConnectedForm_Load(object sender, EventArgs e)
        {


            ServerServices.importBlockedGuestFromSQL();


            new Thread(ServerHandler).Start();
        }




        public void ServerHandler()
        {

            ServerSock.Listen(100);


            while (true)
            {
                // קבלת חיבורים חדשים ושמירתם בתוך משתנה המייצג את החיבור
                Socket Conn = ServerSock.Accept();
                // חדש כדי שהשרת יוכל לקבל לקוחות חדשים ובמקביל יטפל בחיבור החדש thread פתיחת
                new Thread(() => SelectionOfConnections(Conn)).Start();    
            }




        }




        public void SelectionOfConnections(Socket Conn)
        {


            Guest TempG = null;
            try
            {

                bool IsMicro = false;
                byte[] PublicKeyBytes = new byte[1024];
                int Keylength = Conn.Receive(PublicKeyBytes);
                string PublicKey = Encoding.UTF8.GetString(PublicKeyBytes, 0, Keylength);
                Conn.Send(RsaEncryption.getpublickey());


                byte[] recognition = new byte[128];
                Conn.Receive(recognition);
                string re = RsaEncryption.Decrypt(recognition);
                if (re == "Micro")
                    IsMicro = true;
                



                if (!IsMicro)
                {
                    byte[] bytes = new byte[128];
                    Conn.Receive(bytes);
                    string MotherBoardSerialNumber = "";
                    

                    try
                    {


                        MotherBoardSerialNumber = RsaEncryption.Decrypt(bytes);

                        bool NC = ServerServices.IsBadGuest(MotherBoardSerialNumber);
                        Guest g;
                        Thread.Sleep(200);

                        if (NC)
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
                        else
                        {
                            Conn.Send(Encoding.UTF8.GetBytes("Ok"));
                            g = new Guest(MotherBoardSerialNumber);
                            ServerServices.AddGuest(g);
                        }
                        TempG = g;

                    }
                    catch (Exception e)
                    {
                    }
                }


                string[] SessionAndMicroName;
                if (IsMicro)
                {
                    AesEncryprionForSession AEFS = new AesEncryprionForSession();
                    (byte[] aeskey, byte[] aesIv) = AEFS.GetMicroKeys(PublicKey);
                    Conn.Send(aeskey);
                    Thread.Sleep(400);
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

                        if (!IsMicroNameExist(SessionAndMicroName[0
                            ], SessionAndMicroName[1]))
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
                    
                    session session = new session(Conn, Code, SessionAndMicroName[1],PublicKey ,AEFS);
                    session.SetControllerKnickname(SessionAndMicroName[0]);
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
                            bool NewClientRequest = false;
                            string personalCode = "";
                            if (CodeAndKnickname[0] == "RESETPASS")
                            {
                                success = SqlService.RestPass(username, CodeAndKnickname[2], CodeAndKnickname[3]);
                            } else if (CodeAndKnickname[0] == "CONNECTOSESSION")
                            {
                                success = SqlService.LoginSql(username, CodeAndKnickname[2], false);
                                LoginRequest = true;
                            } else if (CodeAndKnickname[0] == "NEWCLIENT")
                            {

                                (success, personalCode) = SqlService.Register(username, CodeAndKnickname[2], false);
                                NewClientRequest = true;
                            }

                            if (success)
                            {
                                if (LoginRequest)
                                {
                                    if (sessoinSearch(Conn, CodeAndKnickname, PublicKey))
                                    {
                                        TempG.StopCount();
                                        ServerServices.RemoveGueast(TempG);
                                        return;
                                    }
                                    else
                                    {

                                        TempG.Log();
                                    }
                                }
                                else if (NewClientRequest)
                                {
                                    SendToClient(Conn, $"201&{personalCode}");
                                } else
                                {
                                    SendToClient(Conn, $"202&");
                                }
                            }
                            else
                            {
                                TempG.Log();
                                SendToClient(Conn, $"400&");
                            }

                            CodeAndKnickname = reciveIdentifiers(Conn);
                            new Thread(() => { MessageBox.Show(TempG.AvrageLogTime().ToString()); }).Start();
                            if (TempG.GetLogs() > 8 && (TempG.IsConssistent() || TempG.AvrageLogTime() < 4))
                            {
                                TempG.StopCount();
                                SendToClient(Conn, $"999&");
                                BadGuest BG = ServerServices.MakeGuestBad(TempG);
                                BlockedClient NewBad = new BlockedClient(BG);
                                SqlService.AddBlockedGuest(BG);
                                this.BeginInvoke(new Action(() => {
                                    BlockedClients.Controls.Add(NewBad);
                                }));
                                Conn.Close();
                                return;
                            }
                        
                        
                            

                        }
                        catch (Exception ex) {
                            SendToClient(Conn, $"800&");
                            TempG.StopCount();
                            ServerServices.RemoveGueast(TempG);
                            Conn.Close();
                            return;
                        }



                    }

                    while (true);

                }
                
                
            }
            catch (Exception e) { 
                Conn.Close();
                if (TempG != null)
                {
                    TempG.StopCount();
                    ServerServices.RemoveGueast(TempG);
                }
            }
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
                {
                    return new string[7];
                }
                
            }
            catch (Exception e) {
                return new string[7]; }
        }



        private bool sessoinSearch(Socket Conn,  string[] CodeAndKnickname, string publickey)
        {
            string username = CodeAndKnickname[1];
            string sessionCode = CodeAndKnickname[3];
            foreach (var Control in SessionsViewPanel.Controls)
            {
                if (((sessionLayot)Control).GetSession().GetClienKnickname() == username)
                {
                    SendToClient(Conn, "400&");
                    return false;
                }
            }
            if (sessionCode != "-OfflineSession-")
            {
                session DesiredSession = ServerServices.GetSession(sessionCode);

                
                if (DesiredSession == null || DesiredSession.ClientConnected())
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
                        if (((sessionLayot)Control).GetSession().GetCode() == sessionCode)
                            ((sessionLayot)Control).SetClientInLayout(DesiredSession);
                    }
                    return true;
                }
            } else
            {
                session OfflineSession = new session();
                SendToClient(Conn, "200&");
                OfflineSession.AddClient(Conn, "", publickey);
                return true;
            }

            
        }

        private bool IsMicroNameExist(string name, string sessionName)
        {
            foreach (session s in ServerServices.GetSessionsList())
            {
                string CurrentsessionName = s.GetSessionName();
                string CurrentmicroName = s.GetControllerKnickname();
                if (CurrentmicroName == name || CurrentsessionName == sessionName)
                    return true;
            }

            return false;
        }

        public void AddBlockedClientToPanel(BlockedClient c)
        {
            BlockedClients.Controls.Add(c);
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

        


        private void DisconnectServerButton_Click(object sender, EventArgs e)
        {
            ServerServices.CloseConnection();
            ClosingController.btnExit_Click();
        }

        private void ShutSessionsButton_Click(object sender, EventArgs e)
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
            ServerServices.CloseAllConnection();
            ClosingController.btnExit_Click();
        }


        
    }


    
}
