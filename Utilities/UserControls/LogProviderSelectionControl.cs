using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.Layout;
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;

namespace LogScraper.Utilities.UserControls
{
    public partial class LogProviderSelectionControl : UserControl
    {
        public event EventHandler SourceSelectionChanged;
        public event EventHandler<(string message, bool isSuccess)> StatusUpdate;
        public event EventHandler<string> UriChanged;
        public event EventHandler<bool> IsSourceValidChanged;
        public event EventHandler LogProviderChanged;
        public event EventHandler LogLayoutChanged;
        public event EventHandler CollapseStateChanged;

        private bool _isPinned = false;
        private bool _isCollapsed = false;

        public bool IsPinned => _isPinned;
        public bool IsCollapsed => _isCollapsed;
        public int ExpandedHeight { get; private set; }
        public int CollapsedHeight
        {
            get
            {
                return BtnCollapseExpand.Bottom;
            }
        }


        public LogProviderSelectionControl()
        {
            InitializeComponent();
            AttachEventHandlers();
            this.Leave += LogProviderSelectionControl_Leave;
            this.Resize += LogProviderSelectionControl_Resize;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ExpandedHeight = Height;
        }

        private void AttachEventHandlers()
        {
            usrKubernetes.SourceSelectionChanged += HandleSourceSelectionChanged;
            usrKubernetes.StatusUpdate += HandleStatusUpdate;
            usrKubernetes.UriChanged += HandleUriChanged;
            usrKubernetes.IsSourceValidChanged += HandleIsSourceValidChanged;

            usrRuntime.SourceSelectionChanged += HandleSourceSelectionChanged;
            usrRuntime.StatusUpdate += HandleStatusUpdate;
            usrRuntime.UriChanged += HandleUriChanged;
            usrRuntime.IsSourceValidChanged += HandleIsSourceValidChanged;

            usrFileLogProvider.SourceSelectionChanged += HandleSourceSelectionChanged;
            usrFileLogProvider.StatusUpdate += HandleStatusUpdate;
            usrFileLogProvider.UriChanged += HandleUriChanged;
            usrFileLogProvider.IsSourceValidChanged += HandleIsSourceValidChanged;
        }

        private void HandleSourceSelectionChanged(object sender, EventArgs e)
        {
            SourceSelectionChanged?.Invoke(this, e);
        }

        private void HandleStatusUpdate(string message, bool isSuccess)
        {
            StatusUpdate?.Invoke(this, (message, isSuccess));
        }

        private void HandleUriChanged(object sender, string e)
        {
            UpdateProviderDescription();
            UriChanged?.Invoke(this, e);
        }

        private void HandleIsSourceValidChanged(object sender, bool e)
        {
            IsSourceValidChanged?.Invoke(this, e);

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
                usrRuntime.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                cboLogProvider.Items.Add(ConfigurationManager.LogProvidersConfig.RuntimeConfig);
                if (ConfigurationManager.GenericConfig.LogProviderTypeDefault == LogProviderType.Runtime)
                    cboLogProvider.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig;
            }

            if (ConfigurationManager.LogProvidersConfig.KubernetesConfig != null)
            {
                usrKubernetes.Update(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
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
                LogProviderType.Runtime => usrRuntime.GetSourceAdapter(),
                LogProviderType.Kubernetes => usrKubernetes.GetSourceAdapter(null, lastTrailTime),
                LogProviderType.File => usrFileLogProvider.GetSourceAdapter(),
                _ => throw new NotImplementedException()
            };
        }

        public ILogProviderConfig GetSelectedLogProviderConfig()
        {
            ILogProviderConfig config = cboLogProvider.SelectedItem as ILogProviderConfig;
            return config;
        }

        public LogLayout GetSelectedLogLayout()
        {
            return cboLogLayout.SelectedItem as LogLayout;
        }

        private void CboLogProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
            if (logProviderConfig == null) return;

            usrRuntime.Visible = logProviderConfig.LogProviderType == LogProviderType.Runtime;
            usrKubernetes.Visible = logProviderConfig.LogProviderType == LogProviderType.Kubernetes;
            usrFileLogProvider.Visible = logProviderConfig.LogProviderType == LogProviderType.File;

            switch (logProviderConfig.LogProviderType)
            {
                case LogProviderType.Runtime:
                    usrRuntime.UpdateUri();
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.RuntimeConfig.DefaultLogLayout;
                    break;
                case LogProviderType.Kubernetes:
                    usrKubernetes.UpdateUri();
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.KubernetesConfig.DefaultLogLayout;
                    break;
                case LogProviderType.File:
                    usrFileLogProvider.UpdateUri();
                    cboLogLayout.SelectedItem = ConfigurationManager.LogProvidersConfig.FileConfig.DefaultLogLayout;
                    break;
            }

            if (cboLogLayout.SelectedIndex == -1 && cboLogLayout.Items.Count > 0)
            {
                cboLogLayout.SelectedIndex = 0;
            }

            UpdateProviderDescription();
            LogProviderChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CboLogLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProviderDescription();
            LogLayoutChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateProviderConfig()
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
            if (logProviderConfig == null) return;

            switch (logProviderConfig.LogProviderType)
            {
                case LogProviderType.Runtime:
                    usrRuntime.UpdateRuntimeInstances(ConfigurationManager.LogProvidersConfig.RuntimeConfig.Instances);
                    usrRuntime.UpdateUri();
                    break;
                case LogProviderType.Kubernetes:
                    usrKubernetes.UpdateUri();
                    break;
                case LogProviderType.File:
                    usrFileLogProvider.UpdateUri();
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
                    LogProviderType.Runtime => usrRuntime.IsSourceValid,
                    LogProviderType.Kubernetes => usrKubernetes.IsSourceValid,
                    LogProviderType.File => usrFileLogProvider.IsSourceValid,
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

        private void lblProviderDescription_MouseClick(object sender, MouseEventArgs e)
        {
            ExpandProvider();
        }

        private void lblProviderDescription_MouseEnter(object sender, EventArgs e)
        {
            lblProviderDescription.LinkBehavior = LinkBehavior.AlwaysUnderline;
        }

        private void lblProviderDescription_MouseLeave(object sender, EventArgs e)
        {
            lblProviderDescription.LinkBehavior = LinkBehavior.NeverUnderline;
        }
    }
}
