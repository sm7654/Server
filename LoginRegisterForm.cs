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
                bool h = SqlService.ConnectToSql();
            } catch (Exception ex) {
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (SqlService.IsConnected())
                return;
            string User = usernameTextbox.Text;
            string pass = passwordTextbox.Text;

            
            if (User == "" || pass == "")
                return;
            

            if (SqlService.LoginSql(User, pass, true)) 
            {
                this.Hide();
                Form1 initial = new Form1();
                initial.Show();
            } else
            {
                RegisterStatus.Text = "Cloud not login...";
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (SqlService.IsConnected()) 
                return;
            string User = usernameTextbox.Text;
            string pass = passwordTextbox.Text;

            if (User == "" || pass == "")
                return;

            if (SqlService.LoginSql(User, pass, true))
                return;

            if (SqlService.Register(User, pass, true))
            {
                RegisterStatus.Text = "User added!";
                usernameTextbox.Text = "";
                passwordTextbox.Text = "";
            }  else
            {
                RegisterStatus.Text = "Cloud not add user...";
            }
           
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ClosingController.btnExit_Click();
        }
    }
}
