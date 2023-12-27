namespace LogScraper.SourceAdapters
{
    partial class FormHttpCredentials
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
            cboAuthenticationType = new System.Windows.Forms.ComboBox();
            lblAuthenticationType = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            txtBearerToken = new System.Windows.Forms.TextBox();
            txtKey = new System.Windows.Forms.TextBox();
            txtSecret = new System.Windows.Forms.TextBox();
            lblUsername = new System.Windows.Forms.Label();
            lblPassword = new System.Windows.Forms.Label();
            lblBearerToken = new System.Windows.Forms.Label();
            lblKey = new System.Windows.Forms.Label();
            lblSecret = new System.Windows.Forms.Label();
            btnOk = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            lblUrlValue = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // cboAuthenticationType
            // 
            cboAuthenticationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboAuthenticationType.FormattingEnabled = true;
            cboAuthenticationType.Location = new System.Drawing.Point(119, 32);
            cboAuthenticationType.Name = "cboAuthenticationType";
            cboAuthenticationType.Size = new System.Drawing.Size(139, 23);
            cboAuthenticationType.TabIndex = 0;
            cboAuthenticationType.SelectedIndexChanged += CboAuthenticationType_SelectedIndexChanged;
            // 
            // lblAuthenticationType
            // 
            lblAuthenticationType.AutoSize = true;
            lblAuthenticationType.Location = new System.Drawing.Point(10, 35);
            lblAuthenticationType.Name = "lblAuthenticationType";
            lblAuthenticationType.Size = new System.Drawing.Size(103, 15);
            lblAuthenticationType.TabIndex = 1;
            lblAuthenticationType.Text = "Type authenticatie";
            // 
            // txtUsername
            // 
            txtUsername.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUsername.Location = new System.Drawing.Point(119, 65);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(356, 23);
            txtUsername.TabIndex = 2;
            txtUsername.Visible = false;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPassword.Location = new System.Drawing.Point(119, 94);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new System.Drawing.Size(356, 23);
            txtPassword.TabIndex = 3;
            txtPassword.Visible = false;
            // 
            // txtBearerToken
            // 
            txtBearerToken.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtBearerToken.Location = new System.Drawing.Point(119, 65);
            txtBearerToken.Name = "txtBearerToken";
            txtBearerToken.Size = new System.Drawing.Size(357, 23);
            txtBearerToken.TabIndex = 4;
            txtBearerToken.Visible = false;
            // 
            // txtKey
            // 
            txtKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtKey.Location = new System.Drawing.Point(119, 65);
            txtKey.Name = "txtKey";
            txtKey.Size = new System.Drawing.Size(356, 23);
            txtKey.TabIndex = 5;
            txtKey.Visible = false;
            // 
            // txtSecret
            // 
            txtSecret.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSecret.Location = new System.Drawing.Point(118, 94);
            txtSecret.Name = "txtSecret";
            txtSecret.PasswordChar = '*';
            txtSecret.Size = new System.Drawing.Size(357, 23);
            txtSecret.TabIndex = 6;
            txtSecret.Visible = false;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new System.Drawing.Point(12, 65);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(93, 15);
            lblUsername.TabIndex = 7;
            lblUsername.Text = "Gebruikersnaam";
            lblUsername.Visible = false;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(11, 94);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(75, 15);
            lblPassword.TabIndex = 8;
            lblPassword.Text = "Wachtwoord";
            lblPassword.Visible = false;
            // 
            // lblBearerToken
            // 
            lblBearerToken.AutoSize = true;
            lblBearerToken.Location = new System.Drawing.Point(12, 65);
            lblBearerToken.Name = "lblBearerToken";
            lblBearerToken.Size = new System.Drawing.Size(73, 15);
            lblBearerToken.TabIndex = 9;
            lblBearerToken.Text = "Bearer token";
            lblBearerToken.Visible = false;
            // 
            // lblKey
            // 
            lblKey.AutoSize = true;
            lblKey.Location = new System.Drawing.Point(12, 65);
            lblKey.Name = "lblKey";
            lblKey.Size = new System.Drawing.Size(26, 15);
            lblKey.TabIndex = 10;
            lblKey.Text = "Key";
            lblKey.Visible = false;
            // 
            // lblSecret
            // 
            lblSecret.AutoSize = true;
            lblSecret.Location = new System.Drawing.Point(13, 94);
            lblSecret.Name = "lblSecret";
            lblSecret.Size = new System.Drawing.Size(72, 15);
            lblSecret.TabIndex = 11;
            lblSecret.Text = "Secret/Value";
            lblSecret.Visible = false;
            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOk.Location = new System.Drawing.Point(399, 139);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(75, 23);
            btnOk.TabIndex = 12;
            btnOk.Text = "Ok";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += BtnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(318, 139);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // lblUrlValue
            // 
            lblUrlValue.Location = new System.Drawing.Point(11, 9);
            lblUrlValue.Name = "lblUrlValue";
            lblUrlValue.Size = new System.Drawing.Size(477, 16);
            lblUrlValue.TabIndex = 15;
            lblUrlValue.Text = "-";
            // 
            // FormHttpCredentials
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(487, 174);
            Controls.Add(lblUrlValue);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(lblSecret);
            Controls.Add(lblKey);
            Controls.Add(lblBearerToken);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(txtSecret);
            Controls.Add(txtKey);
            Controls.Add(txtBearerToken);
            Controls.Add(txtUsername);
            Controls.Add(lblAuthenticationType);
            Controls.Add(cboAuthenticationType);
            Controls.Add(txtPassword);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Name = "FormHttpCredentials";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            Text = "Geef credentials op om verbinding te kunnen maken";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cboAuthenticationType;
        private System.Windows.Forms.Label lblAuthenticationType;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtBearerToken;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtSecret;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblBearerToken;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Label lblSecret;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUrlValue;
    }
}