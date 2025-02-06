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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        




        public bool StartServer()
        {
            

            try
            {


                Socket ServerSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                int port = 65000;// port forwarding - 65000

                IPEndPoint Ep = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);

                ServerSock.Bind(Ep); /*  */

                //new Thread(() => portProvider(ServerSock)).Start);


                RsaEncryption.GenerateKeys();
                ServerConnectedForm OnlineServerForm = new ServerConnectedForm(ServerSock);

                OnlineServerForm.Show();
                
                return true;
            }
            catch (SocketException ex)
            {
                return false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool Status = StartServer();
            if (Status)
                this.Hide();

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ServerServices.CloseConnection();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        
    }
}
