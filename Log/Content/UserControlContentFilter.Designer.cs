using LogScraper.Utilities;

namespace LogScraper
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
            BtnReset = new System.Windows.Forms.Button();
            txtSearch = new System.Windows.Forms.TextBox();
            BtnFilterOnSameMetadata = new System.Windows.Forms.Button();
            BtnResetMetadataFilter = new System.Windows.Forms.Button();
            ChkShowFlowTree = new System.Windows.Forms.CheckBox();
            ChkShowNoTree = new System.Windows.Forms.CheckBox();
            BtnSelectTop = new System.Windows.Forms.Button();
            BtnSelectEnd = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // LstLogContent
            // 
            LstLogContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LstLogContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstLogContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            LstLogContent.FormattingEnabled = true;
            LstLogContent.IntegralHeight = false;
            LstLogContent.Location = new System.Drawing.Point(0, 81);
            LstLogContent.Name = "LstLogContent";
            LstLogContent.Size = new System.Drawing.Size(243, 159);
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
            CboLogContentType.Location = new System.Drawing.Point(0, 1);
            CboLogContentType.Name = "CboLogContentType";
            CboLogContentType.Size = new System.Drawing.Size(243, 23);
            CboLogContentType.TabIndex = 1;
            CboLogContentType.SelectedIndexChanged += CboLogContentType_SelectedIndexChanged;
            // 
            // BtnReset
            // 
            BtnReset.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnReset.Image = (System.Drawing.Image)resources.GetObject("BtnReset.Image");
            BtnReset.Location = new System.Drawing.Point(215, 52);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(26, 25);
            BtnReset.TabIndex = 2;
            toolTip.SetToolTip(BtnReset, "Reset selecties");
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Location = new System.Drawing.Point(0, 27);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(243, 23);
            txtSearch.TabIndex = 7;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            txtSearch.Leave += TxtSearch_Leave;
            // 
            // BtnFilterOnSameMetadata
            // 
            BtnFilterOnSameMetadata.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BtnFilterOnSameMetadata.Enabled = false;
            BtnFilterOnSameMetadata.Image = (System.Drawing.Image)resources.GetObject("BtnFilterOnSameMetadata.Image");
            BtnFilterOnSameMetadata.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnFilterOnSameMetadata.Location = new System.Drawing.Point(3, 244);
            BtnFilterOnSameMetadata.Name = "BtnFilterOnSameMetadata";
            BtnFilterOnSameMetadata.Size = new System.Drawing.Size(237, 25);
            BtnFilterOnSameMetadata.TabIndex = 8;
            BtnFilterOnSameMetadata.Text = "Filter dezelfde sessie";
            BtnFilterOnSameMetadata.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            BtnFilterOnSameMetadata.UseVisualStyleBackColor = true;
            BtnFilterOnSameMetadata.Click += BtnFilterOnSameMetadata_Click;
            // 
            // BtnResetMetadataFilter
            // 
            BtnResetMetadataFilter.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BtnResetMetadataFilter.Image = (System.Drawing.Image)resources.GetObject("BtnResetMetadataFilter.Image");
            BtnResetMetadataFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnResetMetadataFilter.Location = new System.Drawing.Point(3, 244);
            BtnResetMetadataFilter.Name = "BtnResetMetadataFilter";
            BtnResetMetadataFilter.Size = new System.Drawing.Size(237, 25);
            BtnResetMetadataFilter.TabIndex = 9;
            BtnResetMetadataFilter.Text = "Reset de filter op sessie";
            BtnResetMetadataFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            BtnResetMetadataFilter.UseVisualStyleBackColor = true;
            BtnResetMetadataFilter.Visible = false;
            BtnResetMetadataFilter.Click += BtnResetMetadataFilter_Click;
            // 
            // ChkShowFlowTree
            // 
            ChkShowFlowTree.Appearance = System.Windows.Forms.Appearance.Button;
            ChkShowFlowTree.Image = (System.Drawing.Image)resources.GetObject("ChkShowFlowTree.Image");
            ChkShowFlowTree.Location = new System.Drawing.Point(27, 52);
            ChkShowFlowTree.Name = "ChkShowFlowTree";
            ChkShowFlowTree.Size = new System.Drawing.Size(25, 25);
            ChkShowFlowTree.TabIndex = 17;
            ChkShowFlowTree.Tag = "asd";
            toolTip.SetToolTip(ChkShowFlowTree, "Toon hiërarchie");
            ChkShowFlowTree.UseVisualStyleBackColor = true;
            ChkShowFlowTree.CheckedChanged += ChkShowFlowTree_CheckedChanged;
            // 
            // ChkShowNoTree
            // 
            ChkShowNoTree.Appearance = System.Windows.Forms.Appearance.Button;
            ChkShowNoTree.Image = (System.Drawing.Image)resources.GetObject("ChkShowNoTree.Image");
            ChkShowNoTree.Location = new System.Drawing.Point(2, 52);
            ChkShowNoTree.Name = "ChkShowNoTree";
            ChkShowNoTree.Size = new System.Drawing.Size(25, 25);
            ChkShowNoTree.TabIndex = 18;
            ChkShowNoTree.Tag = "asd";
            toolTip.SetToolTip(ChkShowNoTree, "Toon als een lijst");
            ChkShowNoTree.UseVisualStyleBackColor = true;
            ChkShowNoTree.CheckedChanged += ChkShowNoTree_CheckedChanged;
            // 
            // BtnSelectTop
            // 
            BtnSelectTop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnSelectTop.Image = (System.Drawing.Image)resources.GetObject("BtnSelectTop.Image");
            BtnSelectTop.Location = new System.Drawing.Point(162, 52);
            BtnSelectTop.Name = "BtnSelectTop";
            BtnSelectTop.Size = new System.Drawing.Size(26, 25);
            BtnSelectTop.TabIndex = 21;
            toolTip.SetToolTip(BtnSelectTop, "Selecteer begin [CTRL-B]");
            BtnSelectTop.UseVisualStyleBackColor = true;
            BtnSelectTop.Click += BtnSelectBegin_Click;
            // 
            // BtnSelectEnd
            // 
            BtnSelectEnd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnSelectEnd.Image = (System.Drawing.Image)resources.GetObject("BtnSelectEnd.Image");
            BtnSelectEnd.Location = new System.Drawing.Point(187, 52);
            BtnSelectEnd.Name = "BtnSelectEnd";
            BtnSelectEnd.Size = new System.Drawing.Size(26, 25);
            BtnSelectEnd.TabIndex = 22;
            toolTip.SetToolTip(BtnSelectEnd, "Selecteer eind [CTRL-E]");
            BtnSelectEnd.UseVisualStyleBackColor = true;
            BtnSelectEnd.Click += BtnSelectEnd_Click;
            // 
            // UserControlLogContentFilter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.SystemColors.Window;
            Controls.Add(BtnSelectEnd);
            Controls.Add(BtnSelectTop);
            Controls.Add(ChkShowNoTree);
            Controls.Add(ChkShowFlowTree);
            Controls.Add(BtnFilterOnSameMetadata);
            Controls.Add(txtSearch);
            Controls.Add(BtnReset);
            Controls.Add(CboLogContentType);
            Controls.Add(LstLogContent);
            Controls.Add(BtnResetMetadataFilter);
            Name = "UserControlLogContentFilter";
            Size = new System.Drawing.Size(243, 272);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox LstLogContent;
        private System.Windows.Forms.ComboBox CboLogContentType;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button BtnFilterOnSameMetadata;
        private System.Windows.Forms.Button BtnResetMetadataFilter;
        private System.Windows.Forms.CheckBox ChkShowFlowTree;
        private System.Windows.Forms.CheckBox ChkShowNoTree;
        private System.Windows.Forms.Button BtnSelectTop;
        private System.Windows.Forms.Button BtnSelectEnd;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
