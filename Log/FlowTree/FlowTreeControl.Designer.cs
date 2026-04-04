namespace LogScraper.Log.FlowTree
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
            splitButton1 = new LogScraper.Utilities.UserControls.SplitButton();
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ItemShowTree = new System.Windows.Forms.ToolStripMenuItem();
            ItemHideTree = new System.Windows.Forms.ToolStripMenuItem();
            CboContentProperties = new System.Windows.Forms.ToolStripComboBox();
            imageList1 = new System.Windows.Forms.ImageList(components);
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            ContextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitButton1
            // 
            splitButton1.DropDownMenu = ContextMenuStrip1;
            splitButton1.DropDownWidth = 15;
            splitButton1.Icon = null;
            splitButton1.ImageIndex = 0;
            splitButton1.ImageList = imageList1;
            splitButton1.Location = new System.Drawing.Point(0, 0);
            splitButton1.Name = "splitButton1";
            splitButton1.Size = new System.Drawing.Size(40, 25);
            splitButton1.TabIndex = 4;
            splitButton1.Tag = "";
            toolTip1.SetToolTip(splitButton1, "Hierarchische weergave");
            splitButton1.ButtonClick += SplitButton1_Click;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ItemShowTree, ItemHideTree, CboContentProperties });
            ContextMenuStrip1.Name = "ContextMenuStrip";
            ContextMenuStrip1.Size = new System.Drawing.Size(194, 97);
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
            // CboContentProperty
            // 
            CboContentProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboContentProperties.Name = "CboContentProperty";
            CboContentProperties.Size = new System.Drawing.Size(121, 23);
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "format-list-bulleted tree 5x-16x16.png");
            imageList1.Images.SetKeyName(1, "format-list-bulleted 5x-16x16.png");
            // 
            // FlowTreeControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitButton1);
            MaximumSize = new System.Drawing.Size(40, 25);
            MinimumSize = new System.Drawing.Size(40, 25);
            Name = "FlowTreeControl";
            Size = new System.Drawing.Size(40, 25);
            ContextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Utilities.UserControls.SplitButton splitButton1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem ItemShowTree;
        private System.Windows.Forms.ToolStripMenuItem ItemHideTree;
        private System.Windows.Forms.ToolStripComboBox CboContentProperties;
    }
}
