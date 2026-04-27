using System.Windows.Forms;

namespace LogScraper.Controls.Metadata
{
    partial class LogMetadataValueListControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            ListViewItems = new ListView();
            SuspendLayout();
            // 
            // ListViewItems
            // 
            ListViewItems.BackColor = System.Drawing.SystemColors.Control;
            ListViewItems.BorderStyle = BorderStyle.None;
            ListViewItems.Dock = DockStyle.Fill;
            ListViewItems.FullRowSelect = true;
            ListViewItems.HeaderStyle = ColumnHeaderStyle.None;
            ListViewItems.Location = new System.Drawing.Point(0, 0);
            ListViewItems.Margin = new Padding(0);
            ListViewItems.Name = "ListViewItems";
            ListViewItems.OwnerDraw = true;
            ListViewItems.Size = new System.Drawing.Size(280, 232);
            ListViewItems.TabIndex = 0;
            ListViewItems.UseCompatibleStateImageBehavior = false;
            ListViewItems.View = View.Details;
            ListViewItems.VirtualMode = true;
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
            // LogMetadataValueList
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ListViewItems);
            Margin = new Padding(0);
            Name = "LogMetadataValueList";
            Size = new System.Drawing.Size(280, 232);
            Load += LogMetadataValueList_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListView ListViewItems;
    }
}

