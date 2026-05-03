namespace LogScraper.Controls
{
    partial class OpenLogInTextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenLogInTextEditor));
            btnOpenWithEditor = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // btnOpenWithEditor
            // 
            btnOpenWithEditor.Image = (System.Drawing.Image)resources.GetObject("btnOpenWithEditor.Image");
            btnOpenWithEditor.Location = new System.Drawing.Point(0, 0);
            btnOpenWithEditor.Name = "btnOpenWithEditor";
            btnOpenWithEditor.Size = new System.Drawing.Size(25, 25);
            btnOpenWithEditor.TabIndex = 12;
            btnOpenWithEditor.TabStop = false;
            btnOpenWithEditor.UseVisualStyleBackColor = true;
            btnOpenWithEditor.Click += BtnOpenWithEditor_Click;
            // 
            // OpenLogInTextEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnOpenWithEditor);
            Name = "OpenLogInTextEditor";
            Size = new System.Drawing.Size(25, 25);
            Load += OpenLogInTextEditor_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnOpenWithEditor;
    }
}
