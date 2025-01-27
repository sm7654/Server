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
            this.SuspendLayout();
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(425, 181);
            this.usernameTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(241, 22);
            this.usernameTextbox.TabIndex = 0;
            this.usernameTextbox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.passwordTextbox.Location = new System.Drawing.Point(425, 247);
            this.passwordTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.PasswordChar = '#';
            this.passwordTextbox.Size = new System.Drawing.Size(242, 22);
            this.passwordTextbox.TabIndex = 2;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.Location = new System.Drawing.Point(420, 154);
            this.username.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(89, 22);
            this.username.TabIndex = 3;
            this.username.Text = "username:";
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(420, 224);
            this.password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(81, 22);
            this.password.TabIndex = 4;
            this.password.Text = "password";
            // 
            // RegisterButton
            // 
            this.RegisterButton.BackColor = System.Drawing.Color.White;
            this.RegisterButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RegisterButton.BackgroundImage")));
            this.RegisterButton.Location = new System.Drawing.Point(575, 322);
            this.RegisterButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(93, 28);
            this.RegisterButton.TabIndex = 5;
            this.RegisterButton.Text = "AddUser";
            this.RegisterButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.RegisterButton.UseVisualStyleBackColor = false;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.White;
            this.loginButton.Location = new System.Drawing.Point(424, 321);
            this.loginButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(96, 30);
            this.loginButton.TabIndex = 6;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // RegisterStatus
            // 
            this.RegisterStatus.AutoSize = true;
            this.RegisterStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RegisterStatus.Location = new System.Drawing.Point(528, 391);
            this.RegisterStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RegisterStatus.Name = "RegisterStatus";
            this.RegisterStatus.Size = new System.Drawing.Size(2, 18);
            this.RegisterStatus.TabIndex = 7;
            // 
            // LoginRegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1109, 554);
            this.Controls.Add(this.RegisterStatus);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LoginRegisterForm";
            this.Text = "LoginRegisterForm";
            this.Load += new System.EventHandler(this.LoginRegisterForm_Load);
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
    }
}