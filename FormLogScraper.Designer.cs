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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogScraper));
            btnReadFromUrl = new System.Windows.Forms.Button();
            lblLogEntriesTotalValue = new System.Windows.Forms.Label();
            lblNumberOfLogEntriesFiltered = new System.Windows.Forms.Label();
            lblLogEntriesFiltered = new System.Windows.Forms.Label();
            LbllogEntriesTotal = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            btnConfig = new System.Windows.Forms.Button();
            lblMemoryUsageValue = new System.Windows.Forms.Label();
            lblMemoryUsage = new System.Windows.Forms.Label();
            btnSmallWindow = new System.Windows.Forms.Button();
            lblNumberOfLogEntriesFilteredWithError = new System.Windows.Forms.Label();
            btnReset = new System.Windows.Forms.Button();
            lblLogEntriesFilteredWithError = new System.Windows.Forms.Label();
            BtnClearLog = new System.Windows.Forms.Button();
            btnStop = new System.Windows.Forms.Button();
            btnDowloadLogLongTime = new System.Windows.Forms.Button();
            lblLogProvider = new System.Windows.Forms.Label();
            cboLogProvider = new System.Windows.Forms.ComboBox();
            usrRuntime = new LogScraper.LogProviders.Runtime.UserControlRuntimeLogProvider();
            usrKubernetes = new LogScraper.LogProviders.Kubernetes.UserControlKubernetesLogProvider();
            grpWriteLog = new System.Windows.Forms.GroupBox();
            txtStatusWrite = new System.Windows.Forms.TextBox();
            btnOpenWithEditor = new System.Windows.Forms.Button();
            cboLogLayout = new System.Windows.Forms.ComboBox();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            groupBox2 = new System.Windows.Forms.GroupBox();
            lblEndFilterEnabled = new System.Windows.Forms.Label();
            lblBeginFilterEnabled = new System.Windows.Forms.Label();
            chkShowAllLogEntries = new System.Windows.Forms.CheckBox();
            lblNumberOfLogEntriesShown = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            txtStatusRead = new System.Windows.Forms.TextBox();
            txtLogEntries = new System.Windows.Forms.RichTextBox();
            groupBox5 = new System.Windows.Forms.GroupBox();
            usrSearch = new UserControlSearch();
            groupBox6 = new System.Windows.Forms.GroupBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            lblVersion = new System.Windows.Forms.Label();
            UsrLogContentBegin = new UserControlBeginEndFilter();
            tabPage2 = new System.Windows.Forms.TabPage();
            UsrLogContentEnd = new UserControlBeginEndFilter();
            groupBox1 = new System.Windows.Forms.GroupBox();
            tabControl2 = new System.Windows.Forms.TabControl();
            tabPage3 = new System.Windows.Forms.TabPage();
            PanelFilters = new System.Windows.Forms.Panel();
            tabPage4 = new System.Windows.Forms.TabPage();
            usrControlMetadataFormating = new LogScraper.Log.Metadata.UserControlMetadataFormating();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            GrpLogProvidersSettings = new System.Windows.Forms.GroupBox();
            usrFileLogProvider = new LogScraper.LogProviders.File.UserControlFileLogProvider();
            GrpSourceAndLayout = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            groupBox3.SuspendLayout();
            grpWriteLog.SuspendLayout();
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
            // btnReadFromUrl
            // 
            btnReadFromUrl.Location = new System.Drawing.Point(6, 16);
            btnReadFromUrl.Name = "btnReadFromUrl";
            btnReadFromUrl.Size = new System.Drawing.Size(109, 31);
            btnReadFromUrl.TabIndex = 0;
            btnReadFromUrl.Text = "Uitlezen";
            btnReadFromUrl.UseVisualStyleBackColor = true;
            btnReadFromUrl.Click += BtnReadFromUrl_Click;
            // 
            // lblLogEntriesTotalValue
            // 
            lblLogEntriesTotalValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblLogEntriesTotalValue.Location = new System.Drawing.Point(230, 33);
            lblLogEntriesTotalValue.Name = "lblLogEntriesTotalValue";
            lblLogEntriesTotalValue.Size = new System.Drawing.Size(63, 15);
            lblLogEntriesTotalValue.TabIndex = 7;
            lblLogEntriesTotalValue.Text = "0";
            lblLogEntriesTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNumberOfLogEntriesFiltered
            // 
            lblNumberOfLogEntriesFiltered.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblNumberOfLogEntriesFiltered.Location = new System.Drawing.Point(230, 51);
            lblNumberOfLogEntriesFiltered.Name = "lblNumberOfLogEntriesFiltered";
            lblNumberOfLogEntriesFiltered.Size = new System.Drawing.Size(63, 15);
            lblNumberOfLogEntriesFiltered.TabIndex = 17;
            lblNumberOfLogEntriesFiltered.Text = "0";
            lblNumberOfLogEntriesFiltered.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLogEntriesFiltered
            // 
            lblLogEntriesFiltered.AutoSize = true;
            lblLogEntriesFiltered.Location = new System.Drawing.Point(121, 51);
            lblLogEntriesFiltered.Name = "lblLogEntriesFiltered";
            lblLogEntriesFiltered.Size = new System.Drawing.Size(88, 15);
            lblLogEntriesFiltered.TabIndex = 16;
            lblLogEntriesFiltered.Text = "Regels gefilterd";
            // 
            // LbllogEntriesTotal
            // 
            LbllogEntriesTotal.AutoSize = true;
            LbllogEntriesTotal.Location = new System.Drawing.Point(121, 33);
            LbllogEntriesTotal.Name = "LbllogEntriesTotal";
            LbllogEntriesTotal.Size = new System.Drawing.Size(100, 15);
            LbllogEntriesTotal.TabIndex = 10;
            LbllogEntriesTotal.Text = "Regels opgehaald";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnConfig);
            groupBox3.Controls.Add(lblMemoryUsageValue);
            groupBox3.Controls.Add(lblMemoryUsage);
            groupBox3.Controls.Add(btnSmallWindow);
            groupBox3.Controls.Add(lblNumberOfLogEntriesFilteredWithError);
            groupBox3.Controls.Add(btnReset);
            groupBox3.Controls.Add(lblNumberOfLogEntriesFiltered);
            groupBox3.Controls.Add(btnReadFromUrl);
            groupBox3.Controls.Add(lblLogEntriesFilteredWithError);
            groupBox3.Controls.Add(BtnClearLog);
            groupBox3.Controls.Add(lblLogEntriesTotalValue);
            groupBox3.Controls.Add(LbllogEntriesTotal);
            groupBox3.Controls.Add(lblLogEntriesFiltered);
            groupBox3.Controls.Add(btnStop);
            groupBox3.Controls.Add(btnDowloadLogLongTime);
            groupBox3.Location = new System.Drawing.Point(5, 5);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(304, 147);
            groupBox3.TabIndex = 19;
            groupBox3.TabStop = false;
            groupBox3.Tag = "test";
            groupBox3.Text = "Uitlezen log";
            // 
            // btnConfig
            // 
            btnConfig.Image = Properties.Resources.cog_outline_16px;
            btnConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnConfig.Location = new System.Drawing.Point(197, 116);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new System.Drawing.Size(101, 24);
            btnConfig.TabIndex = 23;
            btnConfig.Text = "Instellingen";
            btnConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnConfig.UseVisualStyleBackColor = true;
            btnConfig.Click += BtnConfig_Click;
            // 
            // lblMemoryUsageValue
            // 
            lblMemoryUsageValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblMemoryUsageValue.Location = new System.Drawing.Point(230, 16);
            lblMemoryUsageValue.Name = "lblMemoryUsageValue";
            lblMemoryUsageValue.Size = new System.Drawing.Size(63, 15);
            lblMemoryUsageValue.TabIndex = 22;
            lblMemoryUsageValue.Text = "0";
            lblMemoryUsageValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMemoryUsage
            // 
            lblMemoryUsage.AutoSize = true;
            lblMemoryUsage.Location = new System.Drawing.Point(120, 16);
            lblMemoryUsage.Name = "lblMemoryUsage";
            lblMemoryUsage.Size = new System.Drawing.Size(101, 15);
            lblMemoryUsage.TabIndex = 21;
            lblMemoryUsage.Text = "Geheugengebruik";
            // 
            // btnSmallWindow
            // 
            btnSmallWindow.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSmallWindow.Image = Properties.Resources.open_in_new;
            btnSmallWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnSmallWindow.Location = new System.Drawing.Point(197, 90);
            btnSmallWindow.Name = "btnSmallWindow";
            btnSmallWindow.Size = new System.Drawing.Size(101, 25);
            btnSmallWindow.TabIndex = 11;
            btnSmallWindow.Text = "Mini controls";
            btnSmallWindow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnSmallWindow.UseVisualStyleBackColor = true;
            btnSmallWindow.Click += BtnMiniTopForm_Click;
            // 
            // lblNumberOfLogEntriesFilteredWithError
            // 
            lblNumberOfLogEntriesFilteredWithError.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblNumberOfLogEntriesFilteredWithError.Location = new System.Drawing.Point(230, 69);
            lblNumberOfLogEntriesFilteredWithError.Name = "lblNumberOfLogEntriesFilteredWithError";
            lblNumberOfLogEntriesFilteredWithError.Size = new System.Drawing.Size(63, 15);
            lblNumberOfLogEntriesFilteredWithError.TabIndex = 19;
            lblNumberOfLogEntriesFilteredWithError.Text = "0";
            lblNumberOfLogEntriesFilteredWithError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReset
            // 
            btnReset.Location = new System.Drawing.Point(6, 116);
            btnReset.Name = "btnReset";
            btnReset.Size = new System.Drawing.Size(109, 24);
            btnReset.TabIndex = 20;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += BtnReset_Click;
            // 
            // lblLogEntriesFilteredWithError
            // 
            lblLogEntriesFilteredWithError.AutoSize = true;
            lblLogEntriesFilteredWithError.Location = new System.Drawing.Point(121, 69);
            lblLogEntriesFilteredWithError.Name = "lblLogEntriesFilteredWithError";
            lblLogEntriesFilteredWithError.Size = new System.Drawing.Size(93, 15);
            lblLogEntriesFilteredWithError.TabIndex = 18;
            lblLogEntriesFilteredWithError.Text = "Regels met error";
            // 
            // BtnClearLog
            // 
            BtnClearLog.Location = new System.Drawing.Point(7, 90);
            BtnClearLog.Name = "BtnClearLog";
            BtnClearLog.Size = new System.Drawing.Size(109, 24);
            BtnClearLog.TabIndex = 11;
            BtnClearLog.Text = "Wis log";
            BtnClearLog.UseVisualStyleBackColor = true;
            BtnClearLog.Click += BtnClearLog_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new System.Drawing.Point(7, 53);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(109, 31);
            btnStop.TabIndex = 17;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Visible = false;
            btnStop.Click += BtnStop_Click;
            // 
            // btnDowloadLogLongTime
            // 
            btnDowloadLogLongTime.Location = new System.Drawing.Point(6, 53);
            btnDowloadLogLongTime.Name = "btnDowloadLogLongTime";
            btnDowloadLogLongTime.Size = new System.Drawing.Size(109, 31);
            btnDowloadLogLongTime.TabIndex = 16;
            btnDowloadLogLongTime.Text = "Lees log 1 minuut";
            btnDowloadLogLongTime.UseVisualStyleBackColor = true;
            btnDowloadLogLongTime.Click += BtnDowloadLogLongTime_Click;
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
            cboLogProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogProvider.FormattingEnabled = true;
            cboLogProvider.Location = new System.Drawing.Point(5, 39);
            cboLogProvider.Name = "cboLogProvider";
            cboLogProvider.Size = new System.Drawing.Size(184, 23);
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
            // grpWriteLog
            // 
            grpWriteLog.Controls.Add(txtStatusWrite);
            grpWriteLog.Controls.Add(btnOpenWithEditor);
            grpWriteLog.Location = new System.Drawing.Point(954, 5);
            grpWriteLog.Name = "grpWriteLog";
            grpWriteLog.Size = new System.Drawing.Size(134, 147);
            grpWriteLog.TabIndex = 23;
            grpWriteLog.TabStop = false;
            grpWriteLog.Text = "Log wegschrijven";
            // 
            // txtStatusWrite
            // 
            txtStatusWrite.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtStatusWrite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtStatusWrite.Location = new System.Drawing.Point(6, 69);
            txtStatusWrite.Multiline = true;
            txtStatusWrite.Name = "txtStatusWrite";
            txtStatusWrite.ReadOnly = true;
            txtStatusWrite.Size = new System.Drawing.Size(122, 69);
            txtStatusWrite.TabIndex = 34;
            txtStatusWrite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnOpenWithEditor
            // 
            btnOpenWithEditor.Location = new System.Drawing.Point(6, 19);
            btnOpenWithEditor.Name = "btnOpenWithEditor";
            btnOpenWithEditor.Size = new System.Drawing.Size(119, 43);
            btnOpenWithEditor.TabIndex = 11;
            btnOpenWithEditor.Text = "Open";
            btnOpenWithEditor.UseVisualStyleBackColor = true;
            btnOpenWithEditor.Click += BtnOpenWithEditor_Click;
            // 
            // cboLogLayout
            // 
            cboLogLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLogLayout.FormattingEnabled = true;
            cboLogLayout.Location = new System.Drawing.Point(6, 85);
            cboLogLayout.Name = "cboLogLayout";
            cboLogLayout.Size = new System.Drawing.Size(183, 23);
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
            groupBox2.Controls.Add(txtStatusRead);
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
            // txtStatusRead
            // 
            txtStatusRead.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtStatusRead.BackColor = System.Drawing.SystemColors.Window;
            txtStatusRead.ForeColor = System.Drawing.Color.DarkRed;
            txtStatusRead.Location = new System.Drawing.Point(91, 32);
            txtStatusRead.Multiline = true;
            txtStatusRead.Name = "txtStatusRead";
            txtStatusRead.ReadOnly = true;
            txtStatusRead.Size = new System.Drawing.Size(497, 59);
            txtStatusRead.TabIndex = 32;
            txtStatusRead.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            txtStatusRead.Visible = false;
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
            tabPage1.Controls.Add(lblVersion);
            tabPage1.Controls.Add(UsrLogContentBegin);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(232, 312);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Tonen vanaf";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblVersion
            // 
            lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            lblVersion.BackColor = System.Drawing.SystemColors.Window;
            lblVersion.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            lblVersion.Location = new System.Drawing.Point(185, 294);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(46, 15);
            lblVersion.TabIndex = 23;
            lblVersion.Text = "2.00.00";
            lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            tabPage3.Controls.Add(PanelFilters);
            tabPage3.Location = new System.Drawing.Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3);
            tabPage3.Size = new System.Drawing.Size(197, 397);
            tabPage3.TabIndex = 0;
            tabPage3.Text = "Filteren";
            tabPage3.UseVisualStyleBackColor = true;
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
            // tabPage4
            // 
            tabPage4.Controls.Add(usrControlMetadataFormating);
            tabPage4.Location = new System.Drawing.Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(3);
            tabPage4.Size = new System.Drawing.Size(197, 397);
            tabPage4.TabIndex = 1;
            tabPage4.Text = "Tonen";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // usrControlMetadataFormating
            // 
            usrControlMetadataFormating.Dock = System.Windows.Forms.DockStyle.Fill;
            usrControlMetadataFormating.Location = new System.Drawing.Point(3, 3);
            usrControlMetadataFormating.MinimumSize = new System.Drawing.Size(190, 0);
            usrControlMetadataFormating.Name = "usrControlMetadataFormating";
            usrControlMetadataFormating.Size = new System.Drawing.Size(191, 391);
            usrControlMetadataFormating.TabIndex = 0;
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
            GrpLogProvidersSettings.Location = new System.Drawing.Point(517, 5);
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
            GrpSourceAndLayout.Location = new System.Drawing.Point(316, 5);
            GrpSourceAndLayout.Name = "GrpSourceAndLayout";
            GrpSourceAndLayout.Size = new System.Drawing.Size(195, 147);
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
            Controls.Add(grpWriteLog);
            Controls.Add(splitContainer1);
            Controls.Add(groupBox3);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "FormLogScraper";
            Text = "Log Scraper - GEBRUIK OP EIGEN RISICO - NIET GEBRUIKEN OP PRODUCTIE OMGEVINGEN";
            Load += FormLogScraper_Load;
            KeyDown += FormLogScraper_KeyDown;
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            grpWriteLog.ResumeLayout(false);
            grpWriteLog.PerformLayout();
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

        private System.Windows.Forms.Button btnReadFromUrl;
        private System.Windows.Forms.Label lblLogEntriesTotalValue;
        private System.Windows.Forms.Label LbllogEntriesTotal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDowloadLogLongTime;
        private System.Windows.Forms.Button BtnClearLog;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblNumberOfLogEntriesFiltered;
        private System.Windows.Forms.Label lblLogEntriesFiltered;
        private System.Windows.Forms.Button btnSmallWindow;
        private System.Windows.Forms.Label lblNumberOfLogEntriesFilteredWithError;
        private System.Windows.Forms.Label lblLogEntriesFilteredWithError;
        private System.Windows.Forms.ComboBox cboLogProvider;
        private System.Windows.Forms.GroupBox grpWriteLog;
        private System.Windows.Forms.Button btnOpenWithEditor;
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
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label lblLogProvider;
        private LogProviders.Kubernetes.UserControlKubernetesLogProvider usrKubernetes;
        private LogProviders.Runtime.UserControlRuntimeLogProvider usrRuntime;
        private System.Windows.Forms.TextBox txtStatusRead;
        private System.Windows.Forms.TextBox txtStatusWrite;
        private System.Windows.Forms.GroupBox GrpLogProvidersSettings;
        private LogProviders.File.UserControlFileLogProvider usrFileLogProvider;
        private System.Windows.Forms.Label lblMemoryUsageValue;
        private System.Windows.Forms.Label lblMemoryUsage;
        private Log.Metadata.UserControlMetadataFormating usrControlMetadataFormating;
        private System.Windows.Forms.ComboBox cboLogLayout;
        private System.Windows.Forms.GroupBox GrpSourceAndLayout;
        private System.Windows.Forms.Label lblVersion;
        private UserControlSearch usrSearch;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblNumberOfLogEntriesShown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkShowAllLogEntries;
        private System.Windows.Forms.Label lblEndFilterEnabled;
        private System.Windows.Forms.Label lblBeginFilterEnabled;
        private System.Windows.Forms.Panel PanelFilters;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Label label2;
    }
}
