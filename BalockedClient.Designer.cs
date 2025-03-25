using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace ServerSide
{
    partial class BalockedClient
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
            this.CooBackground = new System.Windows.Forms.Panel();
            this.MotherBoardlbl = new System.Windows.Forms.Label();
            this.ConnetAttemps = new System.Windows.Forms.Label();
            this.DateOfBlock = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CooBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // CooBackground
            // 
            this.CooBackground.Controls.Add(this.MotherBoardlbl);
            this.CooBackground.Controls.Add(this.ConnetAttemps);
            this.CooBackground.Dock = System.Windows.Forms.DockStyle.Top;
            this.CooBackground.Location = new System.Drawing.Point(0, 0);
            this.CooBackground.Name = "CooBackground";
            this.CooBackground.Size = new System.Drawing.Size(457, 32);
            this.CooBackground.TabIndex = 0;
            this.CooBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.set_background);
            // 
            // MotherBoardlbl
            // 
            this.MotherBoardlbl.AutoSize = true;
            this.MotherBoardlbl.BackColor = System.Drawing.Color.Transparent;
            this.MotherBoardlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.MotherBoardlbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MotherBoardlbl.Location = new System.Drawing.Point(17, 7);
            this.MotherBoardlbl.Name = "MotherBoardlbl";
            this.MotherBoardlbl.Size = new System.Drawing.Size(105, 18);
            this.MotherBoardlbl.TabIndex = 0;
            this.MotherBoardlbl.Text = "Z8ALB8WHRJ";
            // 
            // ConnetAttemps
            // 
            this.ConnetAttemps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnetAttemps.AutoSize = true;
            this.ConnetAttemps.BackColor = System.Drawing.Color.Transparent;
            this.ConnetAttemps.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnetAttemps.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ConnetAttemps.Location = new System.Drawing.Point(428, 7);
            this.ConnetAttemps.Name = "ConnetAttemps";
            this.ConnetAttemps.Size = new System.Drawing.Size(18, 20);
            this.ConnetAttemps.TabIndex = 1;
            this.ConnetAttemps.Text = "0";
            this.ConnetAttemps.Click += new System.EventHandler(this.ConnetAttemps_Click);
            // 
            // DateOfBlock
            // 
            this.DateOfBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DateOfBlock.BackColor = System.Drawing.Color.White;
            this.DateOfBlock.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateOfBlock.Location = new System.Drawing.Point(367, 32);
            this.DateOfBlock.Margin = new System.Windows.Forms.Padding(0);
            this.DateOfBlock.Name = "DateOfBlock";
            this.DateOfBlock.Size = new System.Drawing.Size(90, 18);
            this.DateOfBlock.TabIndex = 2;
            this.DateOfBlock.Text = "15-4-2025";
            this.DateOfBlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 18);
            this.panel1.TabIndex = 3;
            // 
            // BalockedClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.DateOfBlock);
            this.Controls.Add(this.CooBackground);
            this.Name = "BalockedClient";
            this.Size = new System.Drawing.Size(457, 50);
            this.CooBackground.ResumeLayout(false);
            this.CooBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel CooBackground;
        private System.Windows.Forms.Label ConnetAttemps;
        private System.Windows.Forms.Label DateOfBlock;
        private void set_background(object sender, PaintEventArgs e)
        {
            // Get the graphics object
            Graphics graphics = e.Graphics;

            // Create a rectangle that fills the entire form
            Rectangle gradientRectangle = new Rectangle(0, 0, Width, Height);

            // Create the gradient brush: from dark red to orange-red (professional colors for blocking or warning)
            Brush brush = new LinearGradientBrush(gradientRectangle, Color.FromArgb(139, 0, 0), Color.FromArgb(255, 69, 0), 45f);

            // Fill the rectangle with the gradient
            graphics.FillRectangle(brush, gradientRectangle);
        }

        private Label MotherBoardlbl;
        private Panel panel1;
    }
}
