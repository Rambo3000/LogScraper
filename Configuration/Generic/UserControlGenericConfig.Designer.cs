namespace LogScraper.Configuration
{
    partial class UserControlGenericConfig
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
            LblDefaultLogProviderType = new System.Windows.Forms.Label();
            GrpExportSettings = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            TxtEditorLocation = new LogScraper.Extensions.ValidatedTextBox();
            TxtExportFileName = new LogScraper.Extensions.ValidatedTextBox();
            ChkExportToFile = new System.Windows.Forms.CheckBox();
            CboLogProviderType = new System.Windows.Forms.ComboBox();
            LblTimeout = new System.Windows.Forms.Label();
            TxtTimeOut = new LogScraper.Extensions.ValidatedTextBox();
            CboAutomaticReadTime = new System.Windows.Forms.ComboBox();
            LblAutomaticReadTimea = new System.Windows.Forms.Label();
            GrpGeneralSettings = new System.Windows.Forms.GroupBox();
            GrpExportSettings.SuspendLayout();
            GrpGeneralSettings.SuspendLayout();
            SuspendLayout();
            // 
            // LblDefaultLogProviderType
            // 
            LblDefaultLogProviderType.AutoSize = true;
            LblDefaultLogProviderType.Location = new System.Drawing.Point(6, 19);
            LblDefaultLogProviderType.Name = "LblDefaultLogProviderType";
            LblDefaultLogProviderType.Size = new System.Drawing.Size(132, 15);
            LblDefaultLogProviderType.TabIndex = 0;
            LblDefaultLogProviderType.Text = "Standaard logging bron";
            // 
            // GrpExportSettings
            // 
            GrpExportSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpExportSettings.Controls.Add(label3);
            GrpExportSettings.Controls.Add(label2);
            GrpExportSettings.Controls.Add(TxtEditorLocation);
            GrpExportSettings.Controls.Add(TxtExportFileName);
            GrpExportSettings.Enabled = false;
            GrpExportSettings.Location = new System.Drawing.Point(29, 128);
            GrpExportSettings.Name = "GrpExportSettings";
            GrpExportSettings.Size = new System.Drawing.Size(813, 82);
            GrpExportSettings.TabIndex = 1;
            GrpExportSettings.TabStop = false;
            GrpExportSettings.Text = "Log wegschrijven instellingen";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 54);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(79, 15);
            label3.TabIndex = 9;
            label3.Text = "Locatie editor";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 25);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(84, 15);
            label2.TabIndex = 8;
            label2.Text = "Bestandsnaam";
            // 
            // TxtEditorLocation
            // 
            TxtEditorLocation.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtEditorLocation.BackColor = System.Drawing.Color.MistyRose;
            TxtEditorLocation.IsRequired = true;
            TxtEditorLocation.IsWhiteSpaceAllowed = false;
            TxtEditorLocation.Location = new System.Drawing.Point(154, 51);
            TxtEditorLocation.Name = "TxtEditorLocation";
            TxtEditorLocation.Size = new System.Drawing.Size(656, 23);
            TxtEditorLocation.TabIndex = 7;
            // 
            // TxtExportFileName
            // 
            TxtExportFileName.BackColor = System.Drawing.Color.MistyRose;
            TxtExportFileName.IsRequired = true;
            TxtExportFileName.IsWhiteSpaceAllowed = false;
            TxtExportFileName.Location = new System.Drawing.Point(154, 22);
            TxtExportFileName.Name = "TxtExportFileName";
            TxtExportFileName.Size = new System.Drawing.Size(322, 23);
            TxtExportFileName.TabIndex = 5;
            // 
            // ChkExportToFile
            // 
            ChkExportToFile.AutoSize = true;
            ChkExportToFile.Location = new System.Drawing.Point(6, 103);
            ChkExportToFile.Name = "ChkExportToFile";
            ChkExportToFile.Size = new System.Drawing.Size(241, 19);
            ChkExportToFile.TabIndex = 5;
            ChkExportToFile.Text = "Schrijf de gefilterde log naar een bestand";
            ChkExportToFile.UseVisualStyleBackColor = true;
            ChkExportToFile.CheckedChanged += ChkExportToFile_CheckedChanged;
            // 
            // CboLogProviderType
            // 
            CboLogProviderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboLogProviderType.FormattingEnabled = true;
            CboLogProviderType.Location = new System.Drawing.Point(166, 16);
            CboLogProviderType.Name = "CboLogProviderType";
            CboLogProviderType.Size = new System.Drawing.Size(121, 23);
            CboLogProviderType.TabIndex = 2;
            // 
            // LblTimeout
            // 
            LblTimeout.AutoSize = true;
            LblTimeout.Location = new System.Drawing.Point(3, 79);
            LblTimeout.Name = "LblTimeout";
            LblTimeout.Size = new System.Drawing.Size(137, 15);
            LblTimeout.TabIndex = 3;
            LblTimeout.Text = "Timeout downloaden (s)";
            // 
            // TxtTimeOut
            // 
            TxtTimeOut.BackColor = System.Drawing.Color.MistyRose;
            TxtTimeOut.IsRequired = true;
            TxtTimeOut.IsWhiteSpaceAllowed = false;
            TxtTimeOut.Location = new System.Drawing.Point(166, 74);
            TxtTimeOut.Name = "TxtTimeOut";
            TxtTimeOut.Size = new System.Drawing.Size(121, 23);
            TxtTimeOut.TabIndex = 4;
            // 
            // CboAutomaticReadTime
            // 
            CboAutomaticReadTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboAutomaticReadTime.FormattingEnabled = true;
            CboAutomaticReadTime.Location = new System.Drawing.Point(166, 45);
            CboAutomaticReadTime.Name = "CboAutomaticReadTime";
            CboAutomaticReadTime.Size = new System.Drawing.Size(121, 23);
            CboAutomaticReadTime.TabIndex = 7;
            // 
            // LblAutomaticReadTimea
            // 
            LblAutomaticReadTimea.AutoSize = true;
            LblAutomaticReadTimea.Location = new System.Drawing.Point(6, 48);
            LblAutomaticReadTimea.Name = "LblAutomaticReadTimea";
            LblAutomaticReadTimea.Size = new System.Drawing.Size(147, 15);
            LblAutomaticReadTimea.TabIndex = 6;
            LblAutomaticReadTimea.Text = "Automatisch lezen tijd (m)";
            // 
            // GrpGeneralSettings
            // 
            GrpGeneralSettings.Controls.Add(LblDefaultLogProviderType);
            GrpGeneralSettings.Controls.Add(CboAutomaticReadTime);
            GrpGeneralSettings.Controls.Add(GrpExportSettings);
            GrpGeneralSettings.Controls.Add(LblAutomaticReadTimea);
            GrpGeneralSettings.Controls.Add(CboLogProviderType);
            GrpGeneralSettings.Controls.Add(ChkExportToFile);
            GrpGeneralSettings.Controls.Add(LblTimeout);
            GrpGeneralSettings.Controls.Add(TxtTimeOut);
            GrpGeneralSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            GrpGeneralSettings.Location = new System.Drawing.Point(0, 0);
            GrpGeneralSettings.Name = "GrpGeneralSettings";
            GrpGeneralSettings.Size = new System.Drawing.Size(848, 215);
            GrpGeneralSettings.TabIndex = 8;
            GrpGeneralSettings.TabStop = false;
            GrpGeneralSettings.Text = "Algemene instellingen";
            // 
            // UserControlGenericConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(GrpGeneralSettings);
            Name = "UserControlGenericConfig";
            Size = new System.Drawing.Size(848, 215);
            GrpExportSettings.ResumeLayout(false);
            GrpExportSettings.PerformLayout();
            GrpGeneralSettings.ResumeLayout(false);
            GrpGeneralSettings.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label LblDefaultLogProviderType;
        private System.Windows.Forms.GroupBox GrpExportSettings;
        private System.Windows.Forms.ComboBox CboLogProviderType;
        private System.Windows.Forms.Label LblTimeout;
        private Extensions.ValidatedTextBox TxtTimeOut;
        private System.Windows.Forms.CheckBox ChkExportToFile;
        private System.Windows.Forms.Label label2;
        private Extensions.ValidatedTextBox TxtEditorLocation;
        private Extensions.ValidatedTextBox TxtExportFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CboAutomaticReadTime;
        private System.Windows.Forms.Label LblAutomaticReadTimea;
        private System.Windows.Forms.GroupBox GrpGeneralSettings;
    }
}
