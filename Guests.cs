using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ServerSide
{
    public class Guest
    {
        protected string MotherBoard_SN;
        private int logs;
        private double Timer;
        private bool Suspicius = false;
        List<double> gaps = new List<double>();
        private readonly object LockObject = new object();

        public Guest() { }

        


        public Guest(string SN)
        {
            logs = 1;
            Timer = 0;
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

        
        
        private void Count()
        {
            double tempTimer = 0;
            //נכנס רק אם הלקוח לא חשוד
            while (!Suspicius)
            {
                Thread.Sleep(10);
                tempTimer += 0.01;
                //מחשיב 10 שניות כניסון
                
                if (tempTimer > 10)
                {
                    //מוסיף את הזמן לזמן הכללי רשימת ההפרשים
                    Timer += tempTimer;
                    gaps.Add(tempTimer);
                    //מוסיף ניסיון כושל ומאפס את הטיימר
                    logs++;
                    tempTimer = 0;
                }
            }
            Timer += tempTimer;
            gaps.Add(tempTimer);
            logs++;
        }

        public virtual int Log()
        {
            lock (LockObject)
            {
                Suspicius = true;
                Thread.Sleep(10);
                Suspicius = false;
                new Thread(Count).Start();
                return 0;
            }
        }

        public double AvrageLogTime()
        {
            //חישוב ממוצע זמן של כל ניסיון
            double t = Timer / (logs);
            //החזרת הממוצע
            return t;
        }

        public void StopCount()
        {
            //לוודא שהנתונים עדכניים
            lock (LockObject)
            {
                //הגדרת מתשנה כאמת בכדי להפסיק את הטיימר
                Suspicius = true;
                //דיליי קטן כדי לוודא שהתיעוד של הפעולה המוצלחת תועד
                
            }
        }
        public void RestartCount()
        {
            Suspicius = false;
            new Thread(Count).Start();
        }

        public bool IsConssistent()
        {
            //לוודא שהנתונים עדכניים
            lock (LockObject)
            {
                //חישוב הפרש ממוצע
                double AvrageGap = Timer / gaps.Count;
                //הגדרת מתשנה המייצג את כמות הפעמים שהוהת עקביות
                double FoundCon = 0;
                //ריצה על הכל ההפרשים
                foreach (double gap in gaps)
                {
                    //בדיקת עקביות
                    if ((-1.5 <= gap - AvrageGap) && (1.5 >= gap - AvrageGap))
                    {
                        //אם נמצא עקביות מגדיל את משתנה עקביות-נמצא באחד
                        FoundCon++;
                    }
                    //בודקת אם עד עכשיו כמות הפעמים שנמצא קביעות בהפרשים גדולה גדולה או שווה ל75% מכמות ההפרשים
                    if (FoundCon / gaps.Count >= 0.75)
                    {
                        //מחזירה אחת אם כן
                        return true;
                    }
                }
                //אם לא נמצא עקביות מחזירה שקר
                
                return false;
            }
        }
    }
}
