namespace ServerSide
{
    partial class sessionLayot
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SessionName = new System.Windows.Forms.Label();
            this.SessionCodeLabel = new System.Windows.Forms.Label();
            this.ConectionTable = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ClientPort = new System.Windows.Forms.Label();
            this.ClientIp = new System.Windows.Forms.Label();
            this.ClientKickname = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.MicroPort = new System.Windows.Forms.Label();
            this.MicroIp = new System.Windows.Forms.Label();
            this.MicroName = new System.Windows.Forms.Label();
            this.ConectionTable.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SessionName
            // 
            this.SessionName.AutoSize = true;
            this.SessionName.BackColor = System.Drawing.Color.Transparent;
            this.SessionName.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionName.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SessionName.Location = new System.Drawing.Point(3, 4);
            this.SessionName.Name = "SessionName";
            this.SessionName.Size = new System.Drawing.Size(77, 15);
            this.SessionName.TabIndex = 0;
            this.SessionName.Text = "Session #56";
            // 
            // SessionCodeLabel
            // 
            this.SessionCodeLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SessionCodeLabel.AutoSize = true;
            this.SessionCodeLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionCodeLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.SessionCodeLabel.Location = new System.Drawing.Point(306, 4);
            this.SessionCodeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.SessionCodeLabel.Name = "SessionCodeLabel";
            this.SessionCodeLabel.Size = new System.Drawing.Size(40, 15);
            this.SessionCodeLabel.TabIndex = 1;
            this.SessionCodeLabel.Text = "Yi4Bg";
            // 
            // ConectionTable
            // 
            this.ConectionTable.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ConectionTable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ConectionTable.ColumnCount = 2;
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ConectionTable.Controls.Add(this.panel1, 1, 0);
            this.ConectionTable.Controls.Add(this.panel2, 0, 0);
            this.ConectionTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ConectionTable.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ConectionTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.ConectionTable.Location = new System.Drawing.Point(0, 30);
            this.ConectionTable.Name = "ConectionTable";
            this.ConectionTable.RowCount = 1;
            this.ConectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ConectionTable.Size = new System.Drawing.Size(354, 73);
            this.ConectionTable.TabIndex = 2;
            this.ConectionTable.Paint += new System.Windows.Forms.PaintEventHandler(this.ConectionTable_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.ClientPort);
            this.panel1.Controls.Add(this.ClientIp);
            this.panel1.Controls.Add(this.ClientKickname);
            this.panel1.Location = new System.Drawing.Point(180, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 67);
            this.panel1.TabIndex = 0;
            // 
            // ClientPort
            // 
            this.ClientPort.AutoSize = true;
            this.ClientPort.Location = new System.Drawing.Point(31, 42);
            this.ClientPort.Name = "ClientPort";
            this.ClientPort.Size = new System.Drawing.Size(62, 13);
            this.ClientPort.TabIndex = 2;
            this.ClientPort.Text = "Port: 52148";
            // 
            // ClientIp
            // 
            this.ClientIp.AutoSize = true;
            this.ClientIp.Location = new System.Drawing.Point(31, 24);
            this.ClientIp.Name = "ClientIp";
            this.ClientIp.Size = new System.Drawing.Size(73, 13);
            this.ClientIp.TabIndex = 1;
            this.ClientIp.Text = "Ip: 100.24.1.0";
            // 
            // ClientKickname
            // 
            this.ClientKickname.AutoSize = true;
            this.ClientKickname.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientKickname.Location = new System.Drawing.Point(3, 0);
            this.ClientKickname.Name = "ClientKickname";
            this.ClientKickname.Size = new System.Drawing.Size(38, 16);
            this.ClientKickname.TabIndex = 0;
            this.ClientKickname.Text = "Client";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.MicroPort);
            this.panel2.Controls.Add(this.MicroIp);
            this.panel2.Controls.Add(this.MicroName);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(165, 67);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // MicroPort
            // 
            this.MicroPort.AutoSize = true;
            this.MicroPort.Location = new System.Drawing.Point(22, 37);
            this.MicroPort.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.MicroPort.Name = "MicroPort";
            this.MicroPort.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.MicroPort.Size = new System.Drawing.Size(62, 18);
            this.MicroPort.TabIndex = 2;
            this.MicroPort.Text = "Port: 45826";
            // 
            // MicroIp
            // 
            this.MicroIp.AutoSize = true;
            this.MicroIp.Location = new System.Drawing.Point(22, 24);
            this.MicroIp.Name = "MicroIp";
            this.MicroIp.Size = new System.Drawing.Size(85, 13);
            this.MicroIp.TabIndex = 1;
            this.MicroIp.Text = "Ip: 145.182.54.2";
            // 
            // MicroName
            // 
            this.MicroName.AutoSize = true;
            this.MicroName.BackColor = System.Drawing.Color.Transparent;
            this.MicroName.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MicroName.Location = new System.Drawing.Point(3, 0);
            this.MicroName.Name = "MicroName";
            this.MicroName.Size = new System.Drawing.Size(61, 16);
            this.MicroName.TabIndex = 0;
            this.MicroName.Text = "Kickname";
            // 
            // sessionLayot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.ConectionTable);
            this.Controls.Add(this.SessionCodeLabel);
            this.Controls.Add(this.SessionName);
            this.Name = "sessionLayot";
            this.Size = new System.Drawing.Size(354, 103);
            this.Load += new System.EventHandler(this.sessionLayot_Load);
            this.DoubleClick += new System.EventHandler(this.Test_DoubleClick);
            this.ConectionTable.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SessionName;
        private System.Windows.Forms.Label SessionCodeLabel;
        private System.Windows.Forms.TableLayoutPanel ConectionTable;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ClientKickname;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label MicroPort;
        private System.Windows.Forms.Label MicroIp;
        private System.Windows.Forms.Label MicroName;
        private System.Windows.Forms.Label ClientPort;
        private System.Windows.Forms.Label ClientIp;

        
    }
}
