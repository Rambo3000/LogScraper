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
            label1 = new System.Windows.Forms.Label();
            GrpExportSettings = new System.Windows.Forms.GroupBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            TxtEditorLocation = new LogScraper.Extensions.ValidatedTextBox();
            TxtEditorDescription = new LogScraper.Extensions.ValidatedTextBox();
            TxtExportFileName = new LogScraper.Extensions.ValidatedTextBox();
            CboLogProviderType = new System.Windows.Forms.ComboBox();
            lblTimeout = new System.Windows.Forms.Label();
            TxtTimeOut = new LogScraper.Extensions.ValidatedTextBox();
            ChkExportToFile = new System.Windows.Forms.CheckBox();
            GrpExportSettings.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(0, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(132, 15);
            label1.TabIndex = 0;
            label1.Text = "Standaard logging bron";
            // 
            // GrpExportSettings
            // 
            GrpExportSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpExportSettings.Controls.Add(label4);
            GrpExportSettings.Controls.Add(label3);
            GrpExportSettings.Controls.Add(label2);
            GrpExportSettings.Controls.Add(TxtEditorLocation);
            GrpExportSettings.Controls.Add(TxtEditorDescription);
            GrpExportSettings.Controls.Add(TxtExportFileName);
            GrpExportSettings.Enabled = false;
            GrpExportSettings.Location = new System.Drawing.Point(6, 88);
            GrpExportSettings.Name = "GrpExportSettings";
            GrpExportSettings.Size = new System.Drawing.Size(639, 115);
            GrpExportSettings.TabIndex = 1;
            GrpExportSettings.TabStop = false;
            GrpExportSettings.Text = "Log wegschrijven instellingen";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 54);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(112, 15);
            label4.TabIndex = 10;
            label4.Text = "Omschrijving editor";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 83);
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
            TxtEditorLocation.Location = new System.Drawing.Point(154, 80);
            TxtEditorLocation.Name = "TxtEditorLocation";
            TxtEditorLocation.Size = new System.Drawing.Size(482, 23);
            TxtEditorLocation.TabIndex = 7;
            // 
            // TxtEditorDescription
            // 
            TxtEditorDescription.BackColor = System.Drawing.Color.MistyRose;
            TxtEditorDescription.IsRequired = true;
            TxtEditorDescription.Location = new System.Drawing.Point(154, 51);
            TxtEditorDescription.Name = "TxtEditorDescription";
            TxtEditorDescription.Size = new System.Drawing.Size(322, 23);
            TxtEditorDescription.TabIndex = 6;
            // 
            // TxtExportFileLocation
            // 
            TxtExportFileName.BackColor = System.Drawing.Color.MistyRose;
            TxtExportFileName.IsRequired = true;
            TxtExportFileName.Location = new System.Drawing.Point(154, 22);
            TxtExportFileName.Name = "TxtExportFileLocation";
            TxtExportFileName.Size = new System.Drawing.Size(322, 23);
            TxtExportFileName.TabIndex = 5;
            // 
            // CboLogProviderType
            // 
            CboLogProviderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboLogProviderType.FormattingEnabled = true;
            CboLogProviderType.Location = new System.Drawing.Point(160, 3);
            CboLogProviderType.Name = "CboLogProviderType";
            CboLogProviderType.Size = new System.Drawing.Size(121, 23);
            CboLogProviderType.TabIndex = 2;
            // 
            // lblTimeout
            // 
            lblTimeout.AutoSize = true;
            lblTimeout.Location = new System.Drawing.Point(0, 35);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new System.Drawing.Size(137, 15);
            lblTimeout.TabIndex = 3;
            lblTimeout.Text = "Timeout downloaden (s)";
            // 
            // TxtTimeOut
            // 
            TxtTimeOut.BackColor = System.Drawing.Color.MistyRose;
            TxtTimeOut.IsRequired = true;
            TxtTimeOut.Location = new System.Drawing.Point(160, 30);
            TxtTimeOut.Name = "TxtTimeOut";
            TxtTimeOut.Size = new System.Drawing.Size(121, 23);
            TxtTimeOut.TabIndex = 4;
            // 
            // ChkExportToFile
            // 
            ChkExportToFile.AutoSize = true;
            ChkExportToFile.Location = new System.Drawing.Point(6, 63);
            ChkExportToFile.Name = "ChkExportToFile";
            ChkExportToFile.Size = new System.Drawing.Size(241, 19);
            ChkExportToFile.TabIndex = 5;
            ChkExportToFile.Text = "Schrijf de gefilterde log naar een bestand";
            ChkExportToFile.UseVisualStyleBackColor = true;
            ChkExportToFile.CheckedChanged += ChkExportToFile_CheckedChanged;
            // 
            // UserControlGenericConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(ChkExportToFile);
            Controls.Add(TxtTimeOut);
            Controls.Add(lblTimeout);
            Controls.Add(CboLogProviderType);
            Controls.Add(label1);
            Controls.Add(GrpExportSettings);
            Name = "UserControlGenericConfig";
            Size = new System.Drawing.Size(645, 241);
            GrpExportSettings.ResumeLayout(false);
            GrpExportSettings.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GrpExportSettings;
        private System.Windows.Forms.ComboBox CboLogProviderType;
        private System.Windows.Forms.Label lblTimeout;
        private Extensions.ValidatedTextBox TxtTimeOut;
        private System.Windows.Forms.CheckBox ChkExportToFile;
        private System.Windows.Forms.Label label2;
        private Extensions.ValidatedTextBox TxtEditorLocation;
        private Extensions.ValidatedTextBox TxtEditorDescription;
        private Extensions.ValidatedTextBox TxtExportFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}
