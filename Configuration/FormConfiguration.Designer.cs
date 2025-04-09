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
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            userControlFileConfig = new LogScraper.LogProviders.Kubernetes.UserControlFileConfig();
            tabMain = new System.Windows.Forms.TabControl();
            tabPageLogProviders = new System.Windows.Forms.TabPage();
            TabLogProviders.SuspendLayout();
            tabKubernetes.SuspendLayout();
            tabUrl.SuspendLayout();
            tabFile.SuspendLayout();
            tabMain.SuspendLayout();
            tabPageLogProviders.SuspendLayout();
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
            TabLogProviders.Size = new System.Drawing.Size(793, 494);
            TabLogProviders.TabIndex = 0;
            // 
            // tabKubernetes
            // 
            tabKubernetes.Controls.Add(userControlKubernetesConfig);
            tabKubernetes.Location = new System.Drawing.Point(4, 24);
            tabKubernetes.Name = "tabKubernetes";
            tabKubernetes.Padding = new System.Windows.Forms.Padding(3);
            tabKubernetes.Size = new System.Drawing.Size(800, 502);
            tabKubernetes.TabIndex = 0;
            tabKubernetes.Text = "Kubernetes";
            tabKubernetes.UseVisualStyleBackColor = true;
            // 
            // userControlKubernetesConfig
            // 
            userControlKubernetesConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlKubernetesConfig.Location = new System.Drawing.Point(3, 3);
            userControlKubernetesConfig.Name = "userControlKubernetesConfig";
            userControlKubernetesConfig.Size = new System.Drawing.Size(794, 496);
            userControlKubernetesConfig.TabIndex = 0;
            // 
            // tabUrl
            // 
            tabUrl.Controls.Add(userControlRuntimeConfig);
            tabUrl.Location = new System.Drawing.Point(4, 24);
            tabUrl.Name = "tabUrl";
            tabUrl.Size = new System.Drawing.Size(800, 502);
            tabUrl.TabIndex = 1;
            tabUrl.Text = "Directe Url";
            tabUrl.UseVisualStyleBackColor = true;
            // 
            // userControlRuntimeConfig1
            // 
            userControlRuntimeConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlRuntimeConfig.Location = new System.Drawing.Point(0, 0);
            userControlRuntimeConfig.Name = "userControlRuntimeConfig1";
            userControlRuntimeConfig.Size = new System.Drawing.Size(800, 502);
            userControlRuntimeConfig.TabIndex = 0;
            // 
            // tabFile
            // 
            tabFile.Controls.Add(userControlFileConfig);
            tabFile.Location = new System.Drawing.Point(4, 24);
            tabFile.Name = "tabFile";
            tabFile.Size = new System.Drawing.Size(785, 466);
            tabFile.TabIndex = 2;
            tabFile.Text = "Lokaal bestand";
            tabFile.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOk.Location = new System.Drawing.Point(725, 536);
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
            btnCancel.Location = new System.Drawing.Point(644, 536);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // userControlFileConfig
            // 
            userControlFileConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlFileConfig.Location = new System.Drawing.Point(0, 0);
            userControlFileConfig.Name = "userControlFileConfig";
            userControlFileConfig.Size = new System.Drawing.Size(785, 466);
            userControlFileConfig.TabIndex = 0;
            // 
            // tabMain
            // 
            tabMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabMain.Controls.Add(tabPageLogProviders);
            tabMain.Location = new System.Drawing.Point(2, 2);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(807, 528);
            tabMain.TabIndex = 3;
            // 
            // tabPageLogProviders
            // 
            tabPageLogProviders.Controls.Add(TabLogProviders);
            tabPageLogProviders.Location = new System.Drawing.Point(4, 24);
            tabPageLogProviders.Name = "tabPageLogProviders";
            tabPageLogProviders.Padding = new System.Windows.Forms.Padding(3);
            tabPageLogProviders.Size = new System.Drawing.Size(799, 500);
            tabPageLogProviders.TabIndex = 1;
            tabPageLogProviders.Text = "Bron van logging";
            tabPageLogProviders.UseVisualStyleBackColor = true;
            // 
            // FormConfiguration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(812, 571);
            Controls.Add(tabMain);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "FormConfiguration";
            Text = "Logscraper instellingen";
            Load += FormConfiguration_Load;
            TabLogProviders.ResumeLayout(false);
            tabKubernetes.ResumeLayout(false);
            tabUrl.ResumeLayout(false);
            tabFile.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabPageLogProviders.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPageLogProviders;
    }
}