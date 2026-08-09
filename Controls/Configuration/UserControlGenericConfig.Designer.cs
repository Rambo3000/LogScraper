using LogScraper.Controls.Generic;

namespace LogScraper.Controls.Configuration
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
            LblDefaultLogProviderType = new System.Windows.Forms.Label();
            GrpExportSettings = new System.Windows.Forms.GroupBox();
            BtnBrowseEditor = new System.Windows.Forms.Button();
            BtnBrowseExportFIle = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            TxtEditorLocation = new ValidatedTextBox();
            TxtExportFileName = new ValidatedTextBox();
            CboLogProviderType = new System.Windows.Forms.ComboBox();
            LblTimeout = new System.Windows.Forms.Label();
            TxtTimeOut = new ValidatedTextBox();
            CboAutomaticReadTime = new System.Windows.Forms.ComboBox();
            LblAutomaticReadTimea = new System.Windows.Forms.Label();
            GrpGeneralSettings = new System.Windows.Forms.GroupBox();
            ChkCheckForExternalViewingFilesOnExit = new System.Windows.Forms.CheckBox();
            ChkIsDebugModeEnabled = new System.Windows.Forms.CheckBox();
            ChkShowTimeLine = new System.Windows.Forms.CheckBox();
            ChkIncludePrereleaseUpdates = new System.Windows.Forms.CheckBox();
            ChkPinLogProvidersByDefault = new System.Windows.Forms.CheckBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            pictureBox16 = new System.Windows.Forms.PictureBox();
            ChkShowErrorsInBeginAndEndFilters = new System.Windows.Forms.CheckBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            PnlUsedForScalingCompatiblity = new System.Windows.Forms.Panel();
            GrpExportSettings.SuspendLayout();
            GrpGeneralSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).BeginInit();
            PnlUsedForScalingCompatiblity.SuspendLayout();
            SuspendLayout();
            // 
            // LblDefaultLogProviderType
            // 
            LblDefaultLogProviderType.AutoSize = true;
            LblDefaultLogProviderType.Location = new System.Drawing.Point(3, 19);
            LblDefaultLogProviderType.Name = "LblDefaultLogProviderType";
            LblDefaultLogProviderType.Size = new System.Drawing.Size(127, 15);
            LblDefaultLogProviderType.TabIndex = 0;
            LblDefaultLogProviderType.Text = "Default logging source";
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
            GrpExportSettings.Location = new System.Drawing.Point(6, 200);
            GrpExportSettings.Name = "GrpExportSettings";
            GrpExportSettings.Size = new System.Drawing.Size(571, 82);
            GrpExportSettings.TabIndex = 1;
            GrpExportSettings.TabStop = false;
            GrpExportSettings.Text = "View log in external editor";
            // 
            // BtnBrowseEditor
            // 
            BtnBrowseEditor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnBrowseEditor.Location = new System.Drawing.Point(536, 26);
            BtnBrowseEditor.Name = "BtnBrowseEditor";
            BtnBrowseEditor.Size = new System.Drawing.Size(29, 20);
            BtnBrowseEditor.TabIndex = 11;
            BtnBrowseEditor.Text = "...";
            BtnBrowseEditor.UseVisualStyleBackColor = true;
            BtnBrowseEditor.Click += BtnBrowseEditor_Click;
            // 
            // BtnBrowseExportFIle
            // 
            BtnBrowseExportFIle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnBrowseExportFIle.Location = new System.Drawing.Point(536, 49);
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
            label3.Location = new System.Drawing.Point(6, 26);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(141, 15);
            label3.TabIndex = 9;
            label3.Text = "External editor executable";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 52);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(251, 15);
            label2.TabIndex = 8;
            label2.Text = "Default temporary file name for external editor";
            // 
            // TxtEditorLocation
            // 
            TxtEditorLocation.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtEditorLocation.BackColor = System.Drawing.Color.MistyRose;
            TxtEditorLocation.IsRequired = true;
            TxtEditorLocation.IsWhiteSpaceAllowed = false;
            TxtEditorLocation.Location = new System.Drawing.Point(263, 23);
            TxtEditorLocation.Name = "TxtEditorLocation";
            TxtEditorLocation.Size = new System.Drawing.Size(267, 23);
            TxtEditorLocation.TabIndex = 7;
            // 
            // TxtExportFileName
            // 
            TxtExportFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtExportFileName.BackColor = System.Drawing.Color.MistyRose;
            TxtExportFileName.IsRequired = true;
            TxtExportFileName.IsWhiteSpaceAllowed = false;
            TxtExportFileName.Location = new System.Drawing.Point(263, 49);
            TxtExportFileName.Name = "TxtExportFileName";
            TxtExportFileName.Size = new System.Drawing.Size(267, 23);
            TxtExportFileName.TabIndex = 5;
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
            LblTimeout.Size = new System.Drawing.Size(122, 15);
            LblTimeout.TabIndex = 3;
            LblTimeout.Text = "Download timeout (s)";
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
            LblAutomaticReadTimea.Location = new System.Drawing.Point(3, 48);
            LblAutomaticReadTimea.Name = "LblAutomaticReadTimea";
            LblAutomaticReadTimea.Size = new System.Drawing.Size(138, 15);
            LblAutomaticReadTimea.TabIndex = 6;
            LblAutomaticReadTimea.Text = "Automatic read time (m)";
            // 
            // GrpGeneralSettings
            // 
            GrpGeneralSettings.Controls.Add(ChkCheckForExternalViewingFilesOnExit);
            GrpGeneralSettings.Controls.Add(ChkIsDebugModeEnabled);
            GrpGeneralSettings.Controls.Add(ChkShowTimeLine);
            GrpGeneralSettings.Controls.Add(ChkIncludePrereleaseUpdates);
            GrpGeneralSettings.Controls.Add(ChkPinLogProvidersByDefault);
            GrpGeneralSettings.Controls.Add(pictureBox1);
            GrpGeneralSettings.Controls.Add(pictureBox16);
            GrpGeneralSettings.Controls.Add(ChkShowErrorsInBeginAndEndFilters);
            GrpGeneralSettings.Controls.Add(LblDefaultLogProviderType);
            GrpGeneralSettings.Controls.Add(CboAutomaticReadTime);
            GrpGeneralSettings.Controls.Add(GrpExportSettings);
            GrpGeneralSettings.Controls.Add(LblAutomaticReadTimea);
            GrpGeneralSettings.Controls.Add(CboLogProviderType);
            GrpGeneralSettings.Controls.Add(LblTimeout);
            GrpGeneralSettings.Controls.Add(TxtTimeOut);
            GrpGeneralSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            GrpGeneralSettings.Location = new System.Drawing.Point(0, 0);
            GrpGeneralSettings.Name = "GrpGeneralSettings";
            GrpGeneralSettings.Size = new System.Drawing.Size(606, 349);
            GrpGeneralSettings.TabIndex = 8;
            GrpGeneralSettings.TabStop = false;
            GrpGeneralSettings.Text = "General settings";
            // 
            // ChkCheckForExternalViewingFilesOnExit
            // 
            ChkCheckForExternalViewingFilesOnExit.AutoSize = true;
            ChkCheckForExternalViewingFilesOnExit.Location = new System.Drawing.Point(6, 175);
            ChkCheckForExternalViewingFilesOnExit.Name = "ChkCheckForExternalViewingFilesOnExit";
            ChkCheckForExternalViewingFilesOnExit.Size = new System.Drawing.Size(284, 19);
            ChkCheckForExternalViewingFilesOnExit.TabIndex = 44;
            ChkCheckForExternalViewingFilesOnExit.Text = "Check for external viewing files to remove on exit";
            ChkCheckForExternalViewingFilesOnExit.UseVisualStyleBackColor = true;
            // 
            // ChkIsDebugModeEnabled
            // 
            ChkIsDebugModeEnabled.AutoSize = true;
            ChkIsDebugModeEnabled.Location = new System.Drawing.Point(6, 313);
            ChkIsDebugModeEnabled.Name = "ChkIsDebugModeEnabled";
            ChkIsDebugModeEnabled.Size = new System.Drawing.Size(207, 19);
            ChkIsDebugModeEnabled.TabIndex = 43;
            ChkIsDebugModeEnabled.Text = "Save LogScraper errors to a log file";
            ChkIsDebugModeEnabled.UseVisualStyleBackColor = true;
            // 
            // ChkShowTimeLine
            // 
            ChkShowTimeLine.AutoSize = true;
            ChkShowTimeLine.Location = new System.Drawing.Point(6, 150);
            ChkShowTimeLine.Name = "ChkShowTimeLine";
            ChkShowTimeLine.Size = new System.Drawing.Size(178, 19);
            ChkShowTimeLine.TabIndex = 42;
            ChkShowTimeLine.Text = "Show the timeline histogram";
            ChkShowTimeLine.UseVisualStyleBackColor = true;
            // 
            // ChkIncludePrereleaseUpdates
            // 
            ChkIncludePrereleaseUpdates.AutoSize = true;
            ChkIncludePrereleaseUpdates.Location = new System.Drawing.Point(6, 288);
            ChkIncludePrereleaseUpdates.Name = "ChkIncludePrereleaseUpdates";
            ChkIncludePrereleaseUpdates.Size = new System.Drawing.Size(241, 19);
            ChkIncludePrereleaseUpdates.TabIndex = 41;
            ChkIncludePrereleaseUpdates.Text = "Also check for beta updates (prereleases)";
            ChkIncludePrereleaseUpdates.UseVisualStyleBackColor = true;
            // 
            // ChkPinLogProvidersByDefault
            // 
            ChkPinLogProvidersByDefault.AutoSize = true;
            ChkPinLogProvidersByDefault.Location = new System.Drawing.Point(6, 128);
            ChkPinLogProvidersByDefault.Name = "ChkPinLogProvidersByDefault";
            ChkPinLogProvidersByDefault.Size = new System.Drawing.Size(335, 19);
            ChkPinLogProvidersByDefault.TabIndex = 39;
            ChkPinLogProvidersByDefault.Text = "Automatically hide source settings once they are complete";
            ChkPinLogProvidersByDefault.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.help;
            pictureBox1.Location = new System.Drawing.Point(332, 103);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(16, 16);
            pictureBox1.TabIndex = 37;
            pictureBox1.TabStop = false;
            toolTip.SetToolTip(pictureBox1, "Shows a red line with the time and the text ERROR in the navigation (right side of the screen).");
            // 
            // pictureBox16
            // 
            pictureBox16.Image = Properties.Resources.help;
            pictureBox16.Location = new System.Drawing.Point(293, 78);
            pictureBox16.Name = "pictureBox16";
            pictureBox16.Size = new System.Drawing.Size(16, 16);
            pictureBox16.TabIndex = 36;
            pictureBox16.TabStop = false;
            toolTip.SetToolTip(pictureBox16, "This concerns the number of seconds a single download may take. If large files need to be downloaded, this value can be increased.");
            // 
            // ChkShowErrorsInBeginAndEndFilters
            // 
            ChkShowErrorsInBeginAndEndFilters.AutoSize = true;
            ChkShowErrorsInBeginAndEndFilters.Location = new System.Drawing.Point(6, 103);
            ChkShowErrorsInBeginAndEndFilters.Name = "ChkShowErrorsInBeginAndEndFilters";
            ChkShowErrorsInBeginAndEndFilters.Size = new System.Drawing.Size(326, 19);
            ChkShowErrorsInBeginAndEndFilters.TabIndex = 8;
            ChkShowErrorsInBeginAndEndFilters.Text = "Always show error log lines among the navigation results";
            ChkShowErrorsInBeginAndEndFilters.UseVisualStyleBackColor = true;
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 9999999;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
            // 
            // PnlUsedForScalingCompatiblity
            // 
            PnlUsedForScalingCompatiblity.Controls.Add(GrpGeneralSettings);
            PnlUsedForScalingCompatiblity.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForScalingCompatiblity.Location = new System.Drawing.Point(0, 0);
            PnlUsedForScalingCompatiblity.Name = "PnlUsedForScalingCompatiblity";
            PnlUsedForScalingCompatiblity.Size = new System.Drawing.Size(606, 349);
            PnlUsedForScalingCompatiblity.TabIndex = 9;
            // 
            // UserControlGenericConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlUsedForScalingCompatiblity);
            Name = "UserControlGenericConfig";
            Size = new System.Drawing.Size(606, 349);
            GrpExportSettings.ResumeLayout(false);
            GrpExportSettings.PerformLayout();
            GrpGeneralSettings.ResumeLayout(false);
            GrpGeneralSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox16).EndInit();
            PnlUsedForScalingCompatiblity.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label LblDefaultLogProviderType;
        private System.Windows.Forms.GroupBox GrpExportSettings;
        private System.Windows.Forms.ComboBox CboLogProviderType;
        private System.Windows.Forms.Label LblTimeout;
        private ValidatedTextBox TxtTimeOut;
        private System.Windows.Forms.Label label2;
        private ValidatedTextBox TxtEditorLocation;
        private ValidatedTextBox TxtExportFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CboAutomaticReadTime;
        private System.Windows.Forms.Label LblAutomaticReadTimea;
        private System.Windows.Forms.GroupBox GrpGeneralSettings;
        private System.Windows.Forms.CheckBox ChkShowErrorsInBeginAndEndFilters;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBox16;
        private System.Windows.Forms.Button BtnBrowseExportFIle;
        private System.Windows.Forms.Button BtnBrowseEditor;
        private System.Windows.Forms.CheckBox ChkPinLogProvidersByDefault;
        private System.Windows.Forms.Panel PnlUsedForScalingCompatiblity;
        private System.Windows.Forms.CheckBox ChkIncludePrereleaseUpdates;
        private System.Windows.Forms.CheckBox ChkShowTimeLine;
        private System.Windows.Forms.CheckBox ChkIsDebugModeEnabled;
        private System.Windows.Forms.CheckBox ChkCheckForExternalViewingFilesOnExit;
    }
}
