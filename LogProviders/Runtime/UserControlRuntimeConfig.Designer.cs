using LogScraper.Utilities.Extensions;

namespace LogScraper.LogProviders.Kubernetes
{
    partial class UserControlRuntimeConfig
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
            LstUrls = new System.Windows.Forms.ListBox();
            BtnAddUrl = new System.Windows.Forms.Button();
            BtnRemoveUrl = new System.Windows.Forms.Button();
            BtnUrlUp = new System.Windows.Forms.Button();
            BtnUrlDown = new System.Windows.Forms.Button();
            TxtDescription = new ValidatedTextBox();
            TxtUrl = new ValidatedTextBox();
            GrpRuntimes = new System.Windows.Forms.GroupBox();
            grpRuntime = new System.Windows.Forms.GroupBox();
            TxtTestMessage = new System.Windows.Forms.TextBox();
            BtnTest = new System.Windows.Forms.Button();
            LblUrl = new System.Windows.Forms.Label();
            LblClusterDescription = new System.Windows.Forms.Label();
            CboLogLayout = new System.Windows.Forms.ComboBox();
            lblLogLayout = new System.Windows.Forms.Label();
            GrpRuntimes.SuspendLayout();
            grpRuntime.SuspendLayout();
            SuspendLayout();
            // 
            // LstUrls
            // 
            LstUrls.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstUrls.FormattingEnabled = true;
            LstUrls.IntegralHeight = false;
            LstUrls.Location = new System.Drawing.Point(6, 22);
            LstUrls.Name = "LstUrls";
            LstUrls.Size = new System.Drawing.Size(232, 396);
            LstUrls.TabIndex = 0;
            LstUrls.SelectedIndexChanged += LstUrls_SelectedIndexChanged;
            // 
            // BtnAddUrl
            // 
            BtnAddUrl.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnAddUrl.Location = new System.Drawing.Point(6, 424);
            BtnAddUrl.Name = "BtnAddUrl";
            BtnAddUrl.Size = new System.Drawing.Size(80, 23);
            BtnAddUrl.TabIndex = 1;
            BtnAddUrl.Text = "Toevoegen";
            BtnAddUrl.UseVisualStyleBackColor = true;
            BtnAddUrl.Click += BtnAddUrl_Click;
            // 
            // BtnRemoveUrl
            // 
            BtnRemoveUrl.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnRemoveUrl.Location = new System.Drawing.Point(92, 424);
            BtnRemoveUrl.Name = "BtnRemoveUrl";
            BtnRemoveUrl.Size = new System.Drawing.Size(80, 23);
            BtnRemoveUrl.TabIndex = 2;
            BtnRemoveUrl.Text = "Verwijderen";
            BtnRemoveUrl.UseVisualStyleBackColor = true;
            BtnRemoveUrl.Click += BtnRemoveUrl_Click;
            // 
            // BtnUrlUp
            // 
            BtnUrlUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnUrlUp.Image = Properties.Resources.up;
            BtnUrlUp.Location = new System.Drawing.Point(192, 424);
            BtnUrlUp.Name = "BtnUrlUp";
            BtnUrlUp.Size = new System.Drawing.Size(22, 23);
            BtnUrlUp.TabIndex = 3;
            BtnUrlUp.UseVisualStyleBackColor = true;
            BtnUrlUp.Click += BtnUrlUp_Click;
            // 
            // BtnUrlDown
            // 
            BtnUrlDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnUrlDown.Image = Properties.Resources.down;
            BtnUrlDown.Location = new System.Drawing.Point(216, 424);
            BtnUrlDown.Name = "BtnUrlDown";
            BtnUrlDown.Size = new System.Drawing.Size(22, 23);
            BtnUrlDown.TabIndex = 4;
            BtnUrlDown.UseVisualStyleBackColor = true;
            BtnUrlDown.Click += BtnUrlDown_Click;
            // 
            // TxtDescription
            // 
            TxtDescription.IsRequired = true;
            TxtDescription.Location = new System.Drawing.Point(8, 37);
            TxtDescription.Name = "TxtDescription";
            TxtDescription.Size = new System.Drawing.Size(239, 23);
            TxtDescription.TabIndex = 5;
            TxtDescription.TextChanged += TxtDescription_TextChanged;
            // 
            // TxtUrl
            // 
            TxtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtUrl.IsRequired = true;
            TxtUrl.Location = new System.Drawing.Point(8, 81);
            TxtUrl.Name = "TxtUrl";
            TxtUrl.Size = new System.Drawing.Size(520, 23);
            TxtUrl.TabIndex = 6;
            TxtUrl.TextChanged += TxtUrl_TextChanged;
            // 
            // GrpRuntimes
            // 
            GrpRuntimes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpRuntimes.Controls.Add(grpRuntime);
            GrpRuntimes.Controls.Add(LstUrls);
            GrpRuntimes.Controls.Add(BtnAddUrl);
            GrpRuntimes.Controls.Add(BtnRemoveUrl);
            GrpRuntimes.Controls.Add(BtnUrlUp);
            GrpRuntimes.Controls.Add(BtnUrlDown);
            GrpRuntimes.Location = new System.Drawing.Point(0, 34);
            GrpRuntimes.Name = "GrpRuntimes";
            GrpRuntimes.Size = new System.Drawing.Size(782, 453);
            GrpRuntimes.TabIndex = 8;
            GrpRuntimes.TabStop = false;
            GrpRuntimes.Text = "Urls";
            // 
            // grpRuntime
            // 
            grpRuntime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpRuntime.Controls.Add(TxtTestMessage);
            grpRuntime.Controls.Add(BtnTest);
            grpRuntime.Controls.Add(LblUrl);
            grpRuntime.Controls.Add(LblClusterDescription);
            grpRuntime.Controls.Add(TxtUrl);
            grpRuntime.Controls.Add(TxtDescription);
            grpRuntime.Location = new System.Drawing.Point(244, 16);
            grpRuntime.Name = "grpRuntime";
            grpRuntime.Size = new System.Drawing.Size(532, 402);
            grpRuntime.TabIndex = 14;
            grpRuntime.TabStop = false;
            grpRuntime.Text = "Urls";
            // 
            // TxtTestMessage
            // 
            TxtTestMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtTestMessage.BackColor = System.Drawing.SystemColors.Control;
            TxtTestMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TxtTestMessage.Location = new System.Drawing.Point(8, 142);
            TxtTestMessage.Multiline = true;
            TxtTestMessage.Name = "TxtTestMessage";
            TxtTestMessage.ReadOnly = true;
            TxtTestMessage.Size = new System.Drawing.Size(518, 206);
            TxtTestMessage.TabIndex = 35;
            TxtTestMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnTest
            // 
            BtnTest.Anchor = System.Windows.Forms.AnchorStyles.Top;
            BtnTest.Location = new System.Drawing.Point(190, 110);
            BtnTest.Name = "BtnTest";
            BtnTest.Size = new System.Drawing.Size(96, 27);
            BtnTest.TabIndex = 16;
            BtnTest.Text = "Test";
            BtnTest.UseVisualStyleBackColor = true;
            BtnTest.Click += BtnTest_Click;
            // 
            // LblUrl
            // 
            LblUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblUrl.AutoSize = true;
            LblUrl.Location = new System.Drawing.Point(8, 63);
            LblUrl.Name = "LblUrl";
            LblUrl.Size = new System.Drawing.Size(22, 15);
            LblUrl.TabIndex = 9;
            LblUrl.Text = "Url";
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
            // UserControlRuntimeConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblLogLayout);
            Controls.Add(CboLogLayout);
            Controls.Add(GrpRuntimes);
            Name = "UserControlRuntimeConfig";
            Size = new System.Drawing.Size(782, 490);
            GrpRuntimes.ResumeLayout(false);
            grpRuntime.ResumeLayout(false);
            grpRuntime.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox LstUrls;
        private System.Windows.Forms.Button BtnAddUrl;
        private System.Windows.Forms.Button BtnRemoveUrl;
        private System.Windows.Forms.Button BtnUrlUp;
        private System.Windows.Forms.Button BtnUrlDown;
        private ValidatedTextBox TxtDescription;
        private ValidatedTextBox TxtUrl;
        private System.Windows.Forms.GroupBox GrpRuntimes;
        private System.Windows.Forms.GroupBox grpRuntime;
        private System.Windows.Forms.Label LblUrl;
        private System.Windows.Forms.Label LblClusterDescription;
        private System.Windows.Forms.ComboBox CboLogLayout;
        private System.Windows.Forms.Label lblLogLayout;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.TextBox TxtTestMessage;
    }
}
