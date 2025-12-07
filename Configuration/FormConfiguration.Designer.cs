namespace LogScraper.Configuration
{
    partial class FormConfiguration
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguration));
            TabLogProviders = new System.Windows.Forms.TabControl();
            tabKubernetes = new System.Windows.Forms.TabPage();
            userControlKubernetesConfig = new LogScraper.LogProviders.Kubernetes.UserControlKubernetesConfig();
            tabUrl = new System.Windows.Forms.TabPage();
            userControlRuntimeConfig = new LogScraper.LogProviders.Kubernetes.UserControlRuntimeConfig();
            tabFile = new System.Windows.Forms.TabPage();
            userControlFileConfig = new LogScraper.LogProviders.Kubernetes.UserControlFileConfig();
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            tabMain = new System.Windows.Forms.TabControl();
            TabPageGeneral = new System.Windows.Forms.TabPage();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabImport = new System.Windows.Forms.TabPage();
            LblSelectedImportFile = new System.Windows.Forms.Label();
            BtnImport = new System.Windows.Forms.Button();
            ChkImportLogLayoutSettings = new System.Windows.Forms.CheckBox();
            ChkImportGeneralSettings = new System.Windows.Forms.CheckBox();
            ChkImportLogProvidersSettings = new System.Windows.Forms.CheckBox();
            BtnImportSelectFile = new System.Windows.Forms.Button();
            tabExport = new System.Windows.Forms.TabPage();
            LblExportFileStatus = new System.Windows.Forms.Label();
            ChkExportLogLayoutSettings = new System.Windows.Forms.CheckBox();
            BtnExport = new System.Windows.Forms.Button();
            ChkExportGeneralSettings = new System.Windows.Forms.CheckBox();
            ChkExportLogProvidersSettings = new System.Windows.Forms.CheckBox();
            userControlGenericConfig = new UserControlGenericConfig();
            TabPageLogLayouts = new System.Windows.Forms.TabPage();
            userControlLogLayoutConfig = new LogScraper.Log.UserControlLogLayoutConfig();
            TabPageLogProviders = new System.Windows.Forms.TabPage();
            TabPageAbout = new System.Windows.Forms.TabPage();
            userControlAbout = new UserControlAbout();
            TabLogProviders.SuspendLayout();
            tabKubernetes.SuspendLayout();
            tabUrl.SuspendLayout();
            tabFile.SuspendLayout();
            tabMain.SuspendLayout();
            TabPageGeneral.SuspendLayout();
            tabControl1.SuspendLayout();
            tabImport.SuspendLayout();
            tabExport.SuspendLayout();
            TabPageLogLayouts.SuspendLayout();
            TabPageLogProviders.SuspendLayout();
            TabPageAbout.SuspendLayout();
            SuspendLayout();
            // 
            // TabLogProviders
            // 
            TabLogProviders.Controls.Add(tabKubernetes);
            TabLogProviders.Controls.Add(tabUrl);
            TabLogProviders.Controls.Add(tabFile);
            TabLogProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            TabLogProviders.Location = new System.Drawing.Point(3, 3);
            TabLogProviders.Name = "TabLogProviders";
            TabLogProviders.SelectedIndex = 0;
            TabLogProviders.Size = new System.Drawing.Size(821, 590);
            TabLogProviders.TabIndex = 0;
            // 
            // tabKubernetes
            // 
            tabKubernetes.Controls.Add(userControlKubernetesConfig);
            tabKubernetes.Location = new System.Drawing.Point(4, 24);
            tabKubernetes.Name = "tabKubernetes";
            tabKubernetes.Padding = new System.Windows.Forms.Padding(3);
            tabKubernetes.Size = new System.Drawing.Size(813, 562);
            tabKubernetes.TabIndex = 0;
            tabKubernetes.Text = "Kubernetes";
            tabKubernetes.UseVisualStyleBackColor = true;
            // 
            // userControlKubernetesConfig
            // 
            userControlKubernetesConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlKubernetesConfig.Location = new System.Drawing.Point(3, 3);
            userControlKubernetesConfig.Name = "userControlKubernetesConfig";
            userControlKubernetesConfig.Size = new System.Drawing.Size(807, 556);
            userControlKubernetesConfig.TabIndex = 0;
            // 
            // tabUrl
            // 
            tabUrl.Controls.Add(userControlRuntimeConfig);
            tabUrl.Location = new System.Drawing.Point(4, 24);
            tabUrl.Name = "tabUrl";
            tabUrl.Size = new System.Drawing.Size(813, 562);
            tabUrl.TabIndex = 1;
            tabUrl.Text = "Directe Url";
            tabUrl.UseVisualStyleBackColor = true;
            // 
            // userControlRuntimeConfig
            // 
            userControlRuntimeConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlRuntimeConfig.Location = new System.Drawing.Point(0, 0);
            userControlRuntimeConfig.Name = "userControlRuntimeConfig";
            userControlRuntimeConfig.Size = new System.Drawing.Size(813, 562);
            userControlRuntimeConfig.TabIndex = 0;
            // 
            // tabFile
            // 
            tabFile.Controls.Add(userControlFileConfig);
            tabFile.Location = new System.Drawing.Point(4, 24);
            tabFile.Name = "tabFile";
            tabFile.Size = new System.Drawing.Size(813, 562);
            tabFile.TabIndex = 2;
            tabFile.Text = "Lokaal bestand";
            tabFile.UseVisualStyleBackColor = true;
            // 
            // userControlFileConfig
            // 
            userControlFileConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlFileConfig.Location = new System.Drawing.Point(0, 0);
            userControlFileConfig.Name = "userControlFileConfig";
            userControlFileConfig.Size = new System.Drawing.Size(813, 562);
            userControlFileConfig.TabIndex = 0;
            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOk.Location = new System.Drawing.Point(753, 632);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(75, 23);
            btnOk.TabIndex = 1;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += BtnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(672, 632);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // tabMain
            // 
            tabMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabMain.Controls.Add(TabPageGeneral);
            tabMain.Controls.Add(TabPageLogLayouts);
            tabMain.Controls.Add(TabPageLogProviders);
            tabMain.Controls.Add(TabPageAbout);
            tabMain.Location = new System.Drawing.Point(2, 2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(835, 624);
            tabMain.TabIndex = 3;
            // 
            // TabPageGeneral
            // 
            TabPageGeneral.Controls.Add(tabControl1);
            TabPageGeneral.Controls.Add(userControlGenericConfig);
            TabPageGeneral.Location = new System.Drawing.Point(4, 24);
            TabPageGeneral.Name = "TabPageGeneral";
            TabPageGeneral.Size = new System.Drawing.Size(827, 596);
            TabPageGeneral.TabIndex = 2;
            TabPageGeneral.Text = "Algemeen";
            TabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl1.Controls.Add(tabImport);
            tabControl1.Controls.Add(tabExport);
            tabControl1.Location = new System.Drawing.Point(2, 414);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(822, 179);
            tabControl1.TabIndex = 3;
            // 
            // tabImport
            // 
            tabImport.Controls.Add(LblSelectedImportFile);
            tabImport.Controls.Add(BtnImport);
            tabImport.Controls.Add(ChkImportLogLayoutSettings);
            tabImport.Controls.Add(ChkImportGeneralSettings);
            tabImport.Controls.Add(ChkImportLogProvidersSettings);
            tabImport.Controls.Add(BtnImportSelectFile);
            tabImport.Location = new System.Drawing.Point(4, 24);
            tabImport.Name = "tabImport";
            tabImport.Padding = new System.Windows.Forms.Padding(3);
            tabImport.Size = new System.Drawing.Size(814, 151);
            tabImport.TabIndex = 0;
            tabImport.Text = "Instellingen importeren";
            tabImport.UseVisualStyleBackColor = true;
            // 
            // LblSelectedImportFile
            // 
            LblSelectedImportFile.AutoSize = true;
            LblSelectedImportFile.Location = new System.Drawing.Point(169, 13);
            LblSelectedImportFile.Name = "LblSelectedImportFile";
            LblSelectedImportFile.Size = new System.Drawing.Size(10, 15);
            LblSelectedImportFile.TabIndex = 15;
            LblSelectedImportFile.Text = " ";
            // 
            // BtnImport
            // 
            BtnImport.Enabled = false;
            BtnImport.Location = new System.Drawing.Point(6, 117);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new System.Drawing.Size(157, 29);
            BtnImport.TabIndex = 14;
            BtnImport.Text = "Importeren";
            BtnImport.UseVisualStyleBackColor = true;
            BtnImport.Click += BtnImport_Click;
            // 
            // ChkImportLogLayoutSettings
            // 
            ChkImportLogLayoutSettings.AutoSize = true;
            ChkImportLogLayoutSettings.Enabled = false;
            ChkImportLogLayoutSettings.Location = new System.Drawing.Point(6, 66);
            ChkImportLogLayoutSettings.Name = "ChkImportLogLayoutSettings";
            ChkImportLogLayoutSettings.Size = new System.Drawing.Size(87, 19);
            ChkImportLogLayoutSettings.TabIndex = 13;
            ChkImportLogLayoutSettings.Text = "Log layouts";
            ChkImportLogLayoutSettings.UseVisualStyleBackColor = true;
            ChkImportLogLayoutSettings.CheckedChanged += ChkImportLogLayoutSettings_CheckedChanged;
            // 
            // ChkImportGeneralSettings
            // 
            ChkImportGeneralSettings.AutoSize = true;
            ChkImportGeneralSettings.Enabled = false;
            ChkImportGeneralSettings.Location = new System.Drawing.Point(6, 41);
            ChkImportGeneralSettings.Name = "ChkImportGeneralSettings";
            ChkImportGeneralSettings.Size = new System.Drawing.Size(144, 19);
            ChkImportGeneralSettings.TabIndex = 11;
            ChkImportGeneralSettings.Text = "Algemene instellingen";
            ChkImportGeneralSettings.UseVisualStyleBackColor = true;
            ChkImportGeneralSettings.CheckedChanged += ChkImportGeneralSettings_CheckedChanged;
            // 
            // ChkImportLogProvidersSettings
            // 
            ChkImportLogProvidersSettings.AutoSize = true;
            ChkImportLogProvidersSettings.Enabled = false;
            ChkImportLogProvidersSettings.Location = new System.Drawing.Point(6, 91);
            ChkImportLogProvidersSettings.Name = "ChkImportLogProvidersSettings";
            ChkImportLogProvidersSettings.Size = new System.Drawing.Size(137, 19);
            ChkImportLogProvidersSettings.TabIndex = 12;
            ChkImportLogProvidersSettings.Text = "Bronnen van logging";
            ChkImportLogProvidersSettings.UseVisualStyleBackColor = true;
            ChkImportLogProvidersSettings.CheckedChanged += ChkImportLogProvidersSettings_CheckedChanged;
            // 
            // BtnImportSelectFile
            // 
            BtnImportSelectFile.Location = new System.Drawing.Point(6, 6);
            BtnImportSelectFile.Name = "BtnImportSelectFile";
            BtnImportSelectFile.Size = new System.Drawing.Size(157, 29);
            BtnImportSelectFile.TabIndex = 10;
            BtnImportSelectFile.Text = "Selecteer bestand...";
            BtnImportSelectFile.UseVisualStyleBackColor = true;
            BtnImportSelectFile.Click += BtnImportSelectFile_Click;
            // 
            // tabExport
            // 
            tabExport.Controls.Add(LblExportFileStatus);
            tabExport.Controls.Add(ChkExportLogLayoutSettings);
            tabExport.Controls.Add(BtnExport);
            tabExport.Controls.Add(ChkExportGeneralSettings);
            tabExport.Controls.Add(ChkExportLogProvidersSettings);
            tabExport.Location = new System.Drawing.Point(4, 24);
            tabExport.Name = "tabExport";
            tabExport.Padding = new System.Windows.Forms.Padding(3);
            tabExport.Size = new System.Drawing.Size(814, 154);
            tabExport.TabIndex = 1;
            tabExport.Text = "Instellingen exporteren";
            tabExport.UseVisualStyleBackColor = true;
            // 
            // LblExportFileStatus
            // 
            LblExportFileStatus.AutoSize = true;
            LblExportFileStatus.Location = new System.Drawing.Point(169, 92);
            LblExportFileStatus.Name = "LblExportFileStatus";
            LblExportFileStatus.Size = new System.Drawing.Size(10, 15);
            LblExportFileStatus.TabIndex = 16;
            LblExportFileStatus.Text = " ";
            // 
            // ChkExportLogLayoutSettings
            // 
            ChkExportLogLayoutSettings.AutoSize = true;
            ChkExportLogLayoutSettings.Checked = true;
            ChkExportLogLayoutSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkExportLogLayoutSettings.Location = new System.Drawing.Point(6, 31);
            ChkExportLogLayoutSettings.Name = "ChkExportLogLayoutSettings";
            ChkExportLogLayoutSettings.Size = new System.Drawing.Size(87, 19);
            ChkExportLogLayoutSettings.TabIndex = 8;
            ChkExportLogLayoutSettings.Text = "Log layouts";
            ChkExportLogLayoutSettings.UseVisualStyleBackColor = true;
            ChkExportLogLayoutSettings.CheckedChanged += ChkExportLogLayoutSettings_CheckedChanged;
            // 
            // BtnExport
            // 
            BtnExport.Location = new System.Drawing.Point(6, 85);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new System.Drawing.Size(157, 29);
            BtnExport.TabIndex = 7;
            BtnExport.Text = "Exporteren...";
            BtnExport.UseVisualStyleBackColor = true;
            BtnExport.Click += BtnExport_Click;
            // 
            // ChkExportGeneralSettings
            // 
            ChkExportGeneralSettings.AutoSize = true;
            ChkExportGeneralSettings.Checked = true;
            ChkExportGeneralSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkExportGeneralSettings.Location = new System.Drawing.Point(6, 6);
            ChkExportGeneralSettings.Name = "ChkExportGeneralSettings";
            ChkExportGeneralSettings.Size = new System.Drawing.Size(144, 19);
            ChkExportGeneralSettings.TabIndex = 5;
            ChkExportGeneralSettings.Text = "Algemene instellingen";
            ChkExportGeneralSettings.UseVisualStyleBackColor = true;
            ChkExportGeneralSettings.CheckedChanged += ChkExportGeneralSettings_CheckedChanged;
            // 
            // ChkExportLogProvidersSettings
            // 
            ChkExportLogProvidersSettings.AutoSize = true;
            ChkExportLogProvidersSettings.Checked = true;
            ChkExportLogProvidersSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkExportLogProvidersSettings.Location = new System.Drawing.Point(6, 56);
            ChkExportLogProvidersSettings.Name = "ChkExportLogProvidersSettings";
            ChkExportLogProvidersSettings.Size = new System.Drawing.Size(137, 19);
            ChkExportLogProvidersSettings.TabIndex = 6;
            ChkExportLogProvidersSettings.Text = "Bronnen van logging";
            ChkExportLogProvidersSettings.UseVisualStyleBackColor = true;
            ChkExportLogProvidersSettings.CheckedChanged += ChkExportLogProvidersSettings_CheckedChanged;
            // 
            // userControlGenericConfig
            // 
            userControlGenericConfig.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            userControlGenericConfig.Location = new System.Drawing.Point(0, 0);
            userControlGenericConfig.Name = "userControlGenericConfig";
            userControlGenericConfig.Size = new System.Drawing.Size(827, 408);
            userControlGenericConfig.TabIndex = 0;
            // 
            // TabPageLogLayouts
            // 
            TabPageLogLayouts.Controls.Add(userControlLogLayoutConfig);
            TabPageLogLayouts.Location = new System.Drawing.Point(4, 24);
            TabPageLogLayouts.Name = "TabPageLogLayouts";
            TabPageLogLayouts.Size = new System.Drawing.Size(827, 596);
            TabPageLogLayouts.TabIndex = 3;
            TabPageLogLayouts.Text = "Log layout";
            TabPageLogLayouts.UseVisualStyleBackColor = true;
            // 
            // userControlLogLayoutConfig
            // 
            userControlLogLayoutConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlLogLayoutConfig.Location = new System.Drawing.Point(0, 0);
            userControlLogLayoutConfig.Name = "userControlLogLayoutConfig";
            userControlLogLayoutConfig.Size = new System.Drawing.Size(827, 596);
            userControlLogLayoutConfig.TabIndex = 0;
            // 
            // TabPageLogProviders
            // 
            TabPageLogProviders.Controls.Add(TabLogProviders);
            TabPageLogProviders.Location = new System.Drawing.Point(4, 24);
            TabPageLogProviders.Name = "TabPageLogProviders";
            TabPageLogProviders.Padding = new System.Windows.Forms.Padding(3);
            TabPageLogProviders.Size = new System.Drawing.Size(827, 596);
            TabPageLogProviders.TabIndex = 1;
            TabPageLogProviders.Text = "Bron van logging";
            TabPageLogProviders.UseVisualStyleBackColor = true;
            // 
            // TabPageAbout
            // 
            TabPageAbout.Controls.Add(userControlAbout);
            TabPageAbout.Location = new System.Drawing.Point(4, 24);
            TabPageAbout.Name = "TabPageAbout";
            TabPageAbout.Size = new System.Drawing.Size(827, 596);
            TabPageAbout.TabIndex = 4;
            TabPageAbout.Text = "Info";
            TabPageAbout.UseVisualStyleBackColor = true;
            // 
            // userControlAbout
            // 
            userControlAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlAbout.Location = new System.Drawing.Point(0, 0);
            userControlAbout.Name = "userControlAbout";
            userControlAbout.Size = new System.Drawing.Size(827, 596);
            userControlAbout.TabIndex = 0;
            // 
            // FormConfiguration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(840, 667);
            Controls.Add(tabMain);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(850, 700);
            Name = "FormConfiguration";
            Text = "LogScraper instellingen";
            Load += FormConfiguration_Load;
            TabLogProviders.ResumeLayout(false);
            tabKubernetes.ResumeLayout(false);
            tabUrl.ResumeLayout(false);
            tabFile.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            TabPageGeneral.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabImport.ResumeLayout(false);
            tabImport.PerformLayout();
            tabExport.ResumeLayout(false);
            tabExport.PerformLayout();
            TabPageLogLayouts.ResumeLayout(false);
            TabPageLogProviders.ResumeLayout(false);
            TabPageAbout.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl TabLogProviders;
        private System.Windows.Forms.TabPage tabKubernetes;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private LogProviders.Kubernetes.UserControlKubernetesConfig userControlKubernetesConfig;
        private System.Windows.Forms.TabPage tabUrl;
        private LogProviders.Kubernetes.UserControlRuntimeConfig userControlRuntimeConfig;
        private System.Windows.Forms.TabPage tabFile;
        private LogProviders.Kubernetes.UserControlFileConfig userControlFileConfig;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage TabPageLogProviders;
        private System.Windows.Forms.TabPage TabPageGeneral;
        private UserControlGenericConfig userControlGenericConfig;
        private System.Windows.Forms.TabPage TabPageLogLayouts;
        private Log.UserControlLogLayoutConfig userControlLogLayoutConfig;
        private System.Windows.Forms.TabPage TabPageAbout;
        private UserControlAbout userControlAbout;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabImport;
        private System.Windows.Forms.TabPage tabExport;
        private System.Windows.Forms.Label LblSelectedImportFile;
        private System.Windows.Forms.Button BtnImport;
        private System.Windows.Forms.CheckBox ChkImportLogLayoutSettings;
        private System.Windows.Forms.CheckBox ChkImportGeneralSettings;
        private System.Windows.Forms.CheckBox ChkImportLogProvidersSettings;
        private System.Windows.Forms.Button BtnImportSelectFile;
        private System.Windows.Forms.CheckBox ChkExportLogLayoutSettings;
        private System.Windows.Forms.Button BtnExport;
        private System.Windows.Forms.CheckBox ChkExportGeneralSettings;
        private System.Windows.Forms.CheckBox ChkExportLogProvidersSettings;
        private System.Windows.Forms.Label LblExportFileStatus;
    }
}