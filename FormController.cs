using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    static class FormController
    {
        private static ServerConnectedForm form;

        public static void SetForm(ServerConnectedForm f)
        {
            form = f;
        }


        public static void disconnectClient(session s)
        {
            foreach (var Control in form.getSessionLayoutControls())
            {
                if (((sessionLayot)Control).GetSession().Equals(s))
                    ((sessionLayot)Control).UpdateClientStatus_dis();
            }
        }

    }
}
