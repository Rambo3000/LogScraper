using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;

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
            LabelCount.Text = count.ToString();
            Description = description;
        }

        private void CheckBoxItem_CheckedChanged(object sender, EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }

        private void AdjustCheckBoxText()
        {
            string originalText = DescriptionOriginalValue;


            Font font = CheckBoxItem.Font;
            int maxWidth = Width - TextRenderer.MeasureText(LabelCount.Text, LabelCount.Font).Width - 20;
            CheckBoxItem.Width = maxWidth + 20;

            if (string.IsNullOrEmpty(originalText))
            {
                CheckBoxItem.Text = string.Empty;
                return;
            }

            // Measure the original text size
            Size textSize = TextRenderer.MeasureText(originalText, font);

            if (textSize.Width <= maxWidth)
            {
                CheckBoxItem.Text = originalText; // Text fits, no truncation needed
                return;
            }

            // Truncate text by reducing characters until it fits
            for (int i = originalText.Length - 1; i > 0; i--)
            {
                string testText = string.Concat(originalText.AsSpan(0, i), "...");
                if (TextRenderer.MeasureText(testText, font).Width <= maxWidth)
                {
                    CheckBoxItem.Text = testText;
                    return;
                }
            }

            // If even "..." is too wide, just show "..."
            CheckBoxItem.Text = "...";
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
            set
            {
                DescriptionOriginalValue = value;
                AdjustCheckBoxText();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get => int.Parse(LabelCount.Text, NumberStyles.AllowThousands);
            set { 
                LabelCount.Text = value.ToString("N0");
                LabelCount.Left = Width - LabelCount.Width + 4;
                ForeColor = value == 0 ? Color.Gray : Color.Black;
            }
        }
    }
}

