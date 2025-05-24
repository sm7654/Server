using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerSide
{
    internal static class Program
    {











        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            (string key, string iv) = ServerServices.GetKeysFromCredential();
            if (key != "" && iv != "")
            {
                AesEncryption.SetAESENVRkeys(key, iv);
            }    


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SqlConnectionForm());
        }
    }
}
