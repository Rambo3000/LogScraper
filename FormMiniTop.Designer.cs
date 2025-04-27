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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMiniTop));
            BtnRecord = new System.Windows.Forms.Button();
            BtnRecordWithTimer = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblLogEntriesFilteredCount = new System.Windows.Forms.Label();
            lblLogEntriesTotalCount = new System.Windows.Forms.Label();
            btnStop = new System.Windows.Forms.Button();
            btnErase = new System.Windows.Forms.Button();
            lblLogEntriesFilteredWithErrorCount = new System.Windows.Forms.Label();
            lblError = new System.Windows.Forms.Label();
            btnBack = new System.Windows.Forms.Button();
            btnOpenWithEditor = new System.Windows.Forms.Button();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // BtnRecord
            // 
            BtnRecord.Image = (System.Drawing.Image)resources.GetObject("BtnRecord.Image");
            BtnRecord.Location = new System.Drawing.Point(6, 5);
            BtnRecord.Name = "BtnRecord";
            BtnRecord.Size = new System.Drawing.Size(40, 40);
            BtnRecord.TabIndex = 0;
            BtnRecord.UseVisualStyleBackColor = true;
            BtnRecord.Click += BtnRecord_Click;
            // 
            // BtnRecordWithTimer
            // 
            BtnRecordWithTimer.Image = (System.Drawing.Image)resources.GetObject("BtnRecordWithTimer.Image");
            BtnRecordWithTimer.Location = new System.Drawing.Point(45, 5);
            BtnRecordWithTimer.Name = "BtnRecordWithTimer";
            BtnRecordWithTimer.Size = new System.Drawing.Size(40, 40);
            BtnRecordWithTimer.TabIndex = 1;
            BtnRecordWithTimer.UseVisualStyleBackColor = true;
            BtnRecordWithTimer.Click += BtnRecordWithTimer_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 48);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(41, 15);
            label1.TabIndex = 2;
            label1.Text = "Regels";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 65);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(52, 15);
            label2.TabIndex = 3;
            label2.Text = "Gefilterd";
            // 
            // lblLogEntriesFilteredCount
            // 
            lblLogEntriesFilteredCount.Location = new System.Drawing.Point(60, 65);
            lblLogEntriesFilteredCount.Name = "lblLogEntriesFilteredCount";
            lblLogEntriesFilteredCount.Size = new System.Drawing.Size(63, 15);
            lblLogEntriesFilteredCount.TabIndex = 8;
            lblLogEntriesFilteredCount.Text = "0";
            lblLogEntriesFilteredCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLogEntriesTotalCount
            // 
            lblLogEntriesTotalCount.Location = new System.Drawing.Point(60, 48);
            lblLogEntriesTotalCount.Name = "lblLogEntriesTotalCount";
            lblLogEntriesTotalCount.Size = new System.Drawing.Size(63, 15);
            lblLogEntriesTotalCount.TabIndex = 9;
            lblLogEntriesTotalCount.Text = "0";
            lblLogEntriesTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnStop
            // 
            btnStop.Image = (System.Drawing.Image)resources.GetObject("btnStop.Image");
            btnStop.Location = new System.Drawing.Point(6, 5);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(40, 40);
            btnStop.TabIndex = 10;
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Visible = false;
            btnStop.Click += BtnStop_Click;
            // 
            // btnErase
            // 
            btnErase.Image = (System.Drawing.Image)resources.GetObject("btnErase.Image");
            btnErase.Location = new System.Drawing.Point(91, 5);
            btnErase.Name = "btnErase";
            btnErase.Size = new System.Drawing.Size(40, 40);
            btnErase.TabIndex = 11;
            btnErase.UseVisualStyleBackColor = true;
            btnErase.Click += BtnErase_Click;
            // 
            // lblLogEntriesFilteredWithErrorCount
            // 
            lblLogEntriesFilteredWithErrorCount.Location = new System.Drawing.Point(60, 82);
            lblLogEntriesFilteredWithErrorCount.Name = "lblLogEntriesFilteredWithErrorCount";
            lblLogEntriesFilteredWithErrorCount.Size = new System.Drawing.Size(63, 15);
            lblLogEntriesFilteredWithErrorCount.TabIndex = 13;
            lblLogEntriesFilteredWithErrorCount.Text = "0";
            lblLogEntriesFilteredWithErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Location = new System.Drawing.Point(6, 82);
            lblError.Name = "lblError";
            lblError.Size = new System.Drawing.Size(32, 15);
            lblError.TabIndex = 12;
            lblError.Text = "Error";
            // 
            // btnBack
            // 
            btnBack.Image = (System.Drawing.Image)resources.GetObject("btnBack.Image");
            btnBack.Location = new System.Drawing.Point(183, 5);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(40, 40);
            btnBack.TabIndex = 14;
            ToolTip.SetToolTip(btnBack, "Sluit dit venster en stop het uitlezen");
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += BtnBack_Click;
            // 
            // btnOpenWithEditor
            // 
            btnOpenWithEditor.Image = (System.Drawing.Image)resources.GetObject("btnOpenWithEditor.Image");
            btnOpenWithEditor.Location = new System.Drawing.Point(137, 5);
            btnOpenWithEditor.Name = "btnOpenWithEditor";
            btnOpenWithEditor.Size = new System.Drawing.Size(40, 40);
            btnOpenWithEditor.TabIndex = 15;
            btnOpenWithEditor.UseVisualStyleBackColor = true;
            btnOpenWithEditor.Click += BtnOpenWithEditor_Click;
            // 
            // FormMiniTop
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(229, 103);
            Controls.Add(btnOpenWithEditor);
            Controls.Add(btnBack);
            Controls.Add(lblLogEntriesFilteredWithErrorCount);
            Controls.Add(lblError);
            Controls.Add(btnErase);
            Controls.Add(lblLogEntriesTotalCount);
            Controls.Add(lblLogEntriesFilteredCount);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(BtnRecord);
            Controls.Add(btnStop);
            Controls.Add(BtnRecordWithTimer);
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

        public System.Windows.Forms.Button BtnRecord;
        public System.Windows.Forms.Button BtnRecordWithTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblLogEntriesFilteredCount;
        public System.Windows.Forms.Label lblLogEntriesTotalCount;
        public System.Windows.Forms.Button btnStop;
        public System.Windows.Forms.Button btnErase;
        public System.Windows.Forms.Label lblLogEntriesFilteredWithErrorCount;
        public System.Windows.Forms.Label lblError;
        public System.Windows.Forms.Button btnBack;
        public System.Windows.Forms.Button btnOpenWithEditor;
        private System.Windows.Forms.ToolTip ToolTip;
    }
}