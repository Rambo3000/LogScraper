namespace LogScraper.Log.Metadata
{
    partial class UserControlMetadataFormatting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMetadataFormatting));
            splitButton1 = new LogScraper.Utilities.UserControls.SplitButton();
            ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            ItemShowOriginalMetadata = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            imageList1 = new System.Windows.Forms.ImageList(components);
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            ItemHideAllMetadata = new System.Windows.Forms.ToolStripMenuItem();
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
            toolTip1.SetToolTip(splitButton1, "Metadata tonen/verbergen");
            splitButton1.ButtonClick += SplitButton1_Click;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ItemShowOriginalMetadata, ItemHideAllMetadata, toolStripSeparator1 });
            ContextMenuStrip1.Name = "ContextMenuStrip";
            ContextMenuStrip1.ShowCheckMargin = true;
            ContextMenuStrip1.ShowImageMargin = false;
            ContextMenuStrip1.Size = new System.Drawing.Size(236, 76);
            // 
            // ItemShowOriginalMetadata
            // 
            ItemShowOriginalMetadata.CheckOnClick = true;
            ItemShowOriginalMetadata.Name = "ItemShowOriginalMetadata";
            ItemShowOriginalMetadata.Size = new System.Drawing.Size(235, 22);
            ItemShowOriginalMetadata.Text = "Originele metadata weergeven";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(232, 6);
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "metadata.png");
            // 
            // ItemHideAllMetadata
            // 
            ItemHideAllMetadata.Name = "ItemHideAllMetadata";
            ItemHideAllMetadata.Size = new System.Drawing.Size(235, 22);
            ItemHideAllMetadata.Text = "Metadata verbergen";
            // 
            // UserControlMetadataFormatting
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitButton1);
            MaximumSize = new System.Drawing.Size(40, 25);
            MinimumSize = new System.Drawing.Size(40, 25);
            Name = "UserControlMetadataFormatting";
            Size = new System.Drawing.Size(40, 25);
            ContextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Utilities.UserControls.SplitButton splitButton1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ItemShowOriginalMetadata;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem ItemHideAllMetadata;
    }
}
