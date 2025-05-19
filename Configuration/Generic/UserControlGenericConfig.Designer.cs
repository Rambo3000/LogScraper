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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlGenericConfig));
            LblDefaultLogProviderType = new System.Windows.Forms.Label();
            GrpExportSettings = new System.Windows.Forms.GroupBox();
            BtnBrowseEditor = new System.Windows.Forms.Button();
            BtnBrowseExportFIle = new System.Windows.Forms.Button();
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
            pictureBox2 = new System.Windows.Forms.PictureBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            pictureBox16 = new System.Windows.Forms.PictureBox();
            ChkShowErrorsInBeginAndEndFilters = new System.Windows.Forms.CheckBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            ChkAutoToggleHierarchy = new System.Windows.Forms.CheckBox();
            pictureBox3 = new System.Windows.Forms.PictureBox();
            GrpExportSettings.SuspendLayout();
            GrpGeneralSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
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
            GrpExportSettings.Controls.Add(BtnBrowseEditor);
            GrpExportSettings.Controls.Add(BtnBrowseExportFIle);
            GrpExportSettings.Controls.Add(label3);
            GrpExportSettings.Controls.Add(label2);
            GrpExportSettings.Controls.Add(TxtEditorLocation);
            GrpExportSettings.Controls.Add(TxtExportFileName);
            GrpExportSettings.Enabled = false;
            GrpExportSettings.Location = new System.Drawing.Point(29, 178);
            GrpExportSettings.Name = "GrpExportSettings";
            GrpExportSettings.Size = new System.Drawing.Size(813, 82);
            GrpExportSettings.TabIndex = 1;
            GrpExportSettings.TabStop = false;
            GrpExportSettings.Text = "Log wegschrijven instellingen";
            // 
            // BtnBrowseEditor
            // 
            BtnBrowseEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnBrowseEditor.Location = new System.Drawing.Point(778, 51);
            BtnBrowseEditor.Name = "BtnBrowseEditor";
            BtnBrowseEditor.Size = new System.Drawing.Size(29, 23);
            BtnBrowseEditor.TabIndex = 11;
            BtnBrowseEditor.Text = "...";
            BtnBrowseEditor.UseVisualStyleBackColor = true;
            BtnBrowseEditor.Click += BtnBrowseEditor_Click;
            // 
            // BtnBrowseExportFIle
            // 
            BtnBrowseExportFIle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnBrowseExportFIle.Location = new System.Drawing.Point(778, 22);
            BtnBrowseExportFIle.Name = "BtnBrowseExportFIle";
            BtnBrowseExportFIle.Size = new System.Drawing.Size(29, 23);
            BtnBrowseExportFIle.TabIndex = 10;
            BtnBrowseExportFIle.Text = "...";
            BtnBrowseExportFIle.UseVisualStyleBackColor = true;
            BtnBrowseExportFIle.Click += BtnBrowseExportFile_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 54);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(62, 15);
            label3.TabIndex = 9;
            label3.Text = "Text editor";
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
            TxtEditorLocation.Size = new System.Drawing.Size(618, 23);
            TxtEditorLocation.TabIndex = 7;
            // 
            // TxtExportFileName
            // 
            TxtExportFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtExportFileName.BackColor = System.Drawing.Color.MistyRose;
            TxtExportFileName.IsRequired = true;
            TxtExportFileName.IsWhiteSpaceAllowed = false;
            TxtExportFileName.Location = new System.Drawing.Point(154, 22);
            TxtExportFileName.Name = "TxtExportFileName";
            TxtExportFileName.Size = new System.Drawing.Size(618, 23);
            TxtExportFileName.TabIndex = 5;
            // 
            // ChkExportToFile
            // 
            ChkExportToFile.AutoSize = true;
            ChkExportToFile.Location = new System.Drawing.Point(6, 153);
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
            GrpGeneralSettings.Controls.Add(pictureBox3);
            GrpGeneralSettings.Controls.Add(ChkAutoToggleHierarchy);
            GrpGeneralSettings.Controls.Add(pictureBox2);
            GrpGeneralSettings.Controls.Add(pictureBox1);
            GrpGeneralSettings.Controls.Add(pictureBox16);
            GrpGeneralSettings.Controls.Add(ChkShowErrorsInBeginAndEndFilters);
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
            GrpGeneralSettings.Size = new System.Drawing.Size(848, 304);
            GrpGeneralSettings.TabIndex = 8;
            GrpGeneralSettings.TabStop = false;
            GrpGeneralSettings.Text = "Algemene instellingen";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.help;
            pictureBox2.Location = new System.Drawing.Point(253, 153);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(16, 16);
            pictureBox2.TabIndex = 38;
            pictureBox2.TabStop = false;
            toolTip.SetToolTip(pictureBox2, "Indien aangevinkt wordt er bij elke wijziging van het log deze ook naar het geselecteerde bestand weggeschreven.");
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.help;
            pictureBox1.Location = new System.Drawing.Point(327, 103);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(16, 16);
            pictureBox1.TabIndex = 37;
            pictureBox1.TabStop = false;
            toolTip.SetToolTip(pictureBox1, resources.GetString("pictureBox1.ToolTip"));
            // 
            // pictureBox16
            // 
            pictureBox16.Image = Properties.Resources.help;
            pictureBox16.Location = new System.Drawing.Point(293, 79);
            pictureBox16.Name = "pictureBox16";
            pictureBox16.Size = new System.Drawing.Size(16, 16);
            pictureBox16.TabIndex = 36;
            pictureBox16.TabStop = false;
            toolTip.SetToolTip(pictureBox16, "Dit betreft het aantal seconden dat een enkele download mag duren. Indien er grote bestanden gedownload moeten worden dan kan deze waarde verhoogd worden.");
            // 
            // ChkShowErrorsInBeginAndEndFilters
            // 
            ChkShowErrorsInBeginAndEndFilters.AutoSize = true;
            ChkShowErrorsInBeginAndEndFilters.Location = new System.Drawing.Point(6, 103);
            ChkShowErrorsInBeginAndEndFilters.Name = "ChkShowErrorsInBeginAndEndFilters";
            ChkShowErrorsInBeginAndEndFilters.Size = new System.Drawing.Size(315, 19);
            ChkShowErrorsInBeginAndEndFilters.TabIndex = 8;
            ChkShowErrorsInBeginAndEndFilters.Text = "Toon regels met ERROR altijd in de begin en eind filters";
            ChkShowErrorsInBeginAndEndFilters.UseVisualStyleBackColor = true;
            // 
            // ChkAutoToggleHierarchy
            // 
            ChkAutoToggleHierarchy.AutoSize = true;
            ChkAutoToggleHierarchy.Location = new System.Drawing.Point(6, 128);
            ChkAutoToggleHierarchy.Name = "ChkAutoToggleHierarchy";
            ChkAutoToggleHierarchy.Size = new System.Drawing.Size(425, 19);
            ChkAutoToggleHierarchy.TabIndex = 39;
            ChkAutoToggleHierarchy.Text = "Schakel hierarchisch tonen automatisch in zodra er op sessie wordt gefilterd";
            ChkAutoToggleHierarchy.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.help;
            pictureBox3.Location = new System.Drawing.Point(431, 128);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new System.Drawing.Size(16, 16);
            pictureBox3.TabIndex = 40;
            pictureBox3.TabStop = false;
            toolTip.SetToolTip(pictureBox3, resources.GetString("pictureBox3.ToolTip"));
            // 
            // UserControlGenericConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(GrpGeneralSettings);
            Name = "UserControlGenericConfig";
            Size = new System.Drawing.Size(848, 304);
            GrpExportSettings.ResumeLayout(false);
            GrpExportSettings.PerformLayout();
            GrpGeneralSettings.ResumeLayout(false);
            GrpGeneralSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
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
        private System.Windows.Forms.CheckBox ChkShowErrorsInBeginAndEndFilters;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBox16;
        private System.Windows.Forms.Button BtnBrowseExportFIle;
        private System.Windows.Forms.Button BtnBrowseEditor;
        private System.Windows.Forms.CheckBox ChkAutoToggleHierarchy;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}
