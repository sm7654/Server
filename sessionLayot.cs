using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class sessionLayot : UserControl
    {
        session ConnectedSession;
        public sessionLayot()
        {
            InitializeComponent();
        }
        public sessionLayot(session session)
        {

            InitializeComponent();
            SessionCodeLabel.Text = session.GetCode();
            SessionName.Text = $"Session";
            MicroName.Text = session.GetControllerKnickname();
            (MicroIp.Text, MicroPort.Text) = session.GetControllerEndPoint();
            (ClientIp.Text, ClientPort.Text) = ("not connected", "not connected");
            ClientKickname.Text = ("not provided");
            
            this.ConnectedSession = session;//
        }


        public void UpdateClientStatus_dis()
        {
            (ClientIp.Text, ClientPort.Text) = ("____", "____");
            ClientKickname.Text = ("disconnected");

        }



        public session GetSession()
        {
            return ConnectedSession;
        }
        public void SetClientInLayout(session Session)
        {
            (ClientIp.Text, ClientPort.Text) = Session.GetCLientEndPoint();
            ClientKickname.Text = Session.GetClienKnickname();
        }
        public System.Windows.Forms.Label GetClientIpLabels()
        {
            return ClientIp;
        }
        public System.Windows.Forms.Label GetClientPortLabels()
        {
            return ClientPort;
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ConectionTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sessionLayot_Load(object sender, EventArgs e)
        {

        }
    }
}
