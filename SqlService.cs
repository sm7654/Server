    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Linq;

    namespace ServerSide
    {
        static class SqlService
        {
            private static SqlConnection SqlConnection;
            private static string ConnectionString = "";



            public static bool ConnectToSql(string name, string pass, string db)
            {
                try
                {
                    Console.WriteLine("Connecting to SQL server...");
                    string ConnectionString = $"Server={Environment.MachineName};Database={db};User ID={name};Password={pass};MultipleActiveResultSets=True;";

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
                pass = Hash(pass, GetScramblerOfUser(user, Ismaneger));
                try
                {
                    if (Ismaneger)
                    {
                        command = $"SELECT * FROM ManagementUsers WHERE password = @pass AND username = @user;";
                    } else
                    {
                        command = $"SELECT * FROM ClientUsers WHERE clientPassword = @pass AND clientName = @user;";
                    }
                
                
                    using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@pass", pass);
                        builder.Parameters.AddWithValue("@user", user);
                        using (SqlDataReader results = builder.ExecuteReader())
                        {
                            if (results.Read())
                            {
                                results.Close();
                                return true;
                            }
                            results.Close();
                            return false;
                        }
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return false; 
                }
            }

            public static bool IsUsernameAvailable(string user, bool ismeneger)
            {
                string command;

                try
                {
                    if (ismeneger)
                        command = "SELECT * FROM ManagementUsers WHERE username = @user;";
                
                    else
                        command = "SELECT * FROM ClientUsers WHERE clientName = @user;";
               

                    using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@user", user);
                        using (SqlDataReader results = builder.ExecuteReader())
                        {
                            if (results.Read())
                            {
                                results.Close();
                                return false; // Username exists, so NOT available
                            }
                            results.Close();
                            return true; // Username doesn't exist, so IS available
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false; // Return false on error to be safe
                }
            }
        
            public static bool RestPass(string User, string newPass, string personalCode)
            {
                try
                {
                    string Scrambler = GetScramblerOfUser(User, false);
                    newPass = Hash(newPass, Scrambler);
                    personalCode = Hash(personalCode, Scrambler);
                
                    string command = $"UPDATE ClientUsers SET clientPassword = @newpass WHERE clientName = @user AND personalCode = @persoanlCode";
                    using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@newpass", newPass);
                        builder.Parameters.AddWithValue("@user", User);
                        builder.Parameters.AddWithValue("@persoanlCode", personalCode);
                        int rowsEffected = builder.ExecuteNonQuery();
                        if (rowsEffected > 0)
                            return true;
                        return false;
                    }
                    

                } catch (Exception ex)
                { return false;
                }

            }



            public static (bool, string) Register(string User, string pass, bool Ismaneger)
            {
                string command;

                string personalCode = "";

                if (!IsUsernameAvailable(User, Ismaneger))
                {
                    return (false, "");
                }
                string Scrambler = AddUserToScramblerTable(User, Ismaneger);
            
                pass = Hash(pass, Scrambler);

                try
                {
                
                    bool ExsistingCode;
                    using (SqlCommand builder = new SqlCommand("", SqlConnection))
                    {
                        do
                        {
                            personalCode = GenerateRandomString(5);
                            string CheckCommand = $"SELECT * FROM ClientUsers WHERE personalCode = @personalCode;";
                            builder.CommandText = CheckCommand;
                            builder.Parameters.AddWithValue("@personalCode", Hash(personalCode, Scrambler));
                            using (SqlDataReader results = builder.ExecuteReader())
                            {
                                ExsistingCode = results.Read();
                            }
                                ;
                        } while (ExsistingCode);
                    }
                    if (Ismaneger)
                    {
                        command = $"INSERT INTO ManagementUsers (username, password) VALUES (@user,@pass)";
                    
                    }
                    else
                        command = $"INSERT INTO ClientUsers (clientName, clientPassword, personalCode) VALUES (@user, @pass, @Persoanlcode)";


                    using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@user", User);
                        builder.Parameters.AddWithValue("@pass", pass);
                        if (!Ismaneger)
                            builder.Parameters.AddWithValue("@Persoanlcode", Hash(personalCode, Scrambler));
                        int rowsEffected = builder.ExecuteNonQuery();
                        if (rowsEffected > 0)
                        {
                            return (true, personalCode);
                        }
                        return (false, "");
                    }
                
                }
                catch (Exception ex) {
                    return (false, ""); }
            }


            public static string AddUserToScramblerTable(string user, bool ismeneger)
            {
                string command;
                string Scrambler = GenerateRandomScrambler();

                try
                {
                    if (ismeneger)
                    {
                        command = "INSERT INTO ScramblerTableMeneger (Username, ScramblerString) VALUES (@user, @scrambler)";
                    }
                    else
                    {
                        command = "INSERT INTO ScramblerTableClients (Username, ScramblerString) VALUES (@user, @scrambler)";
                    }

                    using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@user", user);
                        builder.Parameters.AddWithValue("@scrambler", Scrambler);
                        int rowsEffected = builder.ExecuteNonQuery();

                        if (rowsEffected > 0)
                            return Scrambler;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return null; // Failed to insert
            }


            public static string GetScramblerOfUser(string username, bool ismeneger)
            {
                string command;
                //בדיקה האם להשתמש בטבלה של המנהלים או של הלקוחות
                if (ismeneger)
                {
                    command = "SELECT * FROM ScramblerTableMeneger WHERE Username = @username;";
                }
                else
                {
                    command = "SELECT * FROM ScramblerTableClients WHERE Username = @username;";
                }
                //יצירת אובייקט כדי להשתמש ב sql
                using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                {
                //בצורה בטוחה username במשתנה  @username החלפת הערך
                builder.Parameters.AddWithValue("@username", username);
                    //הרצת הפקודה
                    using (SqlDataReader results = builder.ExecuteReader())
                    {
                        // בדיקה האם יש קריאה של מידע אם לא
                        if (results.Read())
                        {
                            return results.GetString(1);
                        }
                    
                    }
                }
                return "";
            }


            public static void AddExperimentToDatabase(string resultsData, string username)
            {
                try
                {
                    resultsData = Convert.ToBase64String(AesEncryption.EncryptDataToSql(Encoding.UTF8.GetBytes(resultsData)));
                    string command = $"INSERT INTO Experiments (username, CreationString) VALUES (@username,@resultData)";
                    using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@username", username);

                        builder.Parameters.AddWithValue("@resultData", resultsData);
                        int rowsEffected = builder.ExecuteNonQuery();
                    }
                
                }
                catch (Exception e) {
                    return;
                }
            }


        public static string GetExpererimentOfUser(string request)
        {
            try
            {

                string username = request.Split('&')[2];
                string[] Filtters = request.Split('&')[3].Split(',');

                string command = $"SELECT * FROM Experiments WHERE username = @user;";

                string hh = "";
                using (SqlCommand sqlCommand = new SqlCommand(command, SqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@user", username);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        bool MinCalc = false;
                        if (request.Contains("min") || request.Contains("max"))
                        {
                            if (request.Contains("min"))
                                MinCalc = true;
                            string Par = request.Split('&')[request.Split('&').Length - 1].Replace("min", "");
                            Par = Par.Replace("max", "");
                            reader.Read();

                            string TempCreationstring = AesEncryption.DecryptDataToSqlToString(Convert.FromBase64String(reader.GetString(2)));
                            string[] TempParameters = TempCreationstring.Split(';');
                            double MaxOrMin = 0;
                            try
                            {
                                foreach (string item in TempParameters)
                                {
                                    if (item.Contains(Par))
                                    {
                                        MaxOrMin = double.Parse(item.Split(':')[1].Split('|')[0]);
                                        hh = $"&{TempCreationstring}";
                                        break;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }
                            while (reader.Read())
                            {
                                string creationstring = AesEncryption.DecryptDataToSqlToString(Convert.FromBase64String(reader.GetString(2)));
                                string[] parameters = creationstring.Split(';');

                                foreach (string item in parameters)
                                {
                                    if (item.Contains(Par))
                                    {
                                        double val = double.Parse(item.Split(':')[1].Split('|')[0]);
                                        if (!MinCalc)
                                        {
                                            if (val > MaxOrMin)
                                            {
                                                hh = $"&{creationstring}";
                                                MaxOrMin = val;
                                            }
                                            else if (val == MaxOrMin)
                                            {
                                                hh += $"&{creationstring}";
                                                MaxOrMin = val;
                                            }
                                        }
                                        else
                                        {
                                            if (val < MaxOrMin)
                                            {
                                                hh = $"&{creationstring}";
                                                MaxOrMin = val;
                                            }
                                            else if (val == MaxOrMin)
                                            {
                                                hh += $"&{creationstring}";
                                                MaxOrMin = val;
                                            }
                                        }
                                        
                                    }
                                }
                            }
                            return hh;
                        }
                        else
                        {


                            while (reader.Read())
                            {
                                string de = AesEncryption.DecryptDataToSqlToString(Convert.FromBase64String(reader.GetString(2)));
                                string CreationString = GetCreationStringIfExperStandConditions(Filtters, de);
                                if (CreationString != "")
                                    hh += $"&{CreationString}";
                            }
                            return hh;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message );
            }

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
                            if (!DoesExperStandCondition(">=", item, CreationString))
                                return "";
                        }
                        else if (item.Contains("<="))
                        {
                            if (!DoesExperStandCondition("<=", item, CreationString))
                                return "";
                        }
                        else if (item.Contains("="))
                        {
                            if (!DoesExperStandCondition("=", item, CreationString))
                                return "";
                        }
                        else if (item.Contains(">"))
                        {
                            if (!DoesExperStandCondition(">", item, CreationString))
                                return "";
                        }
                        else if (item.Contains("<"))
                        {
                            if (!DoesExperStandCondition("<", item, CreationString))
                                return "";
                        }
                        else if (item.Contains("!="))
                        {
                            if (!DoesExperStandCondition("!=", item, CreationString))
                                return "";
                        }
                        else
                            return "";

                    }

                    return CreationString;

                }
                catch (Exception e) {
                    return ""; }


            }
            
            private static bool DoesExperStandCondition(string Oparation, string item, string CreationString)
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

                        if (ExperResult.ToLower().Contains(Result.ToLower()))
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
                    
                    string command = $"SELECT * FROM Experiments WHERE username = @user;";

                    using (SqlCommand sqlCommand = new SqlCommand(command, SqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@user", username);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string decrypted = AesEncryption.DecryptDataToSqlToString(Convert.FromBase64String(reader.GetString(2)));
                                CreationStrings.Add(decrypted);
                            }
                        }
                    }

                    return CreationStrings;
                } catch (Exception e) { 
                    return null; }

            }
            public static bool IsScramblerExist(string Scrambler)
            {
                try
                {
                    string commandManager = "SELECT * FROM ScramblerTableMeneger WHERE ScramblerString = @scrambler";
                    using (SqlCommand builder = new SqlCommand(commandManager, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@scrambler", Scrambler);
                        using (SqlDataReader results = builder.ExecuteReader())
                        {
                            if (results.Read())
                            {
                                results.Close();
                                return true;
                            }
                        }
                    }

                    string commandClients = "SELECT * FROM ScramblerTableClients WHERE ScramblerString = @scrambler";
                    using (SqlCommand builder = new SqlCommand(commandClients, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@scrambler", Scrambler);
                        using (SqlDataReader results = builder.ExecuteReader())
                        {
                            if (results.Read())
                            {
                                results.Close();
                                return true; 
                            }
                        }
                    }

                    return false; 
                }
                catch (Exception ex)
                {
                    return true; 
                }
            }
            public static void AddBlockedGuest(BadGuest c)
            {
                try
                {
                    string SR = Convert.ToBase64String(AesEncryption.EncryptDataToSql(Encoding.UTF8.GetBytes(c.GEt_MotherBoard_SN())));

                    string BD = Convert.ToBase64String(AesEncryption.EncryptDataToSql(Encoding.UTF8.GetBytes(c.GetBlockedDate())));
                    string command = $"INSERT INTO BlockedGuest (MotherboardID, BlockDate) VALUES (@SR, @BD)";
                    using (SqlCommand builder = new SqlCommand(command, SqlConnection))
                    {
                        builder.Parameters.AddWithValue("@SR", SR);
                        builder.Parameters.AddWithValue("@BD", BD);
                        int rowsEffected = builder.ExecuteNonQuery();
                    }
                }
                catch (Exception e) 
                { }
            }
            public static List<BadGuest> GetBlockedGuestFromSQL()
            {
                try
                {
                    List<BadGuest> LBG = new List<BadGuest>();

                    string command = $"SELECT * FROM BlockedGuest";


                    SqlCommand builder = new SqlCommand(command, SqlConnection);
                    SqlDataReader BlockedGuests = builder.ExecuteReader();

                    while (BlockedGuests.Read())
                    {
                        string SR = BlockedGuests.GetString(0);
                        string BD = BlockedGuests.GetString(1);

                        SR = Encoding.UTF8.GetString(AesEncryption.DecryptDataToSql(Convert.FromBase64String(SR)));
                        BD = Encoding.UTF8.GetString(AesEncryption.DecryptDataToSql(Convert.FromBase64String(BD)));

                        BadGuest BG = new BadGuest(SR, BD);
                        LBG.Add(BG);
                    }

                    return LBG;


                }
                catch (Exception e) { }



                return null;
            }



            private static string Hash(string data, string Scrambler)
            {
                //הוספה של המחזרות היחודית ללקוח למידע המקורי
                data = data + Scrambler; 
                // יצירת אובייקט זמני של האלגוריתם
                using (SHA384 sha384 = SHA384.Create())
                {
                    //המרה של המידע למערך של בייטים והצפנה שלו
                    byte[] hashedBytes = sha384.ComputeHash(Encoding.UTF8.GetBytes(data));
                    //המרה של המידע המוצפן למחרוזת
                    return Convert.ToBase64String(hashedBytes);
                }
            }




            public static string GenerateRandomString(int length)
            {
                string code = "";
                do
                {
                    Random _random = new Random();
                
                    string chars = "a2QjWz3n0v1p7Gm5kI9oXebVd4yHcL6f8sT";
                
                    for (int i = 0; i < length; i++)
                    {
                        code = code + chars[_random.Next(chars.Length)];
                    }
                } while (!ServerServices.Avalible(code));

                return code;
            }

            public static string GenerateRandomScrambler()
            {
                string Scrambler = "";
                do
                {
                    Random _random = new Random();

                    string chars = "22$@$$$Wz34$n02v1%Sp#Gm@F#5k2#I$#AA9@oXebVd6f%$8";
                
                    for (int i = 0; i < 6; i++)
                    {
                        Scrambler = Scrambler + chars[_random.Next(chars.Length)];
                    }
                } while (IsScramblerExist(Scrambler));

                return Scrambler;
            }
            public static bool IsConnected()
            {
                if (SqlConnection == null)
                    return true;
                return false;
            }
        }
    }
