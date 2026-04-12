namespace LogScraper.Controls.Generic
{
    partial class ClearableTextBoxControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClearableTextBoxControl));
            TxtTextBox = new System.Windows.Forms.TextBox();
            BtnClear = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // TxtTextBox
            // 
            TxtTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TxtTextBox.Location = new System.Drawing.Point(3, 2);
            TxtTextBox.Name = "TxtTextBox";
            TxtTextBox.Size = new System.Drawing.Size(220, 16);
            TxtTextBox.TabIndex = 0;
            TxtTextBox.TextChanged += TxtTextBox_TextChanged;
            TxtTextBox.Enter += TxtTextBox_Enter;
            TxtTextBox.Leave += TxtTextBox_Leave;
            // 
            // BtnClear
            // 
            BtnClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnClear.BackColor = System.Drawing.Color.White;
            BtnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            BtnClear.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            BtnClear.FlatAppearance.BorderSize = 0;
            BtnClear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            BtnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnClear.Image = (System.Drawing.Image)resources.GetObject("BtnClear.Image");
            BtnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnClear.Location = new System.Drawing.Point(203, -1);
            BtnClear.Name = "BtnClear";
            BtnClear.Size = new System.Drawing.Size(20, 20);
            BtnClear.TabIndex = 11;
            BtnClear.UseVisualStyleBackColor = false;
            BtnClear.Click += BtnClear_Click;
            // 
            // ClearableTextBoxControl
            // 
            BackColor = System.Drawing.SystemColors.Window;
            Controls.Add(BtnClear);
            Controls.Add(TxtTextBox);
            Name = "ClearableTextBoxControl";
            Size = new System.Drawing.Size(223, 20);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox TxtTextBox;
        private System.Windows.Forms.Button BtnClear;
    }
}
