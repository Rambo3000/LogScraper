namespace LogScraper.Configuration
{
    partial class UserControlAbout
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
            lblVersion = new System.Windows.Forms.Label();
            LblAuthor = new System.Windows.Forms.Label();
            GrpAbout = new System.Windows.Forms.GroupBox();
            LblDisclaimer = new System.Windows.Forms.Label();
            LblRuntime = new System.Windows.Forms.Label();
            LblDisclaimerFullText = new System.Windows.Forms.Label();
            LblGnuLicense = new System.Windows.Forms.Label();
            BtnUpdate = new System.Windows.Forms.Button();
            LinkGitHub = new System.Windows.Forms.LinkLabel();
            GrpAbout.SuspendLayout();
            SuspendLayout();
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new System.Drawing.Point(8, 21);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(13, 15);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "v";
            // 
            // LblAuthor
            // 
            LblAuthor.AutoSize = true;
            LblAuthor.Location = new System.Drawing.Point(8, 59);
            LblAuthor.Name = "LblAuthor";
            LblAuthor.Size = new System.Drawing.Size(137, 15);
            LblAuthor.TabIndex = 2;
            LblAuthor.Text = "Author: Robert de Volder";
            // 
            // GrpAbout
            // 
            GrpAbout.Controls.Add(LblDisclaimer);
            GrpAbout.Controls.Add(LblRuntime);
            GrpAbout.Controls.Add(LblDisclaimerFullText);
            GrpAbout.Controls.Add(LblGnuLicense);
            GrpAbout.Controls.Add(BtnUpdate);
            GrpAbout.Controls.Add(LinkGitHub);
            GrpAbout.Controls.Add(LblAuthor);
            GrpAbout.Controls.Add(lblVersion);
            GrpAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            GrpAbout.Location = new System.Drawing.Point(0, 0);
            GrpAbout.Name = "GrpAbout";
            GrpAbout.Padding = new System.Windows.Forms.Padding(5);
            GrpAbout.Size = new System.Drawing.Size(723, 422);
            GrpAbout.TabIndex = 3;
            GrpAbout.TabStop = false;
            GrpAbout.Text = "Over Log Scraper";
            // 
            // LblDisclaimer
            // 
            LblDisclaimer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            LblDisclaimer.Location = new System.Drawing.Point(8, 186);
            LblDisclaimer.Name = "LblDisclaimer";
            LblDisclaimer.Size = new System.Drawing.Size(75, 16);
            LblDisclaimer.TabIndex = 7;
            LblDisclaimer.Text = "Disclaimer:";
            // 
            // LblRuntime
            // 
            LblRuntime.AutoSize = true;
            LblRuntime.Location = new System.Drawing.Point(8, 40);
            LblRuntime.Name = "LblRuntime";
            LblRuntime.Size = new System.Drawing.Size(52, 15);
            LblRuntime.TabIndex = 6;
            LblRuntime.Text = "Runtime";
            // 
            // LblDisclaimerFullText
            // 
            LblDisclaimerFullText.Location = new System.Drawing.Point(8, 186);
            LblDisclaimerFullText.Name = "LblDisclaimerFullText";
            LblDisclaimerFullText.Size = new System.Drawing.Size(712, 118);
            LblDisclaimerFullText.TabIndex = 5;
            // 
            // LblGnuLicense
            // 
            LblGnuLicense.Location = new System.Drawing.Point(8, 147);
            LblGnuLicense.Name = "LblGnuLicense";
            LblGnuLicense.Size = new System.Drawing.Size(707, 39);
            LblGnuLicense.TabIndex = 5;
            LblGnuLicense.Text = "This application is licensed under the GNU General Public License v3.0. You are free to use, modify, and distribute it under the terms of that license.";
            // 
            // BtnUpdate
            // 
            BtnUpdate.Location = new System.Drawing.Point(8, 100);
            BtnUpdate.Name = "BtnUpdate";
            BtnUpdate.Size = new System.Drawing.Size(165, 34);
            BtnUpdate.TabIndex = 4;
            BtnUpdate.Text = "Controleer op updates";
            BtnUpdate.UseVisualStyleBackColor = true;
            BtnUpdate.Click += BtnUpdate_Click;
            // 
            // LinkGitHub
            // 
            LinkGitHub.AutoSize = true;
            LinkGitHub.Location = new System.Drawing.Point(8, 78);
            LinkGitHub.Name = "LinkGitHub";
            LinkGitHub.Size = new System.Drawing.Size(114, 15);
            LinkGitHub.TabIndex = 3;
            LinkGitHub.TabStop = true;
            LinkGitHub.Text = "GitHub Project Page";
            LinkGitHub.LinkClicked += LinkGitHub_LinkClicked;
            // 
            // UserControlAbout
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(GrpAbout);
            Name = "UserControlAbout";
            Size = new System.Drawing.Size(723, 422);
            GrpAbout.ResumeLayout(false);
            GrpAbout.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox GrpAbout;
        private System.Windows.Forms.LinkLabel LinkGitHub;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.Label LblDisclaimerFullText;
        private System.Windows.Forms.Label LblGnuLicense;
        private System.Windows.Forms.Label LblRuntime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LblAuthor;
        private System.Windows.Forms.Label LblDisclaimer;
    }
}
