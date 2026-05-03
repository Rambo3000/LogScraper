namespace LogScraper.Controls.Viewport
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
            TxtLogEntries = new ScintillaNET.Scintilla();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            LblExplenation = new System.Windows.Forms.Label();
            LblExplenation2 = new System.Windows.Forms.Label();
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
            TxtLogEntries.UpdateUI += TrackedScintilla_UpdateUI;
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 250;
            // 
            // LblExplenation
            // 
            LblExplenation.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblExplenation.BackColor = System.Drawing.SystemColors.Window;
            LblExplenation.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            LblExplenation.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            LblExplenation.Location = new System.Drawing.Point(0, 179);
            LblExplenation.Margin = new System.Windows.Forms.Padding(0);
            LblExplenation.Name = "LblExplenation";
            LblExplenation.Size = new System.Drawing.Size(712, 26);
            LblExplenation.TabIndex = 43;
            LblExplenation.Text = "Geen log geladen...\r\n";
            LblExplenation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblExplenation2
            // 
            LblExplenation2.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblExplenation2.BackColor = System.Drawing.SystemColors.Window;
            LblExplenation2.ForeColor = System.Drawing.SystemColors.ControlDark;
            LblExplenation2.Location = new System.Drawing.Point(0, 202);
            LblExplenation2.Margin = new System.Windows.Forms.Padding(0);
            LblExplenation2.Name = "LblExplenation2";
            LblExplenation2.Size = new System.Drawing.Size(712, 26);
            LblExplenation2.TabIndex = 43;
            LblExplenation2.Text = "Start met het (automatisch) ophalen van log";
            LblExplenation2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogViewportControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LblExplenation);
            Controls.Add(LblExplenation2);
            Controls.Add(TxtLogEntries);
            Name = "LogViewportControl";
            Size = new System.Drawing.Size(712, 403);
            ResumeLayout(false);
        }

        #endregion
        private ScintillaNET.Scintilla TxtLogEntries;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label LblExplenation;
        private System.Windows.Forms.Label LblExplenation2;
    }
}
