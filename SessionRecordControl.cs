using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class SessionRecordControl : UserControl
    {
        public SessionRecordControl(SessionRecord SR)
        {
            InitializeComponent();
            this.NewClientLabel.Text = $"New Client - {SR.GetClientName()} - {SR.GetClientEndPoint()}";
            this.SessionCodelabel.Text = $"🔑 Session Code: {SR.GetSessionCode()}";
            this.SessionDurationlabel.Text = $"🕔 Session Duration: {SR.GetSessionDuration()}";
            this.ClientDataFlowlabel.Text = $"📦 Client Dataflow: {SR.GetClientDataString()}";
            this.MicroDataFlowlabel.Text = $"📦 Client Dataflow: {SR.GetMicroDataString()}";
            this.StartDatelabel.Text = $"🗓️ Start Date: {SR.GetStartDate()}";
            this.ClientDisconnectedLabel.Text = $"{SR.GetClientName()} disconnected at {SR.GetEndDate()}";
        }

        private void SessionDetailsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StartDatelabel_Click(object sender, EventArgs e)
        {

        }
    }
}
