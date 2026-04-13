using LogScraper.Controls.LogProviders;

namespace LogScraper.Controls
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
            usrKubernetes = new UserControlKubernetesLogProvider();
            usrRuntime = new UserControlRuntimeLogProvider();
            usrFileLogProvider = new UserControlFileLogProvider();
            lblProviderDescription = new System.Windows.Forms.LinkLabel();
            btnPin = new System.Windows.Forms.Button();
            imageListPins = new System.Windows.Forms.ImageList(components);
            imageListChevron = new System.Windows.Forms.ImageList(components);
            BtnCollapseExpand = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            LblStatusIcon = new System.Windows.Forms.Label();
            imageListStatus = new System.Windows.Forms.ImageList(components);
            PnlLogProviders = new System.Windows.Forms.Panel();
            PnlSeparator2 = new System.Windows.Forms.Panel();
            PnlSeparator1 = new System.Windows.Forms.Panel();
            PnlLogProviders.SuspendLayout();
            SuspendLayout();
            // 
            // lblLogProvider
            // 
            lblLogProvider.AutoSize = true;
            lblLogProvider.Location = new System.Drawing.Point(3, 29);
            lblLogProvider.Name = "lblLogProvider";
            lblLogProvider.Size = new System.Drawing.Size(32, 15);
            lblLogProvider.TabIndex = 21;
            lblLogProvider.Text = "Bron";
            // 
            // cboLogProvider
            // 
            cboLogProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogProvider.FormattingEnabled = true;
            cboLogProvider.Location = new System.Drawing.Point(52, 26);
            cboLogProvider.Name = "cboLogProvider";
            cboLogProvider.Size = new System.Drawing.Size(163, 23);
            cboLogProvider.TabIndex = 18;
            cboLogProvider.SelectedIndexChanged += CboLogProvider_SelectedIndexChanged;
            // 
            // lblLogLayout
            // 
            lblLogLayout.AutoSize = true;
            lblLogLayout.Location = new System.Drawing.Point(3, 55);
            lblLogLayout.Name = "lblLogLayout";
            lblLogLayout.Size = new System.Drawing.Size(43, 15);
            lblLogLayout.TabIndex = 29;
            lblLogLayout.Text = "Layout";
            // 
            // cboLogLayout
            // 
            cboLogLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogLayout.FormattingEnabled = true;
            cboLogLayout.Location = new System.Drawing.Point(52, 52);
            cboLogLayout.Name = "cboLogLayout";
            cboLogLayout.Size = new System.Drawing.Size(163, 23);
            cboLogLayout.TabIndex = 30;
            cboLogLayout.SelectedIndexChanged += CboLogLayout_SelectedIndexChanged;
            // 
            // usrKubernetes
            // 
            usrKubernetes.Dock = System.Windows.Forms.DockStyle.Fill;
            usrKubernetes.Location = new System.Drawing.Point(0, 0);
            usrKubernetes.Name = "usrKubernetes";
            usrKubernetes.Size = new System.Drawing.Size(308, 119);
            usrKubernetes.TabIndex = 8;
            // 
            // usrRuntime
            // 
            usrRuntime.Dock = System.Windows.Forms.DockStyle.Fill;
            usrRuntime.Location = new System.Drawing.Point(0, 0);
            usrRuntime.Name = "usrRuntime";
            usrRuntime.Size = new System.Drawing.Size(308, 119);
            usrRuntime.TabIndex = 0;
            // 
            // usrFileLogProvider
            // 
            usrFileLogProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            usrFileLogProvider.Location = new System.Drawing.Point(0, 0);
            usrFileLogProvider.Name = "usrFileLogProvider";
            usrFileLogProvider.Size = new System.Drawing.Size(308, 119);
            usrFileLogProvider.TabIndex = 3;
            // 
            // lblProviderDescription
            // 
            lblProviderDescription.ActiveLinkColor = System.Drawing.Color.Black;
            lblProviderDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblProviderDescription.AutoEllipsis = true;
            lblProviderDescription.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            lblProviderDescription.LinkColor = System.Drawing.Color.Black;
            lblProviderDescription.Location = new System.Drawing.Point(23, 6);
            lblProviderDescription.Name = "lblProviderDescription";
            lblProviderDescription.Size = new System.Drawing.Size(266, 17);
            lblProviderDescription.TabIndex = 25;
            lblProviderDescription.TabStop = true;
            lblProviderDescription.Text = "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW";
            lblProviderDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblProviderDescription.VisitedLinkColor = System.Drawing.Color.Black;
            lblProviderDescription.MouseClick += lblProviderDescription_MouseClick;
            lblProviderDescription.MouseEnter += lblProviderDescription_MouseEnter;
            lblProviderDescription.MouseLeave += lblProviderDescription_MouseLeave;
            // 
            // btnPin
            // 
            btnPin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            btnPin.FlatAppearance.BorderSize = 0;
            btnPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPin.ImageIndex = 0;
            btnPin.ImageList = imageListPins;
            btnPin.Location = new System.Drawing.Point(283, 26);
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
            // LblStatusIcon
            // 
            LblStatusIcon.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            LblStatusIcon.ImageIndex = 1;
            LblStatusIcon.ImageList = imageListStatus;
            LblStatusIcon.Location = new System.Drawing.Point(288, 5);
            LblStatusIcon.Name = "LblStatusIcon";
            LblStatusIcon.Size = new System.Drawing.Size(16, 16);
            LblStatusIcon.TabIndex = 33;
            // 
            // imageListStatus
            // 
            imageListStatus.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListStatus.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListStatus.ImageStream");
            imageListStatus.TransparentColor = System.Drawing.Color.Transparent;
            imageListStatus.Images.SetKeyName(0, "download 16x16.png");
            imageListStatus.Images.SetKeyName(1, "cogs-custom 16x16.png");
            // 
            // PnlLogProviders
            // 
            PnlLogProviders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PnlLogProviders.Controls.Add(PnlSeparator2);
            PnlLogProviders.Controls.Add(usrKubernetes);
            PnlLogProviders.Controls.Add(usrFileLogProvider);
            PnlLogProviders.Controls.Add(usrRuntime);
            PnlLogProviders.Location = new System.Drawing.Point(0, 85);
            PnlLogProviders.Name = "PnlLogProviders";
            PnlLogProviders.Size = new System.Drawing.Size(308, 119);
            PnlLogProviders.TabIndex = 31;
            // 
            // PnlSeparator2
            // 
            PnlSeparator2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PnlSeparator2.BackColor = System.Drawing.Color.LightGray;
            PnlSeparator2.Location = new System.Drawing.Point(3, 118);
            PnlSeparator2.Name = "PnlSeparator2";
            PnlSeparator2.Size = new System.Drawing.Size(300, 1);
            PnlSeparator2.TabIndex = 33;
            // 
            // PnlSeparator1
            // 
            PnlSeparator1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PnlSeparator1.BackColor = System.Drawing.Color.LightGray;
            PnlSeparator1.Location = new System.Drawing.Point(3, 81);
            PnlSeparator1.Name = "PnlSeparator1";
            PnlSeparator1.Size = new System.Drawing.Size(300, 1);
            PnlSeparator1.TabIndex = 32;
            // 
            // LogProviderSelectionControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(PnlSeparator1);
            Controls.Add(PnlLogProviders);
            Controls.Add(BtnCollapseExpand);
            Controls.Add(btnPin);
            Controls.Add(cboLogLayout);
            Controls.Add(lblLogLayout);
            Controls.Add(cboLogProvider);
            Controls.Add(lblLogProvider);
            Controls.Add(LblStatusIcon);
            Controls.Add(lblProviderDescription);
            Name = "LogProviderSelectionControl";
            Size = new System.Drawing.Size(308, 207);
            PnlLogProviders.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblLogProvider;
        private System.Windows.Forms.ComboBox cboLogProvider;
        private System.Windows.Forms.Label lblLogLayout;
        private System.Windows.Forms.ComboBox cboLogLayout;
        private UserControlRuntimeLogProvider usrRuntime;
        private UserControlKubernetesLogProvider usrKubernetes;
        private UserControlFileLogProvider usrFileLogProvider;
        private System.Windows.Forms.LinkLabel lblProviderDescription;
        private System.Windows.Forms.Button btnPin;
        private System.Windows.Forms.ImageList imageListPins;
        private System.Windows.Forms.ImageList imageListChevron;
        private System.Windows.Forms.Button BtnCollapseExpand;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel PnlLogProviders;
        private System.Windows.Forms.Panel PnlSeparator1;
        private System.Windows.Forms.Panel PnlSeparator2;
        private System.Windows.Forms.ImageList imageListStatus;
        private System.Windows.Forms.Label LblStatusIcon;
    }
}
