﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    static class SqlService
    {
        private static SqlConnection SqlConnection;
        private static string ConnectionString = "DataSource=DESKTOP-03BNVH4;InitialCatalog=WindTunnel_Users;IntegratedSecurity=True;ConnectTimeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


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
            return true;
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
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }


        public static bool Register(string User, string pass, bool Ismaneger)
        {
            string command;

            pass = Hash(pass);
            if (LoginSql(User, pass, Ismaneger))
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
            catch (Exception ex) {
                return false; }
        }


        public static void AddExperimentToDatabase(string[] resultsData)
        {
            try
            {
                string ExperName = resultsData[2]; 
                string timestamp = resultsData[3]; 
                string deltaSpeed = resultsData[4];
                string temp = resultsData[5];
                string cameraSpeed = resultsData[6];
                string innerPressure = resultsData[7];
                string humidity = resultsData[8];

            }
            catch (Exception e) { }
        }

        public static bool IsConnected()
        {
            if (SqlConnection == null)
                return false;
            return true;
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
