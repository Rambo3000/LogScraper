using LogScraper.Controls.Generic;

namespace LogScraper.Controls
{
    partial class FlowTreeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlowTreeControl));
            BtnTreeView = new SplitButton();
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ItemShowTree = new System.Windows.Forms.ToolStripMenuItem();
            ItemHideTree = new System.Windows.Forms.ToolStripMenuItem();
            CboContentProperties = new System.Windows.Forms.ToolStripComboBox();
            imageList1 = new System.Windows.Forms.ImageList(components);
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            ContextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // BtnTreeView
            // 
            BtnTreeView.DropDownMenu = ContextMenuStrip1;
            BtnTreeView.DropDownWidth = 15;
            BtnTreeView.Icon = null;
            BtnTreeView.ImageIndex = 0;
            BtnTreeView.ImageList = imageList1;
            BtnTreeView.Location = new System.Drawing.Point(0, 0);
            BtnTreeView.Name = "BtnTreeView";
            BtnTreeView.Size = new System.Drawing.Size(40, 25);
            BtnTreeView.TabIndex = 4;
            BtnTreeView.Tag = "";
            toolTip1.SetToolTip(BtnTreeView, "Hierarchische weergave aan/uit [CTRL-T]");
            BtnTreeView.ButtonClick += BtnTreeView_Click;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ItemShowTree, ItemHideTree, CboContentProperties });
            ContextMenuStrip1.Name = "ContextMenuStrip";
            ContextMenuStrip1.Size = new System.Drawing.Size(194, 75);
            // 
            // ItemShowTree
            // 
            ItemShowTree.CheckOnClick = true;
            ItemShowTree.Image = (System.Drawing.Image)resources.GetObject("ItemShowTree.Image");
            ItemShowTree.Name = "ItemShowTree";
            ItemShowTree.Size = new System.Drawing.Size(193, 22);
            ItemShowTree.Text = "Hirarchische weergave";
            // 
            // ItemHideTree
            // 
            ItemHideTree.Image = (System.Drawing.Image)resources.GetObject("ItemHideTree.Image");
            ItemHideTree.Name = "ItemHideTree";
            ItemHideTree.Size = new System.Drawing.Size(193, 22);
            ItemHideTree.Text = "Normale weergave";
            // 
            // CboContentProperties
            // 
            CboContentProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboContentProperties.Name = "CboContentProperties";
            CboContentProperties.Size = new System.Drawing.Size(121, 23);
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "log entries tree custommade 16x16.png");
            imageList1.Images.SetKeyName(1, "log entries tree hide custommade 16x16.png");
            // 
            // FlowTreeControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BtnTreeView);
            MaximumSize = new System.Drawing.Size(40, 25);
            MinimumSize = new System.Drawing.Size(40, 25);
            Name = "FlowTreeControl";
            Size = new System.Drawing.Size(40, 25);
            Load += FlowTreeControl_Load;
            ContextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private SplitButton BtnTreeView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem ItemShowTree;
        private System.Windows.Forms.ToolStripMenuItem ItemHideTree;
        private System.Windows.Forms.ToolStripComboBox CboContentProperties;
    }
}
