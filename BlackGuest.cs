using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    internal class BlackGuest : Guest
    {

        public BlackGuest(Guest g) : base(g)
        {

        }
        public BlackGuest() : base()
        {

        }


        public override void Log()
        {
            return;
        }
    }
}
