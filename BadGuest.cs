using System;

namespace ServerSide
{
    public class BadGuest : Guest
    {
        private int ConnectAttemps = 0;
        private string blockeDate;
        public BadGuest(Guest g) : base(g)
        {
            blockeDate = DateTime.Now.GetDateTimeFormats().ToString();
        }
        public BadGuest() : base()
        {
        }
        public BadGuest(string SR, string DT) : base(SR, false)
        {
            blockeDate=DT;
        }
        public int AddAttempt()
        {
            ConnectAttemps++;
            return ConnectAttemps;
        }
        public string GetBlockedDate()
        {
            return blockeDate;
        }
        public override void Log()
        {
            return;
        }
    }
}
