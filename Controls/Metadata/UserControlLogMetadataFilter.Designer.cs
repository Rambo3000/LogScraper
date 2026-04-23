using System.Windows.Forms;

namespace LogScraper.Controls.Metadata
{
    partial class UserControlLogMetadataFilter
    {
        private System.ComponentModel.IContainer components = null;
        private LogMetadataValueList ValueList;

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
            PnlHeader = new DoubleBufferedPanel();
            ValueList = new LogMetadataValueList();
            LblIncludeExclude = new Label();
            SuspendLayout();
            // 
            // PnlHeader
            // 
            PnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PnlHeader.BackColor = System.Drawing.SystemColors.Control;
            PnlHeader.Cursor = Cursors.Hand;
            PnlHeader.Location = new System.Drawing.Point(0, 0);
            PnlHeader.Name = "PnlHeader";
            PnlHeader.Size = new System.Drawing.Size(280, 20);
            PnlHeader.TabIndex = 0;
            PnlHeader.Paint += PnlHeader_Paint;
            PnlHeader.MouseClick += Header_Click;
            PnlHeader.MouseEnter += Header_MouseEnter;
            PnlHeader.MouseLeave += Header_MouseLeave;
            // 
            // ValueList
            // 
            ValueList.Dock = DockStyle.Bottom;
            ValueList.Location = new System.Drawing.Point(0, 23);
            ValueList.Margin = new Padding(0);
            ValueList.Name = "ValueList";
            ValueList.Size = new System.Drawing.Size(280, 209);
            ValueList.TabIndex = 3;
            ValueList.MouseEnter += ValueList_MouseEnter;
            // 
            // LblIncludeExclude
            // 
            LblIncludeExclude.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LblIncludeExclude.BackColor = System.Drawing.Color.Transparent;
            LblIncludeExclude.Cursor = Cursors.Hand;
            LblIncludeExclude.Location = new System.Drawing.Point(239, 2);
            LblIncludeExclude.Name = "LblIncludeExclude";
            LblIncludeExclude.Size = new System.Drawing.Size(38, 15);
            LblIncludeExclude.TabIndex = 4;
            LblIncludeExclude.TextAlign = System.Drawing.ContentAlignment.TopRight;
            LblIncludeExclude.Visible = false;
            LblIncludeExclude.Paint += LblIncludeExclude_Paint;
            LblIncludeExclude.MouseClick += LblIncludeExclude_MouseClick;
            LblIncludeExclude.MouseEnter += Header_MouseEnter;
            LblIncludeExclude.MouseLeave += Header_MouseLeave;
            // 
            // UserControlLogMetadataFilter
            // 
            BackColor = System.Drawing.SystemColors.Control;
            Controls.Add(LblIncludeExclude);
            Controls.Add(PnlHeader);
            Controls.Add(ValueList);
            Name = "UserControlLogMetadataFilter";
            Size = new System.Drawing.Size(280, 232);
            ResumeLayout(false);
        }

        #endregion

        private Label LblIncludeExclude;
        private DoubleBufferedPanel PnlHeader;
    }
}
