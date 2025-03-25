using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    public class BlackGuest : Guest
    {
        private int ConnectAttemps = 0;
        public BlackGuest(Guest g) : base(g)
        {

        }
        public BlackGuest() : base()
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
