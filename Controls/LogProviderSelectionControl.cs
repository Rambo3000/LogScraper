using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;

namespace LogScraper.Controls
{
    public partial class LogProviderSelectionControl : UserControl
    {
        public enum StatusType
        {
            Retrieving,
            Processing,
            Finished
        }
        public event EventHandler<string> UriChanged;
        public event EventHandler CollapseStateChanged;

        private bool _isPinned = false;
        private bool _isCollapsed = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPinned
        {
            get { return _isPinned; }
            set
            {
                _isPinned = value;
                UpdatePinButtonImage();
            }
        }
        public bool IsCollapsed => _isCollapsed;
        public int ExpandedHeight { get; private set; }
        public int CollapsedHeight
        {
            get
            {
                return BtnCollapseExpand.Bottom;
            }
        }

        public void UpdateStatus(StatusType status)
        { 
            if (status == StatusType.Retrieving) LblStatusIcon.ImageIndex = 0;
            else if (status == StatusType.Processing) LblStatusIcon.ImageIndex = 1;

            LblStatusIcon.Visible = status != StatusType.Finished;

            //Force UI update to reflect status change immediately
            Application.DoEvents();
        }

        public LogProviderSelectionControl()
        {
            InitializeComponent();
            AttachEventHandlers();
            this.Leave += LogProviderSelectionControl_Leave;
            this.Resize += LogProviderSelectionControl_Resize;
            ExpandedHeight = Height;
        }

        private void AttachEventHandlers()
        {
            KubernetesProviderControl.SourceSelectionChanged += HandleSourceSelectionChanged;
            KubernetesProviderControl.StatusUpdate += HandleStatusUpdate;
            KubernetesProviderControl.UriChanged += HandleUriChanged;
            KubernetesProviderControl.IsSourceValidChanged += HandleIsSourceValidChanged;

            RuntimeProviderControl.SourceSelectionChanged += HandleSourceSelectionChanged;
            RuntimeProviderControl.StatusUpdate += HandleStatusUpdate;
            RuntimeProviderControl.UriChanged += HandleUriChanged;
            RuntimeProviderControl.IsSourceValidChanged += HandleIsSourceValidChanged;

            FileProviderControl.SourceSelectionChanged += HandleSourceSelectionChanged;
            FileProviderControl.StatusUpdate += HandleStatusUpdate;
            FileProviderControl.UriChanged += HandleUriChanged;
            FileProviderControl.IsSourceValidChanged += HandleIsSourceValidChanged;
        }

        private void HandleSourceSelectionChanged(object sender, EventArgs e)
        {
            LogAppState.Instance.Reset(keepFilters: false);
        }

        private void HandleStatusUpdate(string message, bool isSucces)
        {
            LogAppState.Instance.StatusMessage.Set((message, isSucces));
        }

        private void HandleUriChanged(object sender, string e)
        {
            UpdateProviderDescription();
            UriChanged?.Invoke(this, e);
        }

        private void HandleIsSourceValidChanged(object sender, bool e)
        {
            LogAppState.Instance.IsSourceValid.Set(e);

            // Force expand if source becomes invalid
            if (!e && _isCollapsed)
            {
                ExpandProvider();
            }
        }

        public void PopulateLogProviders()
        {
            cboLogProvider.Items.Clear();

            if (ConfigurationManager.LogProvidersConfig.FileConfig != null)
            {
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.FileConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.File)
                    cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.FileConfig;
            }

            if (ConfigurationManager.LogProvidersConfig.RuntimeConfig != null)
            {
                RuntimeProviderControl.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.RuntimeConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.Runtime)
                    cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig;
            }

            if (ConfigurationManager.LogProvidersConfig.KubernetesConfig != null)
            {
                KubernetesProviderControl.Update(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.Kubernetes)
                    cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.KubernetesConfig;
            }
        }

        public void PopulateLogLayouts(List<LogLayout> logLayouts)
        {
            cboLogLayout.Items.Clear();
            if (logLayouts != null)
            {
                cboLogLayout.Items.AddRange([.. logLayouts]);
                if (cboLogLayout.Items.Count > 0)
                {
                    // Select default layout for the currently selected provider
                    ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
                    if (logProviderConfig != null)
                    {
                        switch (logProviderConfig.LogProviderType)
                        {
                            case LogProviderType.Runtime:
                                cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig.DefaultLogLayout;
                                break;
                            case LogProviderType.Kubernetes:
                                cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.KubernetesConfig.DefaultLogLayout;
                                break;
                            case LogProviderType.File:
                                cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.FileConfig.DefaultLogLayout;
                                break;
                        }
                    }

                    // If no default layout was selected, select the first one
                    if (cboLogLayout.SelectedIndex == -1 && cboLogLayout.Items.Count > 0)
                    {
                        cboLogLayout.SelectedIndex = 0;
                    }

                    CboLogLayout_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        public ISourceAdapter GetSelectedSourceAdapter(DateTime? lastTrailTime = null)
        {
            LogProviderType logProviderType = ((ILogProviderConfig)cboLogProvider.SelectedItem).LogProviderType;
            return logProviderType switch
            {
                LogProviderType.Runtime => RuntimeProviderControl.GetSourceAdapter(),
                LogProviderType.Kubernetes => KubernetesProviderControl.GetSourceAdapter(null, lastTrailTime),
                LogProviderType.File => FileProviderControl.GetSourceAdapter(),
                _ => throw new NotImplementedException()
            };
        }

        public ILogProviderConfig GetSelectedLogProviderConfig()
        {
            ILogProviderConfig config = cboLogProvider.SelectedItem as ILogProviderConfig;
            return config;
        }

        private LogLayout GetSelectedLogLayout()
        {
            return cboLogLayout.SelectedItem as LogLayout;
        }

        private void CboLogProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
            if (logProviderConfig == null) return;

            RuntimeProviderControl.Visible = logProviderConfig.LogProviderType == LogProviderType.Runtime;
            KubernetesProviderControl.Visible = logProviderConfig.LogProviderType == LogProviderType.Kubernetes;
            FileProviderControl.Visible = logProviderConfig.LogProviderType == LogProviderType.File;

            switch (logProviderConfig.LogProviderType)
            {
                case LogProviderType.Runtime:
                    RuntimeProviderControl.UpdateUri();
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig.DefaultLogLayout;
                    break;
                case LogProviderType.Kubernetes:
                    KubernetesProviderControl.UpdateUri();
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.KubernetesConfig.DefaultLogLayout;
                    break;
                case LogProviderType.File:
                    FileProviderControl.UpdateUri();
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.FileConfig.DefaultLogLayout;
                    break;
            }

            if (cboLogLayout.SelectedIndex == -1 && cboLogLayout.Items.Count > 0)
            {
                cboLogLayout.SelectedIndex = 0;
            }

            UpdateProviderDescription();
            LogAppState.Instance.Reset(keepFilters: false);
        }

        private void CboLogLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProviderDescription();

            LogAppState.Instance.Layout.Set(GetSelectedLogLayout());
            LogAppState.Instance.Reset(false);
        }

        public void UpdateProviderConfig()
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
            if (logProviderConfig == null) return;

            switch (logProviderConfig.LogProviderType)
            {
                case LogProviderType.Runtime:
                    RuntimeProviderControl.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                    RuntimeProviderControl.UpdateUri();
                    break;
                case LogProviderType.Kubernetes:
                    KubernetesProviderControl.UpdateUri();
                    break;
                case LogProviderType.File:
                    FileProviderControl.UpdateUri();
                    break;
            }
        }

        public void SetEnabled(bool enabled)
        {
            cboLogProvider.Enabled = enabled;
            cboLogLayout.Enabled = enabled;
            PnlLogProviders.Enabled = enabled;
        }

        public bool IsSourceValid
        {
            get
            {
                LogProviderType logProviderType = ((ILogProviderConfig)cboLogProvider.SelectedItem)?.LogProviderType ?? LogProviderType.File;
                return logProviderType switch
                {
                    LogProviderType.Runtime => RuntimeProviderControl.IsSourceValid,
                    LogProviderType.Kubernetes => KubernetesProviderControl.IsSourceValid,
                    LogProviderType.File => FileProviderControl.IsSourceValid,
                    _ => false
                };
            }
        }
        private void CollapseProvider()
        {
            lblLogProvider.Visible = false;
            cboLogProvider.Visible = false;
            lblLogLayout.Visible = false;
            cboLogLayout.Visible = false;
            PnlLogProviders.Visible = false;
            BtnCollapseExpand.ImageIndex = 1;
            btnPin.Visible = false;
            _isCollapsed = true;
            UpdateProviderDescription();
            AutoSize = false;
            Height = CollapsedHeight;

            CollapseStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ExpandProvider()
        {
            lblLogProvider.Visible = true;
            cboLogProvider.Visible = true;
            lblLogLayout.Visible = true;
            cboLogLayout.Visible = true;
            PnlLogProviders.Visible = true;
            BtnCollapseExpand.ImageIndex = 0;
            btnPin.Visible = true;
            _isCollapsed = false;

            AutoSize = false;
            Height = ExpandedHeight;
            CollapseStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateProviderDescription()
        {
            ILogProviderConfig config = cboLogProvider.SelectedItem as ILogProviderConfig;
            LogLayout layout = cboLogLayout.SelectedItem as LogLayout;

            string layoutText = layout?.Description ?? "Unknown";

            lblProviderDescription.Text = $"{config} - {layoutText}";
        }

        private void TogglePinButton()
        {
            _isPinned = !_isPinned;
            UpdatePinButtonImage();
        }

        private void UpdatePinButtonImage()
        {
            btnPin.ImageIndex = _isPinned ? 1 : 0;
        }

        private void LogProviderSelectionControl_Leave(object sender, EventArgs e)
        {
            if (IsSourceValid && !_isPinned && !_isCollapsed)
            {
                CollapseProvider();
            }
        }

        private void LogProviderSelectionControl_Resize(object sender, EventArgs e)
        {
            lblProviderDescription.Invalidate();
        }

        private void BtnPin_Click(object sender, EventArgs e)
        {
            TogglePinButton();
        }

        private void BtnCollapseExpand_Click(object sender, EventArgs e)
        {
            if (_isCollapsed)
                ExpandProvider();
            else if (IsSourceValid)
                CollapseProvider();
        }

        private void LblProviderDescription_MouseClick(object sender, MouseEventArgs e)
        {
            ExpandProvider();
        }

        private void LblProviderDescription_MouseEnter(object sender, EventArgs e)
        {
            lblProviderDescription.LinkBehavior = LinkBehavior.AlwaysUnderline;
        }

        private void LblProviderDescription_MouseLeave(object sender, EventArgs e)
        {
            lblProviderDescription.LinkBehavior = LinkBehavior.NeverUnderline;
        }
    }
}
