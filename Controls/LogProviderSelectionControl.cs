using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Log.Layout;
using LogScraper.Log.LogAppState;
using LogScraper.Log.Processing;
using LogScraper.LogProviders;
using LogScraper.Sources.Adapters;
using LogScraper.Utilities;

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
            bool enabled = !LogAppState.Instance.ProcessingState.Value.IsActive;
            cboLogProvider.Enabled = enabled;
            cboLogLayout.Enabled = enabled;
            PnlLogProviders.Enabled = enabled;

            LogProcessingStatus status = LogAppState.Instance.ProcessingState.Value?.Status ?? LogProcessingStatus.Idle;
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

            ShortcutManager.Register(this, AppShortcut.CollapseExpandProvider, () => ToggleCollapse());
            ShortcutManager.Register(this, AppShortcut.CloseBottomPanel, () => { if (!_isCollapsed) CollapseProvider(); });
            LogAppState.Instance.ProcessingState.Changed += (s, e) => UpdateProcessingStatus();
            LogAppState.Instance.SourceAdapterProvider = GetSelectedSourceAdapter;

            ConfigAppState.Instance.LogLayoutsConfig.Changed += (s, e) => PopulateLogLayouts();
            ConfigAppState.Instance.LogProvidersConfig.Changed += (s, e) => UpdateSelectedLogProvider();
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
            UpdateSelectedLogProvider();
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

            // Store the description of the currently active layout to re-select it
            string currentLayoutDescription = LogAppState.Instance.Layout.Value?.Description;
            cboLogLayout.Items.Clear();
            if (logLayouts != null)
            {
                cboLogLayout.Items.AddRange([.. logLayouts]);
                if (cboLogLayout.Items.Count > 0)
                {
                    // Try to re-select the layout that was active before (by description)
                    LogLayout layoutToSelect = null;

                    if (!string.IsNullOrEmpty(currentLayoutDescription))
                    {
                        layoutToSelect = logLayouts.FirstOrDefault(l => l.Description == currentLayoutDescription);
                    }

                    // If we couldn't find the previously active layout, use the default for the current provider
                    if (layoutToSelect == null)
                    {
                        ILogProviderConfig logProviderConfig = (ILogProviderConfig)cboLogProvider.SelectedItem;
                        if (logProviderConfig != null)
                        {
                            layoutToSelect = logProviderConfig.LogProviderType switch
                            {
                                LogProviderType.Runtime => ConfigAppState.Instance.LogProvidersConfig.Value.RuntimeConfig.DefaultLogLayout,
                                LogProviderType.Kubernetes => ConfigAppState.Instance.LogProvidersConfig.Value.KubernetesConfig.DefaultLogLayout,
                                LogProviderType.File => ConfigAppState.Instance.LogProvidersConfig.Value.FileConfig.DefaultLogLayout,
                                _ => null
                            };
                        }
                    }

                    // If we still don't have a layout, just select the first one
                    if (layoutToSelect == null && logLayouts.Count > 0)
                    {
                        layoutToSelect = logLayouts[0];
                    }

                    // Select the layout in the combo box
                    if (layoutToSelect != null)
                    {
                        cboLogLayout.SelectedItem = layoutToSelect;
                    }

                    // Update LogAppState with the new layout object
                    LogAppState.Instance.Layout.Set(GetSelectedLogLayout());
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

        private void UpdateSelectedLogProvider()
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

            lblProviderDescription.Text = $"{layoutText} - {config}";
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
            ToggleCollapse();
        }
        private void ToggleCollapse()
        {
            if (_isCollapsed)
                ExpandProvider();
            else if (IsSourceValid)
                CollapseProvider();
        }

        private void LblProviderDescription_MouseClick(object sender, MouseEventArgs e)
        {
            ToggleCollapse();
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
