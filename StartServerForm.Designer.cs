using System;
using System.Drawing;
using System.Windows.Forms;

namespace ServerSide
{
    partial class StartServerForm : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartServerForm));
            this.StartServerButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.njlkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.klklToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.ClientConnectedStatus = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartServerButton
            // 
            this.StartServerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.StartServerButton.FlatAppearance.BorderSize = 0;
            this.StartServerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartServerButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.StartServerButton.ForeColor = System.Drawing.Color.White;
            this.StartServerButton.Location = new System.Drawing.Point(468, 396);
            this.StartServerButton.Margin = new System.Windows.Forms.Padding(4);
            this.StartServerButton.Name = "StartServerButton";
            this.StartServerButton.Size = new System.Drawing.Size(180, 45);
            this.StartServerButton.TabIndex = 1;
            this.StartServerButton.Text = "Start Server";
            this.StartServerButton.UseVisualStyleBackColor = false;
            this.StartServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.njlkToolStripMenuItem,
            this.klklToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(114, 52);
            // 
            // njlkToolStripMenuItem
            // 
            this.njlkToolStripMenuItem.Name = "njlkToolStripMenuItem";
            this.njlkToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.njlkToolStripMenuItem.Text = "n/jlk/";
            // 
            // klklToolStripMenuItem
            // 
            this.klklToolStripMenuItem.Name = "klklToolStripMenuItem";
            this.klklToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.klklToolStripMenuItem.Text = "kl;kl;";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label1.Location = new System.Drawing.Point(418, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(265, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server binds to 0.0.0.0:65000";
            // 
            // ClientConnectedStatus
            // 
            this.ClientConnectedStatus.AutoSize = true;
            this.ClientConnectedStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ClientConnectedStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.ClientConnectedStatus.Location = new System.Drawing.Point(496, 190);
            this.ClientConnectedStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ClientConnectedStatus.Name = "ClientConnectedStatus";
            this.ClientConnectedStatus.Size = new System.Drawing.Size(0, 23);
            this.ClientConnectedStatus.TabIndex = 3;
            // 
            // StartServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1092, 554);
            this.Controls.Add(this.StartServerButton);
            this.Controls.Add(this.ClientConnectedStatus);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StartServerForm";
            this.Text = "Server Control Panel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartServerButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem njlkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem klklToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ClientConnectedStatus;
    }
}