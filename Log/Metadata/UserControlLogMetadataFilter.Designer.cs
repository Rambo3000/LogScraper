using System.Windows.Forms;

namespace LogScraper
{
    partial class UserControlLogMetadataFilter
    {
        private System.ComponentModel.IContainer components = null;
        private Label LblLogFilterDescription;
        private FlowLayoutPanel FlowLayoutPanelItems;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            LblLogFilterDescription = new Label();
            FlowLayoutPanelItems = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // LblLogFilterDescription
            // 
            LblLogFilterDescription.AutoSize = true;
            LblLogFilterDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            LblLogFilterDescription.Location = new System.Drawing.Point(3, 1);
            LblLogFilterDescription.Name = "LblLogFilterDescription";
            LblLogFilterDescription.Size = new System.Drawing.Size(32, 15);
            LblLogFilterDescription.TabIndex = 0;
            LblLogFilterDescription.Text = "Title";
            // 
            // FlowLayoutPanelItems
            // 
            FlowLayoutPanelItems.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FlowLayoutPanelItems.FlowDirection = FlowDirection.TopDown;
            FlowLayoutPanelItems.Location = new System.Drawing.Point(6, 20);
            FlowLayoutPanelItems.Name = "FlowLayoutPanelItems";
            FlowLayoutPanelItems.Size = new System.Drawing.Size(274, 200);
            FlowLayoutPanelItems.TabIndex = 2;
            FlowLayoutPanelItems.WrapContents = false;
            // 
            // UserControlLogMetadataFilter
            // 
            BackColor = System.Drawing.Color.White;
            Controls.Add(FlowLayoutPanelItems);
            Controls.Add(LblLogFilterDescription);
            Name = "UserControlLogMetadataFilter";
            Size = new System.Drawing.Size(280, 232);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
