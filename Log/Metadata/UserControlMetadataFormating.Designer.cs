namespace LogScraper.Log.Metadata
{
    partial class UserControlMetadataFormating
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
            chkShowOriginalMetdata = new System.Windows.Forms.CheckBox();
            pnlCheckBoxes = new System.Windows.Forms.FlowLayoutPanel();
            grpAddMetadata = new System.Windows.Forms.GroupBox();
            grpAddMetadata.SuspendLayout();
            SuspendLayout();
            // 
            // chkShowOriginalMetdata
            // 
            chkShowOriginalMetdata.AutoSize = true;
            chkShowOriginalMetdata.Location = new System.Drawing.Point(2, 5);
            chkShowOriginalMetdata.Name = "chkShowOriginalMetdata";
            chkShowOriginalMetdata.Size = new System.Drawing.Size(187, 19);
            chkShowOriginalMetdata.TabIndex = 0;
            chkShowOriginalMetdata.Text = "Originele metadata weergeven";
            chkShowOriginalMetdata.UseVisualStyleBackColor = true;
            chkShowOriginalMetdata.CheckedChanged += ChkShowOriginalMetdata_CheckedChanged;
            // 
            // pnlCheckBoxes
            // 
            pnlCheckBoxes.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlCheckBoxes.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            pnlCheckBoxes.Location = new System.Drawing.Point(3, 19);
            pnlCheckBoxes.Margin = new System.Windows.Forms.Padding(0);
            pnlCheckBoxes.Name = "pnlCheckBoxes";
            pnlCheckBoxes.Size = new System.Drawing.Size(181, 95);
            pnlCheckBoxes.TabIndex = 3;
            pnlCheckBoxes.WrapContents = false;
            // 
            // grpAddMetadata
            // 
            grpAddMetadata.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpAddMetadata.Controls.Add(pnlCheckBoxes);
            grpAddMetadata.Location = new System.Drawing.Point(0, 30);
            grpAddMetadata.Name = "grpAddMetadata";
            grpAddMetadata.Size = new System.Drawing.Size(187, 117);
            grpAddMetadata.TabIndex = 4;
            grpAddMetadata.TabStop = false;
            grpAddMetadata.Text = "Metadata toevoegen";
            // 
            // UserControlMetadataFormating
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(grpAddMetadata);
            Controls.Add(chkShowOriginalMetdata);
            MinimumSize = new System.Drawing.Size(190, 0);
            Name = "UserControlMetadataFormating";
            Size = new System.Drawing.Size(190, 150);
            grpAddMetadata.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox chkShowOriginalMetdata;
        private System.Windows.Forms.FlowLayoutPanel pnlCheckBoxes;
        private System.Windows.Forms.GroupBox grpAddMetadata;
    }
}
