namespace LogScraper.LogProviders.File
{
    partial class UserControlFileLogProvider
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
            txtFilePath = new System.Windows.Forms.TextBox();
            btnBrowse = new System.Windows.Forms.Button();
            lblFileName = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // txtFilePath
            // 
            txtFilePath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtFilePath.Location = new System.Drawing.Point(4, 18);
            txtFilePath.Multiline = true;
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new System.Drawing.Size(254, 92);
            txtFilePath.TabIndex = 0;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBrowse.Location = new System.Drawing.Point(262, 18);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new System.Drawing.Size(30, 23);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += BtnBrowse_Click;
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Location = new System.Drawing.Point(4, 0);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new System.Drawing.Size(84, 15);
            lblFileName.TabIndex = 2;
            lblFileName.Text = "Bestandsnaam";
            // 
            // UserControlFileLogProvider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblFileName);
            Controls.Add(btnBrowse);
            Controls.Add(txtFilePath);
            Name = "UserControlFileLogProvider";
            Size = new System.Drawing.Size(295, 113);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblFileName;
    }
}
