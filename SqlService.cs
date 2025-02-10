using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.SqlServer.Server;

namespace ServerSide
{
    static class SqlService
    {
        private static SqlConnection SqlConnection;
        private static string ConnectionString = @"Server=DESKTOP-RJMPR2D;Database=DataBase_Windtunnel;Trusted_Connection=True;";
        

        public static bool ConnectToSql()
        {
            try
            {
                Console.WriteLine("Connecting to SQL server...");

                SqlConnection = new SqlConnection(ConnectionString);
                SqlConnection.Open();
                return true;
            }
            catch (SqlException ex) {
                return false; }
        }

        public static bool LoginSql(string user, string pass, bool Ismaneger)
        {
            //return true;
            string command;
            user = Hash(user);
            pass = Hash(pass);
            try
            {
                if (Ismaneger)
                {
                    command = $"SELECT * FROM ManagementUsers WHERE password = '{pass}' OR username = '{user}';";
                } else
                {
                    command = $"SELECT * FROM ClientUsers WHERE clientPassword = '{pass}' OR clientName = '{user}';";
                }
                SqlCommand builder = new SqlCommand(command, SqlConnection);
                SqlDataReader results = builder.ExecuteReader();

                if (results.Read())
                {
                    results.Close();
                    return true;
                }
                results.Close();
                return false;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message); return false; }
        }


        public static bool Register(string User, string pass, bool Ismaneger)
        {
            string command;

            pass = Hash(pass);
            User = Hash(User);
            if (LoginSql(User, pass, Ismaneger))
                return false;
            try
            {
                if (Ismaneger)
                    command = $"INSERT INTO ManagementUsers (username, password) VALUES ('{User}', '{pass}')";
                else
                    command = $"INSERT INTO ClientUsers (clientName, clientPassword) VALUES ('{User}', '{pass}')";


                SqlCommand builder = new SqlCommand(command, SqlConnection);
                int rowsEffected = builder.ExecuteNonQuery();
                if (rowsEffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                return false; }
        }


        public static void AddExperimentToDatabase(string resultsData, string username, string Time)
        {
            try
            {
                string CreationString = resultsData.Substring(ServerServices.GetServerRole().Length + 1);
                string command = $"INSERT INTO Experiments (username, CreationString, TimeCreated, expername) VALUES ('{Hash(username)}','{CreationString}', '{Time}', '{resultsData.Split(';')[resultsData.Split(';').Length - 1]}')";
                SqlCommand builder = new SqlCommand(command, SqlConnection);
                int rowsEffected = builder.ExecuteNonQuery();
            }
            catch (Exception e) {
                return;
            }
        }


        public static string GetExpererimentOfUser(string request)
        {
            try
            {

                string username = Hash(request.Split(';')[2]);
                string[] Filtters = request.Split(';')[3].Split(',');

                string command = $"SELECT * FROM Experiments WHERE username = '{username}';";

                string hh = "";
                SqlCommand sqlCommand = new SqlCommand(command, SqlConnection);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string CreationString = GetCreationStringIfExperStandConditions(Filtters, reader.GetString(2));
                        if (CreationString != "")
                            hh += $"&{CreationString}";
                    }
                    return hh;
                }

            }
            catch (Exception ex) 
            { }

            return "";
        }

        private static string GetCreationStringIfExperStandConditions(string[] Filtters, string CreationString)
        {
            try
            {

                foreach (string item in Filtters)
                {
                    if (item.Contains("expername"))
                    {
                        if (item.Split('=')[1] != CreationString.Split(';')[CreationString.Split(';').Length - 1])
                            return "";
                    }
                    else if (item.Contains(">="))
                    {
                        if (!DoesExperStandConditions(">=", item, CreationString))
                            return "";
                    }
                    else if (item.Contains("<="))
                    {
                        if (!DoesExperStandConditions("<=", item, CreationString))
                            return "";
                    }
                    else if (item.Contains("="))
                    {
                        if (!DoesExperStandConditions("=", item, CreationString))
                            return "";
                    }
                    else if (item.Contains(">"))
                    {
                        if (!DoesExperStandConditions(">", item, CreationString))
                            return "";
                    }
                    else if (item.Contains("<"))
                    {
                        if (!DoesExperStandConditions("<", item, CreationString))
                            return "";
                    }
                    else if (item.Contains("!="))
                    {
                        if (!DoesExperStandConditions("!=", item, CreationString))
                            return "";
                    }

                }

                return CreationString;

            }
            catch (Exception e) { 
                return ""; }


        }
        private static bool DoesExperStandConditions(string Oparation, string item, string CreationString)
        {
            string Result = item.Replace($"{Oparation}", " ").Split(' ')[0];
            string Val = item.Replace($"{Oparation}", " ").Split(' ')[1];

            foreach (string ExperResult in CreationString.Split(';'))
            {
                if (ExperResult.Contains(Result))
                {
                    try
                    {
                        double ExperVal = double.Parse(ExperResult.Split(':')[1].Split('|')[0]);
                        double UserFillter = double.Parse(Val);

                        switch (Oparation)
                        {
                            case "<=":
                                if (ExperVal > UserFillter)
                                    return false;
                                break;

                            case "<":
                                if (ExperVal > UserFillter || ExperVal == UserFillter)
                                    return false;
                                break;
                            case ">=":
                                if (ExperVal < UserFillter)
                                    return false;
                                break;
                            case ">":
                                if (ExperVal < UserFillter || ExperVal == UserFillter)
                                    return false;
                                break;
                            case "=":
                                if (ExperVal != UserFillter)
                                    return false;
                                break;

                            case "!=":
                                if (ExperVal == UserFillter)
                                    return false;
                                break;

                            default: return false;
                        }



                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            }
            return true;
        }






        public static List<string> GetAllUserHistoryAndSendToClient(string username)
        {
            try
            {

                if (username == "")
                    return null;

                List<string> CreationStrings = new List<string>();
                username = Hash(username);
                string command = $"SELECT * FROM Experiments WHERE username = '{username}';";

                SqlCommand sqlCommand = new SqlCommand(command, SqlConnection);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CreationStrings.Add(reader.GetString(2));
                    }
                }

                return CreationStrings;
            } catch (Exception e) { 
                return null; }

        }








        public static bool IsConnected()
        {
            if (SqlConnection == null)
                return true;
            return false;
        }


        private static string Hash(string pass)
        {
            SHA1 sHA1 = SHA1.Create();
            byte[] hashedPass = sHA1.ComputeHash(Encoding.UTF8.GetBytes(pass));

            string gg = Convert.ToBase64String(hashedPass);

            return gg;
        }

    }
}
