using System;

namespace ServerSide
{
    public class BadGuest : Guest
    {
        private int ConnectAttemps = 0;
        private string blockeDate;
        public BadGuest(Guest g) : base(g.GEt_MotherBoard_SN())
        {
            blockeDate = DateTime.Now.GetDateTimeFormats().ToString();
        }
        public BadGuest() : base()
        {
        }
        public BadGuest(string SR, string DT) : base(SR)
        {
            blockeDate=DT;
        }
        public string GetBlockedDate()
        {
            return blockeDate;
        }
        public override int Log()
        {
            ConnectAttemps++;
            return ConnectAttemps;
        }
    }
}
