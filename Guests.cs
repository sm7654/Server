using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerSide
{
    internal class Guest
    {
        protected string ip;
        private int logs;
        private double Timer;
        private bool HasEntered = false;
        List<double> gaps = new List<double>();
        private readonly object LockObject = new object();

        // Default constructor
        public Guest() { }

        public Guest(Guest ForgainGuest)
        {
            // Set all the variables of the current object (this) to match the passed object (ForgainGuest)
            this.ip = ForgainGuest.GetIp();
            this.logs = ForgainGuest.GetLogs();
            this.Timer = ForgainGuest.GetTimer();
            this.HasEntered = ForgainGuest.GetHasEntered();
            this.gaps = new List<double>(ForgainGuest.GetGaps());  // Make sure we create a new list to avoid reference issues
        }


        // Constructor with IP address
        public Guest(string ip)
        {
            logs = 1;
            Timer = 0;
            new Thread(Count).Start();
            this.ip = ip;
        }

        // Getter for ip
        public string GetIp()
        {
            return this.ip;
        }

        // Setter for ip
        public void SetIp(string ipAddress)
        {
            this.ip = ipAddress;
        }

        // Getter for logs
        public int GetLogs()
        {
            return this.logs;
        }

        // Setter for logs
        public void SetLogs(int logCount)
        {
            this.logs = logCount;
        }

        // Getter for Timer
        public double GetTimer()
        {
            return this.Timer;
        }

        // Setter for Timer
        public void SetTimer(double time)
        {
            this.Timer = time;
        }

        // Getter for HasEntered
        public bool GetHasEntered()
        {
            return this.HasEntered;
        }

        // Setter for HasEntered
        public void SetHasEntered(bool hasEntered)
        {
            this.HasEntered = hasEntered;
        }

        // Getter for gaps
        public List<double> GetGaps()
        {
            return this.gaps;
        }

        // Setter for gaps
        public void SetGaps(List<double> gapList)
        {
            this.gaps = gapList;
        }

        // Function that counts the time and updates the gaps
        private void Count()
        {
            double tempTimer = 0;
            while (!HasEntered)
            {
                Thread.Sleep(100);
                tempTimer += 0.1;
                if (tempTimer > 10)
                {
                    Timer += tempTimer;
                    tempTimer = 0;
                }
            }
            gaps.Add(tempTimer);
            Timer += tempTimer;
        }

        // Logs the event and creates a new thread to count time
        public virtual void Log()
        {
            lock (LockObject)
            {
                logs++;
                HasEntered = true;
                Thread.Sleep(100);
                HasEntered = false;
                new Thread(Count).Start();
            }
        }

        // Returns the average log time
        public double AvrageLogTime()
        {
            lock (LockObject)
            {
                HasEntered = true;
                double t = Timer / (logs);
                Thread.Sleep(100);
                HasEntered = false;
                return t;
            }
        }

        public bool IsConssistent()
        {
            lock (LockObject)
            {
                double tempgap = 0;
                foreach (double item in gaps)
                    tempgap += item;

                double AvrageGap = tempgap / gaps.Count;
                double FoundCon = 0;
                foreach (double item in gaps)
                {
                    if ((-1 <= item - AvrageGap) && (1 >= item - AvrageGap))
                    {
                        FoundCon++;
                    }
                    if (FoundCon / gaps.Count > 0.5)
                        return true;
                }

                return false;
            }
        }
    }
}
