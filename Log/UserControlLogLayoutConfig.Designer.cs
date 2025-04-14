using LogScraper.Extensions;

namespace LogScraper.Log
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLogLayoutConfig));
            LstLayouts = new System.Windows.Forms.ListBox();
            BtnLayoutAdd = new System.Windows.Forms.Button();
            BtnLayoutRemove = new System.Windows.Forms.Button();
            BtnLayoutUp = new System.Windows.Forms.Button();
            BtnLayoutDown = new System.Windows.Forms.Button();
            TxtDescription = new ValidatedTextBox();
            TxtDateTimeFormat = new ValidatedTextBox();
            GrpLayouts = new System.Windows.Forms.GroupBox();
            grpRuntime = new System.Windows.Forms.GroupBox();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            GrpContent = new System.Windows.Forms.GroupBox();
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
            pictureBox5 = new System.Windows.Forms.PictureBox();
            pictureBox4 = new System.Windows.Forms.PictureBox();
            pictureBox3 = new System.Windows.Forms.PictureBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
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
            toolTip = new System.Windows.Forms.ToolTip(components);
            pictureBox7 = new System.Windows.Forms.PictureBox();
            pictureBox6 = new System.Windows.Forms.PictureBox();
            pictureBox8 = new System.Windows.Forms.PictureBox();
            pictureBox9 = new System.Windows.Forms.PictureBox();
            pictureBox10 = new System.Windows.Forms.PictureBox();
            GrpLayouts.SuspendLayout();
            grpRuntime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            GrpContent.SuspendLayout();
            grpMetadata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            SuspendLayout();
            // 
            // LstLayouts
            // 
            LstLayouts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstLayouts.FormattingEnabled = true;
            LstLayouts.IntegralHeight = false;
            LstLayouts.Location = new System.Drawing.Point(6, 22);
            LstLayouts.Name = "LstLayouts";
            LstLayouts.Size = new System.Drawing.Size(232, 532);
            LstLayouts.TabIndex = 0;
            LstLayouts.SelectedIndexChanged += LstUrls_SelectedIndexChanged;
            // 
            // BtnLayoutAdd
            // 
            BtnLayoutAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnLayoutAdd.Location = new System.Drawing.Point(6, 560);
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
            BtnLayoutRemove.Location = new System.Drawing.Point(92, 560);
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
            BtnLayoutUp.Location = new System.Drawing.Point(192, 560);
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
            BtnLayoutDown.Location = new System.Drawing.Point(216, 560);
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
            TxtDateTimeFormat.IsRequired = true;
            TxtDateTimeFormat.Location = new System.Drawing.Point(8, 81);
            TxtDateTimeFormat.Name = "TxtDateTimeFormat";
            TxtDateTimeFormat.Size = new System.Drawing.Size(239, 23);
            TxtDateTimeFormat.TabIndex = 6;
            TxtDateTimeFormat.TextChanged += TxtUrl_TextChanged;
            // 
            // GrpLayouts
            // 
            GrpLayouts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            GrpLayouts.Controls.Add(pictureBox7);
            GrpLayouts.Controls.Add(grpRuntime);
            GrpLayouts.Controls.Add(LstLayouts);
            GrpLayouts.Controls.Add(BtnLayoutAdd);
            GrpLayouts.Controls.Add(BtnLayoutRemove);
            GrpLayouts.Controls.Add(BtnLayoutUp);
            GrpLayouts.Controls.Add(BtnLayoutDown);
            GrpLayouts.Location = new System.Drawing.Point(0, 3);
            GrpLayouts.Name = "GrpLayouts";
            GrpLayouts.Size = new System.Drawing.Size(977, 589);
            GrpLayouts.TabIndex = 8;
            GrpLayouts.TabStop = false;
            GrpLayouts.Text = "Log layouts";
            // 
            // grpRuntime
            // 
            grpRuntime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpRuntime.Controls.Add(pictureBox2);
            grpRuntime.Controls.Add(GrpContent);
            grpRuntime.Controls.Add(grpMetadata);
            grpRuntime.Controls.Add(LblDateTimeFormat);
            grpRuntime.Controls.Add(LblDescription);
            grpRuntime.Controls.Add(TxtDateTimeFormat);
            grpRuntime.Controls.Add(TxtDescription);
            grpRuntime.Location = new System.Drawing.Point(244, 16);
            grpRuntime.Name = "grpRuntime";
            grpRuntime.Size = new System.Drawing.Size(727, 538);
            grpRuntime.TabIndex = 14;
            grpRuntime.TabStop = false;
            grpRuntime.Text = "Log layout";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.help;
            pictureBox2.Location = new System.Drawing.Point(231, 62);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(16, 16);
            pictureBox2.TabIndex = 27;
            pictureBox2.TabStop = false;
            toolTip.SetToolTip(pictureBox2, resources.GetString("pictureBox2.ToolTip"));
            // 
            // GrpContent
            // 
            GrpContent.Controls.Add(pictureBox8);
            GrpContent.Controls.Add(pictureBox9);
            GrpContent.Controls.Add(pictureBox10);
            GrpContent.Controls.Add(pictureBox6);
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
            grpMetadata.Controls.Add(pictureBox5);
            grpMetadata.Controls.Add(pictureBox4);
            grpMetadata.Controls.Add(pictureBox3);
            grpMetadata.Controls.Add(pictureBox1);
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
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.help;
            pictureBox5.Location = new System.Drawing.Point(498, 109);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new System.Drawing.Size(16, 16);
            pictureBox5.TabIndex = 29;
            pictureBox5.TabStop = false;
            toolTip.SetToolTip(pictureBox5, "Geef de tekst die net na de metadata waarde staat.\r\nBijvoorbeeld ', indien in de logregel staat metadataX='waardeY',");
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.help;
            pictureBox4.Location = new System.Drawing.Point(498, 65);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new System.Drawing.Size(16, 16);
            pictureBox4.TabIndex = 28;
            pictureBox4.TabStop = false;
            toolTip.SetToolTip(pictureBox4, "Geef de tekst die net voor de metadata waarde staat, dit bevat meestal de omschrijving van de metadata.\r\nBijvoorbeeld metadataX=' indien in de logregel staat metadataX='waardeY',");
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.help;
            pictureBox3.Location = new System.Drawing.Point(498, 21);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new System.Drawing.Size(16, 16);
            pictureBox3.TabIndex = 27;
            pictureBox3.TabStop = false;
            toolTip.SetToolTip(pictureBox3, "De omschrijving wordt getoond in de logscraper ");
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.help;
            pictureBox1.Location = new System.Drawing.Point(64, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(16, 16);
            pictureBox1.TabIndex = 21;
            pictureBox1.TabStop = false;
            toolTip.SetToolTip(pictureBox1, resources.GetString("pictureBox1.ToolTip"));
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
            // pictureBox7
            // 
            pictureBox7.Image = Properties.Resources.help;
            pictureBox7.Location = new System.Drawing.Point(79, 0);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new System.Drawing.Size(16, 16);
            pictureBox7.TabIndex = 22;
            pictureBox7.TabStop = false;
            toolTip.SetToolTip(pictureBox7, resources.GetString("pictureBox7.ToolTip"));
            // 
            // pictureBox6
            // 
            pictureBox6.Image = Properties.Resources.help;
            pictureBox6.Location = new System.Drawing.Point(167, 0);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new System.Drawing.Size(16, 16);
            pictureBox6.TabIndex = 27;
            pictureBox6.TabStop = false;
            toolTip.SetToolTip(pictureBox6, resources.GetString("pictureBox6.ToolTip"));
            // 
            // pictureBox8
            // 
            pictureBox8.Image = Properties.Resources.help;
            pictureBox8.Location = new System.Drawing.Point(498, 109);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new System.Drawing.Size(16, 16);
            pictureBox8.TabIndex = 32;
            pictureBox8.TabStop = false;
            toolTip.SetToolTip(pictureBox8, "Geef de tekst die net na de content waarde staat.\r\nBijvoorbeeld ! indien in de logregel staat API call: X!\r\n\r\nLaat dit veld leeg indien er geen tekst na de content waarde staat\r\n");
            // 
            // pictureBox9
            // 
            pictureBox9.Image = Properties.Resources.help;
            pictureBox9.Location = new System.Drawing.Point(498, 65);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new System.Drawing.Size(16, 16);
            pictureBox9.TabIndex = 31;
            pictureBox9.TabStop = false;
            toolTip.SetToolTip(pictureBox9, "Geef de tekst die net voor de content waarde staat\r\nBijvoorbeeld API call: indien in de logregel staat API call: X");
            // 
            // pictureBox10
            // 
            pictureBox10.Image = Properties.Resources.help;
            pictureBox10.Location = new System.Drawing.Point(498, 21);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new System.Drawing.Size(16, 16);
            pictureBox10.TabIndex = 30;
            pictureBox10.TabStop = false;
            toolTip.SetToolTip(pictureBox10, "De omschrijving wordt getoond in de logscraper ");
            // 
            // UserControlLogLayoutConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(GrpLayouts);
            Name = "UserControlLogLayoutConfig";
            Size = new System.Drawing.Size(977, 595);
            GrpLayouts.ResumeLayout(false);
            grpRuntime.ResumeLayout(false);
            grpRuntime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            GrpContent.ResumeLayout(false);
            GrpContent.PerformLayout();
            grpMetadata.ResumeLayout(false);
            grpMetadata.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
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
        private System.Windows.Forms.Label LblMetadataAfterPhrase;
        private ValidatedTextBox TxtMetadataAfterPhrase;
        private System.Windows.Forms.Label LblMetadataBeforePhrase;
        private ValidatedTextBox TxtMetadataBeforePhrase;
        private System.Windows.Forms.Label LblMetadataDescription;
        private ValidatedTextBox TxtMetadataDescription;
        private System.Windows.Forms.GroupBox GrpContent;
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
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.PictureBox pictureBox10;
    }
}
