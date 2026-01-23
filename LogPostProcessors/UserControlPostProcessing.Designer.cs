namespace LogScraper.LogPostProcessors
{
    partial class UserControlPostProcessing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlPostProcessing));
            PnlUsedForScalingCompatibility = new System.Windows.Forms.Panel();
            BtnPostProcess = new System.Windows.Forms.Button();
            imageList1 = new System.Windows.Forms.ImageList(components);
            ContextMenuPostProcessing = new System.Windows.Forms.ContextMenuStrip(components);
            ApplyToVisibleLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            StopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteAllePostprocessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            prettyPrintJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            prettyPrintXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            PnlUsedForScalingCompatibility.SuspendLayout();
            ContextMenuPostProcessing.SuspendLayout();
            SuspendLayout();
            // 
            // PnlUsedForScalingCompatibility
            // 
            PnlUsedForScalingCompatibility.Controls.Add(BtnPostProcess);
            PnlUsedForScalingCompatibility.Dock = System.Windows.Forms.DockStyle.Fill;
            PnlUsedForScalingCompatibility.Location = new System.Drawing.Point(0, 0);
            PnlUsedForScalingCompatibility.Name = "PnlUsedForScalingCompatibility";
            PnlUsedForScalingCompatibility.Size = new System.Drawing.Size(50, 35);
            PnlUsedForScalingCompatibility.TabIndex = 1;
            // 
            // BtnPostProcess
            // 
            BtnPostProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            BtnPostProcess.ImageIndex = 0;
            BtnPostProcess.ImageList = imageList1;
            BtnPostProcess.Location = new System.Drawing.Point(0, 0);
            BtnPostProcess.Name = "BtnPostProcess";
            BtnPostProcess.Size = new System.Drawing.Size(50, 35);
            BtnPostProcess.TabIndex = 0;
            BtnPostProcess.TabStop = false;
            toolTip1.SetToolTip(BtnPostProcess, "Pretty print XML en JSON. Rechtermuisklik voor meer opties");
            BtnPostProcess.UseVisualStyleBackColor = true;
            BtnPostProcess.Click += BtnPostProcess_Click;
            BtnPostProcess.MouseUp += BtnPostProcess_MouseUp;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "auto-fix-with-dropdown-16x16.png");
            imageList1.Images.SetKeyName(1, "cogs-custom-with-dropdown-24x16.png");
            // 
            // ContextMenuPostProcessing
            // 
            ContextMenuPostProcessing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ApplyToVisibleLogToolStripMenuItem, StopToolStripMenuItem, DeleteAllePostprocessingToolStripMenuItem, toolStripSeparator1, prettyPrintJSONToolStripMenuItem, prettyPrintXMLToolStripMenuItem });
            ContextMenuPostProcessing.Name = "ContextMenu";
            ContextMenuPostProcessing.ShowCheckMargin = true;
            ContextMenuPostProcessing.Size = new System.Drawing.Size(212, 142);
            // 
            // ApplyToVisibleLogToolStripMenuItem
            // 
            ApplyToVisibleLogToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("ApplyToVisibleLogToolStripMenuItem.Image");
            ApplyToVisibleLogToolStripMenuItem.Name = "ApplyToVisibleLogToolStripMenuItem";
            ApplyToVisibleLogToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            ApplyToVisibleLogToolStripMenuItem.Text = "Pretty print toepassen";
            ApplyToVisibleLogToolStripMenuItem.Click += ApplyToVisibleLogToolStripMenuItem_Click;
            // 
            // StopToolStripMenuItem
            // 
            StopToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("StopToolStripMenuItem.Image");
            StopToolStripMenuItem.Name = "StopToolStripMenuItem";
            StopToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            StopToolStripMenuItem.Text = "Stop";
            StopToolStripMenuItem.Click += StopToolStripMenuItem_Click;
            // 
            // DeleteAllePostprocessingToolStripMenuItem
            // 
            DeleteAllePostprocessingToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("DeleteAllePostprocessingToolStripMenuItem.Image");
            DeleteAllePostprocessingToolStripMenuItem.Name = "DeleteAllePostprocessingToolStripMenuItem";
            DeleteAllePostprocessingToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            DeleteAllePostprocessingToolStripMenuItem.Text = "Wissen";
            DeleteAllePostprocessingToolStripMenuItem.Click += DeleteAllePostprocessingToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // prettyPrintJSONToolStripMenuItem
            // 
            prettyPrintJSONToolStripMenuItem.Checked = true;
            prettyPrintJSONToolStripMenuItem.CheckOnClick = true;
            prettyPrintJSONToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            prettyPrintJSONToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("prettyPrintJSONToolStripMenuItem.Image");
            prettyPrintJSONToolStripMenuItem.Name = "prettyPrintJSONToolStripMenuItem";
            prettyPrintJSONToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            prettyPrintJSONToolStripMenuItem.Text = "Pretty print JSON";
            prettyPrintJSONToolStripMenuItem.Click += prettyPrintJSONToolStripMenuItem_Click;
            // 
            // prettyPrintXMLToolStripMenuItem
            // 
            prettyPrintXMLToolStripMenuItem.Checked = true;
            prettyPrintXMLToolStripMenuItem.CheckOnClick = true;
            prettyPrintXMLToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            prettyPrintXMLToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("prettyPrintXMLToolStripMenuItem.Image");
            prettyPrintXMLToolStripMenuItem.Name = "prettyPrintXMLToolStripMenuItem";
            prettyPrintXMLToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            prettyPrintXMLToolStripMenuItem.Text = "Pretty print XML";
            prettyPrintXMLToolStripMenuItem.Click += prettyPrintXMLToolStripMenuItem_Click;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 99999999;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // UserControlPostProcessing
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlUsedForScalingCompatibility);
            Name = "UserControlPostProcessing";
            Size = new System.Drawing.Size(50, 35);
            PnlUsedForScalingCompatibility.ResumeLayout(false);
            ContextMenuPostProcessing.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel PnlUsedForScalingCompatibility;
        private System.Windows.Forms.Button BtnPostProcess;
        private System.Windows.Forms.ContextMenuStrip ContextMenuPostProcessing;
        private System.Windows.Forms.ToolStripMenuItem prettyPrintJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prettyPrintXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ApplyToVisibleLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteAllePostprocessingToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem StopToolStripMenuItem;
    }
}
