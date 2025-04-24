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
    public partial class SessionDisplayForm : Form
    {
        private List<SessionRecord> sessionRecords = new List<SessionRecord>();
        public SessionDisplayForm(List<SessionRecord> sessionRecords)
        {
            InitializeComponent();
            this.sessionRecords = sessionRecords;

            foreach (SessionRecord record in sessionRecords)
            {
                SessionRecordControl control = new SessionRecordControl(record);
                this.SessionsRecordsPanel.Controls.Add(control);
            }
        }
    }
}
