using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class SqlConnectionForm : Form
    {
        public SqlConnectionForm()
        {
            InitializeComponent();
        }


        private void connectButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = "";
            new Thread(() =>
            {
                // Get login details
                string username = usernameTextBox.Text;
                string password = passwordTextBox.Text;
                string dbName = dbNameTextBox.Text;

                // Basic validation
                /*if (username == "" || password == "" || dbName == "")
                {
                    return;
                }*/
                bool Connected = SqlService.ConnectToSql("shai", "1234", "DB");

                if (Connected)
                {
                    LoginRegisterForm form = new LoginRegisterForm();
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Close();
                    }));

                    Application.Run(form);
                }
                else
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        ErrorLabel.Text = "Could not connect to Sql.";
                    }));
                }
            }

            ).Start();




        }
    }
}
