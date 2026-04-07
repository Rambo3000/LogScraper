using System;
using System.Windows.Forms;
using LogScraper.Configuration;
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

        public LogProviderSelectionControl()
        {
            InitializeComponent();
            AttachEventHandlers();
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

            cboLogProvider.SelectedIndexChanged += CboLogProvider_SelectedIndexChanged;
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
            UriChanged?.Invoke(this, e);
        }

        private void HandleIsSourceValidChanged(object sender, bool e)
        {
            IsSourceValidChanged?.Invoke(this, e);
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
                    GrpLogProvidersSettings.Text = "Directe URL instellingen";
                    usrRuntime.UpdateUri();
                    break;
                case LogProviderType.Kubernetes:
                    GrpLogProvidersSettings.Text = "Kubernetes instellingen";
                    usrKubernetes.UpdateUri();
                    break;
                case LogProviderType.File:
                    GrpLogProvidersSettings.Text = "Lokaal bestand instellingen";
                    usrFileLogProvider.UpdateUri();
                    break;
            }

            LogProviderChanged?.Invoke(this, EventArgs.Empty);
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
            GrpLogProvidersSettings.Enabled = enabled;
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
    }
}
