using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class SessionDisplayForm : Form
    {
        private List<SessionRecord> sessionRecords = new List<SessionRecord>();
        private uint TotalBytesFromMicro = 0;
        private bool FormOpen = true;



        public SessionDisplayForm(session SessionToRepresent)
        {
            InitializeComponent();
            if (SessionToRepresent == null)
                return;
            sessionName.Text = SessionToRepresent.GetSessionName();
            MicroName.Text = SessionToRepresent.GetControllerKnickname();
            sessionName.Location = new Point((HeaderPanel.Width - sessionName.Width)/2, (HeaderPanel.Height - sessionName.Height) / 2);
            this.sessionRecords = SessionToRepresent.GetSessionsRecords();

            ServerServices.setDisplayFrom(this);

            foreach (SessionRecord record in sessionRecords)
            {
                SessionRecordControl control = new SessionRecordControl(record);
                this.flowLayoutPanel1.Controls.Add(control);
            }
            new Thread(() => KeepMicroBytesUpdated(SessionToRepresent)).Start();
            
        }

        private void KeepMicroBytesUpdated(session S)
        {
            while (FormOpen)
            {
                if (TotalBytesFromMicro != S.GetTotalMicroBytes())
                {
                    TotalBytesFromMicro = S.GetTotalMicroBytes();
                    TotalBytesFromMicroLabel.Text = $"📦 Micro Total Dataflow: {ServerServices.MakeBytesString(TotalBytesFromMicro)}";
                }
                Thread.Sleep(10);

                
            }
        }
        public void CleanForm()
        {
            FormOpen = false;
            sessionRecords = null;
            TotalBytesFromMicro = 0;
            ServerServices.setDisplayFrom(null);
            this.flowLayoutPanel1.Controls.Clear();
        }
        
        public void AddSessoinToFrom(SessionRecord SR)
        {

            this.BeginInvoke(new Action(() =>
            {
                SessionRecordControl control = new SessionRecordControl(SR);
                this.flowLayoutPanel1.Controls.Add(control);
            }));

        }

        private void exitbutton_Click(object sender, EventArgs e)
        {
            CleanForm();
            this.Close();
        }

    }
}
