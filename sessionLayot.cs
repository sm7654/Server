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
            SessionName.Text = session.GetSessionName();
            this.ConnectedSession = session;
            session.SetSessionUi(this);
        }


        public void UpdateClientStatus_dis(string newcode)
        {
            (ClientIp.Text, ClientPort.Text) = ("____", "____");
            ClientKickname.Text = ("disconnected");
            BytesClient.Text = "0 bytes";
            BytesMicro.Text = "0 bytes";
            SessionCodeLabel.Text = newcode;

        }
        public void UpdateControllerStatus_dis()
        {
            (MicroIp.Text, MicroPort.Text) = ("____", "____");
            MicroName.Text = ("disconnected");
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ServerServices.removeSession(this.ConnectedSession);
            FormController.RemoveSession(this);
            this.ConnectedSession.disconnect();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void sessionLayot_Load(object sender, EventArgs e)
        {

        }

        private void sessionLayot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            SessionDisplayForm sessionDisplayForm = new SessionDisplayForm(this.ConnectedSession.GetSessionsRecords());
            sessionDisplayForm.ShowDialog();
        }
    }
}
