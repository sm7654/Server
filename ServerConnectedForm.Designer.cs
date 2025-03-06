using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace ServerSide
{
    partial class ServerConnectedForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Panel SearchPanel;
        private System.Windows.Forms.Panel MainContainer;
        private System.Windows.Forms.FlowLayoutPanel SessionsViewPanel;

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label SearchLabel;

        private System.Windows.Forms.TextBox SessionSearch;
        private System.Windows.Forms.Button ShutSessionsButton;
        private System.Windows.Forms.Button ShutServerButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.SearchPanel = new System.Windows.Forms.Panel();
            this.ShutServerButton = new System.Windows.Forms.Button();
            this.ShutSessionsButton = new System.Windows.Forms.Button();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.SessionSearch = new System.Windows.Forms.TextBox();
            this.SessionsViewPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.MainContainer = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.HeaderPanel.SuspendLayout();
            this.SearchPanel.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(144)))), ((int)(((byte)(255)))));
            this.HeaderPanel.Controls.Add(this.TitleLabel);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Padding = new System.Windows.Forms.Padding(10);
            this.HeaderPanel.Size = new System.Drawing.Size(1200, 60);
            this.HeaderPanel.TabIndex = 2;
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.Location = new System.Drawing.Point(10, 10);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(348, 46);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Server Control Panel";
            // 
            // SearchPanel
            // 
            this.SearchPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.SearchPanel.Controls.Add(this.button1);
            this.SearchPanel.Controls.Add(this.ShutServerButton);
            this.SearchPanel.Controls.Add(this.ShutSessionsButton);
            this.SearchPanel.Controls.Add(this.SearchLabel);
            this.SearchPanel.Controls.Add(this.SessionSearch);
            this.SearchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SearchPanel.Location = new System.Drawing.Point(0, 60);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Padding = new System.Windows.Forms.Padding(10);
            this.SearchPanel.Size = new System.Drawing.Size(1200, 49);
            this.SearchPanel.TabIndex = 1;
            // 
            // ShutServerButton
            // 
            this.ShutServerButton.BackColor = System.Drawing.Color.Red;
            this.ShutServerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShutServerButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ShutServerButton.ForeColor = System.Drawing.Color.White;
            this.ShutServerButton.Location = new System.Drawing.Point(1068, 11);
            this.ShutServerButton.Margin = new System.Windows.Forms.Padding(30, 5, 5, 0);
            this.ShutServerButton.Name = "ShutServerButton";
            this.ShutServerButton.Size = new System.Drawing.Size(118, 31);
            this.ShutServerButton.TabIndex = 2;
            this.ShutServerButton.Text = "Shut Server";
            this.ShutServerButton.UseVisualStyleBackColor = false;
            this.ShutServerButton.Click += new System.EventHandler(this.DisconnectServerButton_Click);
            // 
            // ShutSessionsButton
            // 
            this.ShutSessionsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.ShutSessionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShutSessionsButton.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ShutSessionsButton.ForeColor = System.Drawing.Color.White;
            this.ShutSessionsButton.Location = new System.Drawing.Point(940, 11);
            this.ShutSessionsButton.Margin = new System.Windows.Forms.Padding(5, 5, 20, 0);
            this.ShutSessionsButton.Name = "ShutSessionsButton";
            this.ShutSessionsButton.Size = new System.Drawing.Size(125, 31);
            this.ShutSessionsButton.TabIndex = 1;
            this.ShutSessionsButton.Text = "Shut Sessions";
            this.ShutSessionsButton.UseVisualStyleBackColor = false;
            this.ShutSessionsButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // SearchLabel
            // 
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.SearchLabel.ForeColor = System.Drawing.Color.Black;
            this.SearchLabel.Location = new System.Drawing.Point(10, 11);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Size = new System.Drawing.Size(152, 28);
            this.SearchLabel.TabIndex = 1;
            this.SearchLabel.Text = "Search Sessions:";
            // 
            // SessionSearch
            // 
            this.SessionSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.SessionSearch.ForeColor = System.Drawing.Color.Black;
            this.SessionSearch.Location = new System.Drawing.Point(160, 8);
            this.SessionSearch.Name = "SessionSearch";
            this.SessionSearch.Size = new System.Drawing.Size(300, 34);
            this.SessionSearch.TabIndex = 2;
            this.SessionSearch.TextChanged += new System.EventHandler(this.SessionSearch_TextChanged);
            // 
            // SessionsViewPanel
            // 
            this.SessionsViewPanel.AutoScroll = true;
            this.SessionsViewPanel.BackColor = System.Drawing.Color.White;
            this.SessionsViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionsViewPanel.Location = new System.Drawing.Point(0, 109);
            this.SessionsViewPanel.Name = "SessionsViewPanel";
            this.SessionsViewPanel.Padding = new System.Windows.Forms.Padding(10);
            this.SessionsViewPanel.Size = new System.Drawing.Size(1200, 691);
            this.SessionsViewPanel.TabIndex = 0;
            // 
            // MainContainer
            // 
            this.MainContainer.BackColor = System.Drawing.Color.White;
            this.MainContainer.Controls.Add(this.SessionsViewPanel);
            this.MainContainer.Controls.Add(this.SearchPanel);
            this.MainContainer.Controls.Add(this.HeaderPanel);
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.Size = new System.Drawing.Size(1200, 800);
            this.MainContainer.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(821, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "Change IV";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // ServerConnectedForm
            // 
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.MainContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ServerConnectedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Control Panel";
            this.Load += new System.EventHandler(this.ServerConnectedForm_Load);
            this.HeaderPanel.ResumeLayout(false);
            this.HeaderPanel.PerformLayout();
            this.SearchPanel.ResumeLayout(false);
            this.SearchPanel.PerformLayout();
            this.MainContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        // Updated helper methods
        
        public System.Windows.Forms.FlowLayoutPanel.ControlCollection GetSessionLayoutControls()
        {
            return this.SessionsViewPanel.Controls;
        }

        private Button button1;
    }
}



