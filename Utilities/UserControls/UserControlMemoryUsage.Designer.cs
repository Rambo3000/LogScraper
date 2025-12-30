namespace LogScraper
{
    partial class UserControlMemoryUsage
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
            LblMemoryUsageValue = new System.Windows.Forms.Label();
            PnlUsedForScalingCompatibility = new System.Windows.Forms.Panel();
            PnlUsedForScalingCompatibility.SuspendLayout();
            SuspendLayout();
            // 
            // LblMemoryUsageValue
            // 
            LblMemoryUsageValue.Dock = System.Windows.Forms.DockStyle.Fill;
            LblMemoryUsageValue.Location = new System.Drawing.Point(0, 0);
            LblMemoryUsageValue.Name = "LblMemoryUsageValue";
            LblMemoryUsageValue.Size = new System.Drawing.Size(69, 17);
            LblMemoryUsageValue.TabIndex = 0;
            LblMemoryUsageValue.Text = "- MB";
            LblMemoryUsageValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PnlUsedForScalingCompatibility
            // 
            PnlUsedForScalingCompatibility.Controls.Add(LblMemoryUsageValue);
            PnlUsedForScalingCompatibility.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForScalingCompatibility.Location = new System.Drawing.Point(0, 0);
            PnlUsedForScalingCompatibility.Name = "PnlUsedForScalingCompatibility";
            PnlUsedForScalingCompatibility.Size = new System.Drawing.Size(69, 17);
            PnlUsedForScalingCompatibility.TabIndex = 1;
            // 
            // UserControlMemoryUsage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlUsedForScalingCompatibility);
            Name = "UserControlMemoryUsage";
            Size = new System.Drawing.Size(69, 17);
            Load += UserControlMemoryUsage_Load;
            PnlUsedForScalingCompatibility.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label LblMemoryUsageValue;
        private System.Windows.Forms.Panel PnlUsedForScalingCompatibility;
    }
}
