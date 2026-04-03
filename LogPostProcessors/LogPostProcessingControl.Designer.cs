namespace LogScraper.LogPostProcessors
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
            BtnJson = new System.Windows.Forms.Button();
            BtnXml = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "code-json-custom 16x16.png");
            imageList1.Images.SetKeyName(1, "xml-custom 16x16.png");
            imageList1.Images.SetKeyName(2, "cogs-custom 16x16.png");
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 99999999;
            toolTip1.InitialDelay = 250;
            toolTip1.ReshowDelay = 100;
            // 
            // BtnJson
            // 
            BtnJson.ImageIndex = 0;
            BtnJson.ImageList = imageList1;
            BtnJson.Location = new System.Drawing.Point(0, 0);
            BtnJson.Name = "BtnJson";
            BtnJson.Size = new System.Drawing.Size(25, 25);
            BtnJson.TabIndex = 1;
            toolTip1.SetToolTip(BtnJson, "Pretty print JSON");
            BtnJson.UseVisualStyleBackColor = true;
            BtnJson.Click += BtnJson_Click;
            // 
            // BtnXml
            // 
            BtnXml.ImageIndex = 1;
            BtnXml.ImageList = imageList1;
            BtnXml.Location = new System.Drawing.Point(25, 0);
            BtnXml.Name = "BtnXml";
            BtnXml.Size = new System.Drawing.Size(25, 25);
            BtnXml.TabIndex = 2;
            toolTip1.SetToolTip(BtnXml, "Pretty print XML");
            BtnXml.UseVisualStyleBackColor = true;
            BtnXml.Click += BtnXml_Click;
            // 
            // UserControlPostProcessing
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BtnXml);
            Controls.Add(BtnJson);
            Name = "UserControlPostProcessing";
            Size = new System.Drawing.Size(53, 28);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button BtnJson;
        private System.Windows.Forms.Button BtnXml;
    }
}
