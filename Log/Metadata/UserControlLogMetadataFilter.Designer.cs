namespace LogScraper
{
    partial class UserControlLogMetadataFilter
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
            LblLogFilterDescription = new System.Windows.Forms.Label();
            BtnOpenOrCollapse = new System.Windows.Forms.Button();
            LstFilterValues = new System.Windows.Forms.ListView();
            SuspendLayout();
            // 
            // LblLogFilterDescription
            // 
            LblLogFilterDescription.AutoSize = true;
            LblLogFilterDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            LblLogFilterDescription.Location = new System.Drawing.Point(20, 3);
            LblLogFilterDescription.Name = "LblLogFilterDescription";
            LblLogFilterDescription.Size = new System.Drawing.Size(77, 15);
            LblLogFilterDescription.TabIndex = 8;
            LblLogFilterDescription.Text = "Projectnaam";
            // 
            // BtnOpenOrCollapse
            // 
            BtnOpenOrCollapse.FlatAppearance.BorderSize = 0;
            BtnOpenOrCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BtnOpenOrCollapse.Image = Properties.Resources.chevron_down;
            BtnOpenOrCollapse.Location = new System.Drawing.Point(3, 0);
            BtnOpenOrCollapse.Margin = new System.Windows.Forms.Padding(0);
            BtnOpenOrCollapse.Name = "BtnOpenOrCollapse";
            BtnOpenOrCollapse.Size = new System.Drawing.Size(18, 21);
            BtnOpenOrCollapse.TabIndex = 9;
            BtnOpenOrCollapse.UseVisualStyleBackColor = false;
            BtnOpenOrCollapse.Click += BtnOpenOrCollapse_Click;
            // 
            // LstFilterValues
            // 
            LstFilterValues.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LstFilterValues.BackColor = System.Drawing.SystemColors.Window;
            LstFilterValues.BorderStyle = System.Windows.Forms.BorderStyle.None;
            LstFilterValues.CheckBoxes = true;
            LstFilterValues.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            LstFilterValues.HideSelection = true;
            LstFilterValues.ImeMode = System.Windows.Forms.ImeMode.Off;
            LstFilterValues.Location = new System.Drawing.Point(3, 21);
            LstFilterValues.MultiSelect = false;
            LstFilterValues.Name = "LstFilterValues";
            LstFilterValues.Scrollable = false;
            LstFilterValues.Size = new System.Drawing.Size(144, 54);
            LstFilterValues.Sorting = System.Windows.Forms.SortOrder.Ascending;
            LstFilterValues.TabIndex = 10;
            LstFilterValues.UseCompatibleStateImageBehavior = false;
            LstFilterValues.View = System.Windows.Forms.View.Details;
            LstFilterValues.ItemCheck += LstFilterValues_ItemCheck;
            LstFilterValues.ItemChecked += LstFilterValues_ItemChecked;
            LstFilterValues.ItemSelectionChanged += LstFilterValues_ItemSelectionChanged;
            LstFilterValues.Resize += LstFilterValues_Resize;
            // 
            // UserControlLogMetadataFilter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackColor = System.Drawing.SystemColors.Window;
            Controls.Add(LstFilterValues);
            Controls.Add(BtnOpenOrCollapse);
            Controls.Add(LblLogFilterDescription);
            Name = "UserControlLogMetadataFilter";
            Size = new System.Drawing.Size(147, 75);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label LblLogFilterDescription;
        private System.Windows.Forms.Button BtnOpenOrCollapse;
        private System.Windows.Forms.ListView LstFilterValues;
    }
}
