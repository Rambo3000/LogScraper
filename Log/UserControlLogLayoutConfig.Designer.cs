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
            pictureBox7 = new System.Windows.Forms.PictureBox();
            grpRuntime = new System.Windows.Forms.GroupBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            pictureBox13 = new System.Windows.Forms.PictureBox();
            pictureBox11 = new System.Windows.Forms.PictureBox();
            pictureBox12 = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            TxtMetadataEnd = new ValidatedTextBox();
            label2 = new System.Windows.Forms.Label();
            TxtMetadataBegin = new ValidatedTextBox();
            TabControl = new System.Windows.Forms.TabControl();
            tabPageMetadata = new System.Windows.Forms.TabPage();
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
            tabPageContentFilters = new System.Windows.Forms.TabPage();
            GrpContent = new System.Windows.Forms.GroupBox();
            pictureBox8 = new System.Windows.Forms.PictureBox();
            pictureBox9 = new System.Windows.Forms.PictureBox();
            pictureBox10 = new System.Windows.Forms.PictureBox();
            pictureBox6 = new System.Windows.Forms.PictureBox();
            LblLblContentAfterPhrase = new System.Windows.Forms.Label();
            TxtContentAfterPhrase = new ValidatedTextBox();
            LblContentBeforePhrase = new System.Windows.Forms.Label();
            TxtContentBeforePhrase = new ValidatedTextBox();
            LblContentDescription = new System.Windows.Forms.Label();
            TxtContentDescription = new ValidatedTextBox();
            LstContent = new System.Windows.Forms.ListBox();
            BtnContentAdd = new System.Windows.Forms.Button();
            BtnContentRemove = new System.Windows.Forms.Button();
            BtnContentDown = new System.Windows.Forms.Button();
            BtnContentUp = new System.Windows.Forms.Button();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            LblDateTimeFormat = new System.Windows.Forms.Label();
            LblDescription = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            GrpLayouts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            grpRuntime.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).BeginInit();
            TabControl.SuspendLayout();
            tabPageMetadata.SuspendLayout();
            grpMetadata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPageContentFilters.SuspendLayout();
            GrpContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
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
            LstLayouts.SelectedIndexChanged += LstLogLayouts_SelectedIndexChanged;
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
            TxtDescription.IsWhiteSpaceAllowed = false;
            TxtDescription.Location = new System.Drawing.Point(8, 37);
            TxtDescription.Name = "TxtDescription";
            TxtDescription.Size = new System.Drawing.Size(279, 23);
            TxtDescription.TabIndex = 5;
            TxtDescription.TextChanged += TxtDescription_TextChanged;
            // 
            // TxtDateTimeFormat
            // 
            TxtDateTimeFormat.IsRequired = true;
            TxtDateTimeFormat.IsWhiteSpaceAllowed = false;
            TxtDateTimeFormat.Location = new System.Drawing.Point(8, 81);
            TxtDateTimeFormat.Name = "TxtDateTimeFormat";
            TxtDateTimeFormat.Size = new System.Drawing.Size(279, 23);
            TxtDateTimeFormat.TabIndex = 6;
            TxtDateTimeFormat.TextChanged += TxtDateTimeFormat_TextChanged;
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
            // grpRuntime
            // 
            grpRuntime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpRuntime.Controls.Add(groupBox1);
            grpRuntime.Controls.Add(TabControl);
            grpRuntime.Controls.Add(pictureBox2);
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
            // groupBox1
            // 
            groupBox1.Controls.Add(pictureBox13);
            groupBox1.Controls.Add(pictureBox11);
            groupBox1.Controls.Add(pictureBox12);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(TxtMetadataEnd);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(TxtMetadataBegin);
            groupBox1.Location = new System.Drawing.Point(8, 110);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(288, 115);
            groupBox1.TabIndex = 29;
            groupBox1.TabStop = false;
            groupBox1.Text = "Metadata begin en eind";
            // 
            // pictureBox13
            // 
            pictureBox13.Image = Properties.Resources.help;
            pictureBox13.Location = new System.Drawing.Point(263, 18);
            pictureBox13.Name = "pictureBox13";
            pictureBox13.Size = new System.Drawing.Size(16, 16);
            pictureBox13.TabIndex = 33;
            pictureBox13.TabStop = false;
            toolTip.SetToolTip(pictureBox13, resources.GetString("pictureBox13.ToolTip"));
            // 
            // pictureBox11
            // 
            pictureBox11.Image = Properties.Resources.help;
            pictureBox11.Location = new System.Drawing.Point(263, 62);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new System.Drawing.Size(16, 16);
            pictureBox11.TabIndex = 35;
            pictureBox11.TabStop = false;
            toolTip.SetToolTip(pictureBox11, "Geef de tekst die het einde van de metadata identficeerd.\r\n\r\nBijvoorbeeld 2025-01-01T01:02:03.123 INFO metadataA=WaardeX, MetadataB=WaardeY - Voorbeeld logregel tekst\r\nGebruik in dit voorbeeld: \" - \"");
            // 
            // pictureBox12
            // 
            pictureBox12.Image = Properties.Resources.help;
            pictureBox12.Location = new System.Drawing.Point(140, 0);
            pictureBox12.Name = "pictureBox12";
            pictureBox12.Size = new System.Drawing.Size(16, 16);
            pictureBox12.TabIndex = 34;
            pictureBox12.TabStop = false;
            toolTip.SetToolTip(pictureBox12, "De begin en eind criteria van de metadata, deze wordt onder andere gebruik om de metadata te verwijderen uit de logregel om een meer leesbare logregel te tonen");
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 63);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(171, 15);
            label1.TabIndex = 33;
            label1.Text = "Tekst welke de metadata afsluit";
            // 
            // TxtMetadataEnd
            // 
            TxtMetadataEnd.IsRequired = true;
            TxtMetadataEnd.IsWhiteSpaceAllowed = true;
            TxtMetadataEnd.Location = new System.Drawing.Point(6, 81);
            TxtMetadataEnd.Name = "TxtMetadataEnd";
            TxtMetadataEnd.Size = new System.Drawing.Size(273, 23);
            TxtMetadataEnd.TabIndex = 32;
            TxtMetadataEnd.TextChanged += TxtMetadataEnd_TextChanged;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 19);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(130, 15);
            label2.TabIndex = 31;
            label2.Text = "Tekst voor de metadata";
            // 
            // TxtMetadataBegin
            // 
            TxtMetadataBegin.IsRequired = false;
            TxtMetadataBegin.IsWhiteSpaceAllowed = true;
            TxtMetadataBegin.Location = new System.Drawing.Point(6, 37);
            TxtMetadataBegin.Name = "TxtMetadataBegin";
            TxtMetadataBegin.Size = new System.Drawing.Size(273, 23);
            TxtMetadataBegin.TabIndex = 30;
            TxtMetadataBegin.TextChanged += TxtMetadataBegin_TextChanged;
            // 
            // TabControl
            // 
            TabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TabControl.Controls.Add(tabPageMetadata);
            TabControl.Controls.Add(tabPageContentFilters);
            TabControl.Location = new System.Drawing.Point(8, 231);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(713, 301);
            TabControl.TabIndex = 28;
            // 
            // tabPageMetadata
            // 
            tabPageMetadata.Controls.Add(grpMetadata);
            tabPageMetadata.Location = new System.Drawing.Point(4, 24);
            tabPageMetadata.Name = "tabPageMetadata";
            tabPageMetadata.Padding = new System.Windows.Forms.Padding(3);
            tabPageMetadata.Size = new System.Drawing.Size(705, 273);
            tabPageMetadata.TabIndex = 0;
            tabPageMetadata.Text = "Metadata";
            tabPageMetadata.UseVisualStyleBackColor = true;
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
            grpMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            grpMetadata.Location = new System.Drawing.Point(3, 3);
            grpMetadata.Name = "grpMetadata";
            grpMetadata.Size = new System.Drawing.Size(699, 267);
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
            TxtMetadataAfterPhrase.IsWhiteSpaceAllowed = true;
            TxtMetadataAfterPhrase.Location = new System.Drawing.Point(241, 128);
            TxtMetadataAfterPhrase.Name = "TxtMetadataAfterPhrase";
            TxtMetadataAfterPhrase.Size = new System.Drawing.Size(273, 23);
            TxtMetadataAfterPhrase.TabIndex = 23;
            TxtMetadataAfterPhrase.TextChanged += TxtMetadataAfterPhrase_TextChanged;
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
            TxtMetadataBeforePhrase.IsWhiteSpaceAllowed = true;
            TxtMetadataBeforePhrase.Location = new System.Drawing.Point(241, 84);
            TxtMetadataBeforePhrase.Name = "TxtMetadataBeforePhrase";
            TxtMetadataBeforePhrase.Size = new System.Drawing.Size(273, 23);
            TxtMetadataBeforePhrase.TabIndex = 21;
            TxtMetadataBeforePhrase.TextChanged += TxtMetadataBeforePhrase_TextChanged;
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
            TxtMetadataDescription.IsWhiteSpaceAllowed = false;
            TxtMetadataDescription.Location = new System.Drawing.Point(241, 40);
            TxtMetadataDescription.Name = "TxtMetadataDescription";
            TxtMetadataDescription.Size = new System.Drawing.Size(273, 23);
            TxtMetadataDescription.TabIndex = 19;
            TxtMetadataDescription.TextChanged += TxtMetadataDescription_TextChanged;
            // 
            // LstMetadata
            // 
            LstMetadata.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstMetadata.FormattingEnabled = true;
            LstMetadata.IntegralHeight = false;
            LstMetadata.Location = new System.Drawing.Point(6, 22);
            LstMetadata.Name = "LstMetadata";
            LstMetadata.Size = new System.Drawing.Size(220, 210);
            LstMetadata.TabIndex = 10;
            LstMetadata.SelectedIndexChanged += LstMetadata_SelectedIndexChanged;
            // 
            // BtnMetadataAdd
            // 
            BtnMetadataAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataAdd.Location = new System.Drawing.Point(6, 238);
            BtnMetadataAdd.Name = "BtnMetadataAdd";
            BtnMetadataAdd.Size = new System.Drawing.Size(80, 23);
            BtnMetadataAdd.TabIndex = 15;
            BtnMetadataAdd.Text = "Toevoegen";
            BtnMetadataAdd.UseVisualStyleBackColor = true;
            BtnMetadataAdd.Click += BtnMetadataAdd_Click;
            // 
            // BtnMetadataRemove
            // 
            BtnMetadataRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataRemove.Location = new System.Drawing.Point(90, 238);
            BtnMetadataRemove.Name = "BtnMetadataRemove";
            BtnMetadataRemove.Size = new System.Drawing.Size(80, 23);
            BtnMetadataRemove.TabIndex = 16;
            BtnMetadataRemove.Text = "Verwijderen";
            BtnMetadataRemove.UseVisualStyleBackColor = true;
            BtnMetadataRemove.Click += BtnMetadataRemove_Click;
            // 
            // BtnMetadataDown
            // 
            BtnMetadataDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataDown.Image = Properties.Resources.down;
            BtnMetadataDown.Location = new System.Drawing.Point(204, 238);
            BtnMetadataDown.Name = "BtnMetadataDown";
            BtnMetadataDown.Size = new System.Drawing.Size(22, 23);
            BtnMetadataDown.TabIndex = 18;
            BtnMetadataDown.UseVisualStyleBackColor = true;
            BtnMetadataDown.Click += BtnMetadataDown_Click;
            // 
            // BtnMetadataUp
            // 
            BtnMetadataUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnMetadataUp.Image = Properties.Resources.up;
            BtnMetadataUp.Location = new System.Drawing.Point(180, 238);
            BtnMetadataUp.Name = "BtnMetadataUp";
            BtnMetadataUp.Size = new System.Drawing.Size(22, 23);
            BtnMetadataUp.TabIndex = 17;
            BtnMetadataUp.UseVisualStyleBackColor = true;
            BtnMetadataUp.Click += BtnMetadataUp_Click;
            // 
            // tabPageContentFilters
            // 
            tabPageContentFilters.Controls.Add(GrpContent);
            tabPageContentFilters.Location = new System.Drawing.Point(4, 24);
            tabPageContentFilters.Name = "tabPageContentFilters";
            tabPageContentFilters.Padding = new System.Windows.Forms.Padding(3);
            tabPageContentFilters.Size = new System.Drawing.Size(705, 273);
            tabPageContentFilters.TabIndex = 1;
            tabPageContentFilters.Text = "Content begin en eind filters";
            tabPageContentFilters.UseVisualStyleBackColor = true;
            // 
            // GrpContent
            // 
            GrpContent.Controls.Add(pictureBox8);
            GrpContent.Controls.Add(pictureBox9);
            GrpContent.Controls.Add(pictureBox10);
            GrpContent.Controls.Add(pictureBox6);
            GrpContent.Controls.Add(LblLblContentAfterPhrase);
            GrpContent.Controls.Add(TxtContentAfterPhrase);
            GrpContent.Controls.Add(LblContentBeforePhrase);
            GrpContent.Controls.Add(TxtContentBeforePhrase);
            GrpContent.Controls.Add(LblContentDescription);
            GrpContent.Controls.Add(TxtContentDescription);
            GrpContent.Controls.Add(LstContent);
            GrpContent.Controls.Add(BtnContentAdd);
            GrpContent.Controls.Add(BtnContentRemove);
            GrpContent.Controls.Add(BtnContentDown);
            GrpContent.Controls.Add(BtnContentUp);
            GrpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            GrpContent.Location = new System.Drawing.Point(3, 3);
            GrpContent.Name = "GrpContent";
            GrpContent.Size = new System.Drawing.Size(699, 267);
            GrpContent.TabIndex = 20;
            GrpContent.TabStop = false;
            GrpContent.Text = "Content begin en eind filters";
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
            // LblLblContentAfterPhrase
            // 
            LblLblContentAfterPhrase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblLblContentAfterPhrase.AutoSize = true;
            LblLblContentAfterPhrase.Location = new System.Drawing.Point(241, 110);
            LblLblContentAfterPhrase.Name = "LblLblContentAfterPhrase";
            LblLblContentAfterPhrase.Size = new System.Drawing.Size(107, 15);
            LblLblContentAfterPhrase.TabIndex = 24;
            LblLblContentAfterPhrase.Text = "Tekst na de waarde";
            // 
            // TxtContentAfterPhrase
            // 
            TxtContentAfterPhrase.IsRequired = false;
            TxtContentAfterPhrase.IsWhiteSpaceAllowed = true;
            TxtContentAfterPhrase.Location = new System.Drawing.Point(241, 128);
            TxtContentAfterPhrase.Name = "TxtContentAfterPhrase";
            TxtContentAfterPhrase.Size = new System.Drawing.Size(273, 23);
            TxtContentAfterPhrase.TabIndex = 23;
            TxtContentAfterPhrase.TextChanged += TxtContentAfterPhrase_TextChanged;
            // 
            // LblContentBeforePhrase
            // 
            LblContentBeforePhrase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LblContentBeforePhrase.AutoSize = true;
            LblContentBeforePhrase.Location = new System.Drawing.Point(241, 66);
            LblContentBeforePhrase.Name = "LblContentBeforePhrase";
            LblContentBeforePhrase.Size = new System.Drawing.Size(118, 15);
            LblContentBeforePhrase.TabIndex = 22;
            LblContentBeforePhrase.Text = "Tekst voor de waarde";
            // 
            // TxtContentBeforePhrase
            // 
            TxtContentBeforePhrase.IsRequired = true;
            TxtContentBeforePhrase.IsWhiteSpaceAllowed = true;
            TxtContentBeforePhrase.Location = new System.Drawing.Point(241, 84);
            TxtContentBeforePhrase.Name = "TxtContentBeforePhrase";
            TxtContentBeforePhrase.Size = new System.Drawing.Size(273, 23);
            TxtContentBeforePhrase.TabIndex = 21;
            TxtContentBeforePhrase.TextChanged += TxtContentBeforePhrase_TextChanged;
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
            TxtContentDescription.IsWhiteSpaceAllowed = false;
            TxtContentDescription.Location = new System.Drawing.Point(241, 40);
            TxtContentDescription.Name = "TxtContentDescription";
            TxtContentDescription.Size = new System.Drawing.Size(273, 23);
            TxtContentDescription.TabIndex = 19;
            TxtContentDescription.TextChanged += TxtContentDescription_TextChanged;
            // 
            // LstContent
            // 
            LstContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            LstContent.FormattingEnabled = true;
            LstContent.IntegralHeight = false;
            LstContent.Location = new System.Drawing.Point(6, 22);
            LstContent.Name = "LstContent";
            LstContent.Size = new System.Drawing.Size(220, 210);
            LstContent.TabIndex = 10;
            LstContent.SelectedIndexChanged += LstContent_SelectedIndexChanged;
            // 
            // BtnContentAdd
            // 
            BtnContentAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentAdd.Location = new System.Drawing.Point(6, 238);
            BtnContentAdd.Name = "BtnContentAdd";
            BtnContentAdd.Size = new System.Drawing.Size(80, 23);
            BtnContentAdd.TabIndex = 15;
            BtnContentAdd.Text = "Toevoegen";
            BtnContentAdd.UseVisualStyleBackColor = true;
            BtnContentAdd.Click += BtnContentAdd_Click;
            // 
            // BtnContentRemove
            // 
            BtnContentRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentRemove.Location = new System.Drawing.Point(90, 238);
            BtnContentRemove.Name = "BtnContentRemove";
            BtnContentRemove.Size = new System.Drawing.Size(80, 23);
            BtnContentRemove.TabIndex = 16;
            BtnContentRemove.Text = "Verwijderen";
            BtnContentRemove.UseVisualStyleBackColor = true;
            BtnContentRemove.Click += BtnContentRemove_Click;
            // 
            // BtnContentDown
            // 
            BtnContentDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentDown.Image = Properties.Resources.down;
            BtnContentDown.Location = new System.Drawing.Point(204, 238);
            BtnContentDown.Name = "BtnContentDown";
            BtnContentDown.Size = new System.Drawing.Size(22, 23);
            BtnContentDown.TabIndex = 18;
            BtnContentDown.UseVisualStyleBackColor = true;
            BtnContentDown.Click += BtnContentDown_Click;
            // 
            // BtnContentUp
            // 
            BtnContentUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BtnContentUp.Image = Properties.Resources.up;
            BtnContentUp.Location = new System.Drawing.Point(180, 238);
            BtnContentUp.Name = "BtnContentUp";
            BtnContentUp.Size = new System.Drawing.Size(22, 23);
            BtnContentUp.TabIndex = 17;
            BtnContentUp.UseVisualStyleBackColor = true;
            BtnContentUp.Click += BtnContentUp_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.help;
            pictureBox2.Location = new System.Drawing.Point(271, 62);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(16, 16);
            pictureBox2.TabIndex = 27;
            pictureBox2.TabStop = false;
            toolTip.SetToolTip(pictureBox2, resources.GetString("pictureBox2.ToolTip"));
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
            Size = new System.Drawing.Size(977, 595);
            GrpLayouts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            grpRuntime.ResumeLayout(false);
            grpRuntime.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).EndInit();
            TabControl.ResumeLayout(false);
            tabPageMetadata.ResumeLayout(false);
            grpMetadata.ResumeLayout(false);
            grpMetadata.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPageContentFilters.ResumeLayout(false);
            GrpContent.ResumeLayout(false);
            GrpContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
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
        private ValidatedTextBox TxtContentAfterPhrase;
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
        private System.Windows.Forms.TabPage tabPageMetadata;
        private System.Windows.Forms.TabPage tabPageContentFilters;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.Label label1;
        private ValidatedTextBox TxtMetadataEnd;
        private System.Windows.Forms.Label label2;
        private ValidatedTextBox TxtMetadataBegin;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.TabControl TabControl;
    }
}
