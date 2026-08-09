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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenLogInTextEditor));
            btnOpenWithEditor = new LogScraper.Controls.Generic.SplitButton();
            RecentFilesMenu = new System.Windows.Forms.ContextMenuStrip(components);
            toolTip1 = new System.Windows.Forms.ToolTip();
            SuspendLayout();
            // 
            // btnOpenWithEditor
            // 
            btnOpenWithEditor.DropDownMenu = RecentFilesMenu;
            btnOpenWithEditor.DropDownWidth = 15;
            btnOpenWithEditor.Icon = (System.Drawing.Image)resources.GetObject("btnOpenWithEditor.Image");
            btnOpenWithEditor.Location = new System.Drawing.Point(0, 0);
            btnOpenWithEditor.Name = "btnOpenWithEditor";
            btnOpenWithEditor.Size = new System.Drawing.Size(40, 25);
            btnOpenWithEditor.TabIndex = 12;
            btnOpenWithEditor.TabStop = false;
            toolTip1.SetToolTip(btnOpenWithEditor, "Open log in editor [CTRL-E]");
            btnOpenWithEditor.ButtonClick += BtnOpenWithEditor_Click;
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
            // OpenLogInTextEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnOpenWithEditor);
            Name = "OpenLogInTextEditor";
            Size = new System.Drawing.Size(40, 25);
            Load += OpenLogInTextEditor_Load;
            ResumeLayout(false);
        }

        #endregion

        private LogScraper.Controls.Generic.SplitButton btnOpenWithEditor;
        private System.Windows.Forms.ContextMenuStrip RecentFilesMenu;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

