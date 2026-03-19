using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;

namespace LogScraper
{
    partial class UserControlLogMetadataFilter
    {
        private System.ComponentModel.IContainer components = null;
        private Label LblLogFilterDescription;
        private ListView ListViewItems;

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
            ListViewItems = new ListView();
            LblIncludeExclude = new Label();
            SuspendLayout();
            // 
            // LblLogFilterDescription
            // 
            LblLogFilterDescription.AutoSize = true;
            LblLogFilterDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            LblLogFilterDescription.Location = new System.Drawing.Point(1, 1);
            LblLogFilterDescription.Name = "LblLogFilterDescription";
            LblLogFilterDescription.Size = new System.Drawing.Size(32, 15);
            LblLogFilterDescription.TabIndex = 0;
            LblLogFilterDescription.Text = "Title";
            // 
            // ListViewItems
            // 
            ListViewItems.BorderStyle = BorderStyle.None;
            ListViewItems.Dock = DockStyle.Fill;
            ListViewItems.FullRowSelect = true;
            ListViewItems.HeaderStyle = ColumnHeaderStyle.None;
            ListViewItems.Location = new System.Drawing.Point(0, 0);
            ListViewItems.Margin = new Padding(0);
            ListViewItems.Name = "ListViewItems";
            ListViewItems.OwnerDraw = true;
            ListViewItems.Size = new System.Drawing.Size(280, 232);
            ListViewItems.TabIndex = 3;
            ListViewItems.UseCompatibleStateImageBehavior = false;
            ListViewItems.View = View.Details;
            ListViewItems.VirtualMode = true;
            ListViewItems.Columns.Add("Description", -2);
            ListViewItems.Columns.Add("Count", 50, HorizontalAlignment.Right);
            ListViewItems.DrawColumnHeader += ListView_DrawColumnHeader;
            ListViewItems.DrawItem += ListView_DrawItem;
            ListViewItems.DrawSubItem += ListView_DrawSubItem;
            ListViewItems.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
            ListViewItems.DoubleClick += ListViewItems_DoubleClick;
            ListViewItems.MouseClick += ListView_MouseClick;
            ListViewItems.MouseDown += ListView_MouseDown;
            ListViewItems.MouseWheel += ListView_MouseWheel;
            ListViewItems.Resize += ListView_Resize;
            // 
            // LblIncludeExclude
            // 
            LblIncludeExclude.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LblIncludeExclude.Location = new System.Drawing.Point(239, 1);
            LblIncludeExclude.Name = "LblIncludeExclude";
            LblIncludeExclude.Size = new System.Drawing.Size(38, 15);
            LblIncludeExclude.TabIndex = 4;
            LblIncludeExclude.Paint += LblIncludeExclude_Paint;
            LblIncludeExclude.MouseClick += LblIncludeExclude_MouseClick;
            // 
            // UserControlLogMetadataFilter
            // 
            BackColor = System.Drawing.Color.White;
            Controls.Add(LblIncludeExclude);
            Controls.Add(LblLogFilterDescription);
            Controls.Add(ListViewItems);
            Name = "UserControlLogMetadataFilter";
            Size = new System.Drawing.Size(280, 232);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label LblIncludeExclude;
    }
}
