namespace LogScraper.Utilities.UserControls
{
    partial class LogProviderSelectionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogProviderSelectionControl));
            lblLogProvider = new System.Windows.Forms.Label();
            cboLogProvider = new System.Windows.Forms.ComboBox();
            lblLogLayout = new System.Windows.Forms.Label();
            cboLogLayout = new System.Windows.Forms.ComboBox();
            GrpLogProvidersSettings = new System.Windows.Forms.GroupBox();
            usrKubernetes = new LogScraper.LogProviders.Kubernetes.UserControlKubernetesLogProvider();
            usrRuntime = new LogScraper.LogProviders.Runtime.UserControlRuntimeLogProvider();
            usrFileLogProvider = new LogScraper.LogProviders.File.UserControlFileLogProvider();
            lblProviderDescription = new System.Windows.Forms.Label();
            btnPin = new System.Windows.Forms.Button();
            imageListPins = new System.Windows.Forms.ImageList(components);
            imageListChevron = new System.Windows.Forms.ImageList(components);
            BtnCollapseExpand = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            GrpLogProvidersSettings.SuspendLayout();
            SuspendLayout();
            // 
            // lblLogProvider
            // 
            lblLogProvider.AutoSize = true;
            lblLogProvider.Location = new System.Drawing.Point(23, 6);
            lblLogProvider.Name = "lblLogProvider";
            lblLogProvider.Size = new System.Drawing.Size(32, 15);
            lblLogProvider.TabIndex = 21;
            lblLogProvider.Text = "Bron";
            // 
            // cboLogProvider
            // 
            cboLogProvider.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboLogProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogProvider.FormattingEnabled = true;
            cboLogProvider.Location = new System.Drawing.Point(72, 3);
            cboLogProvider.Name = "cboLogProvider";
            cboLogProvider.Size = new System.Drawing.Size(217, 23);
            cboLogProvider.TabIndex = 18;
            // 
            // lblLogLayout
            // 
            lblLogLayout.AutoSize = true;
            lblLogLayout.Location = new System.Drawing.Point(23, 32);
            lblLogLayout.Name = "lblLogLayout";
            lblLogLayout.Size = new System.Drawing.Size(43, 15);
            lblLogLayout.TabIndex = 29;
            lblLogLayout.Text = "Layout";
            // 
            // cboLogLayout
            // 
            cboLogLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboLogLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogLayout.FormattingEnabled = true;
            cboLogLayout.Location = new System.Drawing.Point(72, 29);
            cboLogLayout.Name = "cboLogLayout";
            cboLogLayout.Size = new System.Drawing.Size(217, 23);
            cboLogLayout.TabIndex = 30;
            // 
            // GrpLogProvidersSettings
            // 
            GrpLogProvidersSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpLogProvidersSettings.Controls.Add(usrKubernetes);
            GrpLogProvidersSettings.Controls.Add(usrRuntime);
            GrpLogProvidersSettings.Controls.Add(usrFileLogProvider);
            GrpLogProvidersSettings.Location = new System.Drawing.Point(3, 56);
            GrpLogProvidersSettings.MinimumSize = new System.Drawing.Size(300, 0);
            GrpLogProvidersSettings.Name = "GrpLogProvidersSettings";
            GrpLogProvidersSettings.Size = new System.Drawing.Size(311, 147);
            GrpLogProvidersSettings.TabIndex = 24;
            GrpLogProvidersSettings.TabStop = false;
            GrpLogProvidersSettings.Text = "Instellingen";
            // 
            // usrKubernetes
            // 
            usrKubernetes.Dock = System.Windows.Forms.DockStyle.Fill;
            usrKubernetes.Location = new System.Drawing.Point(3, 19);
            usrKubernetes.Name = "usrKubernetes";
            usrKubernetes.Size = new System.Drawing.Size(305, 125);
            usrKubernetes.TabIndex = 8;
            // 
            // usrRuntime
            // 
            usrRuntime.Dock = System.Windows.Forms.DockStyle.Fill;
            usrRuntime.Location = new System.Drawing.Point(3, 19);
            usrRuntime.Name = "usrRuntime";
            usrRuntime.Size = new System.Drawing.Size(305, 125);
            usrRuntime.TabIndex = 0;
            // 
            // usrFileLogProvider
            // 
            usrFileLogProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            usrFileLogProvider.Location = new System.Drawing.Point(3, 19);
            usrFileLogProvider.Name = "usrFileLogProvider";
            usrFileLogProvider.Size = new System.Drawing.Size(305, 125);
            usrFileLogProvider.TabIndex = 3;
            // 
            // lblProviderDescription
            // 
            lblProviderDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblProviderDescription.AutoEllipsis = true;
            lblProviderDescription.Location = new System.Drawing.Point(26, 6);
            lblProviderDescription.Name = "lblProviderDescription";
            lblProviderDescription.Size = new System.Drawing.Size(285, 16);
            lblProviderDescription.TabIndex = 25;
            lblProviderDescription.Text = " ";
            lblProviderDescription.Visible = false;
            // 
            // btnPin
            // 
            btnPin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnPin.FlatAppearance.BorderSize = 0;
            btnPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPin.ImageIndex = 0;
            btnPin.ImageList = imageListPins;
            btnPin.Location = new System.Drawing.Point(290, 1);
            btnPin.Name = "btnPin";
            btnPin.Size = new System.Drawing.Size(24, 24);
            btnPin.TabIndex = 27;
            btnPin.UseVisualStyleBackColor = true;
            btnPin.Click += BtnPin_Click;
            // 
            // imageListPins
            // 
            imageListPins.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListPins.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListPins.ImageStream");
            imageListPins.TransparentColor = System.Drawing.Color.Transparent;
            imageListPins.Images.SetKeyName(0, "pin-outline 16x16.png");
            imageListPins.Images.SetKeyName(1, "pin-off-outline 16x16.png");
            // 
            // imageListChevron
            // 
            imageListChevron.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListChevron.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListChevron.ImageStream");
            imageListChevron.TransparentColor = System.Drawing.Color.Transparent;
            imageListChevron.Images.SetKeyName(0, "chevron-right 16x16.png");
            imageListChevron.Images.SetKeyName(1, "chevron-down.png");
            // 
            // BtnCollapseExpand
            // 
            BtnCollapseExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            BtnCollapseExpand.FlatAppearance.BorderSize = 0;
            BtnCollapseExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnCollapseExpand.ImageIndex = 0;
            BtnCollapseExpand.ImageList = imageListChevron;
            BtnCollapseExpand.Location = new System.Drawing.Point(3, 0);
            BtnCollapseExpand.Name = "BtnCollapseExpand";
            BtnCollapseExpand.Size = new System.Drawing.Size(24, 24);
            BtnCollapseExpand.TabIndex = 28;
            BtnCollapseExpand.UseVisualStyleBackColor = true;
            BtnCollapseExpand.Click += BtnCollapseExpand_Click;
            // 
            // LogProviderSelectionControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(BtnCollapseExpand);
            Controls.Add(btnPin);
            Controls.Add(GrpLogProvidersSettings);
            Controls.Add(cboLogLayout);
            Controls.Add(lblLogLayout);
            Controls.Add(cboLogProvider);
            Controls.Add(lblLogProvider);
            Controls.Add(lblProviderDescription);
            Name = "LogProviderSelectionControl";
            Size = new System.Drawing.Size(317, 207);
            GrpLogProvidersSettings.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblLogProvider;
        private System.Windows.Forms.ComboBox cboLogProvider;
        private System.Windows.Forms.Label lblLogLayout;
        private System.Windows.Forms.ComboBox cboLogLayout;
        private System.Windows.Forms.GroupBox GrpLogProvidersSettings;
        private LogScraper.LogProviders.Runtime.UserControlRuntimeLogProvider usrRuntime;
        private LogScraper.LogProviders.Kubernetes.UserControlKubernetesLogProvider usrKubernetes;
        private LogScraper.LogProviders.File.UserControlFileLogProvider usrFileLogProvider;
        private System.Windows.Forms.Label lblProviderDescription;
        private System.Windows.Forms.Button btnPin;
        private System.Windows.Forms.ImageList imageListPins;
        private System.Windows.Forms.ImageList imageListChevron;
        private System.Windows.Forms.Button BtnCollapseExpand;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
