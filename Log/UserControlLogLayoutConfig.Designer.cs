using LogScraper.Extensions;

namespace LogScraper.LogProviders.Kubernetes
{
    partial class UserControlLogLayoutConfig
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
            LstLayouts = new System.Windows.Forms.ListBox();
            BtnLayoutAdd = new System.Windows.Forms.Button();
            BtnLayoutRemove = new System.Windows.Forms.Button();
            BtnLayoutUp = new System.Windows.Forms.Button();
            BtnLayoutDown = new System.Windows.Forms.Button();
            TxtDescription = new ValidatedTextBox();
            TxtDateTimeFormat = new ValidatedTextBox();
            GrpLayouts = new System.Windows.Forms.GroupBox();
            grpRuntime = new System.Windows.Forms.GroupBox();
            GrpContent = new System.Windows.Forms.GroupBox();
            LblContentStartPosition = new System.Windows.Forms.Label();
            txtContentStartPosition = new ValidatedTextBox();
            LblLblContentAfterPhrase = new System.Windows.Forms.Label();
            TxtLblContentAfterPhrase = new ValidatedTextBox();
            LblContentBeforePhrase = new System.Windows.Forms.Label();
            TxtContentBeforePhrase = new ValidatedTextBox();
            LblContentDescription = new System.Windows.Forms.Label();
            TxtContentDescription = new ValidatedTextBox();
            LstContent = new System.Windows.Forms.ListBox();
            BtnContentAdd = new System.Windows.Forms.Button();
            BtnContentRemove = new System.Windows.Forms.Button();
            BtnContentDown = new System.Windows.Forms.Button();
            BtnContentUp = new System.Windows.Forms.Button();
            grpMetadata = new System.Windows.Forms.GroupBox();
            LblMetadataStartPosition = new System.Windows.Forms.Label();
            TxtMetadataStartPosition = new ValidatedTextBox();
            LblMetadataAfterPhrase = new System.Windows.Forms.Label();
            TxtMetadataAfterPhrase = new ValidatedTextBox();
            LblMetadataBeforePhrase = new System.Windows.Forms.Label();
            TxtMetadataBeforePhrase = new ValidatedTextBox();
            LblMetadataDescription = new System.Windows.Forms.Label();
            TxtMetadataDescription = new ValidatedTextBox();
            LstMetadata = new System.Windows.Forms.ListBox();
            BtnMetadataAdd = new System.Windows.Forms.Button();
            BtnMetadataRemove = new System.Windows.Forms.Button();
            BtnMetadataDown = new System.Windows.Forms.Button();
            BtnMetadataUp = new System.Windows.Forms.Button();
            LblDateTimeFormat = new System.Windows.Forms.Label();
            LblDescription = new System.Windows.Forms.Label();
            GrpLayouts.SuspendLayout();
            grpRuntime.SuspendLayout();
            GrpContent.SuspendLayout();
            grpMetadata.SuspendLayout();
            SuspendLayout();
            // 
            // LstLayouts
            // 
            LstLayouts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstLayouts.FormattingEnabled = true;
            LstLayouts.IntegralHeight = false;
            LstLayouts.Location = new System.Drawing.Point(6, 22);
            LstLayouts.Name = "LstLayouts";
            LstLayouts.Size = new System.Drawing.Size(232, 537);
            LstLayouts.TabIndex = 0;
            LstLayouts.SelectedIndexChanged += LstUrls_SelectedIndexChanged;
            // 
            // BtnLayoutAdd
            // 
            BtnLayoutAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnLayoutAdd.Location = new System.Drawing.Point(6, 565);
            BtnLayoutAdd.Name = "BtnLayoutAdd";
            BtnLayoutAdd.Size = new System.Drawing.Size(80, 23);
            BtnLayoutAdd.TabIndex = 1;
            BtnLayoutAdd.Text = "Toevoegen";
            BtnLayoutAdd.UseVisualStyleBackColor = true;
            BtnLayoutAdd.Click += BtnAddLayout_Click;
            // 
            // BtnLayoutRemove
            // 
            BtnLayoutRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnLayoutRemove.Location = new System.Drawing.Point(92, 565);
            BtnLayoutRemove.Name = "BtnLayoutRemove";
            BtnLayoutRemove.Size = new System.Drawing.Size(80, 23);
            BtnLayoutRemove.TabIndex = 2;
            BtnLayoutRemove.Text = "Verwijderen";
            BtnLayoutRemove.UseVisualStyleBackColor = true;
            BtnLayoutRemove.Click += BtnRemoveLayout_Click;
            // 
            // BtnLayoutUp
            // 
            BtnLayoutUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnLayoutUp.Image = Properties.Resources.up;
            BtnLayoutUp.Location = new System.Drawing.Point(192, 565);
            BtnLayoutUp.Name = "BtnLayoutUp";
            BtnLayoutUp.Size = new System.Drawing.Size(22, 23);
            BtnLayoutUp.TabIndex = 3;
            BtnLayoutUp.UseVisualStyleBackColor = true;
            BtnLayoutUp.Click += BtnUpLayout_Click;
            // 
            // BtnLayoutDown
            // 
            BtnLayoutDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnLayoutDown.Image = Properties.Resources.down;
            BtnLayoutDown.Location = new System.Drawing.Point(216, 565);
            BtnLayoutDown.Name = "BtnLayoutDown";
            BtnLayoutDown.Size = new System.Drawing.Size(22, 23);
            BtnLayoutDown.TabIndex = 4;
            BtnLayoutDown.UseVisualStyleBackColor = true;
            BtnLayoutDown.Click += BtnDownLayout_Click;
            // 
            // TxtDescription
            // 
            TxtDescription.IsRequired = true;
            TxtDescription.Location = new System.Drawing.Point(8, 37);
            TxtDescription.Name = "TxtDescription";
            TxtDescription.Size = new System.Drawing.Size(239, 23);
            TxtDescription.TabIndex = 5;
            TxtDescription.TextChanged += TxtDescription_TextChanged;
            // 
            // TxtDateTimeFormat
            // 
            TxtDateTimeFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtDateTimeFormat.IsRequired = true;
            TxtDateTimeFormat.Location = new System.Drawing.Point(8, 81);
            TxtDateTimeFormat.Name = "TxtDateTimeFormat";
            TxtDateTimeFormat.Size = new System.Drawing.Size(273, 23);
            TxtDateTimeFormat.TabIndex = 6;
            TxtDateTimeFormat.TextChanged += TxtUrl_TextChanged;
            // 
            // GrpLayouts
            // 
            GrpLayouts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpLayouts.Controls.Add(grpRuntime);
            GrpLayouts.Controls.Add(LstLayouts);
            GrpLayouts.Controls.Add(BtnLayoutAdd);
            GrpLayouts.Controls.Add(BtnLayoutRemove);
            GrpLayouts.Controls.Add(BtnLayoutUp);
            GrpLayouts.Controls.Add(BtnLayoutDown);
            GrpLayouts.Location = new System.Drawing.Point(0, 23);
            GrpLayouts.Name = "GrpLayouts";
            GrpLayouts.Size = new System.Drawing.Size(977, 594);
            GrpLayouts.TabIndex = 8;
            GrpLayouts.TabStop = false;
            GrpLayouts.Text = "Log layouts";
            // 
            // grpRuntime
            // 
            grpRuntime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpRuntime.Controls.Add(GrpContent);
            grpRuntime.Controls.Add(grpMetadata);
            grpRuntime.Controls.Add(LblDateTimeFormat);
            grpRuntime.Controls.Add(LblDescription);
            grpRuntime.Controls.Add(TxtDateTimeFormat);
            grpRuntime.Controls.Add(TxtDescription);
            grpRuntime.Location = new System.Drawing.Point(244, 16);
            grpRuntime.Name = "grpRuntime";
            grpRuntime.Size = new System.Drawing.Size(727, 543);
            grpRuntime.TabIndex = 14;
            grpRuntime.TabStop = false;
            grpRuntime.Text = "Log layout";
            // 
            // GrpContent
            // 
            GrpContent.Controls.Add(LblContentStartPosition);
            GrpContent.Controls.Add(txtContentStartPosition);
            GrpContent.Controls.Add(LblLblContentAfterPhrase);
            GrpContent.Controls.Add(TxtLblContentAfterPhrase);
            GrpContent.Controls.Add(LblContentBeforePhrase);
            GrpContent.Controls.Add(TxtContentBeforePhrase);
            GrpContent.Controls.Add(LblContentDescription);
            GrpContent.Controls.Add(TxtContentDescription);
            GrpContent.Controls.Add(LstContent);
            GrpContent.Controls.Add(BtnContentAdd);
            GrpContent.Controls.Add(BtnContentRemove);
            GrpContent.Controls.Add(BtnContentDown);
            GrpContent.Controls.Add(BtnContentUp);
            GrpContent.Location = new System.Drawing.Point(6, 322);
            GrpContent.Name = "GrpContent";
            GrpContent.Size = new System.Drawing.Size(528, 206);
            GrpContent.TabIndex = 20;
            GrpContent.TabStop = false;
            GrpContent.Text = "Content begin en eind filters";
            // 
            // LblContentStartPosition
            // 
            LblContentStartPosition.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblContentStartPosition.AutoSize = true;
            LblContentStartPosition.Location = new System.Drawing.Point(241, 154);
            LblContentStartPosition.Name = "LblContentStartPosition";
            LblContentStartPosition.Size = new System.Drawing.Size(66, 15);
            LblContentStartPosition.TabIndex = 26;
            LblContentStartPosition.Text = "Startpositie";
            // 
            // txtContentStartPosition
            // 
            txtContentStartPosition.IsRequired = true;
            txtContentStartPosition.Location = new System.Drawing.Point(241, 172);
            txtContentStartPosition.Name = "txtContentStartPosition";
            txtContentStartPosition.Size = new System.Drawing.Size(44, 23);
            txtContentStartPosition.TabIndex = 25;
            // 
            // LblLblContentAfterPhrase
            // 
            LblLblContentAfterPhrase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblLblContentAfterPhrase.AutoSize = true;
            LblLblContentAfterPhrase.Location = new System.Drawing.Point(241, 110);
            LblLblContentAfterPhrase.Name = "LblLblContentAfterPhrase";
            LblLblContentAfterPhrase.Size = new System.Drawing.Size(160, 15);
            LblLblContentAfterPhrase.TabIndex = 24;
            LblLblContentAfterPhrase.Text = "Tekst na de metadata waarde";
            // 
            // TxtLblContentAfterPhrase
            // 
            TxtLblContentAfterPhrase.IsRequired = true;
            TxtLblContentAfterPhrase.Location = new System.Drawing.Point(241, 128);
            TxtLblContentAfterPhrase.Name = "TxtLblContentAfterPhrase";
            TxtLblContentAfterPhrase.Size = new System.Drawing.Size(273, 23);
            TxtLblContentAfterPhrase.TabIndex = 23;
            // 
            // LblContentBeforePhrase
            // 
            LblContentBeforePhrase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblContentBeforePhrase.AutoSize = true;
            LblContentBeforePhrase.Location = new System.Drawing.Point(241, 66);
            LblContentBeforePhrase.Name = "LblContentBeforePhrase";
            LblContentBeforePhrase.Size = new System.Drawing.Size(171, 15);
            LblContentBeforePhrase.TabIndex = 22;
            LblContentBeforePhrase.Text = "Tekst voor de metadata waarde";
            // 
            // TxtContentBeforePhrase
            // 
            TxtContentBeforePhrase.IsRequired = true;
            TxtContentBeforePhrase.Location = new System.Drawing.Point(241, 84);
            TxtContentBeforePhrase.Name = "TxtContentBeforePhrase";
            TxtContentBeforePhrase.Size = new System.Drawing.Size(273, 23);
            TxtContentBeforePhrase.TabIndex = 21;
            // 
            // LblContentDescription
            // 
            LblContentDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblContentDescription.AutoSize = true;
            LblContentDescription.Location = new System.Drawing.Point(241, 22);
            LblContentDescription.Name = "LblContentDescription";
            LblContentDescription.Size = new System.Drawing.Size(78, 15);
            LblContentDescription.TabIndex = 20;
            LblContentDescription.Text = "Omschrijving";
            // 
            // TxtContentDescription
            // 
            TxtContentDescription.IsRequired = true;
            TxtContentDescription.Location = new System.Drawing.Point(241, 40);
            TxtContentDescription.Name = "TxtContentDescription";
            TxtContentDescription.Size = new System.Drawing.Size(273, 23);
            TxtContentDescription.TabIndex = 19;
            // 
            // LstContent
            // 
            LstContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstContent.FormattingEnabled = true;
            LstContent.IntegralHeight = false;
            LstContent.Location = new System.Drawing.Point(6, 22);
            LstContent.Name = "LstContent";
            LstContent.Size = new System.Drawing.Size(220, 147);
            LstContent.TabIndex = 10;
            // 
            // BtnContentAdd
            // 
            BtnContentAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentAdd.Location = new System.Drawing.Point(6, 175);
            BtnContentAdd.Name = "BtnContentAdd";
            BtnContentAdd.Size = new System.Drawing.Size(80, 23);
            BtnContentAdd.TabIndex = 15;
            BtnContentAdd.Text = "Toevoegen";
            BtnContentAdd.UseVisualStyleBackColor = true;
            // 
            // BtnContentRemove
            // 
            BtnContentRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentRemove.Location = new System.Drawing.Point(92, 175);
            BtnContentRemove.Name = "BtnContentRemove";
            BtnContentRemove.Size = new System.Drawing.Size(80, 23);
            BtnContentRemove.TabIndex = 16;
            BtnContentRemove.Text = "Verwijderen";
            BtnContentRemove.UseVisualStyleBackColor = true;
            // 
            // BtnContentDown
            // 
            BtnContentDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentDown.Image = Properties.Resources.down;
            BtnContentDown.Location = new System.Drawing.Point(204, 175);
            BtnContentDown.Name = "BtnContentDown";
            BtnContentDown.Size = new System.Drawing.Size(22, 23);
            BtnContentDown.TabIndex = 18;
            BtnContentDown.UseVisualStyleBackColor = true;
            // 
            // BtnContentUp
            // 
            BtnContentUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentUp.Image = Properties.Resources.up;
            BtnContentUp.Location = new System.Drawing.Point(180, 175);
            BtnContentUp.Name = "BtnContentUp";
            BtnContentUp.Size = new System.Drawing.Size(22, 23);
            BtnContentUp.TabIndex = 17;
            BtnContentUp.UseVisualStyleBackColor = true;
            // 
            // grpMetadata
            // 
            grpMetadata.Controls.Add(LblMetadataStartPosition);
            grpMetadata.Controls.Add(TxtMetadataStartPosition);
            grpMetadata.Controls.Add(LblMetadataAfterPhrase);
            grpMetadata.Controls.Add(TxtMetadataAfterPhrase);
            grpMetadata.Controls.Add(LblMetadataBeforePhrase);
            grpMetadata.Controls.Add(TxtMetadataBeforePhrase);
            grpMetadata.Controls.Add(LblMetadataDescription);
            grpMetadata.Controls.Add(TxtMetadataDescription);
            grpMetadata.Controls.Add(LstMetadata);
            grpMetadata.Controls.Add(BtnMetadataAdd);
            grpMetadata.Controls.Add(BtnMetadataRemove);
            grpMetadata.Controls.Add(BtnMetadataDown);
            grpMetadata.Controls.Add(BtnMetadataUp);
            grpMetadata.Location = new System.Drawing.Point(6, 110);
            grpMetadata.Name = "grpMetadata";
            grpMetadata.Size = new System.Drawing.Size(528, 206);
            grpMetadata.TabIndex = 19;
            grpMetadata.TabStop = false;
            grpMetadata.Text = "Metadata";
            // 
            // LblMetadataStartPosition
            // 
            LblMetadataStartPosition.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblMetadataStartPosition.AutoSize = true;
            LblMetadataStartPosition.Location = new System.Drawing.Point(241, 154);
            LblMetadataStartPosition.Name = "LblMetadataStartPosition";
            LblMetadataStartPosition.Size = new System.Drawing.Size(66, 15);
            LblMetadataStartPosition.TabIndex = 26;
            LblMetadataStartPosition.Text = "Startpositie";
            // 
            // TxtMetadataStartPosition
            // 
            TxtMetadataStartPosition.IsRequired = true;
            TxtMetadataStartPosition.Location = new System.Drawing.Point(241, 172);
            TxtMetadataStartPosition.Name = "TxtMetadataStartPosition";
            TxtMetadataStartPosition.Size = new System.Drawing.Size(44, 23);
            TxtMetadataStartPosition.TabIndex = 25;
            // 
            // LblMetadataAfterPhrase
            // 
            LblMetadataAfterPhrase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblMetadataAfterPhrase.AutoSize = true;
            LblMetadataAfterPhrase.Location = new System.Drawing.Point(241, 110);
            LblMetadataAfterPhrase.Name = "LblMetadataAfterPhrase";
            LblMetadataAfterPhrase.Size = new System.Drawing.Size(160, 15);
            LblMetadataAfterPhrase.TabIndex = 24;
            LblMetadataAfterPhrase.Text = "Tekst na de metadata waarde";
            // 
            // TxtMetadataAfterPhrase
            // 
            TxtMetadataAfterPhrase.IsRequired = true;
            TxtMetadataAfterPhrase.Location = new System.Drawing.Point(241, 128);
            TxtMetadataAfterPhrase.Name = "TxtMetadataAfterPhrase";
            TxtMetadataAfterPhrase.Size = new System.Drawing.Size(273, 23);
            TxtMetadataAfterPhrase.TabIndex = 23;
            // 
            // LblMetadataBeforePhrase
            // 
            LblMetadataBeforePhrase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblMetadataBeforePhrase.AutoSize = true;
            LblMetadataBeforePhrase.Location = new System.Drawing.Point(241, 66);
            LblMetadataBeforePhrase.Name = "LblMetadataBeforePhrase";
            LblMetadataBeforePhrase.Size = new System.Drawing.Size(171, 15);
            LblMetadataBeforePhrase.TabIndex = 22;
            LblMetadataBeforePhrase.Text = "Tekst voor de metadata waarde";
            // 
            // TxtMetadataBeforePhrase
            // 
            TxtMetadataBeforePhrase.IsRequired = true;
            TxtMetadataBeforePhrase.Location = new System.Drawing.Point(241, 84);
            TxtMetadataBeforePhrase.Name = "TxtMetadataBeforePhrase";
            TxtMetadataBeforePhrase.Size = new System.Drawing.Size(273, 23);
            TxtMetadataBeforePhrase.TabIndex = 21;
            // 
            // LblMetadataDescription
            // 
            LblMetadataDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblMetadataDescription.AutoSize = true;
            LblMetadataDescription.Location = new System.Drawing.Point(241, 22);
            LblMetadataDescription.Name = "LblMetadataDescription";
            LblMetadataDescription.Size = new System.Drawing.Size(78, 15);
            LblMetadataDescription.TabIndex = 20;
            LblMetadataDescription.Text = "Omschrijving";
            // 
            // TxtMetadataDescription
            // 
            TxtMetadataDescription.IsRequired = true;
            TxtMetadataDescription.Location = new System.Drawing.Point(241, 40);
            TxtMetadataDescription.Name = "TxtMetadataDescription";
            TxtMetadataDescription.Size = new System.Drawing.Size(273, 23);
            TxtMetadataDescription.TabIndex = 19;
            // 
            // LstMetadata
            // 
            LstMetadata.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstMetadata.FormattingEnabled = true;
            LstMetadata.IntegralHeight = false;
            LstMetadata.Location = new System.Drawing.Point(6, 22);
            LstMetadata.Name = "LstMetadata";
            LstMetadata.Size = new System.Drawing.Size(220, 149);
            LstMetadata.TabIndex = 10;
            // 
            // BtnMetadataAdd
            // 
            BtnMetadataAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataAdd.Location = new System.Drawing.Point(8, 177);
            BtnMetadataAdd.Name = "BtnMetadataAdd";
            BtnMetadataAdd.Size = new System.Drawing.Size(80, 23);
            BtnMetadataAdd.TabIndex = 15;
            BtnMetadataAdd.Text = "Toevoegen";
            BtnMetadataAdd.UseVisualStyleBackColor = true;
            // 
            // BtnMetadataRemove
            // 
            BtnMetadataRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataRemove.Location = new System.Drawing.Point(94, 177);
            BtnMetadataRemove.Name = "BtnMetadataRemove";
            BtnMetadataRemove.Size = new System.Drawing.Size(80, 23);
            BtnMetadataRemove.TabIndex = 16;
            BtnMetadataRemove.Text = "Verwijderen";
            BtnMetadataRemove.UseVisualStyleBackColor = true;
            // 
            // BtnMetadataDown
            // 
            BtnMetadataDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataDown.Image = Properties.Resources.down;
            BtnMetadataDown.Location = new System.Drawing.Point(204, 177);
            BtnMetadataDown.Name = "BtnMetadataDown";
            BtnMetadataDown.Size = new System.Drawing.Size(22, 23);
            BtnMetadataDown.TabIndex = 18;
            BtnMetadataDown.UseVisualStyleBackColor = true;
            // 
            // BtnMetadataUp
            // 
            BtnMetadataUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataUp.Image = Properties.Resources.up;
            BtnMetadataUp.Location = new System.Drawing.Point(180, 177);
            BtnMetadataUp.Name = "BtnMetadataUp";
            BtnMetadataUp.Size = new System.Drawing.Size(22, 23);
            BtnMetadataUp.TabIndex = 17;
            BtnMetadataUp.UseVisualStyleBackColor = true;
            // 
            // LblDateTimeFormat
            // 
            LblDateTimeFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblDateTimeFormat.AutoSize = true;
            LblDateTimeFormat.Location = new System.Drawing.Point(8, 63);
            LblDateTimeFormat.Name = "LblDateTimeFormat";
            LblDateTimeFormat.Size = new System.Drawing.Size(102, 15);
            LblDateTimeFormat.TabIndex = 9;
            LblDateTimeFormat.Text = "Datum tijd format";
            // 
            // LblDescription
            // 
            LblDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblDescription.AutoSize = true;
            LblDescription.Location = new System.Drawing.Point(8, 19);
            LblDescription.Name = "LblDescription";
            LblDescription.Size = new System.Drawing.Size(78, 15);
            LblDescription.TabIndex = 8;
            LblDescription.Text = "Omschrijving";
            // 
            // UserControlLogLayoutConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(GrpLayouts);
            Name = "UserControlLogLayoutConfig";
            Size = new System.Drawing.Size(977, 620);
            GrpLayouts.ResumeLayout(false);
            grpRuntime.ResumeLayout(false);
            grpRuntime.PerformLayout();
            GrpContent.ResumeLayout(false);
            GrpContent.PerformLayout();
            grpMetadata.ResumeLayout(false);
            grpMetadata.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox LstLayouts;
        private System.Windows.Forms.Button BtnLayoutAdd;
        private System.Windows.Forms.Button BtnLayoutRemove;
        private System.Windows.Forms.Button BtnLayoutUp;
        private System.Windows.Forms.Button BtnLayoutDown;
        private ValidatedTextBox TxtDescription;
        private ValidatedTextBox TxtDateTimeFormat;
        private System.Windows.Forms.GroupBox GrpLayouts;
        private System.Windows.Forms.GroupBox grpRuntime;
        private System.Windows.Forms.Label LblDateTimeFormat;
        private System.Windows.Forms.Label LblDescription;
        private System.Windows.Forms.Button BtnMetadataAdd;
        private System.Windows.Forms.Button BtnMetadataRemove;
        private System.Windows.Forms.Button BtnMetadataUp;
        private System.Windows.Forms.Button BtnMetadataDown;
        private System.Windows.Forms.ListBox LstMetadata;
        private System.Windows.Forms.GroupBox grpMetadata;
        private System.Windows.Forms.Label LblMetadataStartPosition;
        private ValidatedTextBox TxtMetadataStartPosition;
        private System.Windows.Forms.Label LblMetadataAfterPhrase;
        private ValidatedTextBox TxtMetadataAfterPhrase;
        private System.Windows.Forms.Label LblMetadataBeforePhrase;
        private ValidatedTextBox TxtMetadataBeforePhrase;
        private System.Windows.Forms.Label LblMetadataDescription;
        private ValidatedTextBox TxtMetadataDescription;
        private System.Windows.Forms.GroupBox GrpContent;
        private System.Windows.Forms.Label LblContentStartPosition;
        private ValidatedTextBox txtContentStartPosition;
        private System.Windows.Forms.Label LblLblContentAfterPhrase;
        private ValidatedTextBox TxtLblContentAfterPhrase;
        private System.Windows.Forms.Label LblContentBeforePhrase;
        private ValidatedTextBox TxtContentBeforePhrase;
        private System.Windows.Forms.Label LblContentDescription;
        private ValidatedTextBox TxtContentDescription;
        private System.Windows.Forms.ListBox LstContent;
        private System.Windows.Forms.Button BtnContentAdd;
        private System.Windows.Forms.Button BtnContentRemove;
        private System.Windows.Forms.Button BtnContentDown;
        private System.Windows.Forms.Button BtnContentUp;
    }
}
