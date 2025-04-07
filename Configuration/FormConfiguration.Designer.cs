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
            TabControl = new System.Windows.Forms.TabControl();
            tabKubernetes = new System.Windows.Forms.TabPage();
            userControlKubernetesConfig1 = new LogScraper.LogProviders.Kubernetes.UserControlKubernetesConfig();
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            TabControl.SuspendLayout();
            tabKubernetes.SuspendLayout();
            SuspendLayout();
            // 
            // TabControl
            // 
            TabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TabControl.Controls.Add(tabKubernetes);
            TabControl.Location = new System.Drawing.Point(2, 0);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(873, 546);
            TabControl.TabIndex = 0;
            // 
            // tabKubernetes
            // 
            tabKubernetes.Controls.Add(userControlKubernetesConfig1);
            tabKubernetes.Location = new System.Drawing.Point(4, 24);
            tabKubernetes.Name = "tabKubernetes";
            tabKubernetes.Padding = new System.Windows.Forms.Padding(3);
            tabKubernetes.Size = new System.Drawing.Size(865, 518);
            tabKubernetes.TabIndex = 0;
            tabKubernetes.Text = "Kubernetes";
            tabKubernetes.UseVisualStyleBackColor = true;
            // 
            // userControlKubernetesConfig1
            // 
            userControlKubernetesConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlKubernetesConfig1.Location = new System.Drawing.Point(3, 3);
            userControlKubernetesConfig1.Name = "userControlKubernetesConfig1";
            userControlKubernetesConfig1.Size = new System.Drawing.Size(859, 512);
            userControlKubernetesConfig1.TabIndex = 0;
            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOk.Location = new System.Drawing.Point(790, 552);
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
            btnCancel.Location = new System.Drawing.Point(709, 552);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // FormConfiguration
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(877, 587);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(TabControl);
            Name = "FormConfiguration";
            Text = "Settings";
            Load += FormConfiguration_Load;
            TabControl.ResumeLayout(false);
            tabKubernetes.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabKubernetes;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private LogProviders.Kubernetes.UserControlKubernetesConfig userControlKubernetesConfig1;
    }
}