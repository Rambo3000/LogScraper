namespace LogScraper
{
    partial class UserControlSearch
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
            components = new System.ComponentModel.Container();
            txtSearch = new System.Windows.Forms.TextBox();
            btnSearchNext = new System.Windows.Forms.Button();
            chkCaseSensitive = new System.Windows.Forms.CheckBox();
            btnSearchPrevious = new System.Windows.Forms.Button();
            chkWholeWordsOnly = new System.Windows.Forms.CheckBox();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            lblNoResults = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Location = new System.Drawing.Point(3, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(154, 23);
            txtSearch.TabIndex = 0;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            // 
            // btnSearchNext
            // 
            btnSearchNext.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSearchNext.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnSearchNext.Location = new System.Drawing.Point(193, 3);
            btnSearchNext.Name = "btnSearchNext";
            btnSearchNext.Size = new System.Drawing.Size(30, 23);
            btnSearchNext.TabIndex = 1;
            btnSearchNext.Text = ">";
            btnSearchNext.UseVisualStyleBackColor = true;
            btnSearchNext.Click += BtnSearchNext_Click;
            // 
            // chkCaseSensitive
            // 
            chkCaseSensitive.Appearance = System.Windows.Forms.Appearance.Button;
            chkCaseSensitive.AutoSize = true;
            chkCaseSensitive.Location = new System.Drawing.Point(3, 28);
            chkCaseSensitive.Name = "chkCaseSensitive";
            chkCaseSensitive.Size = new System.Drawing.Size(31, 25);
            chkCaseSensitive.TabIndex = 3;
            chkCaseSensitive.Text = "Aa";
            chkCaseSensitive.UseVisualStyleBackColor = true;
            chkCaseSensitive.CheckedChanged += ChkCaseSensitive_CheckedChanged;
            // 
            // btnSearchPrevious
            // 
            btnSearchPrevious.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSearchPrevious.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnSearchPrevious.Location = new System.Drawing.Point(163, 3);
            btnSearchPrevious.Name = "btnSearchPrevious";
            btnSearchPrevious.Size = new System.Drawing.Size(30, 23);
            btnSearchPrevious.TabIndex = 4;
            btnSearchPrevious.Text = "<";
            btnSearchPrevious.UseVisualStyleBackColor = true;
            btnSearchPrevious.Click += BtnSearchPrevious_Click;
            // 
            // chkWholeWordsOnly
            // 
            chkWholeWordsOnly.Appearance = System.Windows.Forms.Appearance.Button;
            chkWholeWordsOnly.AutoSize = true;
            chkWholeWordsOnly.Location = new System.Drawing.Point(34, 28);
            chkWholeWordsOnly.Name = "chkWholeWordsOnly";
            chkWholeWordsOnly.Size = new System.Drawing.Size(44, 25);
            chkWholeWordsOnly.TabIndex = 5;
            chkWholeWordsOnly.Text = "|Abc|";
            chkWholeWordsOnly.UseVisualStyleBackColor = true;
            chkWholeWordsOnly.CheckedChanged += ChkWholeWordsOnly_CheckedChanged;
            // 
            // lblNoResults
            // 
            lblNoResults.AutoSize = true;
            lblNoResults.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
            lblNoResults.Location = new System.Drawing.Point(84, 33);
            lblNoResults.Name = "lblNoResults";
            lblNoResults.Size = new System.Drawing.Size(89, 15);
            lblNoResults.TabIndex = 6;
            lblNoResults.Text = "Geen resultaten";
            lblNoResults.Visible = false;
            // 
            // UserControlSearch
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lblNoResults);
            Controls.Add(chkWholeWordsOnly);
            Controls.Add(btnSearchPrevious);
            Controls.Add(chkCaseSensitive);
            Controls.Add(btnSearchNext);
            Controls.Add(txtSearch);
            Name = "UserControlSearch";
            Size = new System.Drawing.Size(223, 59);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearchNext;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.Button btnSearchPrevious;
        private System.Windows.Forms.CheckBox chkWholeWordsOnly;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Label lblNoResults;
    }
}
