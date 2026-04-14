namespace LogScraper.Controls.FilterOverview
{
    partial class ActiveFilterOverviewControl
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
            FlowLayoutFilterChips = new System.Windows.Forms.FlowLayoutPanel();
            LblCount = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // FlowLayoutFilterChips
            // 
            FlowLayoutFilterChips.Location = new System.Drawing.Point(0, 0);
            FlowLayoutFilterChips.Name = "FlowLayoutFilterChips";
            FlowLayoutFilterChips.Size = new System.Drawing.Size(484, 20);
            FlowLayoutFilterChips.TabIndex = 0;
            FlowLayoutFilterChips.WrapContents = true;
            // 
            // LblCount
            // 
            LblCount.AutoSize = true;
            LblCount.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            LblCount.Location = new System.Drawing.Point(529, 0);
            LblCount.Name = "LblCount";
            LblCount.Size = new System.Drawing.Size(72, 15);
            LblCount.TabIndex = 1;
            LblCount.Text = string.Empty;
            // 
            // ActiveFilterOverviewControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LblCount);
            Controls.Add(FlowLayoutFilterChips);
            Name = "ActiveFilterOverviewControl";
            Size = new System.Drawing.Size(601, 20);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FlowLayoutFilterChips;
        private System.Windows.Forms.Label LblCount;
    }
}
