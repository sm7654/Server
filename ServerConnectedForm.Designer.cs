namespace ServerSide
{
    partial class ServerConnectedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConnectedForm));
            this.label2 = new System.Windows.Forms.Label();
            this.Speedometer = new System.Windows.Forms.Label();
            this.Disconnectbutton = new System.Windows.Forms.Button();
            this.DisconnectServerButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SessionViewPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SessionSearch = new System.Windows.Forms.TextBox();
            this.ControlPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 1;
            // 
            // Speedometer
            // 
            this.Speedometer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Speedometer.AutoSize = true;
            this.Speedometer.Location = new System.Drawing.Point(1226, 31);
            this.Speedometer.Name = "Speedometer";
            this.Speedometer.Size = new System.Drawing.Size(38, 13);
            this.Speedometer.TabIndex = 3;
            this.Speedometer.Text = "0 KPH";
            // 
            // Disconnectbutton
            // 
            this.Disconnectbutton.BackColor = System.Drawing.SystemColors.Menu;
            this.Disconnectbutton.Dock = System.Windows.Forms.DockStyle.Left;
            this.Disconnectbutton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.Disconnectbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Disconnectbutton.ForeColor = System.Drawing.Color.Black;
            this.Disconnectbutton.Location = new System.Drawing.Point(0, 0);
            this.Disconnectbutton.Name = "Disconnectbutton";
            this.Disconnectbutton.Size = new System.Drawing.Size(75, 30);
            this.Disconnectbutton.TabIndex = 5;
            this.Disconnectbutton.Text = "Disconnect";
            this.Disconnectbutton.UseVisualStyleBackColor = false;
            this.Disconnectbutton.Click += new System.EventHandler(this.Disconnectbutton_Click);
            // 
            // DisconnectServerButton
            // 
            this.DisconnectServerButton.BackColor = System.Drawing.Color.Red;
            this.DisconnectServerButton.FlatAppearance.BorderSize = 0;
            this.DisconnectServerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DisconnectServerButton.ForeColor = System.Drawing.Color.Snow;
            this.DisconnectServerButton.Location = new System.Drawing.Point(1189, 576);
            this.DisconnectServerButton.Name = "DisconnectServerButton";
            this.DisconnectServerButton.Size = new System.Drawing.Size(75, 23);
            this.DisconnectServerButton.TabIndex = 6;
            this.DisconnectServerButton.Text = "Shut Server";
            this.DisconnectServerButton.UseVisualStyleBackColor = false;
            this.DisconnectServerButton.Click += new System.EventHandler(this.DisconnectServerButton_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(1004, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 30);
            this.button1.TabIndex = 8;
            this.button1.Text = "Shut sessions";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ControlPanel
            // 
            this.ControlPanel.Controls.Add(this.tableLayoutPanel1);
            this.ControlPanel.Location = new System.Drawing.Point(169, 75);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(1095, 495);
            this.ControlPanel.TabIndex = 10;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.SessionViewPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.505071F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.49493F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1095, 495);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // SessionViewPanel
            // 
            this.SessionViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionViewPanel.Location = new System.Drawing.Point(5, 43);
            this.SessionViewPanel.Name = "SessionViewPanel";
            this.SessionViewPanel.Size = new System.Drawing.Size(1085, 447);
            this.SessionViewPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Disconnectbutton);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1085, 30);
            this.panel1.TabIndex = 1;
            // 
            // SessionSearch
            // 
            this.SessionSearch.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.SessionSearch.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionSearch.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.SessionSearch.Location = new System.Drawing.Point(169, 49);
            this.SessionSearch.Name = "SessionSearch";
            this.SessionSearch.Size = new System.Drawing.Size(137, 22);
            this.SessionSearch.TabIndex = 11;
            this.SessionSearch.TextChanged += new System.EventHandler(this.SessionSearch_TextChanged);
            // 
            // ServerConnectedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1437, 640);
            this.Controls.Add(this.SessionSearch);
            this.Controls.Add(this.DisconnectServerButton);
            this.Controls.Add(this.ControlPanel);
            this.Controls.Add(this.Speedometer);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerConnectedForm";
            this.Text = "ServerConnectedForm";
            this.Load += new System.EventHandler(this.ServerConnectedForm_Load);
            this.ControlPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Speedometer;

        public System.Windows.Forms.Label GetSpeedometer()
        {
            return Speedometer;
        }
        private System.Windows.Forms.Button Disconnectbutton;
        private System.Windows.Forms.Button DisconnectServerButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel SessionViewPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox SessionSearch;
    }
}