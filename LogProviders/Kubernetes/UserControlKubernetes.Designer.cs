namespace LogScraper.LogProviders.Kubernetes
{
    partial class UserControlKubernetesLogProvider
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
            cboKubernetesPod = new System.Windows.Forms.ComboBox();
            cboKubernetesNamespace = new System.Windows.Forms.ComboBox();
            cboKubernetesCluster = new System.Windows.Forms.ComboBox();
            lblKubernetesPod = new System.Windows.Forms.Label();
            lblKubernetesNamespace = new System.Windows.Forms.Label();
            lblCluster = new System.Windows.Forms.Label();
            btnKubernetesRefresh = new System.Windows.Forms.Button();
            CboKubernetesTimespan = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // cboKubernetesPod
            // 
            cboKubernetesPod.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboKubernetesPod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboKubernetesPod.FormattingEnabled = true;
            cboKubernetesPod.Location = new System.Drawing.Point(77, 62);
            cboKubernetesPod.Name = "cboKubernetesPod";
            cboKubernetesPod.Size = new System.Drawing.Size(256, 23);
            cboKubernetesPod.TabIndex = 23;
            cboKubernetesPod.SelectedIndexChanged += CboKubernetesPod_SelectedIndexChanged;
            // 
            // cboKubernetesNamespace
            // 
            cboKubernetesNamespace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboKubernetesNamespace.FormattingEnabled = true;
            cboKubernetesNamespace.Location = new System.Drawing.Point(77, 32);
            cboKubernetesNamespace.Name = "cboKubernetesNamespace";
            cboKubernetesNamespace.Size = new System.Drawing.Size(160, 23);
            cboKubernetesNamespace.TabIndex = 24;
            cboKubernetesNamespace.SelectedIndexChanged += CboKubernetesNamespace_SelectedIndexChanged;
            // 
            // cboKubernetesCluster
            // 
            cboKubernetesCluster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboKubernetesCluster.FormattingEnabled = true;
            cboKubernetesCluster.Location = new System.Drawing.Point(77, 3);
            cboKubernetesCluster.Name = "cboKubernetesCluster";
            cboKubernetesCluster.Size = new System.Drawing.Size(160, 23);
            cboKubernetesCluster.TabIndex = 25;
            cboKubernetesCluster.SelectedIndexChanged += CboKubernetesCluster_SelectedIndexChanged;
            // 
            // lblKubernetesPod
            // 
            lblKubernetesPod.AutoSize = true;
            lblKubernetesPod.Location = new System.Drawing.Point(4, 65);
            lblKubernetesPod.Name = "lblKubernetesPod";
            lblKubernetesPod.Size = new System.Drawing.Size(28, 15);
            lblKubernetesPod.TabIndex = 20;
            lblKubernetesPod.Text = "Pod";
            // 
            // lblKubernetesNamespace
            // 
            lblKubernetesNamespace.AutoSize = true;
            lblKubernetesNamespace.Location = new System.Drawing.Point(4, 36);
            lblKubernetesNamespace.Name = "lblKubernetesNamespace";
            lblKubernetesNamespace.Size = new System.Drawing.Size(69, 15);
            lblKubernetesNamespace.TabIndex = 21;
            lblKubernetesNamespace.Text = "Namespace";
            // 
            // lblCluster
            // 
            lblCluster.AutoSize = true;
            lblCluster.Location = new System.Drawing.Point(4, 6);
            lblCluster.Name = "lblCluster";
            lblCluster.Size = new System.Drawing.Size(44, 15);
            lblCluster.TabIndex = 22;
            lblCluster.Text = "Cluster";
            // 
            // btnKubernetesRefresh
            // 
            btnKubernetesRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnKubernetesRefresh.Image = Properties.Resources.reload;
            btnKubernetesRefresh.Location = new System.Drawing.Point(336, 61);
            btnKubernetesRefresh.Name = "btnKubernetesRefresh";
            btnKubernetesRefresh.Size = new System.Drawing.Size(23, 24);
            btnKubernetesRefresh.TabIndex = 28;
            btnKubernetesRefresh.UseVisualStyleBackColor = true;
            btnKubernetesRefresh.Click += BtnKubernetesRefresh_Click;
            // 
            // CboKubernetesTimespan
            // 
            CboKubernetesTimespan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboKubernetesTimespan.FormattingEnabled = true;
            CboKubernetesTimespan.Location = new System.Drawing.Point(77, 91);
            CboKubernetesTimespan.Name = "CboKubernetesTimespan";
            CboKubernetesTimespan.Size = new System.Drawing.Size(160, 23);
            CboKubernetesTimespan.TabIndex = 30;
            CboKubernetesTimespan.SelectedIndexChanged += CboKubernetesTimespan_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 94);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(65, 15);
            label1.TabIndex = 31;
            label1.Text = "Tijdspanne";
            // 
            // UserControlKubernetesLogProvider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(CboKubernetesTimespan);
            Controls.Add(btnKubernetesRefresh);
            Controls.Add(cboKubernetesPod);
            Controls.Add(cboKubernetesNamespace);
            Controls.Add(cboKubernetesCluster);
            Controls.Add(lblKubernetesPod);
            Controls.Add(lblKubernetesNamespace);
            Controls.Add(lblCluster);
            Name = "UserControlKubernetesLogProvider";
            Size = new System.Drawing.Size(363, 120);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox cboKubernetesPod;
        private System.Windows.Forms.ComboBox cboKubernetesNamespace;
        private System.Windows.Forms.ComboBox cboKubernetesCluster;
        private System.Windows.Forms.Label lblKubernetesPod;
        private System.Windows.Forms.Label lblKubernetesNamespace;
        private System.Windows.Forms.Label lblCluster;
        private System.Windows.Forms.Button btnKubernetesRefresh;
        private System.Windows.Forms.ComboBox CboKubernetesTimespan;
        private System.Windows.Forms.Label label1;
    }
}
