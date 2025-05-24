using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ServerSide
{
    partial class SqlConnectionForm
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.dbNameTextBox = new System.Windows.Forms.TextBox();
            this.dbNameLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerLabel = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.Controls.Add(this.ErrorLabel);
            this.mainPanel.Controls.Add(this.dbNameTextBox);
            this.mainPanel.Controls.Add(this.dbNameLabel);
            this.mainPanel.Controls.Add(this.connectButton);
            this.mainPanel.Controls.Add(this.passwordTextBox);
            this.mainPanel.Controls.Add(this.passwordLabel);
            this.mainPanel.Controls.Add(this.usernameTextBox);
            this.mainPanel.Controls.Add(this.usernameLabel);
            this.mainPanel.Controls.Add(this.headerPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(467, 369);
            this.mainPanel.TabIndex = 0;
            // 
            // dbNameTextBox
            // 
            this.dbNameTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dbNameTextBox.Location = new System.Drawing.Point(40, 234);
            this.dbNameTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dbNameTextBox.Name = "dbNameTextBox";
            this.dbNameTextBox.Size = new System.Drawing.Size(385, 29);
            this.dbNameTextBox.TabIndex = 3;
            // 
            // dbNameLabel
            // 
            this.dbNameLabel.AutoSize = true;
            this.dbNameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dbNameLabel.Location = new System.Drawing.Point(40, 209);
            this.dbNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dbNameLabel.Name = "dbNameLabel";
            this.dbNameLabel.Size = new System.Drawing.Size(136, 23);
            this.dbNameLabel.TabIndex = 6;
            this.dbNameLabel.Text = "Database Name:";
            // 
            // connectButton
            // 
            this.connectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.connectButton.FlatAppearance.BorderSize = 0;
            this.connectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connectButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.connectButton.ForeColor = System.Drawing.Color.White;
            this.connectButton.Location = new System.Drawing.Point(167, 295);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(133, 43);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = false;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.passwordTextBox.Location = new System.Drawing.Point(40, 172);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(385, 29);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.passwordLabel.Location = new System.Drawing.Point(40, 148);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(84, 23);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Password:";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.usernameTextBox.Location = new System.Drawing.Point(40, 111);
            this.usernameTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(385, 29);
            this.usernameTextBox.TabIndex = 1;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.usernameLabel.Location = new System.Drawing.Point(40, 86);
            this.usernameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(91, 23);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username:";
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.headerPanel.Controls.Add(this.headerLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(467, 62);
            this.headerPanel.TabIndex = 0;
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.headerLabel.ForeColor = System.Drawing.Color.White;
            this.headerLabel.Location = new System.Drawing.Point(33, 16);
            this.headerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(220, 32);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Database Connect";
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Location = new System.Drawing.Point(41, 279);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(63, 16);
            this.ErrorLabel.TabIndex = 7;
            this.ErrorLabel.Text = "________";
            // 
            // SqlConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 369);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL Login";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox dbNameTextBox;
        private System.Windows.Forms.Label dbNameLabel;
        private Label ErrorLabel;
    }
}