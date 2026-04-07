namespace LogScraper.Utilities.UserControls
{
    partial class LogProviderSelectionControl
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
            lblLogProvider = new System.Windows.Forms.Label();
            cboLogProvider = new System.Windows.Forms.ComboBox();
            GrpLogProvidersSettings = new System.Windows.Forms.GroupBox();
            usrRuntime = new LogScraper.LogProviders.Runtime.UserControlRuntimeLogProvider();
            usrKubernetes = new LogScraper.LogProviders.Kubernetes.UserControlKubernetesLogProvider();
            usrFileLogProvider = new LogScraper.LogProviders.File.UserControlFileLogProvider();
            GrpLogProvidersSettings.SuspendLayout();
            SuspendLayout();
            // 
            // lblLogProvider
            // 
            lblLogProvider.AutoSize = true;
            lblLogProvider.Location = new System.Drawing.Point(3, 9);
            lblLogProvider.Name = "lblLogProvider";
            lblLogProvider.Size = new System.Drawing.Size(32, 15);
            lblLogProvider.TabIndex = 21;
            lblLogProvider.Text = "Bron";
            // 
            // cboLogProvider
            // 
            cboLogProvider.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboLogProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogProvider.FormattingEnabled = true;
            cboLogProvider.Location = new System.Drawing.Point(52, 9);
            cboLogProvider.Name = "cboLogProvider";
            cboLogProvider.Size = new System.Drawing.Size(259, 23);
            cboLogProvider.TabIndex = 18;
            // 
            // GrpLogProvidersSettings
            // 
            GrpLogProvidersSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpLogProvidersSettings.Controls.Add(usrKubernetes);
            GrpLogProvidersSettings.Controls.Add(usrRuntime);
            GrpLogProvidersSettings.Controls.Add(usrFileLogProvider);
            GrpLogProvidersSettings.Location = new System.Drawing.Point(3, 38);
            GrpLogProvidersSettings.MinimumSize = new System.Drawing.Size(300, 0);
            GrpLogProvidersSettings.Name = "GrpLogProvidersSettings";
            GrpLogProvidersSettings.Size = new System.Drawing.Size(311, 147);
            GrpLogProvidersSettings.TabIndex = 24;
            GrpLogProvidersSettings.TabStop = false;
            GrpLogProvidersSettings.Text = "Instellingen";
            // 
            // usrRuntime
            // 
            usrRuntime.Dock = System.Windows.Forms.DockStyle.Fill;
            usrRuntime.Location = new System.Drawing.Point(3, 19);
            usrRuntime.Name = "usrRuntime";
            usrRuntime.Size = new System.Drawing.Size(305, 125);
            usrRuntime.TabIndex = 0;
            // 
            // usrKubernetes
            // 
            usrKubernetes.Dock = System.Windows.Forms.DockStyle.Fill;
            usrKubernetes.Location = new System.Drawing.Point(3, 19);
            usrKubernetes.Name = "usrKubernetes";
            usrKubernetes.Size = new System.Drawing.Size(305, 125);
            usrKubernetes.TabIndex = 8;
            // 
            // usrFileLogProvider
            // 
            usrFileLogProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            usrFileLogProvider.Location = new System.Drawing.Point(3, 19);
            usrFileLogProvider.Name = "usrFileLogProvider";
            usrFileLogProvider.Size = new System.Drawing.Size(305, 125);
            usrFileLogProvider.TabIndex = 3;
            // 
            // LogProviderSelectionControl
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(GrpLogProvidersSettings);
            Controls.Add(cboLogProvider);
            Controls.Add(lblLogProvider);
            Name = "LogProviderSelectionControl";
            Size = new System.Drawing.Size(317, 192);
            GrpLogProvidersSettings.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblLogProvider;
        private System.Windows.Forms.ComboBox cboLogProvider;
        private System.Windows.Forms.GroupBox GrpLogProvidersSettings;
        private LogScraper.LogProviders.Runtime.UserControlRuntimeLogProvider usrRuntime;
        private LogScraper.LogProviders.Kubernetes.UserControlKubernetesLogProvider usrKubernetes;
        private LogScraper.LogProviders.File.UserControlFileLogProvider usrFileLogProvider;
    }
}
