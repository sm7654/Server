using System.Collections.Generic;
using System.Threading;

namespace ServerSide
{
    public class Guest
    {
        protected string MotherBoard_SN;
        private int logs;
        private double Timer;
        private bool HasEntered = false;
        List<double> gaps = new List<double>();
        private readonly object LockObject = new object();

        public Guest() { }

        public Guest(Guest ForgainGuest)
        {
            this.MotherBoard_SN = ForgainGuest.GEt_MotherBoard_SN();
            this.logs = ForgainGuest.GetLogs();
            this.Timer = ForgainGuest.GetTimer();
            this.HasEntered = ForgainGuest.GetHasEntered();
            this.gaps = new List<double>(ForgainGuest.GetGaps()); 
        }


        public Guest(string SN, bool RegularConnection)
        {
            logs = 1;
            Timer = 0;
            if (RegularConnection)
                new Thread(Count).Start();
            this.MotherBoard_SN = SN;
        }

        public string GEt_MotherBoard_SN()
        {
            return this.MotherBoard_SN;
        }

        public void Set_MotherBoard_SN(string SN)
        {
            this.MotherBoard_SN = SN;
        }

        public int GetLogs()
        {
            return this.logs;
        }

        public void SetLogs(int logCount)
        {
            this.logs = logCount;
        }

        public double GetTimer()
        {
            return this.Timer;
        }

        public void SetTimer(double time)
        {
            this.Timer = time;
        }

        public bool GetHasEntered()
        {
            return this.HasEntered;
        }

        public void SetHasEntered(bool hasEntered)
        {
            this.HasEntered = hasEntered;
        }

        public List<double> GetGaps()
        {
            return this.gaps;
        }

        public void SetGaps(List<double> gapList)
        {
            this.gaps = gapList;
        }

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

        public void StopCount()
        {
            HasEntered = true;
        }
        public void RestartCount()
        {
            HasEntered = false;
            new Thread(Count).Start();
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
