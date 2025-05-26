namespace LogScraper.Utilities.UserControls
{
    partial class UserControlLogEntriesTextBox
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
            TxtLogEntries = new ScintillaNET.Scintilla();
            SuspendLayout();
            // 
            // TxtLogEntries
            // 
            TxtLogEntries.AutoCMaxHeight = 9;
            TxtLogEntries.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TxtLogEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            TxtLogEntries.Location = new System.Drawing.Point(0, 0);
            TxtLogEntries.Name = "TxtLogEntries";
            TxtLogEntries.ReadOnly = true;
            TxtLogEntries.Size = new System.Drawing.Size(712, 403);
            TxtLogEntries.TabIndents = true;
            TxtLogEntries.TabIndex = 42;
            // 
            // UserControlLogEntriesTextBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(TxtLogEntries);
            Name = "UserControlLogEntriesTextBox";
            Size = new System.Drawing.Size(712, 403);
            ResumeLayout(false);
        }

        #endregion
        private ScintillaNET.Scintilla TxtLogEntries;
    }
}
