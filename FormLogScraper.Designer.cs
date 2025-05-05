namespace LogScraper
{
    partial class FormLogScraper
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogScraper));
            BtnRecord = new System.Windows.Forms.Button();
            lblLogEntriesTotalValue = new System.Windows.Forms.Label();
            lblNumberOfLogEntriesFiltered = new System.Windows.Forms.Label();
            lblLogEntriesFiltered = new System.Windows.Forms.Label();
            LbllogEntriesTotal = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            userControlMemoryUsage1 = new UserControlMemoryUsage();
            BtnConfig = new System.Windows.Forms.Button();
            btnOpenWithEditor = new System.Windows.Forms.Button();
            BtnFormRecord = new System.Windows.Forms.Button();
            BtnErase = new System.Windows.Forms.Button();
            BtnStop = new System.Windows.Forms.Button();
            lblNumberOfLogEntriesFilteredWithError = new System.Windows.Forms.Label();
            BtnRecordWithTimer = new System.Windows.Forms.Button();
            lblLogEntriesFilteredWithError = new System.Windows.Forms.Label();
            lblLogProvider = new System.Windows.Forms.Label();
            cboLogProvider = new System.Windows.Forms.ComboBox();
            usrRuntime = new LogScraper.LogProviders.Runtime.UserControlRuntimeLogProvider();
            usrKubernetes = new LogScraper.LogProviders.Kubernetes.UserControlKubernetesLogProvider();
            cboLogLayout = new System.Windows.Forms.ComboBox();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            groupBox2 = new System.Windows.Forms.GroupBox();
            lblEndFilterEnabled = new System.Windows.Forms.Label();
            lblBeginFilterEnabled = new System.Windows.Forms.Label();
            chkShowAllLogEntries = new System.Windows.Forms.CheckBox();
            lblNumberOfLogEntriesShown = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            TxtErrorMessage = new System.Windows.Forms.TextBox();
            txtLogEntries = new System.Windows.Forms.RichTextBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            usrSearch = new UserControlSearch();
            groupBox6 = new System.Windows.Forms.GroupBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            UsrLogContentBegin = new UserControlBeginEndFilter();
            tabPage2 = new System.Windows.Forms.TabPage();
            UsrLogContentEnd = new UserControlBeginEndFilter();
            groupBox1 = new System.Windows.Forms.GroupBox();
            tabControl2 = new System.Windows.Forms.TabControl();
            tabPage3 = new System.Windows.Forms.TabPage();
            UsrMetadataFilterOverview = new LogScraper.Log.Metadata.UserControlMetadataFilterOverview();
            tabPage4 = new System.Windows.Forms.TabPage();
            UsrControlMetadataFormating = new LogScraper.Log.Metadata.UserControlMetadataFormatting();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            GrpLogProvidersSettings = new System.Windows.Forms.GroupBox();
            usrFileLogProvider = new LogScraper.LogProviders.File.UserControlFileLogProvider();
            GrpSourceAndLayout = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            ToolTip = new System.Windows.Forms.ToolTip(components);
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabControl2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            GrpLogProvidersSettings.SuspendLayout();
            GrpSourceAndLayout.SuspendLayout();
            SuspendLayout();
            // 
            // BtnRecord
            // 
            BtnRecord.Image = (System.Drawing.Image)resources.GetObject("BtnRecord.Image");
            BtnRecord.Location = new System.Drawing.Point(53, 22);
            BtnRecord.Name = "BtnRecord";
            BtnRecord.Size = new System.Drawing.Size(52, 40);
            BtnRecord.TabIndex = 0;
            ToolTip.SetToolTip(BtnRecord, "Lees log eenmalig uit");
            BtnRecord.UseVisualStyleBackColor = true;
            BtnRecord.Click += BtnRecord_Click;
            // 
            // lblLogEntriesTotalValue
            // 
            lblLogEntriesTotalValue.Location = new System.Drawing.Point(119, 71);
            lblLogEntriesTotalValue.Name = "lblLogEntriesTotalValue";
            lblLogEntriesTotalValue.Size = new System.Drawing.Size(63, 15);
            lblLogEntriesTotalValue.TabIndex = 7;
            lblLogEntriesTotalValue.Text = "0";
            lblLogEntriesTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNumberOfLogEntriesFiltered
            // 
            lblNumberOfLogEntriesFiltered.Location = new System.Drawing.Point(119, 93);
            lblNumberOfLogEntriesFiltered.Name = "lblNumberOfLogEntriesFiltered";
            lblNumberOfLogEntriesFiltered.Size = new System.Drawing.Size(63, 15);
            lblNumberOfLogEntriesFiltered.TabIndex = 17;
            lblNumberOfLogEntriesFiltered.Text = "0";
            lblNumberOfLogEntriesFiltered.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLogEntriesFiltered
            // 
            lblLogEntriesFiltered.AutoSize = true;
            lblLogEntriesFiltered.Location = new System.Drawing.Point(10, 93);
            lblLogEntriesFiltered.Name = "lblLogEntriesFiltered";
            lblLogEntriesFiltered.Size = new System.Drawing.Size(88, 15);
            lblLogEntriesFiltered.TabIndex = 16;
            lblLogEntriesFiltered.Text = "Regels gefilterd";
            // 
            // LbllogEntriesTotal
            // 
            LbllogEntriesTotal.AutoSize = true;
            LbllogEntriesTotal.Location = new System.Drawing.Point(10, 71);
            LbllogEntriesTotal.Name = "LbllogEntriesTotal";
            LbllogEntriesTotal.Size = new System.Drawing.Size(100, 15);
            LbllogEntriesTotal.TabIndex = 10;
            LbllogEntriesTotal.Text = "Regels opgehaald";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(userControlMemoryUsage1);
            groupBox3.Controls.Add(BtnConfig);
            groupBox3.Controls.Add(btnOpenWithEditor);
            groupBox3.Controls.Add(BtnFormRecord);
            groupBox3.Controls.Add(BtnRecord);
            groupBox3.Controls.Add(BtnErase);
            groupBox3.Controls.Add(LbllogEntriesTotal);
            groupBox3.Controls.Add(lblLogEntriesFiltered);
            groupBox3.Controls.Add(BtnStop);
            groupBox3.Controls.Add(lblNumberOfLogEntriesFilteredWithError);
            groupBox3.Controls.Add(BtnRecordWithTimer);
            groupBox3.Controls.Add(lblLogEntriesTotalValue);
            groupBox3.Controls.Add(lblLogEntriesFilteredWithError);
            groupBox3.Controls.Add(lblNumberOfLogEntriesFiltered);
            groupBox3.Location = new System.Drawing.Point(5, 5);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(257, 147);
            groupBox3.TabIndex = 19;
            groupBox3.TabStop = false;
            groupBox3.Tag = "test";
            groupBox3.Text = "Uitlezen log";
            // 
            // userControlMemoryUsage1
            // 
            userControlMemoryUsage1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            userControlMemoryUsage1.AutoSize = true;
            userControlMemoryUsage1.Location = new System.Drawing.Point(206, 100);
            userControlMemoryUsage1.Name = "userControlMemoryUsage1";
            userControlMemoryUsage1.Size = new System.Drawing.Size(46, 17);
            userControlMemoryUsage1.TabIndex = 24;
            // 
            // BtnConfig
            // 
            BtnConfig.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BtnConfig.Image = (System.Drawing.Image)resources.GetObject("BtnConfig.Image");
            BtnConfig.Location = new System.Drawing.Point(226, 117);
            BtnConfig.Name = "BtnConfig";
            BtnConfig.Size = new System.Drawing.Size(24, 24);
            BtnConfig.TabIndex = 23;
            BtnConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnConfig, "Instellingen");
            BtnConfig.UseVisualStyleBackColor = true;
            BtnConfig.Click += BtnConfig_Click;
            // 
            // btnOpenWithEditor
            // 
            btnOpenWithEditor.Image = (System.Drawing.Image)resources.GetObject("btnOpenWithEditor.Image");
            btnOpenWithEditor.Location = new System.Drawing.Point(166, 22);
            btnOpenWithEditor.Name = "btnOpenWithEditor";
            btnOpenWithEditor.Size = new System.Drawing.Size(40, 40);
            btnOpenWithEditor.TabIndex = 11;
            ToolTip.SetToolTip(btnOpenWithEditor, "Open in externe editor");
            btnOpenWithEditor.UseVisualStyleBackColor = true;
            btnOpenWithEditor.Click += BtnOpenWithEditor_Click;
            // 
            // BtnFormRecord
            // 
            BtnFormRecord.Image = (System.Drawing.Image)resources.GetObject("BtnFormRecord.Image");
            BtnFormRecord.Location = new System.Drawing.Point(7, 21);
            BtnFormRecord.Name = "BtnFormRecord";
            BtnFormRecord.Size = new System.Drawing.Size(40, 40);
            BtnFormRecord.TabIndex = 11;
            BtnFormRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            ToolTip.SetToolTip(BtnFormRecord, "Open mini controls en start met uitlezen [CTRL-R]");
            BtnFormRecord.UseVisualStyleBackColor = true;
            BtnFormRecord.Click += BtnFormRecord_Click;
            // 
            // BtnErase
            // 
            BtnErase.Image = (System.Drawing.Image)resources.GetObject("BtnErase.Image");
            BtnErase.Location = new System.Drawing.Point(212, 22);
            BtnErase.Name = "BtnErase";
            BtnErase.Size = new System.Drawing.Size(40, 40);
            BtnErase.TabIndex = 11;
            ToolTip.SetToolTip(BtnErase, "Wis het log");
            BtnErase.UseVisualStyleBackColor = true;
            BtnErase.Click += BtnErase_Click;
            // 
            // BtnStop
            // 
            BtnStop.Image = (System.Drawing.Image)resources.GetObject("BtnStop.Image");
            BtnStop.Location = new System.Drawing.Point(53, 22);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new System.Drawing.Size(52, 40);
            BtnStop.TabIndex = 17;
            ToolTip.SetToolTip(BtnStop, "Stop [CTRL-S]");
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // lblNumberOfLogEntriesFilteredWithError
            // 
            lblNumberOfLogEntriesFilteredWithError.Location = new System.Drawing.Point(119, 114);
            lblNumberOfLogEntriesFilteredWithError.Name = "lblNumberOfLogEntriesFilteredWithError";
            lblNumberOfLogEntriesFilteredWithError.Size = new System.Drawing.Size(63, 15);
            lblNumberOfLogEntriesFilteredWithError.TabIndex = 19;
            lblNumberOfLogEntriesFilteredWithError.Text = "0";
            lblNumberOfLogEntriesFilteredWithError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnRecordWithTimer
            // 
            BtnRecordWithTimer.Image = Properties.Resources.timer_record_outline_24x24;
            BtnRecordWithTimer.Location = new System.Drawing.Point(104, 22);
            BtnRecordWithTimer.Name = "BtnRecordWithTimer";
            BtnRecordWithTimer.Size = new System.Drawing.Size(56, 40);
            BtnRecordWithTimer.TabIndex = 16;
            BtnRecordWithTimer.UseVisualStyleBackColor = true;
            BtnRecordWithTimer.Click += BtnRecordWithTimer_Click;
            // 
            // lblLogEntriesFilteredWithError
            // 
            lblLogEntriesFilteredWithError.AutoSize = true;
            lblLogEntriesFilteredWithError.Location = new System.Drawing.Point(10, 114);
            lblLogEntriesFilteredWithError.Name = "lblLogEntriesFilteredWithError";
            lblLogEntriesFilteredWithError.Size = new System.Drawing.Size(93, 15);
            lblLogEntriesFilteredWithError.TabIndex = 18;
            lblLogEntriesFilteredWithError.Text = "Regels met error";
            // 
            // lblLogProvider
            // 
            lblLogProvider.AutoSize = true;
            lblLogProvider.Location = new System.Drawing.Point(5, 21);
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
            cboLogProvider.Location = new System.Drawing.Point(5, 39);
            cboLogProvider.Name = "cboLogProvider";
            cboLogProvider.Size = new System.Drawing.Size(178, 23);
            cboLogProvider.TabIndex = 18;
            cboLogProvider.SelectedIndexChanged += CboLogProvider_SelectedIndexChanged;
            // 
            // usrRuntime
            // 
            usrRuntime.Dock = System.Windows.Forms.DockStyle.Fill;
            usrRuntime.Location = new System.Drawing.Point(3, 19);
            usrRuntime.Name = "usrRuntime";
            usrRuntime.Size = new System.Drawing.Size(425, 125);
            usrRuntime.TabIndex = 0;
            // 
            // usrKubernetes
            // 
            usrKubernetes.Dock = System.Windows.Forms.DockStyle.Fill;
            usrKubernetes.Location = new System.Drawing.Point(3, 19);
            usrKubernetes.Name = "usrKubernetes";
            usrKubernetes.Size = new System.Drawing.Size(425, 125);
            usrKubernetes.TabIndex = 8;
            // 
            // cboLogLayout
            // 
            cboLogLayout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboLogLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogLayout.FormattingEnabled = true;
            cboLogLayout.Location = new System.Drawing.Point(6, 85);
            cboLogLayout.Name = "cboLogLayout";
            cboLogLayout.Size = new System.Drawing.Size(177, 23);
            cboLogLayout.TabIndex = 23;
            cboLogLayout.SelectedIndexChanged += CboLogLayout_SelectedIndexChanged;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox5);
            splitContainer2.Panel2.Controls.Add(groupBox6);
            splitContainer2.Size = new System.Drawing.Size(935, 447);
            splitContainer2.SplitterDistance = 685;
            splitContainer2.TabIndex = 8;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lblEndFilterEnabled);
            groupBox2.Controls.Add(lblBeginFilterEnabled);
            groupBox2.Controls.Add(chkShowAllLogEntries);
            groupBox2.Controls.Add(lblNumberOfLogEntriesShown);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(TxtErrorMessage);
            groupBox2.Controls.Add(txtLogEntries);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(685, 447);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Log";
            // 
            // lblEndFilterEnabled
            // 
            lblEndFilterEnabled.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblEndFilterEnabled.AutoSize = true;
            lblEndFilterEnabled.BackColor = System.Drawing.Color.GreenYellow;
            lblEndFilterEnabled.Location = new System.Drawing.Point(593, 427);
            lblEndFilterEnabled.Name = "lblEndFilterEnabled";
            lblEndFilterEnabled.Size = new System.Drawing.Size(86, 15);
            lblEndFilterEnabled.TabIndex = 37;
            lblEndFilterEnabled.Text = "Eindfilter actief";
            lblEndFilterEnabled.Visible = false;
            // 
            // lblBeginFilterEnabled
            // 
            lblBeginFilterEnabled.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblBeginFilterEnabled.AutoSize = true;
            lblBeginFilterEnabled.BackColor = System.Drawing.Color.Orange;
            lblBeginFilterEnabled.Location = new System.Drawing.Point(498, 427);
            lblBeginFilterEnabled.Name = "lblBeginFilterEnabled";
            lblBeginFilterEnabled.Size = new System.Drawing.Size(93, 15);
            lblBeginFilterEnabled.TabIndex = 36;
            lblBeginFilterEnabled.Text = "Beginfilter actief";
            lblBeginFilterEnabled.Visible = false;
            // 
            // chkShowAllLogEntries
            // 
            chkShowAllLogEntries.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chkShowAllLogEntries.AutoSize = true;
            chkShowAllLogEntries.Location = new System.Drawing.Point(172, 426);
            chkShowAllLogEntries.Name = "chkShowAllLogEntries";
            chkShowAllLogEntries.Size = new System.Drawing.Size(180, 19);
            chkShowAllLogEntries.TabIndex = 35;
            chkShowAllLogEntries.Text = "Alle regels tonen (langzamer)";
            chkShowAllLogEntries.UseVisualStyleBackColor = true;
            chkShowAllLogEntries.CheckedChanged += ChkShowAllLogEntries_CheckedChanged;
            // 
            // lblNumberOfLogEntriesShown
            // 
            lblNumberOfLogEntriesShown.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblNumberOfLogEntriesShown.Location = new System.Drawing.Point(96, 427);
            lblNumberOfLogEntriesShown.Name = "lblNumberOfLogEntriesShown";
            lblNumberOfLogEntriesShown.Size = new System.Drawing.Size(80, 15);
            lblNumberOfLogEntriesShown.TabIndex = 34;
            lblNumberOfLogEntriesShown.Text = "-";
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 427);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(92, 15);
            label1.TabIndex = 33;
            label1.Text = "Regels getoond:";
            // 
            // TxtErrorMessage
            // 
            TxtErrorMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TxtErrorMessage.BackColor = System.Drawing.SystemColors.Window;
            TxtErrorMessage.ForeColor = System.Drawing.Color.DarkRed;
            TxtErrorMessage.Location = new System.Drawing.Point(91, 32);
            TxtErrorMessage.Multiline = true;
            TxtErrorMessage.Name = "TxtErrorMessage";
            TxtErrorMessage.ReadOnly = true;
            TxtErrorMessage.Size = new System.Drawing.Size(497, 59);
            TxtErrorMessage.TabIndex = 32;
            TxtErrorMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TxtErrorMessage.Visible = false;
            // 
            // txtLogEntries
            // 
            txtLogEntries.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtLogEntries.BackColor = System.Drawing.Color.White;
            txtLogEntries.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtLogEntries.DetectUrls = false;
            txtLogEntries.Location = new System.Drawing.Point(6, 15);
            txtLogEntries.Name = "txtLogEntries";
            txtLogEntries.ReadOnly = true;
            txtLogEntries.Size = new System.Drawing.Size(673, 409);
            txtLogEntries.TabIndex = 3;
            txtLogEntries.Text = "";
            txtLogEntries.WordWrap = false;
            // 
            // groupBox5
            // 
            groupBox5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox5.Controls.Add(usrSearch);
            groupBox5.Location = new System.Drawing.Point(3, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(243, 79);
            groupBox5.TabIndex = 2;
            groupBox5.TabStop = false;
            groupBox5.Text = "Zoeken";
            // 
            // usrSearch
            // 
            usrSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            usrSearch.Location = new System.Drawing.Point(3, 19);
            usrSearch.Name = "usrSearch";
            usrSearch.Size = new System.Drawing.Size(237, 57);
            usrSearch.TabIndex = 33;
            // 
            // groupBox6
            // 
            groupBox6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox6.Controls.Add(tabControl1);
            groupBox6.Location = new System.Drawing.Point(0, 85);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(246, 362);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "Kies begin en einde";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(3, 19);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(240, 340);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(UsrLogContentBegin);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(232, 312);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tonen vanaf";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // UsrLogContentBegin
            // 
            UsrLogContentBegin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            UsrLogContentBegin.BackColor = System.Drawing.SystemColors.Window;
            UsrLogContentBegin.Dock = System.Windows.Forms.DockStyle.Fill;
            UsrLogContentBegin.Location = new System.Drawing.Point(3, 3);
            UsrLogContentBegin.Name = "UsrLogContentBegin";
            UsrLogContentBegin.Size = new System.Drawing.Size(226, 306);
            UsrLogContentBegin.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(UsrLogContentEnd);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(232, 312);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Tonen tot";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // UsrLogContentEnd
            // 
            UsrLogContentEnd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            UsrLogContentEnd.BackColor = System.Drawing.SystemColors.Window;
            UsrLogContentEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            UsrLogContentEnd.Location = new System.Drawing.Point(3, 3);
            UsrLogContentEnd.Name = "UsrLogContentEnd";
            UsrLogContentEnd.Size = new System.Drawing.Size(226, 306);
            UsrLogContentEnd.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tabControl2);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(211, 447);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Metadata";
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(tabPage3);
            tabControl2.Controls.Add(tabPage4);
            tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl2.Location = new System.Drawing.Point(3, 19);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new System.Drawing.Size(205, 425);
            tabControl2.TabIndex = 10;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(UsrMetadataFilterOverview);
            tabPage3.Location = new System.Drawing.Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3);
            tabPage3.Size = new System.Drawing.Size(197, 397);
            tabPage3.TabIndex = 0;
            tabPage3.Text = "Filteren";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // UsrMetadataFilterOverview
            // 
            UsrMetadataFilterOverview.AutoScroll = true;
            UsrMetadataFilterOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            UsrMetadataFilterOverview.Location = new System.Drawing.Point(3, 3);
            UsrMetadataFilterOverview.Margin = new System.Windows.Forms.Padding(0);
            UsrMetadataFilterOverview.Name = "UsrMetadataFilterOverview";
            UsrMetadataFilterOverview.Size = new System.Drawing.Size(191, 391);
            UsrMetadataFilterOverview.TabIndex = 0;
            UsrMetadataFilterOverview.FilterChanged += UsrControlMetadataFormating_FilterChanged;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(UsrControlMetadataFormating);
            tabPage4.Location = new System.Drawing.Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(3);
            tabPage4.Size = new System.Drawing.Size(197, 397);
            tabPage4.TabIndex = 1;
            tabPage4.Text = "Tonen";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // UsrControlMetadataFormating
            // 
            UsrControlMetadataFormating.Dock = System.Windows.Forms.DockStyle.Fill;
            UsrControlMetadataFormating.Location = new System.Drawing.Point(3, 3);
            UsrControlMetadataFormating.MinimumSize = new System.Drawing.Size(190, 0);
            UsrControlMetadataFormating.Name = "UsrControlMetadataFormating";
            UsrControlMetadataFormating.Size = new System.Drawing.Size(191, 391);
            UsrControlMetadataFormating.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(5, 158);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(1150, 447);
            splitContainer1.SplitterDistance = 211;
            splitContainer1.TabIndex = 13;
            // 
            // GrpLogProvidersSettings
            // 
            GrpLogProvidersSettings.Controls.Add(usrKubernetes);
            GrpLogProvidersSettings.Controls.Add(usrRuntime);
            GrpLogProvidersSettings.Controls.Add(usrFileLogProvider);
            GrpLogProvidersSettings.Location = new System.Drawing.Point(466, 5);
            GrpLogProvidersSettings.MinimumSize = new System.Drawing.Size(300, 0);
            GrpLogProvidersSettings.Name = "GrpLogProvidersSettings";
            GrpLogProvidersSettings.Size = new System.Drawing.Size(431, 147);
            GrpLogProvidersSettings.TabIndex = 24;
            GrpLogProvidersSettings.TabStop = false;
            GrpLogProvidersSettings.Text = "Instellingen";
            // 
            // usrFileLogProvider
            // 
            usrFileLogProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            usrFileLogProvider.Location = new System.Drawing.Point(3, 19);
            usrFileLogProvider.Name = "usrFileLogProvider";
            usrFileLogProvider.Size = new System.Drawing.Size(425, 125);
            usrFileLogProvider.TabIndex = 9;
            // 
            // GrpSourceAndLayout
            // 
            GrpSourceAndLayout.Controls.Add(label2);
            GrpSourceAndLayout.Controls.Add(cboLogLayout);
            GrpSourceAndLayout.Controls.Add(lblLogProvider);
            GrpSourceAndLayout.Controls.Add(cboLogProvider);
            GrpSourceAndLayout.Location = new System.Drawing.Point(268, 5);
            GrpSourceAndLayout.Name = "GrpSourceAndLayout";
            GrpSourceAndLayout.Size = new System.Drawing.Size(192, 147);
            GrpSourceAndLayout.TabIndex = 25;
            GrpSourceAndLayout.TabStop = false;
            GrpSourceAndLayout.Text = "Bron en layout";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 65);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(43, 15);
            label2.TabIndex = 24;
            label2.Text = "Layout";
            // 
            // FormLogScraper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1160, 610);
            Controls.Add(GrpSourceAndLayout);
            Controls.Add(GrpLogProvidersSettings);
            Controls.Add(splitContainer1);
            Controls.Add(groupBox3);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "FormLogScraper";
            Text = "Log Scraper - GEBRUIK OP EIGEN RISICO - NIET GEBRUIKEN OP PRODUCTIE OMGEVINGEN";
            Load += FormLogScraper_Load;
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tabControl2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            GrpLogProvidersSettings.ResumeLayout(false);
            GrpSourceAndLayout.ResumeLayout(false);
            GrpSourceAndLayout.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        //Make these controls public so that they can be accessed from the mini controls form
        public System.Windows.Forms.Button BtnRecord;
        public System.Windows.Forms.Button BtnRecordWithTimer;
        public System.Windows.Forms.Button BtnErase;
        public System.Windows.Forms.Button BtnStop;
        public System.Windows.Forms.Button btnOpenWithEditor;
        public System.Windows.Forms.Label lblLogEntriesTotalValue;
        public System.Windows.Forms.Label lblNumberOfLogEntriesFiltered;
        public System.Windows.Forms.Label lblNumberOfLogEntriesFilteredWithError;
        public System.Windows.Forms.Label lblLogEntriesFilteredWithError;

        private System.Windows.Forms.Label LbllogEntriesTotal;

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblLogEntriesFiltered;
        private System.Windows.Forms.Button BtnFormRecord;
        private System.Windows.Forms.ComboBox cboLogProvider;
        private System.Windows.Forms.RichTextBox txtLogEntries;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private UserControlBeginEndFilter UsrLogContentBegin;
        private UserControlBeginEndFilter UsrLogContentEnd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label lblLogProvider;
        private LogProviders.Kubernetes.UserControlKubernetesLogProvider usrKubernetes;
        private LogProviders.Runtime.UserControlRuntimeLogProvider usrRuntime;
        private System.Windows.Forms.TextBox TxtErrorMessage;
        private System.Windows.Forms.GroupBox GrpLogProvidersSettings;
        private LogProviders.File.UserControlFileLogProvider usrFileLogProvider;
        private Log.Metadata.UserControlMetadataFormatting UsrControlMetadataFormating;
        private System.Windows.Forms.ComboBox cboLogLayout;
        private System.Windows.Forms.GroupBox GrpSourceAndLayout;
        private UserControlSearch usrSearch;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblNumberOfLogEntriesShown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkShowAllLogEntries;
        private System.Windows.Forms.Label lblEndFilterEnabled;
        private System.Windows.Forms.Label lblBeginFilterEnabled;
        private System.Windows.Forms.Button BtnConfig;
        private System.Windows.Forms.Label label2;
        private Log.Metadata.UserControlMetadataFilterOverview UsrMetadataFilterOverview;
        private System.Windows.Forms.ToolTip ToolTip;
        private UserControlMemoryUsage userControlMemoryUsage1;
    }
}
