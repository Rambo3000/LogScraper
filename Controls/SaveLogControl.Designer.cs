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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveLogControl));
            BtnSave = new LogScraper.Controls.Generic.SplitButton();
            RecentFilesMenu = new System.Windows.Forms.ContextMenuStrip(components);
            toolTip1 = new System.Windows.Forms.ToolTip();
            SuspendLayout();
            // 
            // BtnSave
            // 
            BtnSave.DropDownMenu = RecentFilesMenu;
            BtnSave.DropDownWidth = 15;
            BtnSave.Icon = (System.Drawing.Image)resources.GetObject("BtnSave.Image");
            BtnSave.Location = new System.Drawing.Point(0, 0);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new System.Drawing.Size(40, 25);
            BtnSave.TabIndex = 27;
            BtnSave.TabStop = false;
            toolTip1.SetToolTip(BtnSave, "Save log [CTRL-S]");
            BtnSave.ButtonClick += BtnSave_Click;
            // 
            // RecentFilesMenu
            // 
            RecentFilesMenu.Name = "RecentFilesMenu";
            RecentFilesMenu.Size = new System.Drawing.Size(61, 4);
            RecentFilesMenu.Opening += RecentFilesMenu_Opening;
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
            Size = new System.Drawing.Size(40, 25);
            ResumeLayout(false);
        }

        #endregion

        private LogScraper.Controls.Generic.SplitButton BtnSave;
        private System.Windows.Forms.ContextMenuStrip RecentFilesMenu;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
