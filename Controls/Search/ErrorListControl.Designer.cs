using System.Drawing;
using System.Windows.Forms;
using LogScraper.Controls.Generic;

namespace LogScraper.Controls.Search
{
    partial class ErrorListControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorListControl));
            BtnClose = new Button();
            LblCount = new Label();
            PnlForScalingIssues = new Panel();
            LstEntries = new ListBox();
            PnlForScalingIssues.SuspendLayout();
            SuspendLayout();
            // 
            // BtnClose
            // 
            BtnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnClose.Image = (Image)resources.GetObject("BtnClose.Image");
            BtnClose.Location = new Point(130, 1);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new Size(20, 20);
            BtnClose.TabIndex = 0;
            BtnClose.UseVisualStyleBackColor = true;
            BtnClose.Click += BtnClose_Click;
            // 
            // LblCount
            // 
            LblCount.AutoSize = true;
            LblCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            LblCount.ForeColor = Color.FromArgb(192, 0, 0);
            LblCount.Location = new Point(0, 2);
            LblCount.Name = "LblCount";
            LblCount.Size = new Size(12, 15);
            LblCount.TabIndex = 1;
            LblCount.Text = "-";
            // 
            // PnlForScalingIssues
            // 
            PnlForScalingIssues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PnlForScalingIssues.Controls.Add(LstEntries);
            PnlForScalingIssues.Location = new Point(0, 22);
            PnlForScalingIssues.Name = "PnlForScalingIssues";
            PnlForScalingIssues.Size = new Size(150, 128);
            PnlForScalingIssues.TabIndex = 2;
            // 
            // LstEntries
            // 
            LstEntries.BorderStyle = BorderStyle.None;
            LstEntries.Dock = DockStyle.Fill;
            LstEntries.FormattingEnabled = true;
            LstEntries.IntegralHeight = false;
            LstEntries.Location = new Point(0, 0);
            LstEntries.Name = "LstEntries";
            LstEntries.Size = new Size(150, 128);
            LstEntries.TabIndex = 0;
            LstEntries.SelectedIndexChanged += LstEntries_SelectedIndexChanged;
            // 
            // ErrorListControl
            // 
            BackColor = SystemColors.Control;
            Controls.Add(PnlForScalingIssues);
            Controls.Add(LblCount);
            Controls.Add(BtnClose);
            Name = "ErrorListControl";
            PnlForScalingIssues.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button BtnClose;
        private Label LblCount;
        private Panel PnlForScalingIssues;
        private ListBox LstEntries;
    }
}

