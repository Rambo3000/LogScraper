using System.Windows.Forms;

namespace LogScraper.Controls
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewportControl));
            BtnReset = new Button();
            ChkBegin = new CheckBox();
            ChkEnd = new CheckBox();
            toolTip1 = new ToolTip(components);
            SuspendLayout();
            // 
            // BtnReset
            // 
            BtnReset.Image = (System.Drawing.Image)resources.GetObject("BtnReset.Image");
            BtnReset.Location = new System.Drawing.Point(48, 0);
            BtnReset.Name = "BtnReset";
            BtnReset.Size = new System.Drawing.Size(25, 25);
            BtnReset.TabIndex = 3;
            toolTip1.SetToolTip(BtnReset, "Toon alle logregels");
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
            toolTip1.SetToolTip(ChkBegin, "Toon alleen logregels vanaf de geselecteerde regel");
            ChkBegin.UseVisualStyleBackColor = true;
            ChkBegin.CheckedChanged += ChkBegin_CheckedChanged;
            // 
            // ChkEnd
            // 
            ChkEnd.Appearance = Appearance.Button;
            ChkEnd.Image = (System.Drawing.Image)resources.GetObject("ChkEnd.Image");
            ChkEnd.Location = new System.Drawing.Point(24, 0);
            ChkEnd.Name = "ChkEnd";
            ChkEnd.Size = new System.Drawing.Size(25, 25);
            ChkEnd.TabIndex = 5;
            toolTip1.SetToolTip(ChkEnd, "Toon alleen logregels tot en met de geselecteerde regel");
            ChkEnd.UseVisualStyleBackColor = true;
            ChkEnd.CheckedChanged += ChkEnd_CheckedChanged;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 99999999;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // LogViewportControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ChkEnd);
            Controls.Add(ChkBegin);
            Controls.Add(BtnReset);
            Name = "LogViewportControl";
            Size = new System.Drawing.Size(73, 25);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button BtnReset;
        private CheckBox ChkBegin;
        private CheckBox ChkEnd;
        private ToolTip toolTip1;
    }
}
