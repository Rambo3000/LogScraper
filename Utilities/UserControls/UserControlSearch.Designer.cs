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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSearch));
            txtSearch = new System.Windows.Forms.TextBox();
            btnSearchNext = new System.Windows.Forms.Button();
            chkCaseSensitive = new System.Windows.Forms.CheckBox();
            btnSearchPrevious = new System.Windows.Forms.Button();
            chkWholeWordsOnly = new System.Windows.Forms.CheckBox();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            lblResults = new System.Windows.Forms.Label();
            chkWrapAround = new System.Windows.Forms.CheckBox();
            LstLogContent = new System.Windows.Forms.ListBox();
            BtnClear = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            PnlUsedForCorrectScaling = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            PnlUsedForCorrectScaling.SuspendLayout();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Location = new System.Drawing.Point(3, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(178, 23);
            txtSearch.TabIndex = 0;
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            txtSearch.Leave += TxtSearch_Leave;
            // 
            // btnSearchNext
            // 
            btnSearchNext.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSearchNext.BackgroundImage = Properties.Resources.arrow_right;
            btnSearchNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnSearchNext.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnSearchNext.Location = new System.Drawing.Point(212, 2);
            btnSearchNext.Name = "btnSearchNext";
            btnSearchNext.Size = new System.Drawing.Size(30, 25);
            btnSearchNext.TabIndex = 1;
            btnSearchNext.UseVisualStyleBackColor = true;
            btnSearchNext.Click += BtnSearchNext_Click;
            // 
            // chkCaseSensitive
            // 
            chkCaseSensitive.Appearance = System.Windows.Forms.Appearance.Button;
            chkCaseSensitive.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            chkCaseSensitive.Location = new System.Drawing.Point(3, 30);
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
            btnSearchPrevious.BackgroundImage = Properties.Resources.arrow_left;
            btnSearchPrevious.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnSearchPrevious.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            btnSearchPrevious.Location = new System.Drawing.Point(183, 2);
            btnSearchPrevious.Name = "btnSearchPrevious";
            btnSearchPrevious.Size = new System.Drawing.Size(30, 25);
            btnSearchPrevious.TabIndex = 4;
            btnSearchPrevious.UseVisualStyleBackColor = true;
            btnSearchPrevious.Click += BtnSearchPrevious_Click;
            // 
            // chkWholeWordsOnly
            // 
            chkWholeWordsOnly.Appearance = System.Windows.Forms.Appearance.Button;
            chkWholeWordsOnly.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            chkWholeWordsOnly.Location = new System.Drawing.Point(35, 30);
            chkWholeWordsOnly.Name = "chkWholeWordsOnly";
            chkWholeWordsOnly.Size = new System.Drawing.Size(38, 25);
            chkWholeWordsOnly.TabIndex = 5;
            chkWholeWordsOnly.Text = "|Ab|";
            chkWholeWordsOnly.UseVisualStyleBackColor = true;
            chkWholeWordsOnly.CheckedChanged += ChkWholeWordsOnly_CheckedChanged;
            // 
            // lblResults
            // 
            lblResults.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblResults.ForeColor = System.Drawing.Color.Black;
            lblResults.Location = new System.Drawing.Point(110, 35);
            lblResults.Name = "lblResults";
            lblResults.Size = new System.Drawing.Size(108, 20);
            lblResults.TabIndex = 6;
            // 
            // chkWrapAround
            // 
            chkWrapAround.Appearance = System.Windows.Forms.Appearance.Button;
            chkWrapAround.BackgroundImage = Properties.Resources.arrow_wraparound;
            chkWrapAround.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            chkWrapAround.Checked = true;
            chkWrapAround.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWrapAround.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            chkWrapAround.Location = new System.Drawing.Point(74, 30);
            chkWrapAround.Name = "chkWrapAround";
            chkWrapAround.Size = new System.Drawing.Size(30, 25);
            chkWrapAround.TabIndex = 7;
            chkWrapAround.UseVisualStyleBackColor = true;
            // 
            // LstLogContent
            // 
            LstLogContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstLogContent.Dock = System.Windows.Forms.DockStyle.Fill;
            LstLogContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            LstLogContent.FormattingEnabled = true;
            LstLogContent.IntegralHeight = false;
            LstLogContent.Location = new System.Drawing.Point(0, 0);
            LstLogContent.Name = "LstLogContent";
            LstLogContent.Size = new System.Drawing.Size(244, 162);
            LstLogContent.TabIndex = 8;
            LstLogContent.DrawItem += LstLogContent_DrawItem;
            LstLogContent.SelectedIndexChanged += LstLogContent_SelectedIndexChanged;
            // 
            // BtnClear
            // 
            BtnClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            BtnClear.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            BtnClear.Image = (System.Drawing.Image)resources.GetObject("BtnClear.Image");
            BtnClear.Location = new System.Drawing.Point(224, 35);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new System.Drawing.Size(20, 20);
            BtnClear.TabIndex = 9;
            BtnClear.UseVisualStyleBackColor = true;
            BtnClear.Click += BtnClear_Click;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.Controls.Add(LstLogContent);
            panel1.Location = new System.Drawing.Point(0, 57);
            panel1.Margin = new System.Windows.Forms.Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(244, 162);
            panel1.TabIndex = 10;
            // 
            // PnlUsedForCorrectScaling
            // 
            PnlUsedForCorrectScaling.Controls.Add(panel1);
            PnlUsedForCorrectScaling.Controls.Add(txtSearch);
            PnlUsedForCorrectScaling.Controls.Add(lblResults);
            PnlUsedForCorrectScaling.Controls.Add(chkCaseSensitive);
            PnlUsedForCorrectScaling.Controls.Add(BtnClear);
            PnlUsedForCorrectScaling.Controls.Add(chkWholeWordsOnly);
            PnlUsedForCorrectScaling.Controls.Add(chkWrapAround);
            PnlUsedForCorrectScaling.Controls.Add(btnSearchPrevious);
            PnlUsedForCorrectScaling.Controls.Add(btnSearchNext);
            PnlUsedForCorrectScaling.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForCorrectScaling.Location = new System.Drawing.Point(0, 0);
            PnlUsedForCorrectScaling.Name = "PnlUsedForCorrectScaling";
            PnlUsedForCorrectScaling.Size = new System.Drawing.Size(244, 219);
            PnlUsedForCorrectScaling.TabIndex = 11;
            // 
            // UserControlSearch
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlUsedForCorrectScaling);
            Name = "UserControlSearch";
            Size = new System.Drawing.Size(244, 219);
            panel1.ResumeLayout(false);
            PnlUsedForCorrectScaling.ResumeLayout(false);
            PnlUsedForCorrectScaling.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearchNext;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.Button btnSearchPrevious;
        private System.Windows.Forms.CheckBox chkWholeWordsOnly;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Label lblResults;
        private System.Windows.Forms.CheckBox chkWrapAround;
        private System.Windows.Forms.ListBox LstLogContent;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PnlUsedForCorrectScaling;
    }
}
