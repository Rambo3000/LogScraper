using LogScraper.Controls.Generic;
using LogScraper.Utilities;

namespace LogScraper.Content
{
    partial class UserControlLogContentFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLogContentFilter));
            LstLogContent = new System.Windows.Forms.ListBox();
            CboLogContentType = new System.Windows.Forms.ComboBox();
            txtSearch = new ClearableTextBoxControl();
            toolTip = new System.Windows.Forms.ToolTip(components);
            BtnShowTree = new System.Windows.Forms.Button();
            imageList1 = new System.Windows.Forms.ImageList(components);
            PnlUsedForCorrectScaling = new System.Windows.Forms.Panel();
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
            LstLogContent.DoubleClick += LstLogContent_DoubleClick;
            // 
            // CboLogContentType
            // 
            CboLogContentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CboLogContentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboLogContentType.FormattingEnabled = true;
            CboLogContentType.Location = new System.Drawing.Point(3, 0);
            CboLogContentType.Name = "CboLogContentType";
            CboLogContentType.Size = new System.Drawing.Size(213, 23);
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
            BtnShowTree.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnShowTree.ImageIndex = 0;
            BtnShowTree.ImageList = imageList1;
            BtnShowTree.Location = new System.Drawing.Point(218, 0);
            BtnShowTree.Name = "BtnShowTree";
            BtnShowTree.Size = new System.Drawing.Size(25, 25);
            BtnShowTree.TabIndex = 8;
            toolTip.SetToolTip(BtnShowTree, "Hierarchische weergave");
            BtnShowTree.UseVisualStyleBackColor = true;
            BtnShowTree.Click += BtnShowTree_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "log entries tree custommade 16x16.png");
            imageList1.Images.SetKeyName(1, "log entries tree hide custommade 16x16.png");
            // 
            // PnlUsedForCorrectScaling
            // 
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
            // UserControlLogContentFilter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(PnlUsedForCorrectScaling);
            Name = "UserControlLogContentFilter";
            Size = new System.Drawing.Size(243, 272);
            PnlUsedForCorrectScaling.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox LstLogContent;
        private System.Windows.Forms.ComboBox CboLogContentType;
        private ClearableTextBoxControl txtSearch;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel PnlUsedForCorrectScaling;
        private System.Windows.Forms.Button BtnShowTree;
        private System.Windows.Forms.ImageList imageList1;
    }
}
