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
            ChkShowExtraLogEntries = new System.Windows.Forms.CheckBox();
            TxtExtraLogEntries = new System.Windows.Forms.TextBox();
            txtSearch = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // LstLogContent
            // 
            LstLogContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LstLogContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstLogContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            LstLogContent.FormattingEnabled = true;
            LstLogContent.IntegralHeight = false;
            LstLogContent.Location = new System.Drawing.Point(0, 62);
            LstLogContent.Name = "LstLogContent";
            LstLogContent.Size = new System.Drawing.Size(216, 93);
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
            // ChkShowExtraLogEntries
            // 
            ChkShowExtraLogEntries.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ChkShowExtraLogEntries.AutoSize = true;
            ChkShowExtraLogEntries.Location = new System.Drawing.Point(5, 161);
            ChkShowExtraLogEntries.Name = "ChkShowExtraLogEntries";
            ChkShowExtraLogEntries.Size = new System.Drawing.Size(115, 19);
            ChkShowExtraLogEntries.TabIndex = 3;
            ChkShowExtraLogEntries.Text = "Toon extra regels";
            ChkShowExtraLogEntries.UseVisualStyleBackColor = true;
            ChkShowExtraLogEntries.CheckedChanged += ChkShowExtraLogEntries_CheckedChanged;
            // 
            // TxtExtraLogEntries
            // 
            TxtExtraLogEntries.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TxtExtraLogEntries.Location = new System.Drawing.Point(126, 159);
            TxtExtraLogEntries.Name = "TxtExtraLogEntries";
            TxtExtraLogEntries.Size = new System.Drawing.Size(53, 23);
            TxtExtraLogEntries.TabIndex = 4;
            TxtExtraLogEntries.Text = "20";
            TxtExtraLogEntries.Visible = false;
            TxtExtraLogEntries.TextChanged += TxtExtraLogEntries_TextChanged;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Location = new System.Drawing.Point(0, 33);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(213, 23);
            txtSearch.TabIndex = 7;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            txtSearch.Leave += TxtSearch_Leave;
            // 
            // UserControlBeginEndFilter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.SystemColors.Window;
            Controls.Add(txtSearch);
            Controls.Add(TxtExtraLogEntries);
            Controls.Add(ChkShowExtraLogEntries);
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
        private System.Windows.Forms.CheckBox ChkShowExtraLogEntries;
        private System.Windows.Forms.TextBox TxtExtraLogEntries;
        private System.Windows.Forms.TextBox txtSearch;
    }
}
