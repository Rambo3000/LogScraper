using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace LogScraper.Controls.Generic
{
    public partial class ClearableTextBoxControl : UserControl
    {
        private string placeholderText = string.Empty;
        private bool isShowingPlaceholder = false;

        public new event EventHandler TextChanged;
        public event EventHandler Reset;
        public new event KeyEventHandler KeyDown;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string PlaceholderText
        {
            get => placeholderText;
            set
            {
                placeholderText = value;
                UpdatePlaceholder();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new string Text
        {
            get => isShowingPlaceholder ? string.Empty : TxtTextBox.Text;
            set
            {
                isShowingPlaceholder = false;
                TxtTextBox.ForeColor = SystemColors.WindowText;
                TxtTextBox.Text = value;
                UpdatePlaceholder();
            }
        }

        public ClearableTextBoxControl()
        {
            InitializeComponent();
            TxtTextBox.KeyDown += (s, e) => KeyDown?.Invoke(this, e);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            isShowingPlaceholder = false;
            TxtTextBox.ForeColor = SystemColors.WindowText;
            TxtTextBox.Text = string.Empty;
            // TextChanged fires via TxtTextBox_TextChanged
            TxtTextBox.Focus();
            UpdatePlaceholder();
            Reset?.Invoke(this, EventArgs.Empty);
        }

        private void TxtTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isShowingPlaceholder)
                return;

            UpdateClearButtonVisibility();
            TextChanged?.Invoke(this, e);
        }

        private void TxtTextBox_Enter(object sender, EventArgs e)
        {
            if (isShowingPlaceholder)
            {
                isShowingPlaceholder = false;
                TxtTextBox.ForeColor = SystemColors.WindowText;
                TxtTextBox.Text = string.Empty;
            }
        }

        private void TxtTextBox_Leave(object sender, EventArgs e)
        {
            UpdatePlaceholder();
        }

        private void UpdatePlaceholder()
        {
            bool textIsEmpty = string.IsNullOrEmpty(TxtTextBox.Text)
                               || TxtTextBox.Text == placeholderText;

            if (textIsEmpty && !string.IsNullOrEmpty(placeholderText) && !TxtTextBox.Focused)
            {
                isShowingPlaceholder = true;
                TxtTextBox.ForeColor = Color.LightGray;
                TxtTextBox.Text = placeholderText;
            }
            else if (isShowingPlaceholder && TxtTextBox.Focused)
            {
                // Focus came in, already handled in TxtTextBox_Enter
            }

            UpdateClearButtonVisibility();
        }

        private void UpdateClearButtonVisibility()
        {
            bool hasActualText = !isShowingPlaceholder && !string.IsNullOrEmpty(TxtTextBox.Text);
            BtnClear.Visible = hasActualText;
            TxtTextBox.Width = hasActualText ? BtnClear.Left - 4 : Width;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateClearButtonVisibility();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdatePlaceholder();
        }

        internal void Clear()
        {
            TxtTextBox.Clear();
        }
    }
}