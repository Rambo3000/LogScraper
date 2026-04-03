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
            BtnReset = new Button();
            ChkBegin = new CheckBox();
            ChkEnd = new CheckBox();
            SuspendLayout();
            // 
            // BtnReset
            // 
            BtnReset.Image = (System.Drawing.Image)resources.GetObject("BtnReset.Image");
            BtnReset.Location = new System.Drawing.Point(50, 0);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(25, 25);
            BtnReset.TabIndex = 3;
            BtnReset.UseVisualStyleBackColor = true;
            BtnReset.Click += BtnReset_Click;
            // 
            // ChkBegin
            // 
            ChkBegin.Appearance = Appearance.Button;
            ChkBegin.Image = (System.Drawing.Image)resources.GetObject("ChkBegin.Image");
            ChkBegin.Location = new System.Drawing.Point(0, 0);
            ChkBegin.Name = "ChkBegin";
            ChkBegin.Size = new System.Drawing.Size(25, 25);
            ChkBegin.TabIndex = 4;
            ChkBegin.UseVisualStyleBackColor = true;
            ChkBegin.CheckedChanged += ChkBegin_CheckedChanged;
            // 
            // ChkEnd
            // 
            ChkEnd.Appearance = Appearance.Button;
            ChkEnd.Image = (System.Drawing.Image)resources.GetObject("ChkEnd.Image");
            ChkEnd.Location = new System.Drawing.Point(25, 0);
            ChkEnd.Name = "ChkEnd";
            ChkEnd.Size = new System.Drawing.Size(25, 25);
            ChkEnd.TabIndex = 5;
            ChkEnd.UseVisualStyleBackColor = true;
            ChkEnd.CheckedChanged += ChkEnd_CheckedChanged;
            // 
            // LogViewportControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ChkEnd);
            Controls.Add(ChkBegin);
            Controls.Add(BtnReset);
            Name = "LogViewportControl";
            Size = new System.Drawing.Size(74, 28);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button BtnReset;
        private CheckBox ChkBegin;
        private CheckBox ChkEnd;
    }
}
