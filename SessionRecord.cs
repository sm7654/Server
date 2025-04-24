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
        private string microDataString;
        private string clientDataString;
        private string sessionDuration;

        private string startDate;
        private string endDate;

        public SessionRecord(string sessionCode, string clientEndPoint, string clientname)
        {
            this.sessionCode = sessionCode;
            this.clientEndPoint = clientEndPoint;
            this.clientKicname = clientname;
        }

        public void SetSessionDetails(string microDataString, string clientDataString, string sessionDuration, string startDate)
        {
            this.microDataString = microDataString;
            this.clientDataString = clientDataString;
            this.sessionDuration = sessionDuration;
            this.startDate = startDate;
            this.endDate = DateTime.Now.ToString();

            string message =
           $"Session Code: {sessionCode}\n" +
           $"Client Endpoint: {clientEndPoint}\n" +
           $"Client Nickname: {clientKicname}\n" +
           $"Micro Data: {microDataString}\n" +
           $"Client Data: {clientDataString}\n" +
           $"Session Duration: {sessionDuration}\n" +
           $"Start Date: {startDate}\n" +
           $"End Date: {endDate}";

            MessageBox.Show(message, "Session Record Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Temp()
        {
            Console.WriteLine("feweffew");
        }
        public string GetSessionCode() { return sessionCode; }

        public string GetClientName() { return clientKicname;}

        public string GetClientEndPoint() { return clientEndPoint; }

        public string GetMicroDataString() { return microDataString; }

        public string GetClientDataString() { return clientDataString; }

        public string GetSessionDuration() { return sessionDuration; }

        public string GetStartDate() { return startDate; }

        public string GetEndDate() { return endDate; }
    }
}
