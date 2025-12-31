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
            CboFolderList = new System.Windows.Forms.ComboBox();
            CboFileList = new System.Windows.Forms.ComboBox();
            LblFolder = new System.Windows.Forms.Label();
            LblFile = new System.Windows.Forms.Label();
            PnlUsedForScalingCompatibility = new System.Windows.Forms.Panel();
            PnlUsedForScalingCompatibility.SuspendLayout();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 91);
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
            cboRuntimeInstances.Location = new System.Drawing.Point(88, 4);
            cboRuntimeInstances.Name = "cboRuntimeInstances";
            cboRuntimeInstances.Size = new System.Drawing.Size(168, 23);
            cboRuntimeInstances.TabIndex = 22;
            cboRuntimeInstances.SelectedIndexChanged += CboRuntimeInstances_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 7);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 15);
            label2.TabIndex = 21;
            label2.Text = "Omschrijving";
            // 
            // txtUrl
            // 
            txtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUrl.Location = new System.Drawing.Point(88, 91);
            txtUrl.Multiline = true;
            txtUrl.Name = "txtUrl";
            txtUrl.ReadOnly = true;
            txtUrl.Size = new System.Drawing.Size(168, 45);
            txtUrl.TabIndex = 30;
            // 
            // CboFolderList
            // 
            CboFolderList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CboFolderList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboFolderList.Enabled = false;
            CboFolderList.FormattingEnabled = true;
            CboFolderList.Location = new System.Drawing.Point(88, 33);
            CboFolderList.Name = "CboFolderList";
            CboFolderList.Size = new System.Drawing.Size(168, 23);
            CboFolderList.TabIndex = 22;
            CboFolderList.SelectedIndexChanged += CboFolderList_SelectedIndexChanged;
            // 
            // CboFileList
            // 
            CboFileList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CboFileList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboFileList.Enabled = false;
            CboFileList.FormattingEnabled = true;
            CboFileList.Location = new System.Drawing.Point(88, 62);
            CboFileList.Name = "CboFileList";
            CboFileList.Size = new System.Drawing.Size(168, 23);
            CboFileList.TabIndex = 22;
            CboFileList.SelectedIndexChanged += CboFileList_SelectedIndexChanged;
            // 
            // LblFolder
            // 
            LblFolder.AutoSize = true;
            LblFolder.Enabled = false;
            LblFolder.Location = new System.Drawing.Point(3, 36);
            LblFolder.Name = "LblFolder";
            LblFolder.Size = new System.Drawing.Size(31, 15);
            LblFolder.TabIndex = 21;
            LblFolder.Text = "Map";
            // 
            // LblFile
            // 
            LblFile.AutoSize = true;
            LblFile.Enabled = false;
            LblFile.Location = new System.Drawing.Point(3, 65);
            LblFile.Name = "LblFile";
            LblFile.Size = new System.Drawing.Size(72, 15);
            LblFile.TabIndex = 21;
            LblFile.Text = "Log bestand";
            // 
            // PnlUsedForScalingCompatibility
            // 
            PnlUsedForScalingCompatibility.Controls.Add(txtUrl);
            PnlUsedForScalingCompatibility.Controls.Add(label2);
            PnlUsedForScalingCompatibility.Controls.Add(label3);
            PnlUsedForScalingCompatibility.Controls.Add(LblFolder);
            PnlUsedForScalingCompatibility.Controls.Add(CboFileList);
            PnlUsedForScalingCompatibility.Controls.Add(LblFile);
            PnlUsedForScalingCompatibility.Controls.Add(CboFolderList);
            PnlUsedForScalingCompatibility.Controls.Add(cboRuntimeInstances);
            PnlUsedForScalingCompatibility.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForScalingCompatibility.Location = new System.Drawing.Point(0, 0);
            PnlUsedForScalingCompatibility.Name = "PnlUsedForScalingCompatibility";
            PnlUsedForScalingCompatibility.Size = new System.Drawing.Size(259, 138);
            PnlUsedForScalingCompatibility.TabIndex = 31;
            // 
            // UserControlRuntimeLogProvider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlUsedForScalingCompatibility);
            Name = "UserControlRuntimeLogProvider";
            Size = new System.Drawing.Size(259, 138);
            PnlUsedForScalingCompatibility.ResumeLayout(false);
            PnlUsedForScalingCompatibility.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboRuntimeInstances;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.ComboBox CboFolderList;
        private System.Windows.Forms.ComboBox CboFileList;
        private System.Windows.Forms.Label LblFolder;
        private System.Windows.Forms.Label LblFile;
        private System.Windows.Forms.Panel PnlUsedForScalingCompatibility;
    }
}
