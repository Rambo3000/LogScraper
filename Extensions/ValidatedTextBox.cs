using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LogScraper.Extensions
{

    public class ValidatedTextBox : TextBox
    {
        private bool _isRequired = true;

        [Category("Validation")]
        [Description("Indicates whether the field must be filled in.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsRequired
        {
            get { return _isRequired; }
            set { _isRequired = value; Invalidate(); ValidateField(); }
        }
        private bool _isWhiteSpaceAllowed = false;

        [Category("Validation")]
        [Description("Indicates whether whitespace is accepted as a valid value")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsWhiteSpaceAllowed
        {
            get { return _isWhiteSpaceAllowed; }
            set { _isWhiteSpaceAllowed = value; Invalidate(); }
        }

        public bool IsValid
        {
            get
            {
                if (!IsRequired) return true;
                if (IsWhiteSpaceAllowed)
                {
                    return !string.IsNullOrEmpty(Text);
                }
                else
                {
                    // Check if the text is not null or whitespace
                    // and that it contains at least one non-whitespace character
                    return !string.IsNullOrWhiteSpace(Text);
                }
            }
        }

        private readonly Color _errorBackColor = Color.MistyRose;
        private readonly Color _defaultBackColor;

        public ValidatedTextBox()
        {
            _defaultBackColor = SystemColors.Window;
            TextChanged += ValidatedTextBox_TextChanged;
            Leave += ValidatedTextBox_Leave;
            EnabledChanged += ValidatedTextBox_EnabledChanged;
        }

        private void ValidatedTextBox_EnabledChanged(object sender, EventArgs e)
        {
            ValidateField();
        }

        private void ValidatedTextBox_Leave(object sender, EventArgs e)
        {
            ValidateField();
        }

        private void ValidatedTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateField();
        }

        public void ValidateField()
        {
            if (!IsValid && Enabled)
            {
                BackColor = _errorBackColor;
            }
            else
            {
                BackColor = _defaultBackColor;
            }
        }
    }
}