namespace LogScraper
{
    partial class UserControlBeginEndFilter
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
            LstLogContent = new System.Windows.Forms.ListBox();
            CboLogContentType = new System.Windows.Forms.ComboBox();
            BtnReset = new System.Windows.Forms.Button();
            ChkShowExtraLines = new System.Windows.Forms.CheckBox();
            TxtExtraLines = new System.Windows.Forms.TextBox();
            chkShowErrors = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // LstLogContent
            // 
            LstLogContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LstLogContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstLogContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            LstLogContent.FormattingEnabled = true;
            LstLogContent.IntegralHeight = false;
            LstLogContent.ItemHeight = 15;
            LstLogContent.Location = new System.Drawing.Point(0, 33);
            LstLogContent.Name = "LstLogContent";
            LstLogContent.Size = new System.Drawing.Size(216, 97);
            LstLogContent.TabIndex = 0;
            LstLogContent.DrawItem += LstLogContent_DrawItem;
            LstLogContent.SelectedIndexChanged += LstLogContent_SelectedIndexChanged;
            // 
            // CboLogContentType
            // 
            CboLogContentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CboLogContentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboLogContentType.FormattingEnabled = true;
            CboLogContentType.Location = new System.Drawing.Point(0, 4);
            CboLogContentType.Name = "CboLogContentType";
            CboLogContentType.Size = new System.Drawing.Size(154, 23);
            CboLogContentType.TabIndex = 1;
            CboLogContentType.SelectedIndexChanged += CboLogContentType_SelectedIndexChanged;
            // 
            // BtnReset
            // 
            BtnReset.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnReset.Location = new System.Drawing.Point(160, 3);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(53, 25);
            BtnReset.TabIndex = 2;
            BtnReset.Text = "Reset";
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // ChkShowExtraLines
            // 
            ChkShowExtraLines.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ChkShowExtraLines.AutoSize = true;
            ChkShowExtraLines.Location = new System.Drawing.Point(5, 161);
            ChkShowExtraLines.Name = "ChkShowExtraLines";
            ChkShowExtraLines.Size = new System.Drawing.Size(115, 19);
            ChkShowExtraLines.TabIndex = 3;
            ChkShowExtraLines.Text = "Toon extra regels";
            ChkShowExtraLines.UseVisualStyleBackColor = true;
            ChkShowExtraLines.CheckedChanged += ChkShowExtraLines_CheckedChanged;
            // 
            // TxtExtraLines
            // 
            TxtExtraLines.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TxtExtraLines.Enabled = false;
            TxtExtraLines.Location = new System.Drawing.Point(126, 159);
            TxtExtraLines.Name = "TxtExtraLines";
            TxtExtraLines.Size = new System.Drawing.Size(53, 23);
            TxtExtraLines.TabIndex = 4;
            TxtExtraLines.Text = "20";
            TxtExtraLines.TextChanged += TxtExtraLines_TextChanged;
            // 
            // chkShowErrors
            // 
            chkShowErrors.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chkShowErrors.AutoSize = true;
            chkShowErrors.Checked = true;
            chkShowErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            chkShowErrors.Location = new System.Drawing.Point(5, 136);
            chkShowErrors.Name = "chkShowErrors";
            chkShowErrors.Size = new System.Drawing.Size(125, 19);
            chkShowErrors.TabIndex = 5;
            chkShowErrors.Text = "Toon ERROR regels";
            chkShowErrors.UseVisualStyleBackColor = true;
            chkShowErrors.CheckedChanged += ChkShowErrors_CheckedChanged;
            // 
            // UserControlBeginEndFilter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.SystemColors.Window;
            Controls.Add(chkShowErrors);
            Controls.Add(TxtExtraLines);
            Controls.Add(ChkShowExtraLines);
            Controls.Add(BtnReset);
            Controls.Add(CboLogContentType);
            Controls.Add(LstLogContent);
            Name = "UserControlBeginEndFilter";
            Size = new System.Drawing.Size(216, 182);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox LstLogContent;
        private System.Windows.Forms.ComboBox CboLogContentType;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.CheckBox ChkShowExtraLines;
        private System.Windows.Forms.TextBox TxtExtraLines;
        private System.Windows.Forms.CheckBox chkShowErrors;
    }
}
