using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class UserControlLogMetadataFilterItem : UserControl
    {
        public event EventHandler CheckedChanged;

        private string DescriptionOriginalValue;

        public UserControlLogMetadataFilterItem(string description, int count, bool isChecked)
        {
            InitializeComponent();
            CheckBoxItem.Checked = isChecked;
            Description = description;
            Count = count;
        }
        /// <summary>
        ///  Override to handle windows scaling correctly
        /// </summary>
        protected override void OnFontChanged(EventArgs eventArgs)
        {
            base.OnFontChanged(eventArgs);
            UpdateHeightFromFont();
        }

        /// <summary>
        ///  Override to handle windows scaling correctly
        /// </summary>
        protected override void OnLayout(LayoutEventArgs layoutEventArgs)
        {
            base.OnLayout(layoutEventArgs);
            UpdateCount(Count);
            AdjustCheckBoxText();
        }
        
        void UpdateHeightFromFont()
        {
            int textHeight = LabelCount.PreferredHeight;

            int desiredHeight = textHeight + ScaleByDpi(4);

            if (Height != desiredHeight)
            {
                CheckBoxItem.Height = desiredHeight;
                Height = desiredHeight;
            }
        }

        private void CheckBoxItem_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }

        private void AdjustCheckBoxText()
        {
            CheckBoxItem.AutoSize = false;
            CheckBoxItem.AutoEllipsis = true;

            int availableWidth = ClientSize.Width - LabelCount.PreferredWidth;

            if (availableWidth < 10)
            {
                availableWidth = 50;
            }

            CheckBoxItem.Width = availableWidth;
            CheckBoxItem.Text = DescriptionOriginalValue ?? string.Empty;
        }

        private void LogMetadataFilterItem_SizeChanged(object sender, EventArgs e)
        {
            AdjustCheckBoxText();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsChecked
        {
            get => CheckBoxItem.Checked;
            set => CheckBoxItem.Checked = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Description
        {
            get => DescriptionOriginalValue;
            private set
            {
                DescriptionOriginalValue = value;
            }
        }

        private int countValue = -1;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get => countValue;
            set
            {
                // prevent flickering
                if (countValue == value) return;

                UpdateCount(value);
                AdjustCheckBoxText();
                //Recalculate height also here for Windows 10 beacuse of some weird DPI scaling issues
                UpdateHeightFromFont();
                countValue = value;
            }
        }

        public void UpdateCount(int newCountValue)
        {
            LabelCount.Anchor = AnchorStyles.None;
            LabelCount.Text = newCountValue.ToString("N0");
            LabelCount.Left = ClientSize.Width - LabelCount.PreferredWidth;
            LabelCount.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            ForeColor = newCountValue == 0 ? Color.Gray : Color.Black;
        }

        private int ScaleByDpi(int logicalPixels)
        {
            return (int)Math.Ceiling(logicalPixels * DeviceDpi / 96f);
        }

    }
}

