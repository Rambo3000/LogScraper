namespace LogScraper.Controls
{
    partial class ErrorMessageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorMessageControl));
            TxtErrorMessage = new System.Windows.Forms.TextBox();
            BtnClose = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // TxtErrorMessage
            // 
            TxtErrorMessage.BackColor = System.Drawing.SystemColors.Window;
            TxtErrorMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            TxtErrorMessage.ForeColor = System.Drawing.Color.DarkRed;
            TxtErrorMessage.Location = new System.Drawing.Point(0, 0);
            TxtErrorMessage.Multiline = true;
            TxtErrorMessage.Name = "TxtErrorMessage";
            TxtErrorMessage.ReadOnly = true;
            TxtErrorMessage.Size = new System.Drawing.Size(443, 89);
            TxtErrorMessage.TabIndex = 33;
            TxtErrorMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnClose
            // 
            BtnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnClose.Image = (System.Drawing.Image)resources.GetObject("BtnClose.Image");
            BtnClose.Location = new System.Drawing.Point(420, 3);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new System.Drawing.Size(20, 20);
            BtnClose.TabIndex = 34;
            BtnClose.UseVisualStyleBackColor = true;
            BtnClose.Click += BtnClose_Click;
            // 
            // ErrorMessageControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(BtnClose);
            Controls.Add(TxtErrorMessage);
            Name = "ErrorMessageControl";
            Size = new System.Drawing.Size(443, 89);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox TxtErrorMessage;
        private System.Windows.Forms.Button BtnClose;
    }
}
