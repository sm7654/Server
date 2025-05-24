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
        SqlConnectionForm connection;
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
            {
                RegisterStatus.Text = "Invalid password.\nplease check pass role in the info button.";
                return;
            }
                
            if (pass.Length < 8)
            {
                RegisterStatus.Text = "Invalid password.\nplease check pass role in the info button.";
                return;
            }
            if (User.Length < 4)
            {
                RegisterStatus.Text = "Invalid username, must be at least 8 letter init.";
                return;
            }
            int spaciel = 0;
            int regularEN = 0;
            int CapitalEN = 0;
            int num = 0;
            for (int i = 0; i < pass.Length; i++)
            {
                if (spaciel < 1) {
                    if (
                    (pass[i] > 32 && pass[i] < 48) ||
                    (pass[i] > 57 && pass[i] < 65) ||
                    (pass[i] > 90 && pass[i] < 97) ||
                    (pass[i] > 122 && pass[i] < 127)
                    )
                        spaciel++;
                }
                if (CapitalEN < 1)
                {
                    if (pass[i] > 64 && pass[i] < 91)
                        CapitalEN++;
                }
                if (num < 1)
                {
                    if (pass[i] > 47 && pass[i] < 58)
                        num++;
                }
                if (regularEN < 1)
                {
                    if (pass[i] > 96 && pass[i] < 123)
                        regularEN++;
                }
            }
            if (regularEN == 0 || CapitalEN == 0 || num == 0|| spaciel == 0)
            {

                RegisterStatus.Text = "Invalid password.\nplease check pass roles in the info button.";
                return;
            }
            


            if (SqlService.Register(User, pass, true).Item1)
            {
                RegisterStatus.Text = "User added!";
                usernameTextbox.Text = "";
                passwordTextbox.Text = "";
            }
            else
            {
                RegisterStatus.Text = "Cloud not add user...";
            }
           
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            ClosingController.btnExit_Click();
        }

        private void ShowCodeButton_MouseDown(object sender, MouseEventArgs e)
        {
            passwordTextbox.PasswordChar = '\0';
        }

        private void ShowCodeButton_MouseUp(object sender, MouseEventArgs e)
        {
            passwordTextbox.PasswordChar = '*';
        }

        private void PasswordinfoButton_Click(object sender, EventArgs e)
        {
            passwordinfoLabel.Visible = !passwordinfoLabel.Visible;
        }

        private void passwordinfoLabel_Click(object sender, EventArgs e)
        {

        }

        private void ShowCodeButton_Click(object sender, EventArgs e)
        {

        }
    }
}
