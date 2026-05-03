using LogScraper.Controls.Generic;
using LogScraper.Utilities;

namespace LogScraper.Controls.Content
{
    partial class ContentNavigationControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentNavigationControl));
            LstLogContent = new DoubleBufferedListBox();
            CboLogContentType = new System.Windows.Forms.ComboBox();
            txtSearch = new ClearableTextBoxControl();
            toolTip = new System.Windows.Forms.ToolTip(components);
            BtnShowTree = new System.Windows.Forms.Button();
            ImageListTreeView = new System.Windows.Forms.ImageList(components);
            BtnAutoScroll = new System.Windows.Forms.Button();
            ImageListAutoScroll = new System.Windows.Forms.ImageList(components);
            BtnJumpToLogPosition = new System.Windows.Forms.Button();
            PnlUsedForCorrectScaling = new System.Windows.Forms.Panel();
            LblExplenation = new System.Windows.Forms.Label();
            PnlUsedForCorrectScaling.SuspendLayout();
            SuspendLayout();
            // 
            // LstLogContent
            // 
            LstLogContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LstLogContent.BackColor = System.Drawing.SystemColors.Control;
            LstLogContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstLogContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            LstLogContent.FormattingEnabled = true;
            LstLogContent.IntegralHeight = false;
            LstLogContent.Location = new System.Drawing.Point(0, 53);
            LstLogContent.Name = "LstLogContent";
            LstLogContent.Size = new System.Drawing.Size(243, 219);
            LstLogContent.TabIndex = 0;
            LstLogContent.DrawItem += LstLogContent_DrawItem;
            LstLogContent.SelectedIndexChanged += LstLogContent_SelectedIndexChanged;
            // 
            // CboLogContentType
            // 
            CboLogContentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CboLogContentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboLogContentType.FormattingEnabled = true;
            CboLogContentType.ItemHeight = 15;
            CboLogContentType.Location = new System.Drawing.Point(87, 0);
            CboLogContentType.Name = "CboLogContentType";
            CboLogContentType.Size = new System.Drawing.Size(153, 23);
            CboLogContentType.TabIndex = 1;
            CboLogContentType.SelectedIndexChanged += CboLogContentType_SelectedIndexChanged;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.BackColor = System.Drawing.SystemColors.Window;
            txtSearch.Location = new System.Drawing.Point(3, 27);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "";
            txtSearch.Size = new System.Drawing.Size(237, 20);
            txtSearch.TabIndex = 7;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // BtnShowTree
            // 
            BtnShowTree.ImageIndex = 0;
            BtnShowTree.ImageList = ImageListTreeView;
            BtnShowTree.Location = new System.Drawing.Point(56, 0);
            BtnShowTree.Name = "BtnShowTree";
            BtnShowTree.Size = new System.Drawing.Size(25, 25);
            BtnShowTree.TabIndex = 8;
            toolTip.SetToolTip(BtnShowTree, "Hierarchische weergave");
            BtnShowTree.UseVisualStyleBackColor = true;
            BtnShowTree.Click += BtnShowTree_Click;
            // 
            // ImageListTreeView
            // 
            ImageListTreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            ImageListTreeView.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListTreeView.ImageStream");
            ImageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            ImageListTreeView.Images.SetKeyName(0, "log entries tree custommade 16x16.png");
            ImageListTreeView.Images.SetKeyName(1, "log entries tree hide custommade 16x16.png");
            // 
            // BtnAutoScroll
            // 
            BtnAutoScroll.ImageIndex = 0;
            BtnAutoScroll.ImageList = ImageListAutoScroll;
            BtnAutoScroll.Location = new System.Drawing.Point(25, 0);
            BtnAutoScroll.Name = "BtnAutoScroll";
            BtnAutoScroll.Size = new System.Drawing.Size(25, 25);
            BtnAutoScroll.TabIndex = 9;
            toolTip.SetToolTip(BtnAutoScroll, "Volg logweergave automatisch");
            BtnAutoScroll.UseVisualStyleBackColor = true;
            BtnAutoScroll.Click += BtnAutoScroll_Click;
            // 
            // ImageListAutoScroll
            // 
            ImageListAutoScroll.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            ImageListAutoScroll.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ImageListAutoScroll.ImageStream");
            ImageListAutoScroll.TransparentColor = System.Drawing.Color.Transparent;
            ImageListAutoScroll.Images.SetKeyName(0, "arrow-up-down  with A 16x16.png");
            ImageListAutoScroll.Images.SetKeyName(1, "arrow-up-down  with A 16x16 - Copy.png");
            // 
            // BtnJumpToLogPosition
            // 
            BtnJumpToLogPosition.Image = (System.Drawing.Image)resources.GetObject("BtnJumpToLogPosition.Image");
            BtnJumpToLogPosition.Location = new System.Drawing.Point(0, 0);
            BtnJumpToLogPosition.Name = "BtnJumpToLogPosition";
            BtnJumpToLogPosition.Size = new System.Drawing.Size(25, 25);
            BtnJumpToLogPosition.TabIndex = 10;
            toolTip.SetToolTip(BtnJumpToLogPosition, "Spring naar huidige logweergave");
            BtnJumpToLogPosition.UseVisualStyleBackColor = true;
            BtnJumpToLogPosition.Click += BtnJumpToLogPosition_Click;
            // 
            // PnlUsedForCorrectScaling
            // 
            PnlUsedForCorrectScaling.Controls.Add(LblExplenation);
            PnlUsedForCorrectScaling.Controls.Add(BtnJumpToLogPosition);
            PnlUsedForCorrectScaling.Controls.Add(BtnAutoScroll);
            PnlUsedForCorrectScaling.Controls.Add(BtnShowTree);
            PnlUsedForCorrectScaling.Controls.Add(CboLogContentType);
            PnlUsedForCorrectScaling.Controls.Add(LstLogContent);
            PnlUsedForCorrectScaling.Controls.Add(txtSearch);
            PnlUsedForCorrectScaling.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForCorrectScaling.Location = new System.Drawing.Point(0, 0);
            PnlUsedForCorrectScaling.Name = "PnlUsedForCorrectScaling";
            PnlUsedForCorrectScaling.Size = new System.Drawing.Size(243, 272);
            PnlUsedForCorrectScaling.TabIndex = 23;
            // 
            // LblExplenation
            // 
            LblExplenation.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblExplenation.ForeColor = System.Drawing.SystemColors.ControlDark;
            LblExplenation.Location = new System.Drawing.Point(0, 53);
            LblExplenation.Margin = new System.Windows.Forms.Padding(0);
            LblExplenation.Name = "LblExplenation";
            LblExplenation.Padding = new System.Windows.Forms.Padding(5);
            LblExplenation.Size = new System.Drawing.Size(243, 219);
            LblExplenation.TabIndex = 11;
            LblExplenation.Text = "Navigeer door het log";
            LblExplenation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ContentNavigationControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(PnlUsedForCorrectScaling);
            Name = "ContentNavigationControl";
            Size = new System.Drawing.Size(243, 272);
            PnlUsedForCorrectScaling.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DoubleBufferedListBox LstLogContent;
        private System.Windows.Forms.ComboBox CboLogContentType;
        private ClearableTextBoxControl txtSearch;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel PnlUsedForCorrectScaling;
        private System.Windows.Forms.Button BtnShowTree;
        private System.Windows.Forms.Button BtnAutoScroll;
        private System.Windows.Forms.ImageList ImageListTreeView;
        private System.Windows.Forms.ImageList ImageListAutoScroll;
        private System.Windows.Forms.Button BtnJumpToLogPosition;
        private System.Windows.Forms.Label LblExplenation;
    }
}
