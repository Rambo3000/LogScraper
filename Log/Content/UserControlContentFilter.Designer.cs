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
            txtSearch = new System.Windows.Forms.TextBox();
            ChkShowFlowTree = new System.Windows.Forms.CheckBox();
            ChkShowNoTree = new System.Windows.Forms.CheckBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            PnlUsedForCorrectScaling = new System.Windows.Forms.Panel();
            PnlUsedForCorrectScaling.SuspendLayout();
            SuspendLayout();
            // 
            // LstLogContent
            // 
            LstLogContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LstLogContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstLogContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            LstLogContent.FormattingEnabled = true;
            LstLogContent.IntegralHeight = false;
            LstLogContent.Location = new System.Drawing.Point(0, 80);
            LstLogContent.Name = "LstLogContent";
            LstLogContent.Size = new System.Drawing.Size(243, 192);
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
            CboLogContentType.Location = new System.Drawing.Point(0, 0);
            CboLogContentType.Name = "CboLogContentType";
            CboLogContentType.Size = new System.Drawing.Size(243, 23);
            CboLogContentType.TabIndex = 1;
            CboLogContentType.SelectedIndexChanged += CboLogContentType_SelectedIndexChanged;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSearch.Location = new System.Drawing.Point(0, 26);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(243, 23);
            txtSearch.TabIndex = 7;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtSearch.Enter += TxtSearch_Enter;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            txtSearch.Leave += TxtSearch_Leave;
            // 
            // ChkShowFlowTree
            // 
            ChkShowFlowTree.Appearance = System.Windows.Forms.Appearance.Button;
            ChkShowFlowTree.Image = (System.Drawing.Image)resources.GetObject("ChkShowFlowTree.Image");
            ChkShowFlowTree.Location = new System.Drawing.Point(27, 51);
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
            ChkShowNoTree.Location = new System.Drawing.Point(2, 51);
            ChkShowNoTree.Name = "ChkShowNoTree";
            ChkShowNoTree.Size = new System.Drawing.Size(25, 25);
            ChkShowNoTree.TabIndex = 18;
            ChkShowNoTree.Tag = "asd";
            toolTip.SetToolTip(ChkShowNoTree, "Toon als een lijst");
            ChkShowNoTree.UseVisualStyleBackColor = true;
            ChkShowNoTree.CheckedChanged += ChkShowNoTree_CheckedChanged;
            // 
            // PnlUsedForCorrectScaling
            // 
            PnlUsedForCorrectScaling.Controls.Add(CboLogContentType);
            PnlUsedForCorrectScaling.Controls.Add(LstLogContent);
            PnlUsedForCorrectScaling.Controls.Add(ChkShowNoTree);
            PnlUsedForCorrectScaling.Controls.Add(ChkShowFlowTree);
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
            BackColor = System.Drawing.SystemColors.Window;
            Controls.Add(PnlUsedForCorrectScaling);
            Name = "UserControlLogContentFilter";
            Size = new System.Drawing.Size(243, 272);
            PnlUsedForCorrectScaling.ResumeLayout(false);
            PnlUsedForCorrectScaling.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox LstLogContent;
        private System.Windows.Forms.ComboBox CboLogContentType;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox ChkShowFlowTree;
        private System.Windows.Forms.CheckBox ChkShowNoTree;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel PnlUsedForCorrectScaling;
    }
}
