using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class SessionRecordControl : UserControl
    {
        private string ClientString = "";
        private SessionRecord SR;

        private uint microData = 0;
        private uint clientData = 0;
        private uint sessionEnterTime = 0;

        public SessionRecordControl()
        {
            InitializeComponent();
        }
        public SessionRecordControl(SessionRecord SR)
        {
            InitializeComponent();
            this.NewClientLabel.Text = $"New Client - {SR.GetClientName()} - {SR.GetClientEndPoint()}";
            this.SessionCodelabel.Text = $"🔑 Session Code: {SR.GetSessionCode()}";
            this.ClientDataFlowlabel.Text = $"📦 Client Dataflow: {ServerServices.MakeBytesString(SR.GetClientData())}";
            this.MicroDataFlowlabel.Text = $"📦 Micro Dataflow: {ServerServices.MakeBytesString(SR.GetMicroData())}";
            this.StartDatelabel.Text = $"🗓️ Start Date: {SR.GetStartDate()}";
            this.ClientDisconnectedLabel.Text = $"";
            this.sessionEnterTime = SR.GetSessionDuration();
            this.SR = SR;
            if (SR.IsRecordLive())
            {
                Thread microThread = new Thread(RefreshMicroBytes);
                Thread clientThread = new Thread(setClientBytes);
                Thread durationThread = new Thread(setNewDuration);


                microThread.Start();
                clientThread.Start();
                durationThread.Start();
            }
            else
            {
                this.ClientDisconnectedLabel.Text = $"{ClientString} disconnected at {SR.GetEndDate()}";
                this.SessionDurationlabel.Text = $"🕔 Session Duration: {ServerServices.CalcTime(SR.GetSessionTotalTime())}";
                
            }
        }





        public void RefreshMicroBytes()
        {
            while (SR.IsRecordLive())
            {
                if (SR.GetMicroData() != microData)
                {
                    microData = SR.GetMicroData();
                    this.MicroDataFlowlabel.Text = $"📦 Micro Dataflow: {ServerServices.MakeBytesString(microData)}";
                }
                Thread.Sleep(10);
            }
        }
        public void setClientBytes()
        {
            while (SR.IsRecordLive())
            {
                if (SR.GetClientData() != clientData)
                {
                    clientData = SR.GetClientData();
                    this.ClientDataFlowlabel.Text = $"📦 Client Dataflow: {ServerServices.MakeBytesString(clientData)}";
                }
                Thread.Sleep(10);
            }
        }
        public void setNewDuration()
        {
            while (SR.IsRecordLive())
            {
                this.SessionDurationlabel.Text = $"🕔 Session Duration: {ServerServices.CalcTime(ServerServices.GetTime() - this.sessionEnterTime)}";
                Thread.Sleep(970);
            }
            DisconnectClient();
        }
        public void DisconnectClient()
        {
            this.ClientDisconnectedLabel.Text = $"{ClientString} disconnected at {DateTime.Now.ToString()}";
        }
        private void SessionDetailsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StartDatelabel_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SessionDurationlabel_Click(object sender, EventArgs e)
        {

        }
    }
}
