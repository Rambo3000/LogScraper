namespace LogScraper.Controls.Metadata
{
    partial class UserControlMetadataFilterOverview
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
            LblExplenation = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // LblExplenation
            // 
            LblExplenation.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblExplenation.ForeColor = System.Drawing.SystemColors.ControlDark;
            LblExplenation.Location = new System.Drawing.Point(0, 0);
            LblExplenation.Margin = new System.Windows.Forms.Padding(0);
            LblExplenation.Name = "LblExplenation";
            LblExplenation.Padding = new System.Windows.Forms.Padding(5);
            LblExplenation.Size = new System.Drawing.Size(150, 65);
            LblExplenation.TabIndex = 0;
            LblExplenation.Text = "Metadata filters";
            LblExplenation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // UserControlMetadataFilterOverview
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(LblExplenation);
            Margin = new System.Windows.Forms.Padding(0);
            Name = "UserControlMetadataFilterOverview";
            Load += UserControlMetadataFilterOverview_Load;
            Resize += UserControlMetadataFilterOverview_Resize;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label LblExplenation;
    }
}
