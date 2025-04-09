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
            TabControl = new System.Windows.Forms.TabControl();
            tabKubernetes = new System.Windows.Forms.TabPage();
            userControlKubernetesConfig = new LogScraper.LogProviders.Kubernetes.UserControlKubernetesConfig();
            tabUrl = new System.Windows.Forms.TabPage();
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            userControlRuntimeConfig1 = new LogScraper.LogProviders.Kubernetes.UserControlRuntimeConfig();
            TabControl.SuspendLayout();
            tabKubernetes.SuspendLayout();
            tabUrl.SuspendLayout();
            SuspendLayout();
            // 
            // TabControl
            // 
            TabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TabControl.Controls.Add(tabKubernetes);
            TabControl.Controls.Add(tabUrl);
            TabControl.Location = new System.Drawing.Point(2, 0);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(808, 530);
            TabControl.TabIndex = 0;
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
            tabUrl.Controls.Add(userControlRuntimeConfig1);
            tabUrl.Location = new System.Drawing.Point(4, 24);
            tabUrl.Name = "tabUrl";
            tabUrl.Size = new System.Drawing.Size(800, 502);
            tabUrl.TabIndex = 1;
            tabUrl.Text = "Directe Url";
            tabUrl.UseVisualStyleBackColor = true;
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
            // userControlRuntimeConfig1
            // 
            userControlRuntimeConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlRuntimeConfig1.Location = new System.Drawing.Point(0, 0);
            userControlRuntimeConfig1.Name = "userControlRuntimeConfig1";
            userControlRuntimeConfig1.Size = new System.Drawing.Size(800, 502);
            userControlRuntimeConfig1.TabIndex = 0;
            // 
            // FormConfiguration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(812, 571);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(TabControl);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "FormConfiguration";
            Text = "Logscraper instellingen";
            Load += FormConfiguration_Load;
            TabControl.ResumeLayout(false);
            tabKubernetes.ResumeLayout(false);
            tabUrl.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabKubernetes;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private LogProviders.Kubernetes.UserControlKubernetesConfig userControlKubernetesConfig;
        private System.Windows.Forms.TabPage tabUrl;
        private LogProviders.Kubernetes.UserControlRuntimeConfig userControlRuntimeConfig1;
    }
}