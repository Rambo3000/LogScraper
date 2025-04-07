namespace LogScraper.LogProviders.Kubernetes
{
    partial class UserControlKubernetesConfig
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
            LstClusters = new System.Windows.Forms.ListBox();
            BtnAddCluster = new System.Windows.Forms.Button();
            BtnRemoveCluster = new System.Windows.Forms.Button();
            BtnClusterUp = new System.Windows.Forms.Button();
            BtnClusterDown = new System.Windows.Forms.Button();
            TxtClusterDescription = new System.Windows.Forms.TextBox();
            TxtClusterBaseUrl = new System.Windows.Forms.TextBox();
            TxtClusterId = new System.Windows.Forms.TextBox();
            GrpClusters = new System.Windows.Forms.GroupBox();
            GrpNamespaces = new System.Windows.Forms.GroupBox();
            BtnAddNamespace = new System.Windows.Forms.Button();
            LblNamespaceName = new System.Windows.Forms.Label();
            BtnRemoveNamespace = new System.Windows.Forms.Button();
            TxtNamespaceName = new System.Windows.Forms.TextBox();
            LstNamespaces = new System.Windows.Forms.ListBox();
            LblNamespaceDescription = new System.Windows.Forms.Label();
            BtnNamespaceUp = new System.Windows.Forms.Button();
            TxtNamespaceDescription = new System.Windows.Forms.TextBox();
            BtnNamespaceDown = new System.Windows.Forms.Button();
            LblClusterId = new System.Windows.Forms.Label();
            LblClusterBaseUrl = new System.Windows.Forms.Label();
            LblClusterDescription = new System.Windows.Forms.Label();
            GrpClusters.SuspendLayout();
            GrpNamespaces.SuspendLayout();
            SuspendLayout();
            // 
            // LstClusters
            // 
            LstClusters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstClusters.FormattingEnabled = true;
            LstClusters.IntegralHeight = false;
            LstClusters.Location = new System.Drawing.Point(6, 22);
            LstClusters.Name = "LstClusters";
            LstClusters.Size = new System.Drawing.Size(277, 438);
            LstClusters.TabIndex = 0;
            LstClusters.SelectedIndexChanged += LstClusters_SelectedIndexChanged;
            // 
            // BtnAddCluster
            // 
            BtnAddCluster.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnAddCluster.Location = new System.Drawing.Point(6, 466);
            BtnAddCluster.Name = "BtnAddCluster";
            BtnAddCluster.Size = new System.Drawing.Size(80, 23);
            BtnAddCluster.TabIndex = 1;
            BtnAddCluster.Text = "Toevoegen";
            BtnAddCluster.UseVisualStyleBackColor = true;
            BtnAddCluster.Click += BtnAddCluster_Click;
            // 
            // BtnRemoveCluster
            // 
            BtnRemoveCluster.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnRemoveCluster.Location = new System.Drawing.Point(92, 466);
            BtnRemoveCluster.Name = "BtnRemoveCluster";
            BtnRemoveCluster.Size = new System.Drawing.Size(80, 23);
            BtnRemoveCluster.TabIndex = 2;
            BtnRemoveCluster.Text = "Verwijderen";
            BtnRemoveCluster.UseVisualStyleBackColor = true;
            BtnRemoveCluster.Click += BtnRemoveCluster_Click;
            // 
            // BtnClusterUp
            // 
            BtnClusterUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnClusterUp.Image = Properties.Resources.up;
            BtnClusterUp.Location = new System.Drawing.Point(233, 466);
            BtnClusterUp.Name = "BtnClusterUp";
            BtnClusterUp.Size = new System.Drawing.Size(22, 23);
            BtnClusterUp.TabIndex = 3;
            BtnClusterUp.UseVisualStyleBackColor = true;
            BtnClusterUp.Click += BtnClusterUp_Click;
            // 
            // BtnClusterDown
            // 
            BtnClusterDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnClusterDown.Image = Properties.Resources.down;
            BtnClusterDown.Location = new System.Drawing.Point(261, 466);
            BtnClusterDown.Name = "BtnClusterDown";
            BtnClusterDown.Size = new System.Drawing.Size(22, 23);
            BtnClusterDown.TabIndex = 4;
            BtnClusterDown.UseVisualStyleBackColor = true;
            BtnClusterDown.Click += BtnClusterDown_Click;
            // 
            // TxtClusterDescription
            // 
            TxtClusterDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            TxtClusterDescription.Location = new System.Drawing.Point(304, 84);
            TxtClusterDescription.Name = "TxtClusterDescription";
            TxtClusterDescription.Size = new System.Drawing.Size(239, 23);
            TxtClusterDescription.TabIndex = 5;
            // 
            // TxtClusterBaseUrl
            // 
            TxtClusterBaseUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            TxtClusterBaseUrl.Location = new System.Drawing.Point(304, 128);
            TxtClusterBaseUrl.Name = "TxtClusterBaseUrl";
            TxtClusterBaseUrl.Size = new System.Drawing.Size(239, 23);
            TxtClusterBaseUrl.TabIndex = 6;
            // 
            // TxtClusterId
            // 
            TxtClusterId.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            TxtClusterId.Location = new System.Drawing.Point(304, 40);
            TxtClusterId.Name = "TxtClusterId";
            TxtClusterId.Size = new System.Drawing.Size(463, 23);
            TxtClusterId.TabIndex = 7;
            // 
            // GrpClusters
            // 
            GrpClusters.Controls.Add(GrpNamespaces);
            GrpClusters.Controls.Add(LblClusterId);
            GrpClusters.Controls.Add(LblClusterBaseUrl);
            GrpClusters.Controls.Add(LblClusterDescription);
            GrpClusters.Controls.Add(LstClusters);
            GrpClusters.Controls.Add(TxtClusterId);
            GrpClusters.Controls.Add(BtnAddCluster);
            GrpClusters.Controls.Add(TxtClusterBaseUrl);
            GrpClusters.Controls.Add(BtnRemoveCluster);
            GrpClusters.Controls.Add(TxtClusterDescription);
            GrpClusters.Controls.Add(BtnClusterUp);
            GrpClusters.Controls.Add(BtnClusterDown);
            GrpClusters.Location = new System.Drawing.Point(3, 3);
            GrpClusters.Name = "GrpClusters";
            GrpClusters.Size = new System.Drawing.Size(779, 495);
            GrpClusters.TabIndex = 8;
            GrpClusters.TabStop = false;
            GrpClusters.Text = "Clusters";
            // 
            // GrpNamespaces
            // 
            GrpNamespaces.Controls.Add(BtnAddNamespace);
            GrpNamespaces.Controls.Add(LblNamespaceName);
            GrpNamespaces.Controls.Add(BtnRemoveNamespace);
            GrpNamespaces.Controls.Add(TxtNamespaceName);
            GrpNamespaces.Controls.Add(LstNamespaces);
            GrpNamespaces.Controls.Add(LblNamespaceDescription);
            GrpNamespaces.Controls.Add(BtnNamespaceUp);
            GrpNamespaces.Controls.Add(TxtNamespaceDescription);
            GrpNamespaces.Controls.Add(BtnNamespaceDown);
            GrpNamespaces.Location = new System.Drawing.Point(289, 157);
            GrpNamespaces.Name = "GrpNamespaces";
            GrpNamespaces.Size = new System.Drawing.Size(469, 315);
            GrpNamespaces.TabIndex = 14;
            GrpNamespaces.TabStop = false;
            GrpNamespaces.Text = "Namespaces";
            // 
            // BtnAddNamespace
            // 
            BtnAddNamespace.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnAddNamespace.Location = new System.Drawing.Point(6, 286);
            BtnAddNamespace.Name = "BtnAddNamespace";
            BtnAddNamespace.Size = new System.Drawing.Size(80, 23);
            BtnAddNamespace.TabIndex = 15;
            BtnAddNamespace.Text = "Toevoegen";
            BtnAddNamespace.UseVisualStyleBackColor = true;
            BtnAddNamespace.Click += BtnAddNamespace_Click;
            // 
            // LblNamespaceName
            // 
            LblNamespaceName.AutoSize = true;
            LblNamespaceName.Location = new System.Drawing.Point(251, 19);
            LblNamespaceName.Name = "LblNamespaceName";
            LblNamespaceName.Size = new System.Drawing.Size(69, 15);
            LblNamespaceName.TabIndex = 13;
            LblNamespaceName.Text = "Namespace";
            // 
            // BtnRemoveNamespace
            // 
            BtnRemoveNamespace.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnRemoveNamespace.Location = new System.Drawing.Point(92, 286);
            BtnRemoveNamespace.Name = "BtnRemoveNamespace";
            BtnRemoveNamespace.Size = new System.Drawing.Size(80, 23);
            BtnRemoveNamespace.TabIndex = 16;
            BtnRemoveNamespace.Text = "Verwijderen";
            BtnRemoveNamespace.UseVisualStyleBackColor = true;
            BtnRemoveNamespace.Click += BtnRemoveNamespace_Click;
            // 
            // TxtNamespaceName
            // 
            TxtNamespaceName.Location = new System.Drawing.Point(251, 37);
            TxtNamespaceName.Name = "TxtNamespaceName";
            TxtNamespaceName.Size = new System.Drawing.Size(100, 23);
            TxtNamespaceName.TabIndex = 12;
            // 
            // LstNamespaces
            // 
            LstNamespaces.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstNamespaces.FormattingEnabled = true;
            LstNamespaces.IntegralHeight = false;
            LstNamespaces.Location = new System.Drawing.Point(6, 22);
            LstNamespaces.Name = "LstNamespaces";
            LstNamespaces.Size = new System.Drawing.Size(239, 258);
            LstNamespaces.TabIndex = 15;
            LstNamespaces.SelectedIndexChanged += LstNamespaces_SelectedIndexChanged;
            // 
            // LblNamespaceDescription
            // 
            LblNamespaceDescription.AutoSize = true;
            LblNamespaceDescription.Location = new System.Drawing.Point(251, 63);
            LblNamespaceDescription.Name = "LblNamespaceDescription";
            LblNamespaceDescription.Size = new System.Drawing.Size(141, 15);
            LblNamespaceDescription.TabIndex = 11;
            LblNamespaceDescription.Text = "Namespace omschrijving";
            // 
            // BtnNamespaceUp
            // 
            BtnNamespaceUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnNamespaceUp.Image = Properties.Resources.up;
            BtnNamespaceUp.Location = new System.Drawing.Point(195, 286);
            BtnNamespaceUp.Name = "BtnNamespaceUp";
            BtnNamespaceUp.Size = new System.Drawing.Size(22, 23);
            BtnNamespaceUp.TabIndex = 17;
            BtnNamespaceUp.UseVisualStyleBackColor = true;
            BtnNamespaceUp.Click += BtnNamespaceUp_Click;
            // 
            // TxtNamespaceDescription
            // 
            TxtNamespaceDescription.Location = new System.Drawing.Point(251, 81);
            TxtNamespaceDescription.Name = "TxtNamespaceDescription";
            TxtNamespaceDescription.Size = new System.Drawing.Size(212, 23);
            TxtNamespaceDescription.TabIndex = 10;
            // 
            // BtnNamespaceDown
            // 
            BtnNamespaceDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnNamespaceDown.Image = Properties.Resources.down;
            BtnNamespaceDown.Location = new System.Drawing.Point(223, 286);
            BtnNamespaceDown.Name = "BtnNamespaceDown";
            BtnNamespaceDown.Size = new System.Drawing.Size(22, 23);
            BtnNamespaceDown.TabIndex = 18;
            BtnNamespaceDown.UseVisualStyleBackColor = true;
            BtnNamespaceDown.Click += BtnNamespaceDown_Click;
            // 
            // LblClusterId
            // 
            LblClusterId.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblClusterId.AutoSize = true;
            LblClusterId.Location = new System.Drawing.Point(304, 66);
            LblClusterId.Name = "LblClusterId";
            LblClusterId.Size = new System.Drawing.Size(57, 15);
            LblClusterId.TabIndex = 9;
            LblClusterId.Text = "Cluster Id";
            // 
            // LblClusterBaseUrl
            // 
            LblClusterBaseUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblClusterBaseUrl.AutoSize = true;
            LblClusterBaseUrl.Location = new System.Drawing.Point(304, 22);
            LblClusterBaseUrl.Name = "LblClusterBaseUrl";
            LblClusterBaseUrl.Size = new System.Drawing.Size(55, 15);
            LblClusterBaseUrl.TabIndex = 9;
            LblClusterBaseUrl.Text = "Base URL";
            // 
            // LblClusterDescription
            // 
            LblClusterDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblClusterDescription.AutoSize = true;
            LblClusterDescription.Location = new System.Drawing.Point(304, 110);
            LblClusterDescription.Name = "LblClusterDescription";
            LblClusterDescription.Size = new System.Drawing.Size(78, 15);
            LblClusterDescription.TabIndex = 8;
            LblClusterDescription.Text = "Omschrijving";
            // 
            // UserControlKubernetesConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(GrpClusters);
            Name = "UserControlKubernetesConfig";
            Size = new System.Drawing.Size(787, 504);
            GrpClusters.ResumeLayout(false);
            GrpClusters.PerformLayout();
            GrpNamespaces.ResumeLayout(false);
            GrpNamespaces.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox LstClusters;
        private System.Windows.Forms.Button BtnAddCluster;
        private System.Windows.Forms.Button BtnRemoveCluster;
        private System.Windows.Forms.Button BtnClusterUp;
        private System.Windows.Forms.Button BtnClusterDown;
        private System.Windows.Forms.TextBox TxtClusterDescription;
        private System.Windows.Forms.TextBox TxtClusterBaseUrl;
        private System.Windows.Forms.TextBox TxtClusterId;
        private System.Windows.Forms.GroupBox GrpClusters;
        private System.Windows.Forms.GroupBox GrpNamespaces;
        private System.Windows.Forms.Label LblNamespaceName;
        private System.Windows.Forms.TextBox TxtNamespaceName;
        private System.Windows.Forms.Label LblNamespaceDescription;
        private System.Windows.Forms.TextBox TxtNamespaceDescription;
        private System.Windows.Forms.Label LblClusterId;
        private System.Windows.Forms.Label LblClusterBaseUrl;
        private System.Windows.Forms.Label LblClusterDescription;
        private System.Windows.Forms.Button BtnAddNamespace;
        private System.Windows.Forms.Button BtnRemoveNamespace;
        private System.Windows.Forms.ListBox LstNamespaces;
        private System.Windows.Forms.Button BtnNamespaceUp;
        private System.Windows.Forms.Button BtnNamespaceDown;
    }
}
