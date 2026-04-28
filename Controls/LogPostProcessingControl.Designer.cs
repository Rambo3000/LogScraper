namespace LogScraper.Controls
{
    partial class LogPostProcessingControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogPostProcessingControl));
            imageList1 = new System.Windows.Forms.ImageList(components);
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            BtnPrettyPrint = new LogScraper.Controls.Generic.SplitButton();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            this.PrettyPrintJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            PrettyPrintXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            RemoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "code-json-custom 16x16.png");
            imageList1.Images.SetKeyName(1, "cogs-custom 16x16.png");
            imageList1.Images.SetKeyName(2, "code-json-custom 16x16.png");
            imageList1.Images.SetKeyName(3, "xml-custom 16x16.png");
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 99999999;
            toolTip1.InitialDelay = 250;
            toolTip1.ReshowDelay = 100;
            // 
            // BtnPrettyPrint
            // 
            BtnPrettyPrint.DropDownMenu = contextMenuStrip1;
            BtnPrettyPrint.DropDownWidth = 15;
            BtnPrettyPrint.Icon = null;
            BtnPrettyPrint.ImageIndex = 0;
            BtnPrettyPrint.ImageList = imageList1;
            BtnPrettyPrint.Location = new System.Drawing.Point(0, 0);
            BtnPrettyPrint.Name = "BtnPrettyPrint";
            BtnPrettyPrint.Size = new System.Drawing.Size(40, 25);
            BtnPrettyPrint.TabIndex = 0;
            toolTip1.SetToolTip(BtnPrettyPrint, "Pretty print JSON en XML");
            BtnPrettyPrint.ButtonClick += BtnPrettyPrint_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.PrettyPrintJSONToolStripMenuItem, PrettyPrintXMLToolStripMenuItem, toolStripSeparator1, RemoveToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(181, 98);
            contextMenuStrip1.Text = "Pretty print JSON";
            // 
            // PrettyPrintJSONToolStripMenuItem
            // 
            this.PrettyPrintJSONToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("PrettyPrintJSONToolStripMenuItem.Image");
            this.PrettyPrintJSONToolStripMenuItem.Name = "PrettyPrintJSONToolStripMenuItem";
            this.PrettyPrintJSONToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.PrettyPrintJSONToolStripMenuItem.Text = "Pretty print JSON";
            this.PrettyPrintJSONToolStripMenuItem.Click += this.PrettyPrintJSONToolStripMenuItem_Click;
            // 
            // PrettyPrintXMLToolStripMenuItem
            // 
            PrettyPrintXMLToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("PrettyPrintXMLToolStripMenuItem.Image");
            PrettyPrintXMLToolStripMenuItem.Name = "PrettyPrintXMLToolStripMenuItem";
            PrettyPrintXMLToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            PrettyPrintXMLToolStripMenuItem.Text = "Pretty print XML";
            PrettyPrintXMLToolStripMenuItem.Click += PrettyPrintXMLToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // RemoveToolStripMenuItem
            // 
            RemoveToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("RemoveToolStripMenuItem.Image");
            RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem";
            RemoveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            RemoveToolStripMenuItem.Text = "Wis pretty print";
            RemoveToolStripMenuItem.Click += RemoveToolStripMenuItem_Click;
            // 
            // LogPostProcessingControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BtnPrettyPrint);
            Name = "LogPostProcessingControl";
            Size = new System.Drawing.Size(40, 25);
            Load += LogPostProcessingControl_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private Generic.SplitButton BtnPrettyPrint;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem PrettyPrintJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PrettyPrintXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem RemoveToolStripMenuItem;
    }
}
