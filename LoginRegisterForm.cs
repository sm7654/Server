using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class LoginRegisterForm : Form
    {
        SqlConnection connection;
        public LoginRegisterForm()
        {
            InitializeComponent();
        }

        private void LoginRegisterForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.connection = ConnectToSql();
                Console.WriteLine("");
            } catch (Exception ex) { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public static SqlConnection ConnectToSql()
        {
            try
            {
                Console.WriteLine("Connecting to SQL server...");
                string ConnString = "Data Source=DESKTOP-03BNVH4;Initial Catalog=Test_1;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                SqlConnection connection = new SqlConnection(ConnString);
                connection.Open();
                return connection;
            }
            catch (SqlException ex) { return null; }
        }

        public bool LoginSql(string user, string pass)
        {
            try
            {
                string command = $"SELECT * FROM Users WHERE password = '{pass}' AND username = '{user}';";
                SqlCommand builder = new SqlCommand(command, connection);
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

        private void loginButton_Click(object sender, EventArgs e)
        {
            RegisterStatus.Text = "";
            if (connection == null)
                return;
            string User = usernameTextbox.Text;
            string pass = passwordTextbox.Text;

            /*
            if (User == "" || pass == "")
                return;
            */

            if (LoginSql(User, pass)) 
            {
                this.Hide();
                Form1 initial = new Form1();
                initial.Show();
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegisterStatus.Text = "";
            if (connection == null) // pass = 12345
                return;
            string User = usernameTextbox.Text;
            string pass = passwordTextbox.Text;

            if (User == "" || pass == "")
                return;

            if (LoginSql(User, pass))
                return;

            try
            {
                string command = $"INSERT INTO Users (username, password) VALUES ('{User}', '{pass}')";
                SqlCommand builder = new SqlCommand(command, connection);
                int rowsEffected = builder.ExecuteNonQuery();
                if (rowsEffected > 0)
                {
                    RegisterStatus.Text = "User added!";
                    usernameTextbox.Text = "";
                    passwordTextbox.Text = "";
                }
            }
            catch (SqlException ex) { RegisterStatus.Text = "Cloud not add user..."; return; }
           
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ClosingController.btnExit_Click();
        }
    }
}
