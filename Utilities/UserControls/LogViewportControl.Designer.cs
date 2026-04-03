using System.Windows.Forms;

namespace LogScraper.Utilities.UserControls
{
    partial class LogViewportControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewportControl));
            BtnBegin = new Button();
            BtnEnd = new Button();
            BtnReset = new Button();
            SuspendLayout();
            // 
            // BtnBegin
            // 
            BtnBegin.Image = (System.Drawing.Image)resources.GetObject("BtnBegin.Image");
            BtnBegin.Location = new System.Drawing.Point(-1, -1);
            BtnBegin.Name = "BtnBegin";
            BtnBegin.Size = new System.Drawing.Size(25, 25);
            BtnBegin.TabIndex = 1;
            BtnBegin.UseVisualStyleBackColor = true;
            BtnBegin.Click += BtnBegin_Click;
            // 
            // BtnEnd
            // 
            BtnEnd.Image = (System.Drawing.Image)resources.GetObject("BtnEnd.Image");
            BtnEnd.Location = new System.Drawing.Point(23, -1);
            BtnEnd.Name = "BtnEnd";
            BtnEnd.Size = new System.Drawing.Size(25, 25);
            BtnEnd.TabIndex = 2;
            BtnEnd.UseVisualStyleBackColor = true;
            BtnEnd.Click += BtnEnd_Click;
            // 
            // BtnReset
            // 
            BtnReset.Image = (System.Drawing.Image)resources.GetObject("BtnReset.Image");
            BtnReset.Location = new System.Drawing.Point(47, -1);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(25, 25);
            BtnReset.TabIndex = 3;
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // LogViewportControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(BtnReset);
            Controls.Add(BtnEnd);
            Controls.Add(BtnBegin);
            Name = "LogViewportControl";
            Size = new System.Drawing.Size(76, 27);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button BtnReset;
        private Button BtnBegin;
        private Button BtnEnd;
    }
}
