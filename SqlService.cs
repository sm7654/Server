using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ServerSide
{
    static class SqlService
    {
        private static SqlConnection SqlConnection;
        private static string ConnectionString = $@"Server={Environment.MachineName};Database=DataBase_Windtunnel;Trusted_Connection=True;";



        public static string GenerateRandomString(int length)
        {
            string code;
            do
            {
                Random _random = new Random();
                const string chars = "a2QjWz3n0v1p7Gm5kI9oXebVd4yHcL6f8sT";
                char[] stringChars = new char[length];

                for (int i = 0; i < length; i++)
                {
                    stringChars[i] = chars[_random.Next(chars.Length)];
                }
                code = new string(stringChars);
            } while (!ServerServices.Avalible(code));

            return code;
        }





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
            string command;
            user = Hash(user);
            pass = Hash(pass);
            try
            {
                if (Ismaneger)
                {
                    command = $"SELECT * FROM ManagementUsers WHERE password = '{pass}' AND username = '{user}';";
                } else
                {
                    command = $"SELECT * FROM ClientUsers WHERE clientPassword = '{pass}' AND clientName = '{user}';";
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

        public static bool RestPass(string User, string newPass, string personalCode,  bool Ismaneger)
        {
            try
            {
                newPass = Hash(newPass);
                User = Hash(User);
                personalCode = Hash(personalCode);
                string CheckCommand = $"SELECT * FROM ClientUsers WHERE clientPassword = '{newPass}';";

                SqlCommand builder = new SqlCommand(CheckCommand, SqlConnection);
                SqlDataReader results = builder.ExecuteReader();
                if (results.Read())
                    return false;
                results.Close();
                string command = $"UPDATE ClientUsers SET clientPassword = '{newPass}' WHERE clientName = '{User}' AND personalCode = '{personalCode}'";
                builder.CommandText = command;
                int rowsEffected = builder.ExecuteNonQuery();
                if (rowsEffected > 0)
                    return true;
                return false;

            } catch (Exception ex)
            { return false;
            }

        }
        private static bool CheckIfUserOrPassValid(string User, string pass, bool Ismaneger)
        {
            string commandForPass;
            string commandForUser;
            User = Hash(User);
            pass = Hash(pass);
            try
            {
                if (Ismaneger)
                {
                    commandForPass = $"SELECT * FROM ManagementUsers WHERE password = '{pass}';";
                    commandForUser = $"SELECT * FROM ManagementUsers WHERE username = '{User}';";
                }
                else
                {
                    commandForPass = $"SELECT * FROM ClientUsers WHERE clientPassword = '{pass}';";
                    commandForUser = $"SELECT * FROM ClientUsers WHERE clientName = '{User}';";
                }
                SqlCommand builder = new SqlCommand(commandForPass, SqlConnection);
                SqlDataReader results = builder.ExecuteReader();

                if (results.Read())
                {
                    results.Close();
                    return false;
                }
                results.Close();


                builder = new SqlCommand(commandForUser, SqlConnection);
                results = builder.ExecuteReader();

                if (results.Read())
                {
                    results.Close();
                    return false;
                }
                results.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); return false;
            }


            return true;

        }

        public static (bool, string) Register(string User, string pass, bool Ismaneger)
        {
            string command;

            string personalCode = "";
            if (!CheckIfUserOrPassValid(User, pass, Ismaneger))
                return (false, "");

            User = Hash(User);
            pass = Hash(pass);
            try
            {
                SqlDataReader results;
                SqlCommand builder;
                do
                {

                    personalCode = GenerateRandomString(5);
                    string CheckCommand = $"SELECT * FROM ClientUsers WHERE personalCode = '{Hash(personalCode)}';";
                    builder = new SqlCommand(CheckCommand, SqlConnection);
                    results = builder.ExecuteReader();
                } while (results.Read());
                results.Close();

                if (Ismaneger)
                    command = $"INSERT INTO ManagementUsers (username, password) VALUES ('{User}', '{pass}')";
                else
                    command = $"INSERT INTO ClientUsers (clientName, clientPassword, personalCode) VALUES ('{User}', '{pass}', '{Hash(personalCode)}')";


                builder = new SqlCommand(command, SqlConnection);
                int rowsEffected = builder.ExecuteNonQuery();
                if (rowsEffected > 0)
                {
                    return (true, personalCode);
                }
                return (false, "");
            }
            catch (Exception ex) {
                return (false, ""); }
        }


        public static void AddExperimentToDatabase(string resultsData, string username, string Time)
        {
            try
            {
                string command = $"INSERT INTO Experiments (username, CreationString, TimeCreated, expername) VALUES ('{Hash(username)}','{resultsData}', '{Time}', '{resultsData.Split(';')[resultsData.Split(';').Length - 1]}')";
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

                string username = Hash(request.Split('&')[2]);
                string[] Filtters = request.Split('&')[3].Split(',');

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

                    if (item.Contains(">="))
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
            string UserFillterVal = item.Replace($"{Oparation}", " ").Split(' ')[1];

            foreach (string ExperResult in CreationString.Split(';'))
            {
                try
                {
                    string ExperResultVal = ExperResult.Split(':')[1].Split('|')[0];
                    if (Result == "Name" && ExperResult.Contains("Name"))
                    {
                        if (UserFillterVal == ExperResultVal)
                            return true;
                        return false;
                    }

                    if (ExperResult.Contains(Result))
                    {

                        double ExperVal = double.Parse(ExperResultVal);
                        double UserFillter = double.Parse(UserFillterVal);

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
                }
                catch (Exception e)
                {
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
