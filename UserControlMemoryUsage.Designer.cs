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
            SuspendLayout();
            // 
            // LblMemoryUsageValue
            // 
            LblMemoryUsageValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblMemoryUsageValue.Location = new System.Drawing.Point(0, 0);
            LblMemoryUsageValue.Name = "LblMemoryUsageValue";
            LblMemoryUsageValue.Size = new System.Drawing.Size(69, 17);
            LblMemoryUsageValue.TabIndex = 0;
            LblMemoryUsageValue.Text = "123 MB";
            LblMemoryUsageValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UserControlMemoryUsage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LblMemoryUsageValue);
            Name = "UserControlMemoryUsage";
            Size = new System.Drawing.Size(69, 17);
            Load += UserControlMemoryUsage_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label LblMemoryUsageValue;
    }
}
