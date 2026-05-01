namespace LogScraper.Controls
{
    partial class LogRecordingControl
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogRecordingControl));
            BtnRecord = new System.Windows.Forms.Button();
            BtnRecordWithTimer = new System.Windows.Forms.Button();
            BtnStop = new System.Windows.Forms.Button();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // BtnRecord
            // 
            BtnRecord.Enabled = false;
            BtnRecord.Image = (System.Drawing.Image)resources.GetObject("BtnRecord.Image");
            BtnRecord.Location = new System.Drawing.Point(0, 0);
            BtnRecord.Name = "BtnRecord";
            BtnRecord.Size = new System.Drawing.Size(40, 40);
            BtnRecord.TabIndex = 18;
            BtnRecord.TabStop = false;
            BtnRecord.UseVisualStyleBackColor = true;
            BtnRecord.Click += BtnRecord_Click;
            // 
            // BtnRecordWithTimer
            // 
            BtnRecordWithTimer.Enabled = false;
            BtnRecordWithTimer.Image = Properties.Resources.timer_record_outline_24x24;
            BtnRecordWithTimer.Location = new System.Drawing.Point(39, 0);
            BtnRecordWithTimer.Name = "BtnRecordWithTimer";
            BtnRecordWithTimer.Size = new System.Drawing.Size(40, 40);
            BtnRecordWithTimer.TabIndex = 19;
            BtnRecordWithTimer.TabStop = false;
            BtnRecordWithTimer.UseVisualStyleBackColor = true;
            BtnRecordWithTimer.Click += BtnRecordWithTimer_Click;
            // 
            // BtnStop
            // 
            BtnStop.Image = (System.Drawing.Image)resources.GetObject("BtnStop.Image");
            BtnStop.Location = new System.Drawing.Point(0, 0);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new System.Drawing.Size(40, 40);
            BtnStop.TabIndex = 20;
            BtnStop.TabStop = false;
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // LogRecordingControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BtnRecord);
            Controls.Add(BtnRecordWithTimer);
            Controls.Add(BtnStop);
            Name = "LogRecordingControl";
            Size = new System.Drawing.Size(79, 40);
            Load += LogRecordingControl_Load;
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Button BtnRecord;
        public System.Windows.Forms.Button BtnRecordWithTimer;
        public System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.ToolTip ToolTip;
    }
}
