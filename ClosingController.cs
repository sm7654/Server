using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    static class ClosingController
    {
        public static void btnExit_Click()
        {
            string SolutionName = Assembly.GetExecutingAssembly().GetName().Name;
            Process[] processes = Process.GetProcessesByName(SolutionName);
            foreach (Process process in processes)
            {
                try { process.Kill(); } catch { }
            }
        }
    }
}
