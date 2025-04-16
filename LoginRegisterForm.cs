using System;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Text;

namespace ServerSide
{
    public partial class LoginRegisterForm : Form
    {
        SqlConnection connection;
        public LoginRegisterForm()
        {
            InitializeComponent();
            this.FormClosing += CloseForm;
        }
        private void CloseForm(object sender, EventArgs e)
        {
            ClosingController.btnExit_Click();
        }
        private void LoginRegisterForm_Load(object sender, EventArgs e)
        {
            try
            {
                bool h = SqlService.ConnectToSql();
            } catch (Exception ex) {
                
            }
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
                StartServerForm initial = new StartServerForm();
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

            if (SqlService.Register(User, pass, true).Item1)
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

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
