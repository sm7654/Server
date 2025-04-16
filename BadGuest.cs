namespace ServerSide
{
    public class BadGuest : Guest
    {
        private int ConnectAttemps = 0;
        public BadGuest(Guest g) : base(g)
        {

        }
        public BadGuest() : base()
        {

        }
        public int AddAttempt()
        {
            ConnectAttemps++;
            return ConnectAttemps;
        }

        public override void Log()
        {
            return;
        }
    }
}
