namespace LogScraper.Controls
{
    partial class SaveLogControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveLogControl));
            BtnSave = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip();
            SuspendLayout();
            // 
            // BtnSave
            // 
            BtnSave.Image = (System.Drawing.Image)resources.GetObject("BtnSave.Image");
            BtnSave.Location = new System.Drawing.Point(0, 0);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new System.Drawing.Size(25, 25);
            BtnSave.TabIndex = 27;
            BtnSave.TabStop = false;
            toolTip1.SetToolTip(BtnSave, "Log opslaan [CTRL-S]");
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 9999999;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // SaveLogControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BtnSave);
            Name = "SaveLogControl";
            Size = new System.Drawing.Size(25, 25);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
