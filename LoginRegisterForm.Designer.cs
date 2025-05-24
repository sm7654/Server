using System.Runtime.CompilerServices;

namespace ServerSide
{
    partial class LoginRegisterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginRegisterForm));
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.Label();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.loginButton = new System.Windows.Forms.Button();
            this.RegisterStatus = new System.Windows.Forms.Label();
            this.PasswordinfoButton = new System.Windows.Forms.PictureBox();
            this.passwordinfoLabel = new System.Windows.Forms.Label();
            this.ShowCodeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PasswordinfoButton)).BeginInit();
            this.SuspendLayout();
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.usernameTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernameTextbox.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.usernameTextbox.Location = new System.Drawing.Point(150, 140);
            this.usernameTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(300, 25);
            this.usernameTextbox.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.passwordTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTextbox.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.passwordTextbox.Location = new System.Drawing.Point(150, 210);
            this.passwordTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.PasswordChar = '●';
            this.passwordTextbox.Size = new System.Drawing.Size(300, 25);
            this.passwordTextbox.TabIndex = 2;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.username.Location = new System.Drawing.Point(150, 110);
            this.username.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(87, 23);
            this.username.TabIndex = 9;
            this.username.Text = "Username";
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.password.Location = new System.Drawing.Point(150, 180);
            this.password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(82, 23);
            this.password.TabIndex = 8;
            this.password.Text = "Password";
            // 
            // RegisterButton
            // 
            this.RegisterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.RegisterButton.FlatAppearance.BorderSize = 0;
            this.RegisterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RegisterButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.RegisterButton.ForeColor = System.Drawing.Color.White;
            this.RegisterButton.Location = new System.Drawing.Point(150, 280);
            this.RegisterButton.Margin = new System.Windows.Forms.Padding(4);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(140, 40);
            this.RegisterButton.TabIndex = 5;
            this.RegisterButton.Text = "Register";
            this.RegisterButton.UseVisualStyleBackColor = false;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.loginButton.ForeColor = System.Drawing.Color.White;
            this.loginButton.Location = new System.Drawing.Point(310, 280);
            this.loginButton.Margin = new System.Windows.Forms.Padding(4);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(140, 40);
            this.loginButton.TabIndex = 6;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // RegisterStatus
            // 
            this.RegisterStatus.AutoSize = true;
            this.RegisterStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RegisterStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.RegisterStatus.Location = new System.Drawing.Point(150, 340);
            this.RegisterStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RegisterStatus.Name = "RegisterStatus";
            this.RegisterStatus.Size = new System.Drawing.Size(0, 20);
            this.RegisterStatus.TabIndex = 7;
            // 
            // PasswordinfoButton
            // 
            this.PasswordinfoButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PasswordinfoButton.Image = ((System.Drawing.Image)(resources.GetObject("PasswordinfoButton.Image")));
            this.PasswordinfoButton.Location = new System.Drawing.Point(249, 185);
            this.PasswordinfoButton.Name = "PasswordinfoButton";
            this.PasswordinfoButton.Size = new System.Drawing.Size(15, 15);
            this.PasswordinfoButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PasswordinfoButton.TabIndex = 10;
            this.PasswordinfoButton.TabStop = false;
            this.PasswordinfoButton.Click += new System.EventHandler(this.PasswordinfoButton_Click);
            // 
            // passwordinfoLabel
            // 
            this.passwordinfoLabel.AutoSize = true;
            this.passwordinfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordinfoLabel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.passwordinfoLabel.Font = new System.Drawing.Font("Microsoft JhengHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordinfoLabel.Location = new System.Drawing.Point(279, 89);
            this.passwordinfoLabel.Name = "passwordinfoLabel";
            this.passwordinfoLabel.Padding = new System.Windows.Forms.Padding(5);
            this.passwordinfoLabel.Size = new System.Drawing.Size(259, 114);
            this.passwordinfoLabel.TabIndex = 0;
            this.passwordinfoLabel.Text = "Password must:\r\n- Be at least 8 characters long\r\n- Include 1 lowercase letter\r\n- " +
    "Include 1 uppercase letter\r\n- Include 1 number\r\n- Include 1 special character ;\'" +
    "/^#!#%&......\r\n";
            this.passwordinfoLabel.Visible = false;
            this.passwordinfoLabel.Click += new System.EventHandler(this.passwordinfoLabel_Click);
            // 
            // ShowCodeButton
            // 
            this.ShowCodeButton.BackColor = System.Drawing.Color.Transparent;
            this.ShowCodeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ShowCodeButton.BackgroundImage")));
            this.ShowCodeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ShowCodeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ShowCodeButton.FlatAppearance.BorderSize = 0;
            this.ShowCodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowCodeButton.Image = ((System.Drawing.Image)(resources.GetObject("ShowCodeButton.Image")));
            this.ShowCodeButton.Location = new System.Drawing.Point(457, 210);
            this.ShowCodeButton.Name = "ShowCodeButton";
            this.ShowCodeButton.Padding = new System.Windows.Forms.Padding(10);
            this.ShowCodeButton.Size = new System.Drawing.Size(28, 20);
            this.ShowCodeButton.TabIndex = 11;
            this.ShowCodeButton.UseVisualStyleBackColor = false;
            this.ShowCodeButton.Click += new System.EventHandler(this.ShowCodeButton_Click);
            this.ShowCodeButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShowCodeButton_MouseDown);
            this.ShowCodeButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShowCodeButton_MouseUp);
            // 
            // LoginRegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.ShowCodeButton);
            this.Controls.Add(this.passwordinfoLabel);
            this.Controls.Add(this.PasswordinfoButton);
            this.Controls.Add(this.RegisterStatus);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LoginRegisterForm";
            this.Text = "Login / Register";
            this.Load += new System.EventHandler(this.LoginRegisterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PasswordinfoButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label RegisterStatus;
        private System.Windows.Forms.PictureBox PasswordinfoButton;
        private System.Windows.Forms.Label passwordinfoLabel;
        private System.Windows.Forms.Button ShowCodeButton;
    }
}