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
            PnlUsedForScalingCompatibility = new Panel();
            PnlUsedForScalingCompatibility.SuspendLayout();
            SuspendLayout();
            // 
            // CheckBoxItem
            // 
            CheckBoxItem.AutoSize = true;
            CheckBoxItem.Location = new System.Drawing.Point(0, 0);
            CheckBoxItem.Name = "CheckBoxItem";
            CheckBoxItem.Size = new System.Drawing.Size(47, 19);
            CheckBoxItem.TabIndex = 0;
            CheckBoxItem.Text = "Text";
            CheckBoxItem.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            CheckBoxItem.UseVisualStyleBackColor = true;
            CheckBoxItem.CheckedChanged += CheckBoxItem_CheckedChanged;
            // 
            // LabelCount
            // 
            LabelCount.Anchor = AnchorStyles.None;
            LabelCount.AutoSize = true;
            LabelCount.Location = new System.Drawing.Point(127, 1);
            LabelCount.Margin = new Padding(0);
            LabelCount.Name = "LabelCount";
            LabelCount.Size = new System.Drawing.Size(43, 15);
            LabelCount.TabIndex = 1;
            LabelCount.Text = "999999";
            // 
            // PnlUsedForScalingCompatibility
            // 
            PnlUsedForScalingCompatibility.Controls.Add(LabelCount);
            PnlUsedForScalingCompatibility.Controls.Add(CheckBoxItem);
            PnlUsedForScalingCompatibility.Dock = DockStyle.Fill;
            PnlUsedForScalingCompatibility.Location = new System.Drawing.Point(0, 0);
            PnlUsedForScalingCompatibility.Name = "PnlUsedForScalingCompatibility";
            PnlUsedForScalingCompatibility.Size = new System.Drawing.Size(170, 19);
            PnlUsedForScalingCompatibility.TabIndex = 2;
            // 
            // UserControlLogMetadataFilterItem
            // 
            Controls.Add(PnlUsedForScalingCompatibility);
            Margin = new Padding(0);
            Name = "UserControlLogMetadataFilterItem";
            Size = new System.Drawing.Size(170, 19);
            SizeChanged += LogMetadataFilterItem_SizeChanged;
            PnlUsedForScalingCompatibility.ResumeLayout(false);
            PnlUsedForScalingCompatibility.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.CheckBox CheckBoxItem;
        private System.Windows.Forms.Label LabelCount;
        private Panel PnlUsedForScalingCompatibility;
    }
}

