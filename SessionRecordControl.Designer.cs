namespace ServerSide
{
    partial class SessionRecordControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.DividerPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ClientDisconnectedLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SessionDetailsPanel = new System.Windows.Forms.Panel();
            this.StartDatelabel = new System.Windows.Forms.Label();
            this.SessionDurationlabel = new System.Windows.Forms.Label();
            this.MicroDataFlowlabel = new System.Windows.Forms.Label();
            this.ClientDataFlowlabel = new System.Windows.Forms.Label();
            this.SessionCodelabel = new System.Windows.Forms.Label();
            this.NewClientLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SessionDetailsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DividerPanel
            // 
            this.DividerPanel.BackColor = System.Drawing.Color.Gray;
            this.DividerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DividerPanel.Location = new System.Drawing.Point(0, 0);
            this.DividerPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DividerPanel.Name = "DividerPanel";
            this.DividerPanel.Size = new System.Drawing.Size(514, 1);
            this.DividerPanel.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.ClientDisconnectedLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 184);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(510, 33);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Red;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(8, 33);
            this.panel3.TabIndex = 1;
            // 
            // ClientDisconnectedLabel
            // 
            this.ClientDisconnectedLabel.AutoSize = true;
            this.ClientDisconnectedLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientDisconnectedLabel.ForeColor = System.Drawing.Color.Maroon;
            this.ClientDisconnectedLabel.Location = new System.Drawing.Point(19, 8);
            this.ClientDisconnectedLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ClientDisconnectedLabel.Name = "ClientDisconnectedLabel";
            this.ClientDisconnectedLabel.Size = new System.Drawing.Size(318, 19);
            this.ClientDisconnectedLabel.TabIndex = 0;
            this.ClientDisconnectedLabel.Text = "XXXXXX disconnected at 12.5.2026 16:20";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.SessionDetailsPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.167235F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.83276F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(514, 219);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // SessionDetailsPanel
            // 
            this.SessionDetailsPanel.BackColor = System.Drawing.Color.White;
            this.SessionDetailsPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SessionDetailsPanel.Controls.Add(this.StartDatelabel);
            this.SessionDetailsPanel.Controls.Add(this.SessionDurationlabel);
            this.SessionDetailsPanel.Controls.Add(this.MicroDataFlowlabel);
            this.SessionDetailsPanel.Controls.Add(this.ClientDataFlowlabel);
            this.SessionDetailsPanel.Controls.Add(this.SessionCodelabel);
            this.SessionDetailsPanel.Controls.Add(this.NewClientLabel);
            this.SessionDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionDetailsPanel.Location = new System.Drawing.Point(2, 15);
            this.SessionDetailsPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SessionDetailsPanel.Name = "SessionDetailsPanel";
            this.SessionDetailsPanel.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SessionDetailsPanel.Size = new System.Drawing.Size(510, 165);
            this.SessionDetailsPanel.TabIndex = 0;
            this.SessionDetailsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.SessionDetailsPanel_Paint);
            // 
            // StartDatelabel
            // 
            this.StartDatelabel.AutoSize = true;
            this.StartDatelabel.BackColor = System.Drawing.Color.White;
            this.StartDatelabel.Font = new System.Drawing.Font("Microsoft YaHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDatelabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(43)))));
            this.StartDatelabel.Location = new System.Drawing.Point(26, 123);
            this.StartDatelabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StartDatelabel.Name = "StartDatelabel";
            this.StartDatelabel.Size = new System.Drawing.Size(153, 16);
            this.StartDatelabel.TabIndex = 8;
            this.StartDatelabel.Text = "🗓️ Start Date: 12.5.2026 15:55";
            this.StartDatelabel.Click += new System.EventHandler(this.StartDatelabel_Click);
            // 
            // SessionDurationlabel
            // 
            this.SessionDurationlabel.AutoSize = true;
            this.SessionDurationlabel.BackColor = System.Drawing.Color.White;
            this.SessionDurationlabel.Font = new System.Drawing.Font("Microsoft YaHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionDurationlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(43)))));
            this.SessionDurationlabel.Location = new System.Drawing.Point(26, 87);
            this.SessionDurationlabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SessionDurationlabel.Name = "SessionDurationlabel";
            this.SessionDurationlabel.Size = new System.Drawing.Size(140, 16);
            this.SessionDurationlabel.TabIndex = 7;
            this.SessionDurationlabel.Text = "🕔 Sessoin Duration: 50sec";
            this.SessionDurationlabel.Click += new System.EventHandler(this.SessionDurationlabel_Click);
            // 
            // MicroDataFlowlabel
            // 
            this.MicroDataFlowlabel.AutoSize = true;
            this.MicroDataFlowlabel.BackColor = System.Drawing.Color.White;
            this.MicroDataFlowlabel.Font = new System.Drawing.Font("Microsoft YaHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MicroDataFlowlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(43)))));
            this.MicroDataFlowlabel.Location = new System.Drawing.Point(298, 87);
            this.MicroDataFlowlabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MicroDataFlowlabel.Name = "MicroDataFlowlabel";
            this.MicroDataFlowlabel.Size = new System.Drawing.Size(132, 16);
            this.MicroDataFlowlabel.TabIndex = 5;
            this.MicroDataFlowlabel.Text = "📦 Micro Dataflow: 52Mb";
            // 
            // ClientDataFlowlabel
            // 
            this.ClientDataFlowlabel.AutoSize = true;
            this.ClientDataFlowlabel.BackColor = System.Drawing.Color.White;
            this.ClientDataFlowlabel.Font = new System.Drawing.Font("Microsoft YaHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientDataFlowlabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(43)))));
            this.ClientDataFlowlabel.Location = new System.Drawing.Point(298, 50);
            this.ClientDataFlowlabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ClientDataFlowlabel.Name = "ClientDataFlowlabel";
            this.ClientDataFlowlabel.Size = new System.Drawing.Size(139, 16);
            this.ClientDataFlowlabel.TabIndex = 2;
            this.ClientDataFlowlabel.Text = "📦 Client Dataflow: 452Mb";
            // 
            // SessionCodelabel
            // 
            this.SessionCodelabel.AutoSize = true;
            this.SessionCodelabel.BackColor = System.Drawing.Color.White;
            this.SessionCodelabel.Font = new System.Drawing.Font("Microsoft YaHei", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionCodelabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(43)))));
            this.SessionCodelabel.Location = new System.Drawing.Point(26, 50);
            this.SessionCodelabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SessionCodelabel.Name = "SessionCodelabel";
            this.SessionCodelabel.Size = new System.Drawing.Size(124, 16);
            this.SessionCodelabel.TabIndex = 1;
            this.SessionCodelabel.Text = "🔑 Session Code: Rc49S";
            // 
            // NewClientLabel
            // 
            this.NewClientLabel.AutoSize = true;
            this.NewClientLabel.BackColor = System.Drawing.Color.White;
            this.NewClientLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewClientLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.NewClientLabel.Location = new System.Drawing.Point(19, 8);
            this.NewClientLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NewClientLabel.Name = "NewClientLabel";
            this.NewClientLabel.Size = new System.Drawing.Size(182, 22);
            this.NewClientLabel.TabIndex = 0;
            this.NewClientLabel.Text = "New Client - XXXXXX";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(510, 9);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // SessionRecordControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.DividerPanel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SessionRecordControl";
            this.Size = new System.Drawing.Size(514, 224);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.SessionDetailsPanel.ResumeLayout(false);
            this.SessionDetailsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel DividerPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ClientDisconnectedLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel SessionDetailsPanel;
        private System.Windows.Forms.Label StartDatelabel;
        private System.Windows.Forms.Label SessionDurationlabel;
        private System.Windows.Forms.Label MicroDataFlowlabel;
        private System.Windows.Forms.Label ClientDataFlowlabel;
        private System.Windows.Forms.Label SessionCodelabel;
        private System.Windows.Forms.Label NewClientLabel;
        private System.Windows.Forms.Panel panel3;
    }
}
