using LogScraper.Controls.Generic;

namespace LogScraper.Controls.Search
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
            ToolTip = new System.Windows.Forms.ToolTip(components);
            BtnClear = new System.Windows.Forms.Button();
            PnlUsedForCorrectScaling = new System.Windows.Forms.Panel();
            splitButton1 = new SplitButton();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ItemPrevious = new System.Windows.Forms.ToolStripMenuItem();
            ItemNext = new System.Windows.Forms.ToolStripMenuItem();
            ItemAll = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ItemCaseSensitive = new System.Windows.Forms.ToolStripMenuItem();
            ItemWholeWords = new System.Windows.Forms.ToolStripMenuItem();
            ItemWrapAround = new System.Windows.Forms.ToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            PnlFakeWhiteBackground = new System.Windows.Forms.Panel();
            PnlUsedForCorrectScaling.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtSearch.Location = new System.Drawing.Point(2, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(188, 16);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            txtSearch.Leave += TxtSearch_Leave;
            // 
            // BtnClear
            // 
            BtnClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnClear.BackColor = System.Drawing.Color.White;
            BtnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            BtnClear.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            BtnClear.FlatAppearance.BorderSize = 0;
            BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnClear.Image = (System.Drawing.Image)resources.GetObject("BtnClear.Image");
            BtnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnClear.Location = new System.Drawing.Point(188, 1);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new System.Drawing.Size(20, 20);
            BtnClear.TabIndex = 9;
            BtnClear.UseVisualStyleBackColor = false;
            BtnClear.Click += BtnClear_Click;
            // 
            // PnlUsedForCorrectScaling
            // 
            PnlUsedForCorrectScaling.Controls.Add(BtnClear);
            PnlUsedForCorrectScaling.Controls.Add(splitButton1);
            PnlUsedForCorrectScaling.Controls.Add(txtSearch);
            PnlUsedForCorrectScaling.Controls.Add(PnlFakeWhiteBackground);
            PnlUsedForCorrectScaling.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForCorrectScaling.Location = new System.Drawing.Point(0, 0);
            PnlUsedForCorrectScaling.Name = "PnlUsedForCorrectScaling";
            PnlUsedForCorrectScaling.Size = new System.Drawing.Size(244, 25);
            PnlUsedForCorrectScaling.TabIndex = 11;
            // 
            // splitButton1
            // 
            splitButton1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            splitButton1.DropDownMenu = contextMenuStrip1;
            splitButton1.DropDownWidth = 15;
            splitButton1.Icon = null;
            splitButton1.ImageIndex = 0;
            splitButton1.ImageList = imageList1;
            splitButton1.Location = new System.Drawing.Point(209, -1);
            splitButton1.Name = "splitButton1";
            splitButton1.Size = new System.Drawing.Size(35, 25);
            splitButton1.TabIndex = 10;
            splitButton1.ButtonClick += SplitButton1_ButtonClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ItemPrevious, ItemNext, ItemAll, toolStripSeparator1, ItemCaseSensitive, ItemWholeWords, ItemWrapAround });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowCheckMargin = true;
            contextMenuStrip1.Size = new System.Drawing.Size(235, 164);
            // 
            // ItemPrevious
            // 
            ItemPrevious.Image = (System.Drawing.Image)resources.GetObject("ItemPrevious.Image");
            ItemPrevious.Name = "ItemPrevious";
            ItemPrevious.Size = new System.Drawing.Size(234, 22);
            ItemPrevious.Text = "Vorige zoeken";
            // 
            // ItemNext
            // 
            ItemNext.Image = (System.Drawing.Image)resources.GetObject("ItemNext.Image");
            ItemNext.Name = "ItemNext";
            ItemNext.Size = new System.Drawing.Size(234, 22);
            ItemNext.Text = "Volgende zoeken";
            // 
            // ItemAll
            // 
            ItemAll.Image = (System.Drawing.Image)resources.GetObject("ItemAll.Image");
            ItemAll.Name = "ItemAll";
            ItemAll.Size = new System.Drawing.Size(234, 22);
            ItemAll.Text = "Alle zoeken";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // ItemCaseSensitive
            // 
            ItemCaseSensitive.CheckOnClick = true;
            ItemCaseSensitive.Image = (System.Drawing.Image)resources.GetObject("ItemCaseSensitive.Image");
            ItemCaseSensitive.Name = "ItemCaseSensitive";
            ItemCaseSensitive.Size = new System.Drawing.Size(234, 22);
            ItemCaseSensitive.Text = "Hoofdletter gevoelig";
            // 
            // ItemWholeWords
            // 
            ItemWholeWords.CheckOnClick = true;
            ItemWholeWords.Image = (System.Drawing.Image)resources.GetObject("ItemWholeWords.Image");
            ItemWholeWords.Name = "ItemWholeWords";
            ItemWholeWords.Size = new System.Drawing.Size(234, 22);
            ItemWholeWords.Text = "Alleen hele woorden";
            // 
            // ItemWrapAround
            // 
            ItemWrapAround.Checked = true;
            ItemWrapAround.CheckOnClick = true;
            ItemWrapAround.CheckState = System.Windows.Forms.CheckState.Checked;
            ItemWrapAround.Image = (System.Drawing.Image)resources.GetObject("ItemWrapAround.Image");
            ItemWrapAround.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            ItemWrapAround.Name = "ItemWrapAround";
            ItemWrapAround.Size = new System.Drawing.Size(234, 22);
            ItemWrapAround.Text = "Verder zoeken vanaf begin";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "arrow right.png");
            imageList1.Images.SetKeyName(1, "arrow left.png");
            imageList1.Images.SetKeyName(2, "magnify 16x16.png");
            // 
            // PnlFakeWhiteBackground
            // 
            PnlFakeWhiteBackground.BackColor = System.Drawing.Color.White;
            PnlFakeWhiteBackground.Location = new System.Drawing.Point(0, 1);
            PnlFakeWhiteBackground.Name = "PnlFakeWhiteBackground";
            PnlFakeWhiteBackground.Size = new System.Drawing.Size(208, 21);
            PnlFakeWhiteBackground.TabIndex = 11;
            // 
            // UserControlSearch
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlUsedForCorrectScaling);
            Name = "UserControlSearch";
            Size = new System.Drawing.Size(244, 25);
            PnlUsedForCorrectScaling.ResumeLayout(false);
            PnlUsedForCorrectScaling.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Panel PnlUsedForCorrectScaling;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ItemPrevious;
        private System.Windows.Forms.ToolStripMenuItem ItemNext;
        private System.Windows.Forms.ToolStripMenuItem ItemAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ItemCaseSensitive;
        private System.Windows.Forms.ToolStripMenuItem ItemWholeWords;
        private System.Windows.Forms.ToolStripMenuItem ItemWrapAround;
        private System.Windows.Forms.ImageList imageList1;
        private SplitButton splitButton1;
        private System.Windows.Forms.Panel PnlFakeWhiteBackground;
    }
}

