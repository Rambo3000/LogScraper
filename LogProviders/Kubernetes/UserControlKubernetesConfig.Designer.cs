using LogScraper.Extensions;

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
            TxtClusterDescription = new ValidatedTextBox();
            TxtClusterBaseUrl = new ValidatedTextBox();
            TxtClusterId = new ValidatedTextBox();
            GrpClusters = new System.Windows.Forms.GroupBox();
            GrpCluster = new System.Windows.Forms.GroupBox();
            grpNamespace = new System.Windows.Forms.GroupBox();
            BtnTest = new System.Windows.Forms.Button();
            LblNamespaceName = new System.Windows.Forms.Label();
            TxtNamespaceDescription = new ValidatedTextBox();
            LblNamespaceDescription = new System.Windows.Forms.Label();
            TxtNamespaceName = new ValidatedTextBox();
            LblNamespaces = new System.Windows.Forms.Label();
            BtnAddNamespace = new System.Windows.Forms.Button();
            LblClusterId = new System.Windows.Forms.Label();
            LblClusterBaseUrl = new System.Windows.Forms.Label();
            LblClusterDescription = new System.Windows.Forms.Label();
            BtnRemoveNamespace = new System.Windows.Forms.Button();
            LstNamespaces = new System.Windows.Forms.ListBox();
            BtnNamespaceUp = new System.Windows.Forms.Button();
            BtnNamespaceDown = new System.Windows.Forms.Button();
            CboLogLayout = new System.Windows.Forms.ComboBox();
            lblLogLayout = new System.Windows.Forms.Label();
            TxtTestMessage = new System.Windows.Forms.TextBox();
            GrpClusters.SuspendLayout();
            GrpCluster.SuspendLayout();
            grpNamespace.SuspendLayout();
            SuspendLayout();
            // 
            // LstClusters
            // 
            LstClusters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstClusters.FormattingEnabled = true;
            LstClusters.IntegralHeight = false;
            LstClusters.Location = new System.Drawing.Point(6, 22);
            LstClusters.Name = "LstClusters";
            LstClusters.Size = new System.Drawing.Size(232, 396);
            LstClusters.TabIndex = 0;
            LstClusters.SelectedIndexChanged += LstClusters_SelectedIndexChanged;
            // 
            // BtnAddCluster
            // 
            BtnAddCluster.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnAddCluster.Location = new System.Drawing.Point(6, 424);
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
            BtnRemoveCluster.Location = new System.Drawing.Point(92, 424);
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
            BtnClusterUp.Location = new System.Drawing.Point(192, 424);
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
            BtnClusterDown.Location = new System.Drawing.Point(216, 424);
            BtnClusterDown.Name = "BtnClusterDown";
            BtnClusterDown.Size = new System.Drawing.Size(22, 23);
            BtnClusterDown.TabIndex = 4;
            BtnClusterDown.UseVisualStyleBackColor = true;
            BtnClusterDown.Click += BtnClusterDown_Click;
            // 
            // TxtClusterDescription
            // 
            TxtClusterDescription.IsRequired = true;
            TxtClusterDescription.Location = new System.Drawing.Point(8, 37);
            TxtClusterDescription.Name = "TxtClusterDescription";
            TxtClusterDescription.Size = new System.Drawing.Size(239, 23);
            TxtClusterDescription.TabIndex = 5;
            TxtClusterDescription.TextChanged += TxtClusterDescription_TextChanged;
            // 
            // TxtClusterBaseUrl
            // 
            TxtClusterBaseUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtClusterBaseUrl.IsRequired = true;
            TxtClusterBaseUrl.Location = new System.Drawing.Point(8, 125);
            TxtClusterBaseUrl.Name = "TxtClusterBaseUrl";
            TxtClusterBaseUrl.Size = new System.Drawing.Size(520, 23);
            TxtClusterBaseUrl.TabIndex = 6;
            TxtClusterBaseUrl.TextChanged += TxtClusterBaseUrl_TextChanged;
            // 
            // TxtClusterId
            // 
            TxtClusterId.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtClusterId.IsRequired = true;
            TxtClusterId.Location = new System.Drawing.Point(6, 81);
            TxtClusterId.Name = "TxtClusterId";
            TxtClusterId.Size = new System.Drawing.Size(287, 23);
            TxtClusterId.TabIndex = 7;
            TxtClusterId.TextChanged += TxtClusterId_TextChanged;
            // 
            // GrpClusters
            // 
            GrpClusters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpClusters.Controls.Add(GrpCluster);
            GrpClusters.Controls.Add(LstClusters);
            GrpClusters.Controls.Add(BtnAddCluster);
            GrpClusters.Controls.Add(BtnRemoveCluster);
            GrpClusters.Controls.Add(BtnClusterUp);
            GrpClusters.Controls.Add(BtnClusterDown);
            GrpClusters.Location = new System.Drawing.Point(0, 34);
            GrpClusters.Name = "GrpClusters";
            GrpClusters.Size = new System.Drawing.Size(782, 453);
            GrpClusters.TabIndex = 8;
            GrpClusters.TabStop = false;
            GrpClusters.Text = "Clusters";
            // 
            // GrpCluster
            // 
            GrpCluster.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpCluster.Controls.Add(grpNamespace);
            GrpCluster.Controls.Add(LblNamespaces);
            GrpCluster.Controls.Add(BtnAddNamespace);
            GrpCluster.Controls.Add(LblClusterId);
            GrpCluster.Controls.Add(LblClusterBaseUrl);
            GrpCluster.Controls.Add(LblClusterDescription);
            GrpCluster.Controls.Add(BtnRemoveNamespace);
            GrpCluster.Controls.Add(TxtClusterId);
            GrpCluster.Controls.Add(LstNamespaces);
            GrpCluster.Controls.Add(TxtClusterBaseUrl);
            GrpCluster.Controls.Add(BtnNamespaceUp);
            GrpCluster.Controls.Add(TxtClusterDescription);
            GrpCluster.Controls.Add(BtnNamespaceDown);
            GrpCluster.Location = new System.Drawing.Point(244, 16);
            GrpCluster.Name = "GrpCluster";
            GrpCluster.Size = new System.Drawing.Size(532, 402);
            GrpCluster.TabIndex = 14;
            GrpCluster.TabStop = false;
            GrpCluster.Text = "Cluster";
            // 
            // grpNamespace
            // 
            grpNamespace.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpNamespace.Controls.Add(TxtTestMessage);
            grpNamespace.Controls.Add(BtnTest);
            grpNamespace.Controls.Add(LblNamespaceName);
            grpNamespace.Controls.Add(TxtNamespaceDescription);
            grpNamespace.Controls.Add(LblNamespaceDescription);
            grpNamespace.Controls.Add(TxtNamespaceName);
            grpNamespace.Location = new System.Drawing.Point(233, 162);
            grpNamespace.Name = "grpNamespace";
            grpNamespace.Size = new System.Drawing.Size(293, 205);
            grpNamespace.TabIndex = 20;
            grpNamespace.TabStop = false;
            grpNamespace.Text = "Namespace";
            // 
            // TxtTestMessage
            // 
            TxtTestMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtTestMessage.BackColor = System.Drawing.SystemColors.Control;
            TxtTestMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TxtTestMessage.Location = new System.Drawing.Point(8, 143);
            TxtTestMessage.Multiline = true;
            TxtTestMessage.Name = "TxtTestMessage";
            TxtTestMessage.Size = new System.Drawing.Size(279, 56);
            TxtTestMessage.TabIndex = 15;
            TxtTestMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnTest
            // 
            BtnTest.Anchor = System.Windows.Forms.AnchorStyles.Top;
            BtnTest.Location = new System.Drawing.Point(102, 110);
            BtnTest.Name = "BtnTest";
            BtnTest.Size = new System.Drawing.Size(96, 27);
            BtnTest.TabIndex = 14;
            BtnTest.Text = "Test";
            BtnTest.UseVisualStyleBackColor = true;
            BtnTest.Click += BtnTest_Click;
            // 
            // LblNamespaceName
            // 
            LblNamespaceName.AutoSize = true;
            LblNamespaceName.Location = new System.Drawing.Point(6, 63);
            LblNamespaceName.Name = "LblNamespaceName";
            LblNamespaceName.Size = new System.Drawing.Size(102, 15);
            LblNamespaceName.TabIndex = 13;
            LblNamespaceName.Text = "Namespace naam";
            // 
            // TxtNamespaceDescription
            // 
            TxtNamespaceDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtNamespaceDescription.IsRequired = true;
            TxtNamespaceDescription.Location = new System.Drawing.Point(6, 37);
            TxtNamespaceDescription.Name = "TxtNamespaceDescription";
            TxtNamespaceDescription.Size = new System.Drawing.Size(281, 23);
            TxtNamespaceDescription.TabIndex = 10;
            TxtNamespaceDescription.TextChanged += TxtNamespaceDescription_TextChanged;
            // 
            // LblNamespaceDescription
            // 
            LblNamespaceDescription.AutoSize = true;
            LblNamespaceDescription.Location = new System.Drawing.Point(6, 19);
            LblNamespaceDescription.Name = "LblNamespaceDescription";
            LblNamespaceDescription.Size = new System.Drawing.Size(78, 15);
            LblNamespaceDescription.TabIndex = 11;
            LblNamespaceDescription.Text = "Omschrijving";
            // 
            // TxtNamespaceName
            // 
            TxtNamespaceName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtNamespaceName.ForeColor = System.Drawing.SystemColors.WindowText;
            TxtNamespaceName.IsRequired = true;
            TxtNamespaceName.Location = new System.Drawing.Point(6, 81);
            TxtNamespaceName.Name = "TxtNamespaceName";
            TxtNamespaceName.Size = new System.Drawing.Size(281, 23);
            TxtNamespaceName.TabIndex = 12;
            TxtNamespaceName.TextChanged += TxtNamespaceName_TextChanged;
            // 
            // LblNamespaces
            // 
            LblNamespaces.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblNamespaces.AutoSize = true;
            LblNamespaces.Location = new System.Drawing.Point(6, 151);
            LblNamespaces.Name = "LblNamespaces";
            LblNamespaces.Size = new System.Drawing.Size(74, 15);
            LblNamespaces.TabIndex = 19;
            LblNamespaces.Text = "Namespaces";
            // 
            // BtnAddNamespace
            // 
            BtnAddNamespace.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnAddNamespace.Location = new System.Drawing.Point(6, 373);
            BtnAddNamespace.Name = "BtnAddNamespace";
            BtnAddNamespace.Size = new System.Drawing.Size(80, 23);
            BtnAddNamespace.TabIndex = 15;
            BtnAddNamespace.Text = "Toevoegen";
            BtnAddNamespace.UseVisualStyleBackColor = true;
            BtnAddNamespace.Click += BtnAddNamespace_Click;
            // 
            // LblClusterId
            // 
            LblClusterId.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblClusterId.AutoSize = true;
            LblClusterId.Location = new System.Drawing.Point(8, 63);
            LblClusterId.Name = "LblClusterId";
            LblClusterId.Size = new System.Drawing.Size(57, 15);
            LblClusterId.TabIndex = 9;
            LblClusterId.Text = "Cluster Id";
            // 
            // LblClusterBaseUrl
            // 
            LblClusterBaseUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblClusterBaseUrl.AutoSize = true;
            LblClusterBaseUrl.Location = new System.Drawing.Point(8, 107);
            LblClusterBaseUrl.Name = "LblClusterBaseUrl";
            LblClusterBaseUrl.Size = new System.Drawing.Size(55, 15);
            LblClusterBaseUrl.TabIndex = 9;
            LblClusterBaseUrl.Text = "Base URL";
            // 
            // LblClusterDescription
            // 
            LblClusterDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblClusterDescription.AutoSize = true;
            LblClusterDescription.Location = new System.Drawing.Point(8, 19);
            LblClusterDescription.Name = "LblClusterDescription";
            LblClusterDescription.Size = new System.Drawing.Size(78, 15);
            LblClusterDescription.TabIndex = 8;
            LblClusterDescription.Text = "Omschrijving";
            // 
            // BtnRemoveNamespace
            // 
            BtnRemoveNamespace.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnRemoveNamespace.Location = new System.Drawing.Point(92, 373);
            BtnRemoveNamespace.Name = "BtnRemoveNamespace";
            BtnRemoveNamespace.Size = new System.Drawing.Size(80, 23);
            BtnRemoveNamespace.TabIndex = 16;
            BtnRemoveNamespace.Text = "Verwijderen";
            BtnRemoveNamespace.UseVisualStyleBackColor = true;
            BtnRemoveNamespace.Click += BtnRemoveNamespace_Click;
            // 
            // LstNamespaces
            // 
            LstNamespaces.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstNamespaces.FormattingEnabled = true;
            LstNamespaces.IntegralHeight = false;
            LstNamespaces.Location = new System.Drawing.Point(6, 169);
            LstNamespaces.Name = "LstNamespaces";
            LstNamespaces.Size = new System.Drawing.Size(221, 198);
            LstNamespaces.TabIndex = 15;
            LstNamespaces.SelectedIndexChanged += LstNamespaces_SelectedIndexChanged;
            // 
            // BtnNamespaceUp
            // 
            BtnNamespaceUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnNamespaceUp.Image = Properties.Resources.up;
            BtnNamespaceUp.Location = new System.Drawing.Point(181, 373);
            BtnNamespaceUp.Name = "BtnNamespaceUp";
            BtnNamespaceUp.Size = new System.Drawing.Size(22, 23);
            BtnNamespaceUp.TabIndex = 17;
            BtnNamespaceUp.UseVisualStyleBackColor = true;
            BtnNamespaceUp.Click += BtnNamespaceUp_Click;
            // 
            // BtnNamespaceDown
            // 
            BtnNamespaceDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnNamespaceDown.Image = Properties.Resources.down;
            BtnNamespaceDown.Location = new System.Drawing.Point(205, 373);
            BtnNamespaceDown.Name = "BtnNamespaceDown";
            BtnNamespaceDown.Size = new System.Drawing.Size(22, 23);
            BtnNamespaceDown.TabIndex = 18;
            BtnNamespaceDown.UseVisualStyleBackColor = true;
            BtnNamespaceDown.Click += BtnNamespaceDown_Click;
            // 
            // CboLogLayout
            // 
            CboLogLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboLogLayout.FormattingEnabled = true;
            CboLogLayout.Location = new System.Drawing.Point(134, 5);
            CboLogLayout.Name = "CboLogLayout";
            CboLogLayout.Size = new System.Drawing.Size(187, 23);
            CboLogLayout.TabIndex = 9;
            // 
            // lblLogLayout
            // 
            lblLogLayout.AutoSize = true;
            lblLogLayout.Location = new System.Drawing.Point(6, 8);
            lblLogLayout.Name = "lblLogLayout";
            lblLogLayout.Size = new System.Drawing.Size(116, 15);
            lblLogLayout.TabIndex = 10;
            lblLogLayout.Text = "Standaard log layout";
            // 
            // UserControlKubernetesConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblLogLayout);
            Controls.Add(CboLogLayout);
            Controls.Add(GrpClusters);
            Name = "UserControlKubernetesConfig";
            Size = new System.Drawing.Size(782, 490);
            GrpClusters.ResumeLayout(false);
            GrpCluster.ResumeLayout(false);
            GrpCluster.PerformLayout();
            grpNamespace.ResumeLayout(false);
            grpNamespace.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox LstClusters;
        private System.Windows.Forms.Button BtnAddCluster;
        private System.Windows.Forms.Button BtnRemoveCluster;
        private System.Windows.Forms.Button BtnClusterUp;
        private System.Windows.Forms.Button BtnClusterDown;
        private ValidatedTextBox TxtClusterDescription;
        private ValidatedTextBox TxtClusterBaseUrl;
        private ValidatedTextBox TxtClusterId;
        private System.Windows.Forms.GroupBox GrpClusters;
        private System.Windows.Forms.GroupBox GrpCluster;
        private System.Windows.Forms.Label LblNamespaceName;
        private ValidatedTextBox TxtNamespaceName;
        private System.Windows.Forms.Label LblNamespaceDescription;
        private ValidatedTextBox TxtNamespaceDescription;
        private System.Windows.Forms.Label LblClusterId;
        private System.Windows.Forms.Label LblClusterBaseUrl;
        private System.Windows.Forms.Label LblClusterDescription;
        private System.Windows.Forms.Button BtnAddNamespace;
        private System.Windows.Forms.Button BtnRemoveNamespace;
        private System.Windows.Forms.ListBox LstNamespaces;
        private System.Windows.Forms.Button BtnNamespaceUp;
        private System.Windows.Forms.Button BtnNamespaceDown;
        private System.Windows.Forms.Label LblNamespaces;
        private System.Windows.Forms.GroupBox grpNamespace;
        private System.Windows.Forms.ComboBox CboLogLayout;
        private System.Windows.Forms.Label lblLogLayout;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.TextBox TxtTestMessage;
    }
}
