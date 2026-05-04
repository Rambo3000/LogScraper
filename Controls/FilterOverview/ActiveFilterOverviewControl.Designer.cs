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
            components = new System.ComponentModel.Container();
            FlowLayoutFilterChips = new System.Windows.Forms.FlowLayoutPanel();
            LblCount = new System.Windows.Forms.Label();
            LblReset = new System.Windows.Forms.LinkLabel();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // FlowLayoutFilterChips
            // 
            FlowLayoutFilterChips.Location = new System.Drawing.Point(0, 0);
            FlowLayoutFilterChips.Name = "FlowLayoutFilterChips";
            FlowLayoutFilterChips.Size = new System.Drawing.Size(484, 20);
            FlowLayoutFilterChips.TabIndex = 0;
            // 
            // LblCount
            // 
            LblCount.AutoSize = true;
            LblCount.ForeColor = System.Drawing.Color.DimGray;
            LblCount.Location = new System.Drawing.Point(529, 3);
            LblCount.Name = "LblCount";
            LblCount.Size = new System.Drawing.Size(69, 15);
            LblCount.TabIndex = 1;
            LblCount.Text = "123 / 12.345";
            // 
            // LblReset
            // 
            LblReset.ActiveLinkColor = System.Drawing.Color.DarkGray;
            LblReset.AutoSize = true;
            LblReset.DisabledLinkColor = System.Drawing.Color.DarkGray;
            LblReset.ForeColor = System.Drawing.Color.DarkGray;
            LblReset.LinkColor = System.Drawing.Color.DarkGray;
            LblReset.Location = new System.Drawing.Point(553, 1);
            LblReset.Name = "LblReset";
            LblReset.Size = new System.Drawing.Size(32, 15);
            LblReset.TabIndex = 2;
            LblReset.TabStop = true;
            LblReset.Text = "reset";
            toolTip1.SetToolTip(LblReset, "Verwijder alle filters en toon alle logregels");
            LblReset.LinkClicked += LblReset_LinkClicked;
            // 
            // ActiveFilterOverviewControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LblReset);
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
        private System.Windows.Forms.LinkLabel LblReset;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
