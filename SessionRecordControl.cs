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
            //הגדרה ראשונית של קונטרול בנתונים של שיחה
            this.NewClientLabel.Text = $"New Client - {SR.GetClientName()} - {SR.GetClientEndPoint()}";
            this.SessionCodelabel.Text = $"🔑 Session Code: {SR.GetSessionCode()}";
            this.ClientDataFlowlabel.Text = $"📦 Client Dataflow: {ServerServices.MakeBytesString(SR.GetClientData())}";
            this.MicroDataFlowlabel.Text = $"📦 Micro Dataflow: {ServerServices.MakeBytesString(SR.GetMicroData())}";
            this.StartDatelabel.Text = $"🗓️ Start Date: {SR.GetStartDate()}";
            this.ClientDisconnectedLabel.Text = $"";
            this.sessionEnterTime = SR.GetSessionDuration();
            this.SR = SR;
            //בדיקה האם מדובר בשיחה פעילה
            if (SR.IsRecordLive())
            {
                //התחלת תהליכונים חדשים להשוואת הנתונים הקיימים עם הנתונים העדכניים ועדכונם במקרה הצורך
                new Thread(RefreshMicroBytes).Start();
                new Thread(setClientBytes).Start();
                new Thread(setNewDuration).Start();
            }
            else
            {
                //במידה ומדובר בשיחה לא פעילה להציג את תאריך ההתנתקות של הלקוח ואת משך השיחה
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
            //בדיקה האם עדיין מדובר בשיחה פעילה
            while (SR.IsRecordLive())
            {
                // בדיקה האם הנתון הקיים שונה מהנתון המעודכן
                if (SR.GetClientData() != clientData)
                {
                    // עדכון המשתנה הנתון הקיים לנתון העדכני
                    clientData = SR.GetClientData();

                    // עדכון התצוגה על הטופס עם כמות הנתונים בפורמט קריא
                    this.ClientDataFlowlabel.Text = $"📦 Client Dataflow: {ServerServices.MakeBytesString(clientData)}";
                }

                // המתנה קצרה לפני הבדיקה הבאה כדי לא לבזבז משאבים 
                Thread.Sleep(10);
            }
        }





        public void setNewDuration()
        {
            //בדיקה האם עדיין מדובר בשיחה פעילה
            while (SR.IsRecordLive())
            {
                //חישוב משך השיחה והצגתו הפורמט של שעות-דקות-שניות בטופס
                this.SessionDurationlabel.Text = $"🕔 Session Duration: {ServerServices.CalcTime(ServerServices.GetTime() - this.sessionEnterTime)}";
                //דיליי של קצת פחות משניה בגלל הדיליי שנוצר בעקבות חישוב משךף השיחה והצגתו 
                Thread.Sleep(970);
            }
            //במידה ומדובר בשיחה לא פעילה יותר להציג שלקות התנתק מהשיחה עם תאריך מתאים
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
