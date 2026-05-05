using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;

namespace LogScraper.Controls
{
    public partial class LogProviderSelectionControl : UserControl
    {
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
        public int ExpandedHeight => PnlLogProviders.Bottom;
        public int CollapsedHeight => BtnCollapseExpand.Bottom;

        private void UpdateProcessingStatus()
        {
            LogProcessingStatus status = LogAppState.Instance.ProcessingStatus.Value;
            if (status == LogProcessingStatus.Retrieving)
            {
                LblStatusIcon.ImageIndex = 0;
                toolTip1.SetToolTip(LblStatusIcon, "Data ontvangen...");
            }
            else if (status == LogProcessingStatus.Processing)
            {
                LblStatusIcon.ImageIndex = 1;
                toolTip1.SetToolTip(LblStatusIcon, "Data verwerken...");
            }
            else if (status == LogProcessingStatus.Waiting)
            {
                LblStatusIcon.ImageIndex = 2;
                toolTip1.SetToolTip(LblStatusIcon, "Wachten...");
            }

            LblStatusIcon.Visible = status != LogProcessingStatus.Idle;

            //Force UI update to reflect status change immediately
            Application.DoEvents();
        }

        public LogProviderSelectionControl()
        {
            InitializeComponent();
            this.Leave += LogProviderSelectionControl_Leave;
            this.Resize += LogProviderSelectionControl_Resize;

            LogAppState.Instance.IsSourceProcessingActive.Changed += (s, e) => SetEnabled();
            LogAppState.Instance.ProcessingStatus.Changed += (s, e) => UpdateProcessingStatus();
            LogAppState.Instance.SourceAdapterProvider = GetSelectedSourceAdapter;

            ConfigAppState.Instance.LogLayoutsConfig.Changed += (s, e) => PopulateLogLayouts();
            ConfigAppState.Instance.LogProvidersConfig.Changed += (s, e) => UpdateProviderConfig();
            ConfigAppState.Instance.LogProvidersConfig.Changed += (s, e) => PopulateLogProviders();
            ConfigAppState.Instance.GenericConfig.Changed += (s, e) => UpdatedPinned();
        }

        private void UpdatedPinned()
        {
            IsPinned = ConfigAppState.Instance.GenericConfig.Value?.PinLogProvidersByDefault ?? false;
        }

        private void LogProviderSelectionControl_Load(object sender, EventArgs e)
        {
            IsPinned = ConfigAppState.Instance.GenericConfig.Value.PinLogProvidersByDefault;
            PopulateLogProviders();
            PopulateLogLayouts();
            //TODO: check what to do with UpdateProviderConfig, maybe remove? what does it do?
            UpdateProviderConfig();
            AttachEventHandlers();
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

        private void PopulateLogProviders()
        {
            cboLogProvider.Items.Clear();

            if (ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig != null)
            {
                cboLogProvider.Items.Add(ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig);
                if (ConfigAppState.Instance.GenericConfig.Value.LogProviderTypeDefault == LogProviderType.File)
                    cboLogProvider.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig;
            }

            if (ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig != null)
            {
                RuntimeProviderControl.UpdateRuntimeInstances(ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig.Instances);
                cboLogProvider.Items.Add(ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig);
                if (ConfigAppState.Instance.GenericConfig.Value.LogProviderTypeDefault == LogProviderType.Runtime)
                    cboLogProvider.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig;
            }

            if (ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig != null)
            {
                KubernetesProviderControl.Update(ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig);
                cboLogProvider.Items.Add(ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig);
                if (ConfigAppState.Instance.GenericConfig.Value.LogProviderTypeDefault == LogProviderType.Kubernetes)
                    cboLogProvider.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig;
            }
        }

        public void PopulateLogLayouts()
        {
            List<LogLayout> logLayouts = ConfigAppState.Instance.LogLayoutsConfig.Value.layouts;

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
                                cboLogLayout.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig.DefaultLogLayout;
                                break;
                            case LogProviderType.Kubernetes:
                                cboLogLayout.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig.DefaultLogLayout;
                                break;
                            case LogProviderType.File:
                                cboLogLayout.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig.DefaultLogLayout;
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
                    RuntimeProviderControl.UpdateAfterProviderSelected();
                    cboLogLayout.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig.DefaultLogLayout;
                    break;
                case LogProviderType.Kubernetes:
                    KubernetesProviderControl.UpdateAfterProviderSelected();
                    cboLogLayout.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig.DefaultLogLayout;
                    break;
                case LogProviderType.File:
                    FileProviderControl.UpdateAfterProviderSelected();
                    cboLogLayout.SelectedItem = ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig.DefaultLogLayout;
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

        private void UpdateProviderConfig()
        {
            ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
            if (logProviderConfig == null) return;

            switch (logProviderConfig.LogProviderType)
            {
                case LogProviderType.Runtime:
                    RuntimeProviderControl.UpdateRuntimeInstances(ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig.Instances);
                    RuntimeProviderControl.UpdateAfterProviderSelected();
                    break;
                case LogProviderType.Kubernetes:
                    KubernetesProviderControl.UpdateAfterProviderSelected();
                    break;
                case LogProviderType.File:
                    FileProviderControl.UpdateAfterProviderSelected();
                    break;
            }
        }

        public void SetEnabled()
        {
            bool enabled = !LogAppState.Instance.IsSourceProcessingActive.Value;
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
