using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Log.Content;
using LogScraper.Log.Layout;

namespace LogScraper.Log.FlowTree
{
    public partial class FlowTreeControl : UserControl
    {
        public event EventHandler ShowTreeStateChanged;

        public FlowTreeControl()
        {
            InitializeComponent();
            ItemShowTree.Click += ItemShowTree_Click;
            ItemHideTree.Click += ItemHideTree_Click;
            CboContentProperties.SelectedIndexChanged += CboContentProperties_SelectedIndexChanged;
            UpdateControls();
        }

        private bool showTree = false;

        public bool ShowTree
        {
            get
            {
                return showTree;
            }
        }

        public void UpdateLogLayout(LogLayout logLayout)
        {

            CboContentProperties.Items.Clear();

            if (logLayout == null || logLayout.LogContentProperties == null || logLayout.LogContentProperties.Count == 0)
            {
                UpdateControls();
                return;
            }

            List<LogContentProperty> logContentProperties = [.. logLayout.LogContentProperties.Where(item => item.IsBeginFlowTreeFilter)];
            if (logContentProperties.Count > 0)
            {
                CboContentProperties.Items.AddRange([.. logContentProperties]);
                CboContentProperties.SelectedIndex = 0;
            }
            UpdateControls();
        }

        private bool updateShowTreeInProgress = false;
        private void UpdateControls()
        {
            if (updateShowTreeInProgress) return;

            splitButton1.ImageIndex = showTree ? 1 : 0;

            updateShowTreeInProgress = true;
            if (CboContentProperties.Items.Count == 0)
            {
                ItemShowTree.Enabled = false;
                ItemHideTree.Enabled = false;
                CboContentProperties.Enabled = false;
                updateShowTreeInProgress = false;
                return;
            }

            //Also show no tree if previously no tree was available
            ItemShowTree.Enabled = !showTree;
            ItemHideTree.Enabled = showTree;
            CboContentProperties.Enabled = showTree && CboContentProperties.Items.Count > 1;

            updateShowTreeInProgress = false;
        }


        #region User control events

        private void SplitButton1_Click(object sender, EventArgs e)
        {
            showTree = !showTree;
            UpdateControls();
            ShowTreeStateChanged?.Invoke(this, EventArgs.Empty);
            ActiveControl = null;
        }
        private void ItemHideTree_Click(object sender, System.EventArgs e)
        {
            if (updateShowTreeInProgress) return;

            showTree = false;
            UpdateControls();
            ShowTreeStateChanged?.Invoke(this, EventArgs.Empty);
            ActiveControl = null;
        }

        private void ItemShowTree_Click(object sender, System.EventArgs e)
        {
            if (updateShowTreeInProgress) return;

            showTree = true;
            UpdateControls();
            ShowTreeStateChanged?.Invoke(this, EventArgs.Empty);
            ActiveControl = null;
        }

        private void CboContentProperties_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (updateShowTreeInProgress) return;

            ShowTreeStateChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        public LogContentProperty SelectedContentProperty
        {
            get
            {
                if (CboContentProperties.SelectedItem == null) return null;
                return ((LogContentProperty)CboContentProperties.SelectedItem);
            }
        }
    }
}
