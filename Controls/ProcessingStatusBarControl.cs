using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LogScraper.Export;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;

namespace LogScraper.Controls
{
    /// <summary>
    /// Consolidated status bar shown at the bottom of the main form, indicating the
    /// current data retrieval/processing state (retrieving, processing, or waiting for
    /// the next continuous/timed fetch), as well as an on-demand log export progress
    /// indicator on the right. Hidden entirely while no fetch or export is active.
    /// </summary>
    public partial class ProcessingStatusBarControl : UserControl
    {
        private bool _isExporting = false;

        public ProcessingStatusBarControl()
        {
            InitializeComponent();

            if (DesignMode) return;

            LogAppState.Instance.ProcessingState.Changed += (s, e) => UpdateStatus();
            LogExportWorkerManager.IsWritingChanged += OnExportWritingChanged;
            UpdateStatus();
        }

        private void OnExportWritingChanged(bool isWriting)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(OnExportWritingChanged), isWriting);
                return;
            }

            _isExporting = isWriting;
            PnlExportStatus.Visible = isWriting;
            UpdateStatus();
        }

        /// <summary>
        /// Icon shown while data is being retrieved from the source.
        /// </summary>
        [Category("Status Icons")]
        [Description("Icon shown while data is being retrieved from the source.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image IconRetrieving { get; set; }

        /// <summary>
        /// Icon shown while retrieved data is being processed.
        /// </summary>
        [Category("Status Icons")]
        [Description("Icon shown while retrieved data is being processed.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image IconProcessing { get; set; }

        /// <summary>
        /// Icon shown while waiting for the next continuous/timed fetch.
        /// </summary>
        [Category("Status Icons")]
        [Description("Icon shown while waiting for the next continuous/timed fetch.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image IconWaiting { get; set; }

        private void UpdateStatus()
        {
            LogProcessingState processingState = LogAppState.Instance.ProcessingState.Value;

            switch (processingState.Status)
            {
                case LogProcessingStatus.Retrieving:
                    PicStatusIcon.Image = IconRetrieving;
                    LblStatusText.Text = "Retrieving data...";
                    Visible = true;
                    break;
                case LogProcessingStatus.Processing:
                    PicStatusIcon.Image = IconProcessing;
                    LblStatusText.Text = "Processing data...";
                    Visible = true;
                    break;
                case LogProcessingStatus.Waiting:
                    PicStatusIcon.Image = IconWaiting;
                    LblStatusText.Text = "Waiting...";
                    Visible = true;
                    break;
                case LogProcessingStatus.Idle:
                default:
                    PicStatusIcon.Image = null;
                    LblStatusText.Text = string.Empty;
                    Visible = _isExporting;
                    break;
            }
        }
    }
}
