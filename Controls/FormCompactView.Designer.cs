namespace LogScraper.Controls
{
    partial class FormCompactView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompactView));
            BtnRecord = new System.Windows.Forms.Button();
            BtnRecordWithTimer = new System.Windows.Forms.Button();
            LblCount = new System.Windows.Forms.Label();
            btnStop = new System.Windows.Forms.Button();
            btnErase = new System.Windows.Forms.Button();
            btnBack = new System.Windows.Forms.Button();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            LblErrorCount = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // BtnRecord
            // 
            BtnRecord.Image = (System.Drawing.Image)resources.GetObject("BtnRecord.Image");
            BtnRecord.Location = new System.Drawing.Point(6, 5);
            BtnRecord.Name = "BtnRecord";
            BtnRecord.Size = new System.Drawing.Size(40, 40);
            BtnRecord.TabIndex = 0;
            ToolTip.SetToolTip(BtnRecord, "Lees log eenmalig uit");
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
            ToolTip.SetToolTip(BtnRecordWithTimer, "Lees uit voor meerdere minuten [CTRL-S]");
            BtnRecordWithTimer.UseVisualStyleBackColor = true;
            BtnRecordWithTimer.Click += BtnRecordWithTimer_Click;
            // 
            // LblCount
            // 
            LblCount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblCount.ForeColor = System.Drawing.Color.DimGray;
            LblCount.Location = new System.Drawing.Point(0, 48);
            LblCount.Name = "LblCount";
            LblCount.Size = new System.Drawing.Size(163, 15);
            LblCount.TabIndex = 8;
            LblCount.Text = "12.345 / 123.456";
            LblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ToolTip.SetToolTip(LblCount, "Zichtbare logregels / Totaal aantal logregels");
            LblCount.Click += LblCount_Click;
            // 
            // btnStop
            // 
            btnStop.Image = (System.Drawing.Image)resources.GetObject("btnStop.Image");
            btnStop.Location = new System.Drawing.Point(6, 5);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(40, 40);
            btnStop.TabIndex = 10;
            ToolTip.SetToolTip(btnStop, "Stop [CTRL-S]");
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Visible = false;
            btnStop.Click += BtnStop_Click;
            // 
            // btnErase
            // 
            btnErase.Image = (System.Drawing.Image)resources.GetObject("btnErase.Image");
            btnErase.Location = new System.Drawing.Point(137, 5);
            btnErase.Name = "btnErase";
            btnErase.Size = new System.Drawing.Size(25, 25);
            btnErase.TabIndex = 11;
            ToolTip.SetToolTip(btnErase, "Wis het log");
            btnErase.UseVisualStyleBackColor = true;
            btnErase.Click += BtnErase_Click;
            // 
            // btnBack
            // 
            btnBack.Image = (System.Drawing.Image)resources.GetObject("btnBack.Image");
            btnBack.Location = new System.Drawing.Point(91, 5);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(40, 40);
            btnBack.TabIndex = 14;
            ToolTip.SetToolTip(btnBack, "Compacte weergave sluiten [CTRL-R]");
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += BtnBack_Click;
            // 
            // LblErrorCount
            // 
            LblErrorCount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblErrorCount.ForeColor = System.Drawing.Color.DimGray;
            LblErrorCount.Location = new System.Drawing.Point(0, 66);
            LblErrorCount.Name = "LblErrorCount";
            LblErrorCount.Size = new System.Drawing.Size(163, 15);
            LblErrorCount.TabIndex = 13;
            LblErrorCount.Text = "0 errors";
            LblErrorCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormCompactView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(166, 86);
            Controls.Add(btnBack);
            Controls.Add(LblErrorCount);
            Controls.Add(btnErase);
            Controls.Add(LblCount);
            Controls.Add(BtnRecord);
            Controls.Add(btnStop);
            Controls.Add(BtnRecordWithTimer);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCompactView";
            Text = " LogScraper";
            TopMost = true;
            FormClosing += FormMiniTop_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Button BtnRecord;
        public System.Windows.Forms.Button BtnRecordWithTimer;
        public System.Windows.Forms.Label LblCount;
        public System.Windows.Forms.Label lblLogEntriesTotalCount;
        public System.Windows.Forms.Button btnStop;
        public System.Windows.Forms.Button btnErase;
        public System.Windows.Forms.Label lblError;
        public System.Windows.Forms.Button btnBack;
        public System.Windows.Forms.Button btnOpenWithEditor;
        private System.Windows.Forms.ToolTip ToolTip;
        public System.Windows.Forms.Label LblErrorCount;
    }
}