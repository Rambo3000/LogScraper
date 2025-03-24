using System;
using System.Windows.Forms;

namespace LogScraper
{
    public partial class UserControlLogMetadataFilterItem : UserControl
    {
        private void InitializeComponent()
        {
            CheckBoxItem = new CheckBox();
            LabelCount = new Label();
            SuspendLayout();
            // 
            // CheckBoxItem
            // 
            CheckBoxItem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CheckBoxItem.Location = new System.Drawing.Point(0, 0);
            CheckBoxItem.Name = "CheckBoxItem";
            CheckBoxItem.Size = new System.Drawing.Size(134, 19);
            CheckBoxItem.TabIndex = 0;
            CheckBoxItem.Text = "Text";
            CheckBoxItem.UseVisualStyleBackColor = true;
            CheckBoxItem.CheckedChanged += CheckBoxItem_CheckedChanged;
            // 
            // LabelCount
            // 
            LabelCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LabelCount.Location = new System.Drawing.Point(127, 1);
            LabelCount.Margin = new Padding(0);
            LabelCount.Name = "LabelCount";
            LabelCount.Size = new System.Drawing.Size(47, 18);
            LabelCount.TabIndex = 1;
            LabelCount.Text = "999999";
            LabelCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // UserControlLogMetadataFilterItem
            // 
            Controls.Add(CheckBoxItem);
            Controls.Add(LabelCount);
            Margin = new Padding(0);
            Name = "UserControlLogMetadataFilterItem";
            Size = new System.Drawing.Size(170, 19);
            SizeChanged += LogMetadataFilterItem_SizeChanged;
            ResumeLayout(false);
        }

        private System.Windows.Forms.CheckBox CheckBoxItem;
        private System.Windows.Forms.Label LabelCount;
    }
}

