namespace LogScraper.Utilities.UserControls
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
            imageList1 = new System.Windows.Forms.ImageList(components);
            BtnReset = new System.Windows.Forms.Button();
            BtnPrevious = new System.Windows.Forms.Button();
            BtnNext = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // BtnBookMark
            // 
            BtnBookMark.ImageIndex = 0;
            BtnBookMark.ImageList = imageList1;
            BtnBookMark.Location = new System.Drawing.Point(0, 0);
            BtnBookMark.Name = "BtnBookMark";
            BtnBookMark.Size = new System.Drawing.Size(25, 25);
            BtnBookMark.TabIndex = 0;
            BtnBookMark.UseVisualStyleBackColor = true;
            BtnBookMark.Click += BtnBookMark_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "bookmark-outline 16x16.png");
            imageList1.Images.SetKeyName(1, "bookmark-outline-blue-16x16.png");
            // 
            // BtnReset
            // 
            BtnReset.Image = (System.Drawing.Image)resources.GetObject("BtnReset.Image");
            BtnReset.Location = new System.Drawing.Point(75, 0);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(25, 25);
            BtnReset.TabIndex = 1;
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // BtnPrevious
            // 
            BtnPrevious.Image = (System.Drawing.Image)resources.GetObject("BtnPrevious.Image");
            BtnPrevious.Location = new System.Drawing.Point(25, 0);
            BtnPrevious.Name = "BtnPrevious";
            BtnPrevious.Size = new System.Drawing.Size(25, 25);
            BtnPrevious.TabIndex = 2;
            BtnPrevious.UseVisualStyleBackColor = true;
            BtnPrevious.Click += BtnPrevious_Click;
            // 
            // BtnNext
            // 
            BtnNext.Image = (System.Drawing.Image)resources.GetObject("BtnNext.Image");
            BtnNext.Location = new System.Drawing.Point(50, 0);
            BtnNext.Name = "BtnNext";
            BtnNext.Size = new System.Drawing.Size(25, 25);
            BtnNext.TabIndex = 3;
            BtnNext.UseVisualStyleBackColor = true;
            BtnNext.Click += BtnNext_Click;
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
            Size = new System.Drawing.Size(102, 26);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button BtnBookMark;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnPrevious;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.ImageList imageList1;
    }
}
