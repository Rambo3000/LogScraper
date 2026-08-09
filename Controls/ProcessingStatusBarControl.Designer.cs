namespace LogScraper.Controls
{
    partial class ProcessingStatusBarControl
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
            PicStatusIcon = new System.Windows.Forms.PictureBox();
            LblStatusText = new System.Windows.Forms.Label();
            PnlExportStatus = new System.Windows.Forms.Panel();
            PrgExportStatus = new System.Windows.Forms.ProgressBar();
            LblExportStatusText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)PicStatusIcon).BeginInit();
            PnlExportStatus.SuspendLayout();
            SuspendLayout();
            // 
            // PicStatusIcon
            // 
            PicStatusIcon.Dock = System.Windows.Forms.DockStyle.Left;
            PicStatusIcon.Location = new System.Drawing.Point(4, 0);
            PicStatusIcon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PicStatusIcon.Name = "PicStatusIcon";
            PicStatusIcon.Size = new System.Drawing.Size(20, 26);
            PicStatusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            PicStatusIcon.TabIndex = 0;
            PicStatusIcon.TabStop = false;
            // 
            // LblStatusText
            // 
            LblStatusText.Dock = System.Windows.Forms.DockStyle.Fill;
            LblStatusText.Location = new System.Drawing.Point(24, 0);
            LblStatusText.Name = "LblStatusText";
            LblStatusText.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            LblStatusText.Size = new System.Drawing.Size(419, 26);
            LblStatusText.TabIndex = 1;
            LblStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlExportStatus
            // 
            PnlExportStatus.Controls.Add(PrgExportStatus);
            PnlExportStatus.Controls.Add(LblExportStatusText);
            PnlExportStatus.Dock = System.Windows.Forms.DockStyle.Right;
            PnlExportStatus.Location = new System.Drawing.Point(287, 0);
            PnlExportStatus.Name = "PnlExportStatus";
            PnlExportStatus.Padding = new System.Windows.Forms.Padding(16, 3, 4, 3);
            PnlExportStatus.Size = new System.Drawing.Size(228, 26);
            PnlExportStatus.TabIndex = 2;
            PnlExportStatus.Visible = false;
            // 
            // PrgExportStatus
            // 
            PrgExportStatus.Dock = System.Windows.Forms.DockStyle.Right;
            PrgExportStatus.Location = new System.Drawing.Point(108, 3);
            PrgExportStatus.MarqueeAnimationSpeed = 30;
            PrgExportStatus.Name = "PrgExportStatus";
            PrgExportStatus.Size = new System.Drawing.Size(116, 20);
            PrgExportStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            PrgExportStatus.TabIndex = 1;
            // 
            // LblExportStatusText
            // 
            LblExportStatusText.Dock = System.Windows.Forms.DockStyle.Fill;
            LblExportStatusText.Location = new System.Drawing.Point(16, 3);
            LblExportStatusText.Name = "LblExportStatusText";
            LblExportStatusText.Size = new System.Drawing.Size(92, 20);
            LblExportStatusText.TabIndex = 0;
            LblExportStatusText.Text = "Writing file...";
            LblExportStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ProcessingStatusBarControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LblStatusText);
            Controls.Add(PnlExportStatus);
            Controls.Add(PicStatusIcon);
            Name = "ProcessingStatusBarControl";
            Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Size = new System.Drawing.Size(443, 26);
            ((System.ComponentModel.ISupportInitialize)PicStatusIcon).EndInit();
            PnlExportStatus.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox PicStatusIcon;
        private System.Windows.Forms.Label LblStatusText;
        private System.Windows.Forms.Panel PnlExportStatus;
        private System.Windows.Forms.ProgressBar PrgExportStatus;
        private System.Windows.Forms.Label LblExportStatusText;
    }
}
