namespace LogScraper.Log.Metadata
{
    partial class UserControlMetadataFilterOverview
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
            PanelFilters = new System.Windows.Forms.Panel();
            SuspendLayout();
            // 
            // PanelFilters
            //
            PanelFilters.AutoScroll = true;
            PanelFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            PanelFilters.Location = new System.Drawing.Point(3, 3);
            PanelFilters.Name = "PanelFilters";
            PanelFilters.Size = new System.Drawing.Size(191, 391);
            PanelFilters.TabIndex = 0;
            PanelFilters.Resize += PanelFilters_Resize;
            // 
            // UserControlMetadataFilterOverview
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(PanelFilters);
            Name = "UserControlMetadataFilterOverview";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel PanelFilters;
    }
}
