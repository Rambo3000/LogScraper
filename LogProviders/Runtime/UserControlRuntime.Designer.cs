namespace LogScraper.LogProviders.Runtime
{
    partial class UserControlRuntimeLogProvider
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
            label3 = new System.Windows.Forms.Label();
            cboRuntimeInstances = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            txtUrl = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(4, 32);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(28, 15);
            label3.TabIndex = 23;
            label3.Text = "URL";
            // 
            // cboRuntimeInstances
            // 
            cboRuntimeInstances.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboRuntimeInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboRuntimeInstances.FormattingEnabled = true;
            cboRuntimeInstances.Location = new System.Drawing.Point(88, 3);
            cboRuntimeInstances.Name = "cboRuntimeInstances";
            cboRuntimeInstances.Size = new System.Drawing.Size(168, 23);
            cboRuntimeInstances.TabIndex = 22;
            cboRuntimeInstances.SelectedIndexChanged += CboRuntimeInstances_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 6);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 15);
            label2.TabIndex = 21;
            label2.Text = "Omschrijving";
            // 
            // txtUrl
            // 
            txtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUrl.Location = new System.Drawing.Point(88, 32);
            txtUrl.Multiline = true;
            txtUrl.Name = "txtUrl";
            txtUrl.ReadOnly = true;
            txtUrl.Size = new System.Drawing.Size(179, 48);
            txtUrl.TabIndex = 30;
            // 
            // UserControlRuntimeLogProvider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(txtUrl);
            Controls.Add(label3);
            Controls.Add(cboRuntimeInstances);
            Controls.Add(label2);
            Name = "UserControlRuntimeLogProvider";
            Size = new System.Drawing.Size(259, 83);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboRuntimeInstances;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUrl;
    }
}
