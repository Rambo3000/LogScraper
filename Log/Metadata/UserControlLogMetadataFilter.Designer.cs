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
            ListViewItems.MouseClick += ListView_MouseClick;
            ListViewItems.MouseDown += ListView_MouseDown;
            ListViewItems.MouseWheel += ListView_MouseWheel;
            ListViewItems.Resize += ListView_Resize;
            // 
            // UserControlLogMetadataFilter
            // 
            BackColor = System.Drawing.Color.White;
            Controls.Add(LblLogFilterDescription);
            Controls.Add(ListViewItems);
            Name = "UserControlLogMetadataFilter";
            Size = new System.Drawing.Size(280, 232);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
