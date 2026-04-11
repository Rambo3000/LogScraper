namespace LogScraper.Controls.Search
{
    partial class SearchResultListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchResultListControl));
            LstSearchResults = new System.Windows.Forms.ListBox();
            PnlForWIndowsScaling = new System.Windows.Forms.Panel();
            BtnClose = new System.Windows.Forms.Button();
            LblResultCount = new System.Windows.Forms.Label();
            PnlForWIndowsScaling.SuspendLayout();
            SuspendLayout();
            // 
            // LstSearchResults
            // 
            LstSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            LstSearchResults.FormattingEnabled = true;
            LstSearchResults.IntegralHeight = false;
            LstSearchResults.Location = new System.Drawing.Point(0, 0);
            LstSearchResults.Name = "LstSearchResults";
            LstSearchResults.Size = new System.Drawing.Size(666, 59);
            LstSearchResults.TabIndex = 0;
            LstSearchResults.SelectedIndexChanged += LstSearchResults_SelectedIndexChanged;
            // 
            // PnlForWIndowsScaling
            // 
            PnlForWIndowsScaling.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PnlForWIndowsScaling.Controls.Add(LstSearchResults);
            PnlForWIndowsScaling.Location = new System.Drawing.Point(0, 22);
            PnlForWIndowsScaling.Name = "PnlForWIndowsScaling";
            PnlForWIndowsScaling.Size = new System.Drawing.Size(666, 59);
            PnlForWIndowsScaling.TabIndex = 1;
            // 
            // BtnClose
            // 
            BtnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnClose.Image = (System.Drawing.Image)resources.GetObject("BtnClose.Image");
            BtnClose.Location = new System.Drawing.Point(646, -1);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new System.Drawing.Size(20, 22);
            BtnClose.TabIndex = 2;
            BtnClose.UseVisualStyleBackColor = true;
            BtnClose.Click += BtnClose_Click;
            // 
            // LblResultCount
            // 
            LblResultCount.AutoSize = true;
            LblResultCount.Location = new System.Drawing.Point(3, 5);
            LblResultCount.Name = "LblResultCount";
            LblResultCount.Size = new System.Drawing.Size(12, 15);
            LblResultCount.TabIndex = 3;
            LblResultCount.Text = "-";
            // 
            // SearchResultListControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LblResultCount);
            Controls.Add(BtnClose);
            Controls.Add(PnlForWIndowsScaling);
            Name = "SearchResultListControl";
            Size = new System.Drawing.Size(666, 81);
            PnlForWIndowsScaling.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox LstSearchResults;
        private System.Windows.Forms.Panel PnlForWIndowsScaling;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Label LblResultCount;
    }
}
