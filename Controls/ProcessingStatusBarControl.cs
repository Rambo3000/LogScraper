using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;

namespace LogScraper.Controls
{
    /// <summary>
    /// Consolidated status bar shown at the bottom of the main form, indicating the
    /// current data retrieval/processing state (retrieving, processing, or waiting for
    /// the next continuous/timed fetch). Hidden entirely while no fetch is active.
    /// </summary>
    public partial class ProcessingStatusBarControl : UserControl
    {
        public ProcessingStatusBarControl()
        {
            InitializeComponent();

            if (DesignMode) return;

            LogAppState.Instance.ProcessingState.Changed += (s, e) => UpdateStatus();
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
                    LblStatusText.Text = "Data ophalen...";
                    Visible = true;
                    break;
                case LogProcessingStatus.Processing:
                    PicStatusIcon.Image = IconProcessing;
                    LblStatusText.Text = "Data verwerken...";
                    Visible = true;
                    break;
                case LogProcessingStatus.Waiting:
                    PicStatusIcon.Image = IconWaiting;
                    LblStatusText.Text = "Wachten...";
                    Visible = true;
                    break;
                case LogProcessingStatus.Idle:
                default:
                    Visible = false;
                    break;
            }
        }
    }
}
