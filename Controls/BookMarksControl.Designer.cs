namespace LogScraper.Controls
{
    partial class BookMarksControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookMarksControl));
            BtnBookMark = new System.Windows.Forms.Button();
            imageListMark = new System.Windows.Forms.ImageList(components);
            BtnReset = new System.Windows.Forms.Button();
            BtnPrevious = new System.Windows.Forms.Button();
            imageListPrevious = new System.Windows.Forms.ImageList(components);
            BtnNext = new System.Windows.Forms.Button();
            imageListNext = new System.Windows.Forms.ImageList(components);
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // BtnBookMark
            // 
            BtnBookMark.ImageIndex = 0;
            BtnBookMark.ImageList = imageListMark;
            BtnBookMark.Location = new System.Drawing.Point(0, 0);
            BtnBookMark.Name = "BtnBookMark";
            BtnBookMark.Size = new System.Drawing.Size(25, 25);
            BtnBookMark.TabIndex = 0;
            toolTip1.SetToolTip(BtnBookMark, "Bookmark toevoegen");
            BtnBookMark.UseVisualStyleBackColor = true;
            BtnBookMark.Click += BtnBookMark_Click;
            // 
            // imageListMark
            // 
            imageListMark.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListMark.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListMark.ImageStream");
            imageListMark.TransparentColor = System.Drawing.Color.Transparent;
            imageListMark.Images.SetKeyName(0, "bookmark-outline 16x16.png");
            imageListMark.Images.SetKeyName(1, "bookmark-outline-blue-16x16.png");
            // 
            // BtnReset
            // 
            BtnReset.Image = (System.Drawing.Image)resources.GetObject("BtnReset.Image");
            BtnReset.Location = new System.Drawing.Point(72, 0);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(25, 25);
            BtnReset.TabIndex = 1;
            toolTip1.SetToolTip(BtnReset, "Alle bookmarks verwijderen");
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrevious
            // 
            BtnPrevious.ImageIndex = 0;
            BtnPrevious.ImageList = imageListPrevious;
            BtnPrevious.Location = new System.Drawing.Point(24, 0);
            BtnPrevious.Name = "BtnPrevious";
            BtnPrevious.Size = new System.Drawing.Size(25, 25);
            BtnPrevious.TabIndex = 2;
            toolTip1.SetToolTip(BtnPrevious, "Vorige bookmark");
            BtnPrevious.UseVisualStyleBackColor = true;
            BtnPrevious.Click += BtnPrevious_Click;
            // 
            // imageListPrevious
            // 
            imageListPrevious.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListPrevious.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListPrevious.ImageStream");
            imageListPrevious.TransparentColor = System.Drawing.Color.Transparent;
            imageListPrevious.Images.SetKeyName(0, "bookmark-previous outline 16x16.png");
            imageListPrevious.Images.SetKeyName(1, "bookmark-previous filled 16x16.png.png");
            // 
            // BtnNext
            // 
            BtnNext.ImageIndex = 0;
            BtnNext.ImageList = imageListNext;
            BtnNext.Location = new System.Drawing.Point(48, 0);
            BtnNext.Name = "BtnNext";
            BtnNext.Size = new System.Drawing.Size(25, 25);
            BtnNext.TabIndex = 3;
            toolTip1.SetToolTip(BtnNext, "Volgende bookmark");
            BtnNext.UseVisualStyleBackColor = true;
            BtnNext.Click += BtnNext_Click;
            // 
            // imageListNext
            // 
            imageListNext.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListNext.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListNext.ImageStream");
            imageListNext.TransparentColor = System.Drawing.Color.Transparent;
            imageListNext.Images.SetKeyName(0, "bookmark-next outline 16x16.png");
            imageListNext.Images.SetKeyName(1, "bookmark-next filled 16x16.png.png");
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 9999999;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // BookMarksControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BtnNext);
            Controls.Add(BtnPrevious);
            Controls.Add(BtnReset);
            Controls.Add(BtnBookMark);
            Name = "BookMarksControl";
            Size = new System.Drawing.Size(97, 25);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button BtnBookMark;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnPrevious;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.ImageList imageListMark;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ImageList imageListPrevious;
        private System.Windows.Forms.ImageList imageListNext;
    }
}
