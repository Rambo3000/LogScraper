using LogScraper.Controls.Configuration;
using LogScraper.Controls.Configuration.LogProviders;

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
            userControlKubernetesConfig = new UserControlKubernetesConfig();
            tabUrl = new System.Windows.Forms.TabPage();
            userControlRuntimeConfig = new UserControlRuntimeConfig();
            tabFile = new System.Windows.Forms.TabPage();
            userControlFileConfig = new UserControlFileConfig();
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            tabMain = new System.Windows.Forms.TabControl();
            TabPageGeneral = new System.Windows.Forms.TabPage();
            userControlGenericConfig = new UserControlGenericConfig();
            TabPageLogLayouts = new System.Windows.Forms.TabPage();
            userControlLogLayoutConfig = new UserControlLogLayoutConfig();
            TabPageLogProviders = new System.Windows.Forms.TabPage();
            TabPageAbout = new System.Windows.Forms.TabPage();
            userControlAbout = new UserControlAbout();
            BtnImport = new System.Windows.Forms.Button();
            BtnExport = new System.Windows.Forms.Button();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            TabLogProviders.SuspendLayout();
            tabKubernetes.SuspendLayout();
            tabUrl.SuspendLayout();
            tabFile.SuspendLayout();
            tabMain.SuspendLayout();
            TabPageGeneral.SuspendLayout();
            TabPageLogLayouts.SuspendLayout();
            TabPageLogProviders.SuspendLayout();
            TabPageAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            TabLogProviders.Size = new System.Drawing.Size(1033, 576);
            TabLogProviders.TabIndex = 0;
            // 
            // tabKubernetes
            // 
            tabKubernetes.Controls.Add(userControlKubernetesConfig);
            tabKubernetes.Location = new System.Drawing.Point(4, 24);
            tabKubernetes.Name = "tabKubernetes";
            tabKubernetes.Size = new System.Drawing.Size(1025, 548);
            tabKubernetes.TabIndex = 0;
            tabKubernetes.Text = "Kubernetes";
            tabKubernetes.UseVisualStyleBackColor = true;
            // 
            // userControlKubernetesConfig
            // 
            userControlKubernetesConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlKubernetesConfig.Location = new System.Drawing.Point(0, 0);
            userControlKubernetesConfig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            userControlKubernetesConfig.Name = "userControlKubernetesConfig";
            userControlKubernetesConfig.Size = new System.Drawing.Size(1025, 548);
            userControlKubernetesConfig.TabIndex = 0;
            // 
            // tabUrl
            // 
            tabUrl.Controls.Add(userControlRuntimeConfig);
            tabUrl.Location = new System.Drawing.Point(4, 24);
            tabUrl.Name = "tabUrl";
            tabUrl.Size = new System.Drawing.Size(1025, 543);
            tabUrl.TabIndex = 1;
            tabUrl.Text = "HTTP API";
            tabUrl.UseVisualStyleBackColor = true;
            // 
            // userControlRuntimeConfig
            // 
            userControlRuntimeConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlRuntimeConfig.Location = new System.Drawing.Point(0, 0);
            userControlRuntimeConfig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            userControlRuntimeConfig.Name = "userControlRuntimeConfig";
            userControlRuntimeConfig.Size = new System.Drawing.Size(1025, 543);
            userControlRuntimeConfig.TabIndex = 0;
            // 
            // tabFile
            // 
            tabFile.Controls.Add(userControlFileConfig);
            tabFile.Location = new System.Drawing.Point(4, 24);
            tabFile.Name = "tabFile";
            tabFile.Size = new System.Drawing.Size(1025, 543);
            tabFile.TabIndex = 2;
            tabFile.Text = "Bestand";
            tabFile.UseVisualStyleBackColor = true;
            // 
            // userControlFileConfig
            // 
            userControlFileConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlFileConfig.Location = new System.Drawing.Point(0, 0);
            userControlFileConfig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            userControlFileConfig.Name = "userControlFileConfig";
            userControlFileConfig.Size = new System.Drawing.Size(1025, 543);
            userControlFileConfig.TabIndex = 0;
            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnOk.Location = new System.Drawing.Point(963, 0);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(75, 25);
            btnOk.TabIndex = 1;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += BtnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(883, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 25);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(TabPageGeneral);
            tabMain.Controls.Add(TabPageLogLayouts);
            tabMain.Controls.Add(TabPageLogProviders);
            tabMain.Controls.Add(TabPageAbout);
            tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabMain.Location = new System.Drawing.Point(0, 0);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(1047, 609);
            tabMain.TabIndex = 3;
            // 
            // TabPageGeneral
            // 
            TabPageGeneral.Controls.Add(userControlGenericConfig);
            TabPageGeneral.Location = new System.Drawing.Point(4, 24);
            TabPageGeneral.Name = "TabPageGeneral";
            TabPageGeneral.Size = new System.Drawing.Size(1039, 581);
            TabPageGeneral.TabIndex = 2;
            TabPageGeneral.Text = "Algemeen";
            TabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // userControlGenericConfig
            // 
            userControlGenericConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlGenericConfig.Location = new System.Drawing.Point(0, 0);
            userControlGenericConfig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 0);
            userControlGenericConfig.Name = "userControlGenericConfig";
            userControlGenericConfig.Size = new System.Drawing.Size(1039, 581);
            userControlGenericConfig.TabIndex = 0;
            // 
            // TabPageLogLayouts
            // 
            TabPageLogLayouts.Controls.Add(userControlLogLayoutConfig);
            TabPageLogLayouts.Location = new System.Drawing.Point(4, 24);
            TabPageLogLayouts.Name = "TabPageLogLayouts";
            TabPageLogLayouts.Size = new System.Drawing.Size(1039, 582);
            TabPageLogLayouts.TabIndex = 3;
            TabPageLogLayouts.Text = "Log layout";
            TabPageLogLayouts.UseVisualStyleBackColor = true;
            // 
            // userControlLogLayoutConfig
            // 
            userControlLogLayoutConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlLogLayoutConfig.Location = new System.Drawing.Point(0, 0);
            userControlLogLayoutConfig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            userControlLogLayoutConfig.Name = "userControlLogLayoutConfig";
            userControlLogLayoutConfig.Size = new System.Drawing.Size(1039, 582);
            userControlLogLayoutConfig.TabIndex = 0;
            // 
            // TabPageLogProviders
            // 
            TabPageLogProviders.Controls.Add(TabLogProviders);
            TabPageLogProviders.Location = new System.Drawing.Point(4, 24);
            TabPageLogProviders.Name = "TabPageLogProviders";
            TabPageLogProviders.Padding = new System.Windows.Forms.Padding(3);
            TabPageLogProviders.Size = new System.Drawing.Size(1039, 582);
            TabPageLogProviders.TabIndex = 1;
            TabPageLogProviders.Text = "Bron van logging";
            TabPageLogProviders.UseVisualStyleBackColor = true;
            // 
            // TabPageAbout
            // 
            TabPageAbout.Controls.Add(userControlAbout);
            TabPageAbout.Location = new System.Drawing.Point(4, 24);
            TabPageAbout.Name = "TabPageAbout";
            TabPageAbout.Size = new System.Drawing.Size(1039, 582);
            TabPageAbout.TabIndex = 4;
            TabPageAbout.Text = "Info";
            TabPageAbout.UseVisualStyleBackColor = true;
            // 
            // userControlAbout
            // 
            userControlAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlAbout.Location = new System.Drawing.Point(0, 0);
            userControlAbout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            userControlAbout.Name = "userControlAbout";
            userControlAbout.Size = new System.Drawing.Size(1039, 582);
            userControlAbout.TabIndex = 0;
            // 
            // BtnImport
            // 
            BtnImport.Image = (System.Drawing.Image)resources.GetObject("BtnImport.Image");
            BtnImport.Location = new System.Drawing.Point(9, 0);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new System.Drawing.Size(175, 25);
            BtnImport.TabIndex = 15;
            BtnImport.Text = "Instellingen importeren...";
            BtnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            BtnImport.UseVisualStyleBackColor = true;
            BtnImport.Click += BtnImport_Click;
            // 
            // BtnExport
            // 
            BtnExport.Image = (System.Drawing.Image)resources.GetObject("BtnExport.Image");
            BtnExport.Location = new System.Drawing.Point(190, 0);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new System.Drawing.Size(175, 25);
            BtnExport.TabIndex = 8;
            BtnExport.Text = "Instellingen exporteren...\r\n";
            BtnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            BtnExport.UseVisualStyleBackColor = true;
            BtnExport.Click += BtnExport_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tabMain);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(BtnExport);
            splitContainer1.Panel2.Controls.Add(btnOk);
            splitContainer1.Panel2.Controls.Add(BtnImport);
            splitContainer1.Panel2.Controls.Add(btnCancel);
            splitContainer1.Size = new System.Drawing.Size(1047, 645);
            splitContainer1.SplitterDistance = 609;
            splitContainer1.TabIndex = 4;
            // 
            // FormConfiguration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1047, 645);
            Controls.Add(splitContainer1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(848, 619);
            Name = "FormConfiguration";
            Text = "LogScraper instellingen";
            Load += FormConfiguration_Load;
            TabLogProviders.ResumeLayout(false);
            tabKubernetes.ResumeLayout(false);
            tabUrl.ResumeLayout(false);
            tabFile.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            TabPageGeneral.ResumeLayout(false);
            TabPageLogLayouts.ResumeLayout(false);
            TabPageLogProviders.ResumeLayout(false);
            TabPageAbout.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl TabLogProviders;
        private System.Windows.Forms.TabPage tabKubernetes;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private UserControlKubernetesConfig userControlKubernetesConfig;
        private System.Windows.Forms.TabPage tabUrl;
        private UserControlRuntimeConfig userControlRuntimeConfig;
        private System.Windows.Forms.TabPage tabFile;
        private UserControlFileConfig userControlFileConfig;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage TabPageLogProviders;
        private System.Windows.Forms.TabPage TabPageGeneral;
        private UserControlGenericConfig userControlGenericConfig;
        private System.Windows.Forms.TabPage TabPageLogLayouts;
        private UserControlLogLayoutConfig userControlLogLayoutConfig;
        private System.Windows.Forms.TabPage TabPageAbout;
        private UserControlAbout userControlAbout;
        private System.Windows.Forms.Button BtnImport;
        private System.Windows.Forms.Button BtnExport;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}