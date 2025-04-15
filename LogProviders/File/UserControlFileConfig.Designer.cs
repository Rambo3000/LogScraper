using LogScraper.Extensions;

namespace LogScraper.LogProviders.Kubernetes
{
    partial class UserControlFileConfig
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
            CboLogLayout = new System.Windows.Forms.ComboBox();
            lblLogLayout = new System.Windows.Forms.Label();
            SuspendLayout();
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
            // UserControlFileConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblLogLayout);
            Controls.Add(CboLogLayout);
            Name = "UserControlFileConfig";
            Size = new System.Drawing.Size(344, 196);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox CboLogLayout;
        private System.Windows.Forms.Label lblLogLayout;
    }
}
