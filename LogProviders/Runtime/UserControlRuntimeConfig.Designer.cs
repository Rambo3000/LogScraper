﻿using LogScraper.Utilities.Extensions;

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
            GrpWebFormSettings = new System.Windows.Forms.GroupBox();
            LblCsrfFieldName = new System.Windows.Forms.Label();
            LblPasswordFieldName = new System.Windows.Forms.Label();
            LblUserFieldName = new System.Windows.Forms.Label();
            LblLoginPageUrl = new System.Windows.Forms.Label();
            TxtCsrfFieldName = new ValidatedTextBox();
            TxtPasswordFieldName = new ValidatedTextBox();
            TxtUserFieldName = new ValidatedTextBox();
            TxtLoginPageUrl = new ValidatedTextBox();
            ChkWebFormLogin = new System.Windows.Forms.CheckBox();
            TxtTestMessage = new System.Windows.Forms.TextBox();
            BtnTest = new System.Windows.Forms.Button();
            LblUrl = new System.Windows.Forms.Label();
            LblClusterDescription = new System.Windows.Forms.Label();
            CboLogLayout = new System.Windows.Forms.ComboBox();
            lblLogLayout = new System.Windows.Forms.Label();
            GrpRuntimes.SuspendLayout();
            grpRuntime.SuspendLayout();
            GrpWebFormSettings.SuspendLayout();
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
            TxtDescription.BackColor = System.Drawing.Color.MistyRose;
            TxtDescription.IsRequired = true;
            TxtDescription.IsWhiteSpaceAllowed = false;
            TxtDescription.Location = new System.Drawing.Point(8, 37);
            TxtDescription.Name = "TxtDescription";
            TxtDescription.Size = new System.Drawing.Size(239, 23);
            TxtDescription.TabIndex = 5;
            TxtDescription.TextChanged += TxtDescription_TextChanged;
            // 
            // TxtUrl
            // 
            TxtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtUrl.BackColor = System.Drawing.Color.MistyRose;
            TxtUrl.IsRequired = true;
            TxtUrl.IsWhiteSpaceAllowed = false;
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
            grpRuntime.Controls.Add(GrpWebFormSettings);
            grpRuntime.Controls.Add(ChkWebFormLogin);
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
            // GrpWebFormSettings
            // 
            GrpWebFormSettings.Controls.Add(LblCsrfFieldName);
            GrpWebFormSettings.Controls.Add(LblPasswordFieldName);
            GrpWebFormSettings.Controls.Add(LblUserFieldName);
            GrpWebFormSettings.Controls.Add(LblLoginPageUrl);
            GrpWebFormSettings.Controls.Add(TxtCsrfFieldName);
            GrpWebFormSettings.Controls.Add(TxtPasswordFieldName);
            GrpWebFormSettings.Controls.Add(TxtUserFieldName);
            GrpWebFormSettings.Controls.Add(TxtLoginPageUrl);
            GrpWebFormSettings.Location = new System.Drawing.Point(28, 137);
            GrpWebFormSettings.Name = "GrpWebFormSettings";
            GrpWebFormSettings.Size = new System.Drawing.Size(387, 197);
            GrpWebFormSettings.TabIndex = 37;
            GrpWebFormSettings.TabStop = false;
            GrpWebFormSettings.Text = "Webformulier instellingen";
            GrpWebFormSettings.Visible = false;
            // 
            // LblCsrfFieldName
            // 
            LblCsrfFieldName.AutoSize = true;
            LblCsrfFieldName.Location = new System.Drawing.Point(6, 149);
            LblCsrfFieldName.Name = "LblCsrfFieldName";
            LblCsrfFieldName.Size = new System.Drawing.Size(96, 15);
            LblCsrfFieldName.TabIndex = 3;
            LblCsrfFieldName.Text = "Naam CSRF-veld";
            // 
            // LblPasswordFieldName
            // 
            LblPasswordFieldName.AutoSize = true;
            LblPasswordFieldName.Location = new System.Drawing.Point(6, 105);
            LblPasswordFieldName.Name = "LblPasswordFieldName";
            LblPasswordFieldName.Size = new System.Drawing.Size(160, 15);
            LblPasswordFieldName.TabIndex = 2;
            LblPasswordFieldName.Text = "Naam veld voor wachtwoord";
            // 
            // LblUserFieldName
            // 
            LblUserFieldName.AutoSize = true;
            LblUserFieldName.Location = new System.Drawing.Point(6, 61);
            LblUserFieldName.Name = "LblUserFieldName";
            LblUserFieldName.Size = new System.Drawing.Size(179, 15);
            LblUserFieldName.TabIndex = 1;
            LblUserFieldName.Text = "Naam veld voor gebruikersnaam";
            // 
            // LblLoginPageUrl
            // 
            LblLoginPageUrl.AutoSize = true;
            LblLoginPageUrl.Location = new System.Drawing.Point(6, 17);
            LblLoginPageUrl.Name = "LblLoginPageUrl";
            LblLoginPageUrl.Size = new System.Drawing.Size(132, 15);
            LblLoginPageUrl.TabIndex = 0;
            LblLoginPageUrl.Text = "URL van de inlogpagina";
            // 
            // TxtCsrfFieldName
            // 
            TxtCsrfFieldName.BackColor = System.Drawing.SystemColors.Window;
            TxtCsrfFieldName.IsRequired = false;
            TxtCsrfFieldName.IsWhiteSpaceAllowed = false;
            TxtCsrfFieldName.Location = new System.Drawing.Point(6, 167);
            TxtCsrfFieldName.Name = "TxtCsrfFieldName";
            TxtCsrfFieldName.Size = new System.Drawing.Size(239, 23);
            TxtCsrfFieldName.TabIndex = 5;
            TxtCsrfFieldName.TextChanged += TxtCsrfFieldName_TextChanged;
            // 
            // TxtPasswordFieldName
            // 
            TxtPasswordFieldName.BackColor = System.Drawing.SystemColors.Window;
            TxtPasswordFieldName.IsRequired = false;
            TxtPasswordFieldName.IsWhiteSpaceAllowed = false;
            TxtPasswordFieldName.Location = new System.Drawing.Point(6, 123);
            TxtPasswordFieldName.Name = "TxtPasswordFieldName";
            TxtPasswordFieldName.Size = new System.Drawing.Size(239, 23);
            TxtPasswordFieldName.TabIndex = 5;
            TxtPasswordFieldName.TextChanged += TxtPasswordFieldName_TextChanged;
            // 
            // TxtUserFieldName
            // 
            TxtUserFieldName.BackColor = System.Drawing.SystemColors.Window;
            TxtUserFieldName.IsRequired = false;
            TxtUserFieldName.IsWhiteSpaceAllowed = false;
            TxtUserFieldName.Location = new System.Drawing.Point(6, 79);
            TxtUserFieldName.Name = "TxtUserFieldName";
            TxtUserFieldName.Size = new System.Drawing.Size(239, 23);
            TxtUserFieldName.TabIndex = 5;
            TxtUserFieldName.TextChanged += TxtUserFieldName_TextChanged;
            // 
            // TxtLoginPageUrl
            // 
            TxtLoginPageUrl.BackColor = System.Drawing.Color.MistyRose;
            TxtLoginPageUrl.IsRequired = true;
            TxtLoginPageUrl.IsWhiteSpaceAllowed = false;
            TxtLoginPageUrl.Location = new System.Drawing.Point(6, 35);
            TxtLoginPageUrl.Name = "TxtLoginPageUrl";
            TxtLoginPageUrl.Size = new System.Drawing.Size(375, 23);
            TxtLoginPageUrl.TabIndex = 5;
            TxtLoginPageUrl.TextChanged += TxtLoginPageUrl_TextChanged;
            // 
            // ChkWebFormLogin
            // 
            ChkWebFormLogin.AutoSize = true;
            ChkWebFormLogin.Location = new System.Drawing.Point(16, 112);
            ChkWebFormLogin.Name = "ChkWebFormLogin";
            ChkWebFormLogin.Size = new System.Drawing.Size(411, 19);
            ChkWebFormLogin.TabIndex = 36;
            ChkWebFormLogin.Text = "Inloggen via webformulier inschakelen (met CSRF-token en sessiecookie)";
            ChkWebFormLogin.UseVisualStyleBackColor = true;
            ChkWebFormLogin.CheckedChanged += ChkWebFormLogin_CheckedChanged;
            // 
            // TxtTestMessage
            // 
            TxtTestMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtTestMessage.BackColor = System.Drawing.SystemColors.Control;
            TxtTestMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TxtTestMessage.Location = new System.Drawing.Point(108, 369);
            TxtTestMessage.Multiline = true;
            TxtTestMessage.Name = "TxtTestMessage";
            TxtTestMessage.ReadOnly = true;
            TxtTestMessage.Size = new System.Drawing.Size(418, 27);
            TxtTestMessage.TabIndex = 35;
            TxtTestMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnTest
            // 
            BtnTest.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnTest.Location = new System.Drawing.Point(6, 369);
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
            GrpWebFormSettings.ResumeLayout(false);
            GrpWebFormSettings.PerformLayout();
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
        private System.Windows.Forms.CheckBox ChkWebFormLogin;
        private System.Windows.Forms.GroupBox GrpWebFormSettings;
        private System.Windows.Forms.Label LblCsrfFieldName;
        private System.Windows.Forms.Label LblPasswordFieldName;
        private System.Windows.Forms.Label LblUserFieldName;
        private System.Windows.Forms.Label LblLoginPageUrl;
        private ValidatedTextBox TxtLoginPageUrl;
        private ValidatedTextBox TxtCsrfFieldName;
        private ValidatedTextBox TxtPasswordFieldName;
        private ValidatedTextBox TxtUserFieldName;
    }
}
