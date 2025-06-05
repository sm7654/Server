using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    static class FormController
    {
        private static ServerConnectedForm form;

        public static void SetForm(ServerConnectedForm f)
        {
            form = f;
        }


        public static void RemoveSession(sessionLayot SL)
        {
            try
            {
                form.BeginInvoke(new Action(() => {

                    form.GetSessionLayoutControls().Remove(SL);
                }));
            }
            catch (Exception e)
            {
                return;
            }
        }
        public static void RemoveSession(session s)
        {
            try
            {
                form.BeginInvoke(new Action(() => { 
                    foreach (var Control in form.GetSessionLayoutControls())
                    {
                        if (((sessionLayot)Control).GetSession().Equals(s))
                            form.GetSessionLayoutControls().Remove(((sessionLayot)Control));
                    }
                }));
            }
            catch (Exception e)
            {
                MessageBox.Show("Cant remove the session");
                return;
            }
        }
        public static void disconnectClient(session s, string newcode)
        {
            foreach (var Control in form.GetSessionLayoutControls())
            {
                if (((sessionLayot)Control).GetSession().Equals(s))
                {
                    ((sessionLayot)Control).BeginInvoke(new Action(() =>
                    {
                        ((sessionLayot)Control).UpdateClientStatus_dis(newcode);
                    }));
                    
                }
            }
        }


        public static void AddBlockedGuest(BlockedClient c)
        {
            form.BeginInvoke(new Action(() => {
                form.AddBlockedClientToPanel(c);
            }));
        }

    }
}
