using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public class SessionRecord
    {
        private string sessionCode;
        private string clientEndPoint;
        private string clientKicname;
        private uint microData = 0;
        private uint clientData = 0;
        private uint sessionEnterTime = 0;
        private uint sessionTotalTime;
        private string startDate;
        private string endDate;


        private bool IsLiveRecord = true;


        public SessionRecord(string sessionCode, string clientEndPoint, string clientname, uint clientData, uint microData, uint EnterTime, string startDate)
        {
            this.sessionCode = sessionCode;
            this.clientEndPoint = clientEndPoint;
            this.clientKicname = clientname;
            this.microData = microData;
            this.clientData = clientData;
            this.sessionEnterTime = EnterTime;
            this.startDate = DateTime.Now.ToString();
        }

 
        public void SetSessionDuration(uint sessionDuration)
        {
            this.sessionEnterTime = sessionDuration;
        }

        

        public void SetBytesToMicro(uint b)
        {
            this.microData = b;
        }
        public void SetBytesToClient(uint b)
        {
            this.clientData = b;
        }


        public void MakeNotLiveRecored()
        {
            //הפיכת שיחה ללא פעילה
            IsLiveRecord = false;
            this.endDate = DateTime.Now.ToString();
            this.sessionTotalTime = ServerServices.GetTime() - sessionEnterTime;
        }






        public bool IsRecordLive()
        {
            return this.IsLiveRecord;
        }
        public string GetSessionCode() { return sessionCode; }

        public string GetClientName() { return clientKicname;}

        public string GetClientEndPoint() { return clientEndPoint; }

        public uint GetMicroData() { return microData; }

        public uint GetClientData() { return clientData; }

        public uint GetSessionDuration() { return sessionEnterTime; }

        public string GetStartDate() { return startDate; }

        public string GetEndDate() { return endDate; }
        public uint GetSessionTotalTime()
        {
            return sessionTotalTime;
        }
    }
}
