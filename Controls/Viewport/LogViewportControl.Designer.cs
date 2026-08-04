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
            ContextMenuStripViewport = new System.Windows.Forms.ContextMenuStrip(components);
            ItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            ItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparatorMark = new System.Windows.Forms.ToolStripSeparator();
            ItemMark = new System.Windows.Forms.ToolStripMenuItem();
            ItemMarkColor1 = new System.Windows.Forms.ToolStripMenuItem();
            ItemMarkColor2 = new System.Windows.Forms.ToolStripMenuItem();
            ItemMarkColor3 = new System.Windows.Forms.ToolStripMenuItem();
            ItemMarkColor4 = new System.Windows.Forms.ToolStripMenuItem();
            ItemMarkColor5 = new System.Windows.Forms.ToolStripMenuItem();
            ToolStripSeparatorMarkReset = new System.Windows.Forms.ToolStripSeparator();
            ItemMarkReset = new System.Windows.Forms.ToolStripMenuItem();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            LblExplenation = new System.Windows.Forms.Label();
            LblExplenation2 = new System.Windows.Forms.Label();
            LblLoading = new System.Windows.Forms.Label();
            ContextMenuStripViewport.SuspendLayout();
            SuspendLayout();
            // 
            // TxtLogEntries
            // 
            TxtLogEntries.AutoCMaxHeight = 9;
            TxtLogEntries.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TxtLogEntries.ContextMenuStrip = ContextMenuStripViewport;
            TxtLogEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            TxtLogEntries.Location = new System.Drawing.Point(0, 0);
            TxtLogEntries.Name = "TxtLogEntries";
            TxtLogEntries.ReadOnly = true;
            TxtLogEntries.Size = new System.Drawing.Size(712, 403);
            TxtLogEntries.TabIndents = true;
            TxtLogEntries.TabIndex = 42;
            TxtLogEntries.UpdateUI += TrackedScintilla_UpdateUI;
            // 
            // ContextMenuStripViewport
            // 
            ContextMenuStripViewport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ItemCopy, ItemSelectAll, ToolStripSeparatorMark, ItemMark });
            ContextMenuStripViewport.Name = "ContextMenuStripViewport";
            ContextMenuStripViewport.Size = new System.Drawing.Size(180, 76);
            // 
            // ItemCopy
            // 
            ItemCopy.Name = "ItemCopy";
            ItemCopy.Size = new System.Drawing.Size(179, 22);
            ItemCopy.Text = "Kopiëren";
            // 
            // ItemSelectAll
            // 
            ItemSelectAll.Name = "ItemSelectAll";
            ItemSelectAll.Size = new System.Drawing.Size(179, 22);
            ItemSelectAll.Text = "Alles selecteren";
            // 
            // ToolStripSeparatorMark
            // 
            ToolStripSeparatorMark.Name = "ToolStripSeparatorMark";
            ToolStripSeparatorMark.Size = new System.Drawing.Size(176, 6);
            // 
            // ItemMark
            // 
            ItemMark.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ItemMarkColor1, ItemMarkColor2, ItemMarkColor3, ItemMarkColor4, ItemMarkColor5, ToolStripSeparatorMarkReset, ItemMarkReset });
            ItemMark.Name = "ItemMark";
            ItemMark.Size = new System.Drawing.Size(179, 22);
            ItemMark.Text = "Markeren";
            // 
            // ItemMarkColor1
            // 
            ItemMarkColor1.Name = "ItemMarkColor1";
            ItemMarkColor1.Size = new System.Drawing.Size(180, 22);
            ItemMarkColor1.Text = "Kleur 1";
            // 
            // ItemMarkColor2
            // 
            ItemMarkColor2.Name = "ItemMarkColor2";
            ItemMarkColor2.Size = new System.Drawing.Size(180, 22);
            ItemMarkColor2.Text = "Kleur 2";
            // 
            // ItemMarkColor3
            // 
            ItemMarkColor3.Name = "ItemMarkColor3";
            ItemMarkColor3.Size = new System.Drawing.Size(180, 22);
            ItemMarkColor3.Text = "Kleur 3";
            // 
            // ItemMarkColor4
            // 
            ItemMarkColor4.Name = "ItemMarkColor4";
            ItemMarkColor4.Size = new System.Drawing.Size(180, 22);
            ItemMarkColor4.Text = "Kleur 4";
            // 
            // ItemMarkColor5
            // 
            ItemMarkColor5.Name = "ItemMarkColor5";
            ItemMarkColor5.Size = new System.Drawing.Size(180, 22);
            ItemMarkColor5.Text = "Kleur 5";
            // 
            // ToolStripSeparatorMarkReset
            // 
            ToolStripSeparatorMarkReset.Name = "ToolStripSeparatorMarkReset";
            ToolStripSeparatorMarkReset.Size = new System.Drawing.Size(177, 6);
            // 
            // ItemMarkReset
            // 
            ItemMarkReset.Name = "ItemMarkReset";
            ItemMarkReset.Size = new System.Drawing.Size(180, 22);
            ItemMarkReset.Text = "Reset";
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
            // LblPaused
            // 
            LblPaused = new System.Windows.Forms.Label();
            LblPaused.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblPaused.AutoSize = false;
            LblPaused.BackColor = System.Drawing.Color.FromArgb(255, 255, 210);
            LblPaused.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            LblPaused.ForeColor = System.Drawing.Color.FromArgb(100, 80, 0);
            LblPaused.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            LblPaused.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            LblPaused.Size = new System.Drawing.Size(695, 24);
            LblPaused.Location = new System.Drawing.Point(0, 0);
            LblPaused.Text = "⏸ Weergave gepauzeerd – grote hoeveelheid log. Klik om eenmalig te vernieuwen.";
            LblPaused.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            LblPaused.Cursor = System.Windows.Forms.Cursors.Hand;
            LblPaused.Visible = false;
            // 
            // LblLoading
            // 
            LblLoading = new System.Windows.Forms.Label();
            LblLoading.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblLoading.AutoSize = false;
            LblLoading.BackColor = System.Drawing.Color.FromArgb(255, 251, 210);
            LblLoading.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LblLoading.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            LblLoading.ForeColor = System.Drawing.Color.FromArgb(110, 90, 0);
            LblLoading.Location = new System.Drawing.Point(684, 6);
            LblLoading.Name = "LblLoading";
            LblLoading.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            LblLoading.Size = new System.Drawing.Size(90, 26);
            LblLoading.TabIndex = 44;
            LblLoading.Text = "Bezig...";
            LblLoading.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            LblLoading.Visible = false;
            LblLoading.Paint += LblFeedback_Paint;
            // 
            // LogViewportControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(LblLoading);
            Controls.Add(LblPaused);
            Controls.Add(LblExplenation);
            Controls.Add(LblExplenation2);
            Controls.Add(TxtLogEntries);
            Name = "LogViewportControl";
            Size = new System.Drawing.Size(712, 403);
            ContextMenuStripViewport.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ScintillaNET.Scintilla TxtLogEntries;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label LblExplenation;
        private System.Windows.Forms.Label LblExplenation2;
        private System.Windows.Forms.Label LblPaused;
        private System.Windows.Forms.Label LblLoading;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStripViewport;
        private System.Windows.Forms.ToolStripMenuItem ItemCopy;
        private System.Windows.Forms.ToolStripMenuItem ItemSelectAll;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparatorMark;
        private System.Windows.Forms.ToolStripMenuItem ItemMark;
        private System.Windows.Forms.ToolStripMenuItem ItemMarkColor1;
        private System.Windows.Forms.ToolStripMenuItem ItemMarkColor2;
        private System.Windows.Forms.ToolStripMenuItem ItemMarkColor3;
        private System.Windows.Forms.ToolStripMenuItem ItemMarkColor4;
        private System.Windows.Forms.ToolStripMenuItem ItemMarkColor5;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparatorMarkReset;
        private System.Windows.Forms.ToolStripMenuItem ItemMarkReset;
    }
}
