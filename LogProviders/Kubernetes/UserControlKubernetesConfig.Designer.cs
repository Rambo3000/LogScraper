using LogScraper.Utilities.Extensions;

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
            components = new System.ComponentModel.Container();
            LstClusters = new System.Windows.Forms.ListBox();
            BtnAddCluster = new System.Windows.Forms.Button();
            BtnRemoveCluster = new System.Windows.Forms.Button();
            BtnClusterUp = new System.Windows.Forms.Button();
            BtnClusterDown = new System.Windows.Forms.Button();
            TxtClusterDescription = new ValidatedTextBox();
            TxtClusterBaseUrl = new ValidatedTextBox();
            TxtClusterId = new ValidatedTextBox();
            GrpClusters = new System.Windows.Forms.GroupBox();
            BtnCopy = new System.Windows.Forms.Button();
            GrpCluster = new System.Windows.Forms.GroupBox();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            grpNamespace = new System.Windows.Forms.GroupBox();
            TxtTestMessage = new System.Windows.Forms.TextBox();
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
            label1 = new System.Windows.Forms.Label();
            CboKubernetesTimespan = new System.Windows.Forms.ComboBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            GrpClusters.SuspendLayout();
            GrpCluster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            grpNamespace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // LstClusters
            // 
            LstClusters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstClusters.FormattingEnabled = true;
            LstClusters.IntegralHeight = false;
            LstClusters.Location = new System.Drawing.Point(6, 22);
            LstClusters.Name = "LstClusters";
            LstClusters.Size = new System.Drawing.Size(247, 367);
            LstClusters.TabIndex = 0;
            LstClusters.SelectedIndexChanged += LstClusters_SelectedIndexChanged;
            // 
            // BtnAddCluster
            // 
            BtnAddCluster.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnAddCluster.Location = new System.Drawing.Point(6, 395);
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
            BtnRemoveCluster.Location = new System.Drawing.Point(92, 395);
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
            BtnClusterUp.Location = new System.Drawing.Point(206, 395);
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
            BtnClusterDown.Location = new System.Drawing.Point(231, 395);
            BtnClusterDown.Name = "BtnClusterDown";
            BtnClusterDown.Size = new System.Drawing.Size(22, 23);
            BtnClusterDown.TabIndex = 4;
            BtnClusterDown.UseVisualStyleBackColor = true;
            BtnClusterDown.Click += BtnClusterDown_Click;
            // 
            // TxtClusterDescription
            // 
            TxtClusterDescription.BackColor = System.Drawing.Color.MistyRose;
            TxtClusterDescription.IsRequired = true;
            TxtClusterDescription.IsWhiteSpaceAllowed = false;
            TxtClusterDescription.Location = new System.Drawing.Point(8, 37);
            TxtClusterDescription.Name = "TxtClusterDescription";
            TxtClusterDescription.Size = new System.Drawing.Size(239, 23);
            TxtClusterDescription.TabIndex = 5;
            TxtClusterDescription.TextChanged += TxtClusterDescription_TextChanged;
            // 
            // TxtClusterBaseUrl
            // 
            TxtClusterBaseUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtClusterBaseUrl.BackColor = System.Drawing.Color.MistyRose;
            TxtClusterBaseUrl.IsRequired = true;
            TxtClusterBaseUrl.IsWhiteSpaceAllowed = false;
            TxtClusterBaseUrl.Location = new System.Drawing.Point(8, 125);
            TxtClusterBaseUrl.Name = "TxtClusterBaseUrl";
            TxtClusterBaseUrl.Size = new System.Drawing.Size(505, 23);
            TxtClusterBaseUrl.TabIndex = 6;
            TxtClusterBaseUrl.TextChanged += TxtClusterBaseUrl_TextChanged;
            // 
            // TxtClusterId
            // 
            TxtClusterId.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtClusterId.BackColor = System.Drawing.Color.MistyRose;
            TxtClusterId.IsRequired = true;
            TxtClusterId.IsWhiteSpaceAllowed = false;
            TxtClusterId.Location = new System.Drawing.Point(6, 81);
            TxtClusterId.Name = "TxtClusterId";
            TxtClusterId.Size = new System.Drawing.Size(272, 23);
            TxtClusterId.TabIndex = 7;
            TxtClusterId.TextChanged += TxtClusterId_TextChanged;
            // 
            // GrpClusters
            // 
            GrpClusters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpClusters.Controls.Add(BtnCopy);
            GrpClusters.Controls.Add(GrpCluster);
            GrpClusters.Controls.Add(LstClusters);
            GrpClusters.Controls.Add(BtnAddCluster);
            GrpClusters.Controls.Add(BtnRemoveCluster);
            GrpClusters.Controls.Add(BtnClusterUp);
            GrpClusters.Controls.Add(BtnClusterDown);
            GrpClusters.Location = new System.Drawing.Point(0, 63);
            GrpClusters.Name = "GrpClusters";
            GrpClusters.Size = new System.Drawing.Size(782, 424);
            GrpClusters.TabIndex = 8;
            GrpClusters.TabStop = false;
            GrpClusters.Text = "Clusters";
            // 
            // BtnCopy
            // 
            BtnCopy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnCopy.Enabled = false;
            BtnCopy.Image = Properties.Resources.copy;
            BtnCopy.Location = new System.Drawing.Point(178, 395);
            BtnCopy.Name = "BtnCopy";
            BtnCopy.Size = new System.Drawing.Size(22, 23);
            BtnCopy.TabIndex = 24;
            toolTip1.SetToolTip(BtnCopy, "Kopiëren");
            BtnCopy.UseVisualStyleBackColor = true;
            BtnCopy.Click += BtnCopy_Click;
            // 
            // GrpCluster
            // 
            GrpCluster.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpCluster.Controls.Add(pictureBox2);
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
            GrpCluster.Location = new System.Drawing.Point(259, 16);
            GrpCluster.Name = "GrpCluster";
            GrpCluster.Size = new System.Drawing.Size(517, 373);
            GrpCluster.TabIndex = 14;
            GrpCluster.TabStop = false;
            GrpCluster.Text = "Cluster";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.help;
            pictureBox2.Location = new System.Drawing.Point(64, 106);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(16, 16);
            pictureBox2.TabIndex = 35;
            pictureBox2.TabStop = false;
            toolTip1.SetToolTip(pictureBox2, "Dit is de basis kubernetes url. Deze eindigd op k8s, bijvoorbeeld: http://localhost/k8s");
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
            grpNamespace.Size = new System.Drawing.Size(278, 176);
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
            TxtTestMessage.ReadOnly = true;
            TxtTestMessage.Size = new System.Drawing.Size(264, 27);
            TxtTestMessage.TabIndex = 15;
            TxtTestMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnTest
            // 
            BtnTest.Anchor = System.Windows.Forms.AnchorStyles.Top;
            BtnTest.Location = new System.Drawing.Point(95, 110);
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
            TxtNamespaceDescription.BackColor = System.Drawing.Color.MistyRose;
            TxtNamespaceDescription.IsRequired = true;
            TxtNamespaceDescription.IsWhiteSpaceAllowed = false;
            TxtNamespaceDescription.Location = new System.Drawing.Point(6, 37);
            TxtNamespaceDescription.Name = "TxtNamespaceDescription";
            TxtNamespaceDescription.Size = new System.Drawing.Size(266, 23);
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
            TxtNamespaceName.BackColor = System.Drawing.Color.MistyRose;
            TxtNamespaceName.ForeColor = System.Drawing.SystemColors.WindowText;
            TxtNamespaceName.IsRequired = true;
            TxtNamespaceName.IsWhiteSpaceAllowed = false;
            TxtNamespaceName.Location = new System.Drawing.Point(6, 81);
            TxtNamespaceName.Name = "TxtNamespaceName";
            TxtNamespaceName.Size = new System.Drawing.Size(266, 23);
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
            BtnAddNamespace.Location = new System.Drawing.Point(6, 344);
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
            BtnRemoveNamespace.Location = new System.Drawing.Point(92, 344);
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
            LstNamespaces.Size = new System.Drawing.Size(221, 169);
            LstNamespaces.TabIndex = 15;
            LstNamespaces.SelectedIndexChanged += LstNamespaces_SelectedIndexChanged;
            // 
            // BtnNamespaceUp
            // 
            BtnNamespaceUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnNamespaceUp.Image = Properties.Resources.up;
            BtnNamespaceUp.Location = new System.Drawing.Point(181, 344);
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
            BtnNamespaceDown.Location = new System.Drawing.Point(205, 344);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 37);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(118, 15);
            label1.TabIndex = 33;
            label1.Text = "Standaard tijdspanne";
            // 
            // CboKubernetesTimespan
            // 
            CboKubernetesTimespan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboKubernetesTimespan.FormattingEnabled = true;
            CboKubernetesTimespan.Location = new System.Drawing.Point(134, 34);
            CboKubernetesTimespan.Name = "CboKubernetesTimespan";
            CboKubernetesTimespan.Size = new System.Drawing.Size(187, 23);
            CboKubernetesTimespan.TabIndex = 32;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.help;
            pictureBox1.Location = new System.Drawing.Point(327, 37);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(16, 16);
            pictureBox1.TabIndex = 34;
            pictureBox1.TabStop = false;
            toolTip1.SetToolTip(pictureBox1, "De tijdspanne limiteerd het downloaden van het log tot de aangegeven periode. DIt is handig wanneer er een groot logbestand aanwezig is en alleen recente data opgehaald hoeft te worden.");
            // 
            // UserControlKubernetesConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(CboKubernetesTimespan);
            Controls.Add(lblLogLayout);
            Controls.Add(CboLogLayout);
            Controls.Add(GrpClusters);
            Name = "UserControlKubernetesConfig";
            Size = new System.Drawing.Size(782, 490);
            GrpClusters.ResumeLayout(false);
            GrpCluster.ResumeLayout(false);
            GrpCluster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            grpNamespace.ResumeLayout(false);
            grpNamespace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
        private System.Windows.Forms.Button BtnCopy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CboKubernetesTimespan;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
