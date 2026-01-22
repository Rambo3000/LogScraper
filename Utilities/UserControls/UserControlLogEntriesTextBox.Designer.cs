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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLogEntriesTextBox));
            TxtLogEntries = new ScintillaNET.Scintilla();
            ChkShowNoTree = new System.Windows.Forms.CheckBox();
            ChkShowFlowTree = new System.Windows.Forms.CheckBox();
            CboLogContentType = new System.Windows.Forms.ComboBox();
            PnlViewMode = new System.Windows.Forms.Panel();
            UserControlPostProcessing = new LogScraper.LogPostProcessors.UserControlPostProcessing();
            PnlViewMode.SuspendLayout();
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
            TxtLogEntries.SizeChanged += TxtLogEntries_SizeChanged;
            // 
            // ChkShowNoTree
            // 
            ChkShowNoTree.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ChkShowNoTree.Appearance = System.Windows.Forms.Appearance.Button;
            ChkShowNoTree.Checked = true;
            ChkShowNoTree.CheckState = System.Windows.Forms.CheckState.Checked;
            ChkShowNoTree.Enabled = false;
            ChkShowNoTree.Image = (System.Drawing.Image)resources.GetObject("ChkShowNoTree.Image");
            ChkShowNoTree.Location = new System.Drawing.Point(120, 3);
            ChkShowNoTree.Name = "ChkShowNoTree";
            ChkShowNoTree.Size = new System.Drawing.Size(25, 25);
            ChkShowNoTree.TabIndex = 44;
            ChkShowNoTree.Tag = "asd";
            ChkShowNoTree.UseVisualStyleBackColor = true;
            ChkShowNoTree.CheckedChanged += ChkShowNoTree_CheckedChanged;
            // 
            // ChkShowFlowTree
            // 
            ChkShowFlowTree.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ChkShowFlowTree.Appearance = System.Windows.Forms.Appearance.Button;
            ChkShowFlowTree.Image = (System.Drawing.Image)resources.GetObject("ChkShowFlowTree.Image");
            ChkShowFlowTree.Location = new System.Drawing.Point(145, 3);
            ChkShowFlowTree.Name = "ChkShowFlowTree";
            ChkShowFlowTree.Size = new System.Drawing.Size(25, 25);
            ChkShowFlowTree.TabIndex = 43;
            ChkShowFlowTree.Tag = "asd";
            ChkShowFlowTree.UseVisualStyleBackColor = true;
            ChkShowFlowTree.CheckedChanged += ChkShowFlowTree_CheckedChanged;
            // 
            // CboLogContentType
            // 
            CboLogContentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CboLogContentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CboLogContentType.FormattingEnabled = true;
            CboLogContentType.Location = new System.Drawing.Point(13, 4);
            CboLogContentType.Name = "CboLogContentType";
            CboLogContentType.Size = new System.Drawing.Size(105, 23);
            CboLogContentType.TabIndex = 45;
            CboLogContentType.SelectedIndexChanged += CboLogContentType_SelectedIndexChanged;
            // 
            // PnlViewMode
            // 
            PnlViewMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PnlViewMode.BackColor = System.Drawing.Color.White;
            PnlViewMode.Controls.Add(UserControlPostProcessing);
            PnlViewMode.Controls.Add(CboLogContentType);
            PnlViewMode.Controls.Add(ChkShowFlowTree);
            PnlViewMode.Controls.Add(ChkShowNoTree);
            PnlViewMode.Location = new System.Drawing.Point(494, 3);
            PnlViewMode.Name = "PnlViewMode";
            PnlViewMode.Size = new System.Drawing.Size(215, 31);
            PnlViewMode.TabIndex = 46;
            // 
            // UserControlPostProcessing
            // 
            UserControlPostProcessing.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            UserControlPostProcessing.Location = new System.Drawing.Point(176, 3);
            UserControlPostProcessing.Name = "UserControlPostProcessing";
            UserControlPostProcessing.Size = new System.Drawing.Size(36, 25);
            UserControlPostProcessing.TabIndex = 47;
            // 
            // UserControlLogEntriesTextBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PnlViewMode);
            Controls.Add(TxtLogEntries);
            Name = "UserControlLogEntriesTextBox";
            Size = new System.Drawing.Size(712, 403);
            PnlViewMode.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ScintillaNET.Scintilla TxtLogEntries;
        private System.Windows.Forms.CheckBox ChkShowNoTree;
        private System.Windows.Forms.CheckBox ChkShowFlowTree;
        private System.Windows.Forms.ComboBox CboLogContentType;
        private System.Windows.Forms.Panel PnlViewMode;
        private LogPostProcessors.UserControlPostProcessing UserControlPostProcessing;
    }
}
