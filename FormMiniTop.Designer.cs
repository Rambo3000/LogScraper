namespace LogScraper
{
    partial class FormMiniTop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMiniTop));
            btnRead = new System.Windows.Forms.Button();
            btnRead1Minute = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblLogLinesFilteredCount = new System.Windows.Forms.Label();
            lblLogLinesTotalCount = new System.Windows.Forms.Label();
            btnStop = new System.Windows.Forms.Button();
            btnReset = new System.Windows.Forms.Button();
            lblLogLinesFilteredWithErrorCount = new System.Windows.Forms.Label();
            lblError = new System.Windows.Forms.Label();
            btnBack = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // btnRead
            // 
            btnRead.Location = new System.Drawing.Point(6, 5);
            btnRead.Name = "btnRead";
            btnRead.Size = new System.Drawing.Size(88, 23);
            btnRead.TabIndex = 0;
            btnRead.Text = "Lees";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += BtnRead_Click;
            // 
            // btnRead1Minute
            // 
            btnRead1Minute.Location = new System.Drawing.Point(6, 28);
            btnRead1Minute.Name = "btnRead1Minute";
            btnRead1Minute.Size = new System.Drawing.Size(88, 23);
            btnRead1Minute.TabIndex = 1;
            btnRead1Minute.Text = "Lees 1 min";
            btnRead1Minute.UseVisualStyleBackColor = true;
            btnRead1Minute.Click += BtnRead1Minute_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(95, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(41, 15);
            label1.TabIndex = 2;
            label1.Text = "Regels";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(95, 34);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(52, 15);
            label2.TabIndex = 3;
            label2.Text = "Gefilterd";
            // 
            // lblLogLinesFilteredCount
            // 
            lblLogLinesFilteredCount.Location = new System.Drawing.Point(149, 34);
            lblLogLinesFilteredCount.Name = "lblLogLinesFilteredCount";
            lblLogLinesFilteredCount.Size = new System.Drawing.Size(63, 15);
            lblLogLinesFilteredCount.TabIndex = 8;
            lblLogLinesFilteredCount.Text = "0";
            lblLogLinesFilteredCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLogLinesTotalCount
            // 
            lblLogLinesTotalCount.Location = new System.Drawing.Point(149, 9);
            lblLogLinesTotalCount.Name = "lblLogLinesTotalCount";
            lblLogLinesTotalCount.Size = new System.Drawing.Size(63, 15);
            lblLogLinesTotalCount.TabIndex = 9;
            lblLogLinesTotalCount.Text = "0";
            lblLogLinesTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStop
            // 
            btnStop.Location = new System.Drawing.Point(6, 28);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(88, 23);
            btnStop.TabIndex = 10;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Visible = false;
            btnStop.Click += BtnStop_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new System.Drawing.Point(6, 51);
            btnReset.Name = "btnReset";
            btnReset.Size = new System.Drawing.Size(88, 23);
            btnReset.TabIndex = 11;
            btnReset.Text = "Wis en Lees";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += BtnReset_Click;
            // 
            // lblLogLinesFilteredWithErrorCount
            // 
            lblLogLinesFilteredWithErrorCount.Location = new System.Drawing.Point(149, 59);
            lblLogLinesFilteredWithErrorCount.Name = "lblLogLinesFilteredWithErrorCount";
            lblLogLinesFilteredWithErrorCount.Size = new System.Drawing.Size(63, 15);
            lblLogLinesFilteredWithErrorCount.TabIndex = 13;
            lblLogLinesFilteredWithErrorCount.Text = "0";
            lblLogLinesFilteredWithErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Location = new System.Drawing.Point(95, 59);
            lblError.Name = "lblError";
            lblError.Size = new System.Drawing.Size(32, 15);
            lblError.TabIndex = 12;
            lblError.Text = "Error";
            // 
            // btnBack
            // 
            btnBack.Location = new System.Drawing.Point(6, 74);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(88, 23);
            btnBack.TabIndex = 14;
            btnBack.Text = "Terug";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += BtnBack_Click;
            // 
            // FormMiniTop
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(213, 99);
            Controls.Add(btnBack);
            Controls.Add(lblLogLinesFilteredWithErrorCount);
            Controls.Add(lblError);
            Controls.Add(btnReset);
            Controls.Add(lblLogLinesTotalCount);
            Controls.Add(lblLogLinesFilteredCount);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnRead);
            Controls.Add(btnStop);
            Controls.Add(btnRead1Minute);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormMiniTop";
            Text = " Log Scraper";
            TopMost = true;
            FormClosing += FormMiniTop_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public System.Windows.Forms.Button btnRead;
        public System.Windows.Forms.Button btnRead1Minute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblLogLinesFilteredCount;
        public System.Windows.Forms.Label lblLogLinesTotalCount;
        public System.Windows.Forms.Button btnStop;
        public System.Windows.Forms.Button btnReset;
        public System.Windows.Forms.Label lblLogLinesFilteredWithErrorCount;
        public System.Windows.Forms.Label lblError;
        public System.Windows.Forms.Button btnBack;
    }
}