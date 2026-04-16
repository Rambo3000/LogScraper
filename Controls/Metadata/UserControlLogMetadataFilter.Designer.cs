using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;

namespace LogScraper.Controls.Metadata
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLogMetadataFilter));
            LblLogFilterDescription = new Label();
            ListViewItems = new ListView();
            LblIncludeExclude = new Label();
            imageList1 = new ImageList(components);
            BtnChevron = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)BtnChevron).BeginInit();
            SuspendLayout();
            // 
            // LblLogFilterDescription
            // 
            LblLogFilterDescription.AutoSize = true;
            LblLogFilterDescription.BackColor = System.Drawing.SystemColors.Control;
            LblLogFilterDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            LblLogFilterDescription.Location = new System.Drawing.Point(25, 1);
            LblLogFilterDescription.Name = "LblLogFilterDescription";
            LblLogFilterDescription.Size = new System.Drawing.Size(32, 15);
            LblLogFilterDescription.TabIndex = 0;
            LblLogFilterDescription.Text = "Title";
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
            ListViewItems.TabIndex = 3;
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
            // LblIncludeExclude
            // 
            LblIncludeExclude.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LblIncludeExclude.BackColor = System.Drawing.SystemColors.Control;
            LblIncludeExclude.Location = new System.Drawing.Point(239, 1);
            LblIncludeExclude.Name = "LblIncludeExclude";
            LblIncludeExclude.Size = new System.Drawing.Size(38, 15);
            LblIncludeExclude.TabIndex = 4;
            LblIncludeExclude.Paint += LblIncludeExclude_Paint;
            LblIncludeExclude.MouseClick += LblIncludeExclude_MouseClick;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "chevron-right 16x16.png");
            imageList1.Images.SetKeyName(1, "chevron-down.png");
            // 
            // BtnChevron
            // 
            BtnChevron.BackColor = System.Drawing.SystemColors.Control;
            BtnChevron.Cursor = Cursors.Hand;
            BtnChevron.Location = new System.Drawing.Point(7, 1);
            BtnChevron.Name = "BtnChevron";
            BtnChevron.Size = new System.Drawing.Size(16, 16);
            BtnChevron.SizeMode = PictureBoxSizeMode.Zoom;
            BtnChevron.TabIndex = 5;
            BtnChevron.TabStop = false;
            BtnChevron.Click += BtnChevron_Click;
            // 
            // UserControlLogMetadataFilter
            // 
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(LblIncludeExclude);
            Controls.Add(LblLogFilterDescription);
            Controls.Add(BtnChevron);
            Controls.Add(ListViewItems);
            Name = "UserControlLogMetadataFilter";
            Size = new System.Drawing.Size(280, 232);
            ((System.ComponentModel.ISupportInitialize)BtnChevron).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label LblIncludeExclude;
        private ImageList imageList1;
        private PictureBox BtnChevron;
    }
}


