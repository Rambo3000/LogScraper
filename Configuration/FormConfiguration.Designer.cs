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
            userControlGenericConfig = new UserControlGenericConfig();
            TabPageLogProviders = new System.Windows.Forms.TabPage();
            TabPageLogLayouts = new System.Windows.Forms.TabPage();
            userControlLogLayoutConfig = new LogScraper.Log.UserControlLogLayoutConfig();
            lblVersion = new System.Windows.Forms.Label();
            TabLogProviders.SuspendLayout();
            tabKubernetes.SuspendLayout();
            tabUrl.SuspendLayout();
            tabFile.SuspendLayout();
            tabMain.SuspendLayout();
            TabPageGeneral.SuspendLayout();
            TabPageLogProviders.SuspendLayout();
            TabPageLogLayouts.SuspendLayout();
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
            tabMain.Controls.Add(TabPageLogProviders);
            tabMain.Controls.Add(TabPageLogLayouts);
            tabMain.Location = new System.Drawing.Point(2, 2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(835, 624);
            tabMain.TabIndex = 3;
            // 
            // TabPageGeneral
            // 
            TabPageGeneral.Controls.Add(userControlGenericConfig);
            TabPageGeneral.Location = new System.Drawing.Point(4, 24);
            TabPageGeneral.Name = "TabPageGeneral";
            TabPageGeneral.Size = new System.Drawing.Size(827, 596);
            TabPageGeneral.TabIndex = 2;
            TabPageGeneral.Text = "Algemeen";
            TabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // userControlGenericConfig
            // 
            userControlGenericConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlGenericConfig.Location = new System.Drawing.Point(0, 0);
            userControlGenericConfig.Name = "userControlGenericConfig";
            userControlGenericConfig.Size = new System.Drawing.Size(827, 596);
            userControlGenericConfig.TabIndex = 0;
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
            // lblVersion
            // 
            lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblVersion.BackColor = System.Drawing.SystemColors.Control;
            lblVersion.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            lblVersion.Location = new System.Drawing.Point(786, 5);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(46, 15);
            lblVersion.TabIndex = 24;
            lblVersion.Text = "2.00.00";
            lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormConfiguration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(840, 667);
            Controls.Add(lblVersion);
            Controls.Add(tabMain);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(850, 700);
            Name = "FormConfiguration";
            Text = "Logscraper instellingen";
            Load += FormConfiguration_Load;
            TabLogProviders.ResumeLayout(false);
            tabKubernetes.ResumeLayout(false);
            tabUrl.ResumeLayout(false);
            tabFile.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            TabPageGeneral.ResumeLayout(false);
            TabPageLogProviders.ResumeLayout(false);
            TabPageLogLayouts.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblVersion;
    }
}