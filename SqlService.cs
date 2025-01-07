using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    static class SqlService
    {
        private static SqlConnection SqlConnection;
        private static string ConnectionString = "Data Source=DESKTOP-03BNVH4;Initial Catalog=Test_1;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public static bool ConnectToSql()
        {
            try
            {
                Console.WriteLine("Connecting to SQL server...");

                SqlConnection = new SqlConnection(ConnectionString);
                SqlConnection.Open();
                return true;
            }
            catch (SqlException ex) { return false; }
        }

        public static bool LoginSql(string user, string pass, bool Ismaneger)
        {
            string command;
            try
            {
                if (Ismaneger)
                {
                    command = $"SELECT * FROM Users WHERE password = '{pass}' OR username = '{user}';";
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
            catch (SqlException ex) { Console.WriteLine(ex.Message); return false; }
        }


        public static bool Register(string User, string pass, bool Ismaneger)
        {
            string command;
            if (LoginSql(User, pass, false))
                return false;
            try
            {
                if (Ismaneger)
                    command = $"INSERT INTO Users (username, password) VALUES ('{User}', '{pass}')";
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
            catch (SqlException ex) {
                return false; }
        }


        public static bool IsConnected()
        {
            return SqlConnection.Equals(null);
        }


    }
}
