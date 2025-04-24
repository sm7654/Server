namespace ServerSide
{
    partial class SessionDisplayForm
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
            this.SessionsRecordsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // SessionsRecordsPanel
            // 
            this.SessionsRecordsPanel.AutoScroll = true;
            this.SessionsRecordsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionsRecordsPanel.Location = new System.Drawing.Point(0, 0);
            this.SessionsRecordsPanel.Name = "SessionsRecordsPanel";
            this.SessionsRecordsPanel.Size = new System.Drawing.Size(1034, 450);
            this.SessionsRecordsPanel.TabIndex = 0;
            // 
            // SessionDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1034, 450);
            this.Controls.Add(this.SessionsRecordsPanel);
            this.Name = "SessionDisplayForm";
            this.Text = "SessionDisplayForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel SessionsRecordsPanel;
    }
}