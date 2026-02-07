namespace LogScraper.LogProviders.File
{
    partial class UserControlFileLogProvider
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlFileLogProvider));
            btnBrowse = new System.Windows.Forms.Button();
            PnlUsedForScalingCompatibility = new System.Windows.Forms.Panel();
            PctWarning = new System.Windows.Forms.PictureBox();
            LblFileSizeValue = new System.Windows.Forms.Label();
            LblModificationDateTimeValue = new System.Windows.Forms.Label();
            LblModificationDateTime = new System.Windows.Forms.Label();
            LblFileSize = new System.Windows.Forms.Label();
            txtFilePath = new System.Windows.Forms.TextBox();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            PnlUsedForScalingCompatibility.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PctWarning).BeginInit();
            SuspendLayout();
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBrowse.Image = (System.Drawing.Image)resources.GetObject("btnBrowse.Image");
            btnBrowse.Location = new System.Drawing.Point(256, 3);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(36, 36);
            btnBrowse.TabIndex = 1;
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += BtnBrowse_Click;
            // 
            // PnlUsedForScalingCompatibility
            // 
            PnlUsedForScalingCompatibility.Controls.Add(PctWarning);
            PnlUsedForScalingCompatibility.Controls.Add(LblFileSizeValue);
            PnlUsedForScalingCompatibility.Controls.Add(LblModificationDateTimeValue);
            PnlUsedForScalingCompatibility.Controls.Add(LblModificationDateTime);
            PnlUsedForScalingCompatibility.Controls.Add(LblFileSize);
            PnlUsedForScalingCompatibility.Controls.Add(btnBrowse);
            PnlUsedForScalingCompatibility.Controls.Add(txtFilePath);
            PnlUsedForScalingCompatibility.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForScalingCompatibility.Location = new System.Drawing.Point(0, 0);
            PnlUsedForScalingCompatibility.Name = "PnlUsedForScalingCompatibility";
            PnlUsedForScalingCompatibility.Size = new System.Drawing.Size(295, 113);
            PnlUsedForScalingCompatibility.TabIndex = 3;
            // 
            // PctWarning
            // 
            PctWarning.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            PctWarning.ErrorImage = null;
            PctWarning.Image = (System.Drawing.Image)resources.GetObject("PctWarning.Image");
            PctWarning.Location = new System.Drawing.Point(261, 79);
            PctWarning.Name = "PctWarning";
            PctWarning.Size = new System.Drawing.Size(24, 24);
            PctWarning.TabIndex = 7;
            PctWarning.TabStop = false;
            toolTip1.SetToolTip(PctWarning, "Groot bestand, inlezen, verwerken en navigeren duurt mogelijk lang en gebruikt veel geheugen");
            PctWarning.Visible = false;
            // 
            // LblFileSizeValue
            // 
            LblFileSizeValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LblFileSizeValue.AutoSize = true;
            LblFileSizeValue.Location = new System.Drawing.Point(103, 94);
            LblFileSizeValue.Name = "LblFileSizeValue";
            LblFileSizeValue.Size = new System.Drawing.Size(12, 15);
            LblFileSizeValue.TabIndex = 6;
            LblFileSizeValue.Text = "-";
            // 
            // LblModificationDateTimeValue
            // 
            LblModificationDateTimeValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LblModificationDateTimeValue.AutoSize = true;
            LblModificationDateTimeValue.Location = new System.Drawing.Point(103, 76);
            LblModificationDateTimeValue.Name = "LblModificationDateTimeValue";
            LblModificationDateTimeValue.Size = new System.Drawing.Size(12, 15);
            LblModificationDateTimeValue.TabIndex = 5;
            LblModificationDateTimeValue.Text = "-";
            // 
            // LblModificationDateTime
            // 
            LblModificationDateTime.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LblModificationDateTime.AutoSize = true;
            LblModificationDateTime.Location = new System.Drawing.Point(3, 76);
            LblModificationDateTime.Name = "LblModificationDateTime";
            LblModificationDateTime.Size = new System.Drawing.Size(91, 15);
            LblModificationDateTime.TabIndex = 4;
            LblModificationDateTime.Text = "Laatst gewijzigd";
            // 
            // LblFileSize
            // 
            LblFileSize.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LblFileSize.AutoSize = true;
            LblFileSize.Location = new System.Drawing.Point(3, 94);
            LblFileSize.Name = "LblFileSize";
            LblFileSize.Size = new System.Drawing.Size(93, 15);
            LblFileSize.TabIndex = 3;
            LblFileSize.Text = "Bestandsgrootte";
            // 
            // txtFilePath
            // 
            txtFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtFilePath.BackColor = System.Drawing.SystemColors.Control;
            txtFilePath.Location = new System.Drawing.Point(3, 3);
            txtFilePath.Multiline = true;
            txtFilePath.Name = "txtFilePath";
            txtFilePath.ReadOnly = true;
            txtFilePath.Size = new System.Drawing.Size(247, 70);
            txtFilePath.TabIndex = 0;
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 1;
            toolTip1.AutoPopDelay = 9999999;
            toolTip1.InitialDelay = 1;
            toolTip1.ReshowDelay = 1;
            // 
            // UserControlFileLogProvider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlUsedForScalingCompatibility);
            Name = "UserControlFileLogProvider";
            Size = new System.Drawing.Size(295, 113);
            PnlUsedForScalingCompatibility.ResumeLayout(false);
            PnlUsedForScalingCompatibility.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PctWarning).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Panel PnlUsedForScalingCompatibility;
        private System.Windows.Forms.Label LblFileSize;
        private System.Windows.Forms.Label LblFileSizeValue;
        private System.Windows.Forms.Label LblModificationDateTimeValue;
        private System.Windows.Forms.Label LblModificationDateTime;
        private System.Windows.Forms.PictureBox PctWarning;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtFilePath;
    }
}
