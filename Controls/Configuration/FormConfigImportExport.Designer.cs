namespace LogScraper.Controls.Configuration
{
    partial class FormConfigImportExport
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigImportExport));
            BtnCancel = new System.Windows.Forms.Button();
            BtnImportExport = new System.Windows.Forms.Button();
            ChkImportLogLayoutSettings = new System.Windows.Forms.CheckBox();
            ChkImportGeneralSettings = new System.Windows.Forms.CheckBox();
            ChkImportLogProvidersSettings = new System.Windows.Forms.CheckBox();
            LblExplenation = new System.Windows.Forms.Label();
            imageList1 = new System.Windows.Forms.ImageList(components);
            SuspendLayout();
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BtnCancel.Enabled = false;
            BtnCancel.Location = new System.Drawing.Point(202, 115);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new System.Drawing.Size(109, 29);
            BtnCancel.TabIndex = 5;
            BtnCancel.Text = "Annuleren";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // BtnImportExport
            // 
            BtnImportExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BtnImportExport.Enabled = false;
            BtnImportExport.ImageIndex = 0;
            BtnImportExport.ImageList = imageList1;
            BtnImportExport.Location = new System.Drawing.Point(317, 115);
            BtnImportExport.Name = "BtnImportExport";
            BtnImportExport.Size = new System.Drawing.Size(109, 29);
            BtnImportExport.TabIndex = 4;
            BtnImportExport.Text = "Importeren";
            BtnImportExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            BtnImportExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            BtnImportExport.UseVisualStyleBackColor = true;
            BtnImportExport.Click += BtnImportExport_Click;
            // 
            // ChkImportLogLayoutSettings
            // 
            ChkImportLogLayoutSettings.AutoSize = true;
            ChkImportLogLayoutSettings.Enabled = false;
            ChkImportLogLayoutSettings.Location = new System.Drawing.Point(12, 59);
            ChkImportLogLayoutSettings.Name = "ChkImportLogLayoutSettings";
            ChkImportLogLayoutSettings.Size = new System.Drawing.Size(87, 19);
            ChkImportLogLayoutSettings.TabIndex = 2;
            ChkImportLogLayoutSettings.Text = "Log layouts";
            ChkImportLogLayoutSettings.UseVisualStyleBackColor = true;
            ChkImportLogLayoutSettings.CheckedChanged += ChkChanged;
            // 
            // ChkImportGeneralSettings
            // 
            ChkImportGeneralSettings.AutoSize = true;
            ChkImportGeneralSettings.Enabled = false;
            ChkImportGeneralSettings.Location = new System.Drawing.Point(12, 34);
            ChkImportGeneralSettings.Name = "ChkImportGeneralSettings";
            ChkImportGeneralSettings.Size = new System.Drawing.Size(144, 19);
            ChkImportGeneralSettings.TabIndex = 1;
            ChkImportGeneralSettings.Text = "Algemene instellingen";
            ChkImportGeneralSettings.UseVisualStyleBackColor = true;
            ChkImportGeneralSettings.CheckedChanged += ChkChanged;
            // 
            // ChkImportLogProvidersSettings
            // 
            ChkImportLogProvidersSettings.AutoSize = true;
            ChkImportLogProvidersSettings.Enabled = false;
            ChkImportLogProvidersSettings.Location = new System.Drawing.Point(12, 84);
            ChkImportLogProvidersSettings.Name = "ChkImportLogProvidersSettings";
            ChkImportLogProvidersSettings.Size = new System.Drawing.Size(137, 19);
            ChkImportLogProvidersSettings.TabIndex = 3;
            ChkImportLogProvidersSettings.Text = "Bronnen van logging";
            ChkImportLogProvidersSettings.UseVisualStyleBackColor = true;
            ChkImportLogProvidersSettings.CheckedChanged += ChkChanged;
            // 
            // LblExplenation
            // 
            LblExplenation.AutoSize = true;
            LblExplenation.Location = new System.Drawing.Point(10, 12);
            LblExplenation.Name = "LblExplenation";
            LblExplenation.Size = new System.Drawing.Size(57, 15);
            LblExplenation.TabIndex = 0;
            LblExplenation.Text = "Selecteer ";
            LblExplenation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "import 16x16.png");
            imageList1.Images.SetKeyName(1, "export 16x16.png");
            // 
            // FormConfigImportExport
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(438, 156);
            Controls.Add(LblExplenation);
            Controls.Add(ChkImportGeneralSettings);
            Controls.Add(ChkImportLogLayoutSettings);
            Controls.Add(ChkImportLogProvidersSettings);
            Controls.Add(BtnCancel);
            Controls.Add(BtnImportExport);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormConfigImportExport";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Instellingen importeren";
            Load += FormConfigImportExport_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnImportExport;
        private System.Windows.Forms.CheckBox ChkImportLogLayoutSettings;
        private System.Windows.Forms.CheckBox ChkImportGeneralSettings;
        private System.Windows.Forms.CheckBox ChkImportLogProvidersSettings;
        private System.Windows.Forms.Label LblExplenation;
        private System.Windows.Forms.ImageList imageList1;
    }
}
