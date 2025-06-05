using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ServerSide
{
    public partial class StartServerForm : Form
    {
        public StartServerForm()
        {
            InitializeComponent();

            
        }


        public bool StartServer()
        {
            

            try
            {


                Socket ServerSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                int port = 65000;

                IPEndPoint Ep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);

                ServerSock.Bind(Ep);


                RsaEncryption.GenerateKeys();
                AesEncryption.GenerateTempKeys();
                ServerConnectedForm OnlineServerForm = new ServerConnectedForm(ServerSock);

                OnlineServerForm.Show();
                
                return true;
            }
            catch (SocketException ex)
            {
                return false;
            }

        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            bool Status = StartServer();
            if (Status)
                this.Hide();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosingController.btnExit_Click();
        }
    }
}
