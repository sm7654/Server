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
        public SessionDisplayForm(session SessionToRepresent)
        {
            InitializeComponent();
            if (SessionToRepresent == null)
                return;
            sessionName.Text = SessionToRepresent.GetSessionName();
            MicroName.Text = SessionToRepresent.GetControllerKnickname();
            sessionName.Location = new Point((HeaderPanel.Width - sessionName.Width)/2, (HeaderPanel.Height - sessionName.Height) / 2);
            this.sessionRecords = SessionToRepresent.GetSessionsRecords();

            foreach (SessionRecord record in sessionRecords)
            {
                SessionRecordControl control = new SessionRecordControl(record);
                this.flowLayoutPanel1.Controls.Add(control);
            }
            new Thread(new ThreadStart(() => KeepMicroBytesUpdated(SessionToRepresent))).Start();
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void KeepMicroBytesUpdated(session S)
        {
            while (true)
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
            sessionRecords = null;
            TotalBytesFromMicro = 0;
            this.flowLayoutPanel1.Controls.Clear();
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void exitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
