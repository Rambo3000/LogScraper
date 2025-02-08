using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Credentials;
using LogScraper.Sources.Adapters;
using LogScraper.Sources.Adapters.Http;

namespace LogScraper.LogProviders.Runtime
{
    public partial class UserControlRuntimeLogProvider : UserControl, ILogProviderUserControl
    {
        public event EventHandler SourceSelectionChanged;

        public event Action<string, bool> StatusUpdate;

        private List<RuntimeInstance> RuntimeInstances { get; set; }

        public UserControlRuntimeLogProvider()
        {
            InitializeComponent();
        }

        public void UpdateRuntimeInstances(List<RuntimeInstance> runtimeInstance)
        {
            RuntimeInstances = runtimeInstance;

            PopulateRuntimeInstances();
        }

        public ISourceAdapter GetSourceAdapter()
        {
            RuntimeInstance RuntimeInstance = (RuntimeInstance)cboRuntimeInstances.SelectedItem ?? throw new Exception("Er is geen runtime instantie geselecteerd");
            if (string.IsNullOrEmpty(RuntimeInstance.Description)) throw new Exception("De runtime instantie is verplicht");

            ISourceAdapter sourceAdapter;
            if (Debugger.IsAttached && RuntimeInstance.Description != "BasicTest")
            {
                sourceAdapter = SourceAdapterFactory.CreateFileSourceAdapter(RuntimeInstance.UrlRuntimeLog);
            }
            else
            {
                sourceAdapter = SourceAdapterFactory.CreateHttpSourceAdapter(RuntimeInstance.UrlRuntimeLog, CredentialManager.GenerateTargetLogProvider("Runtime", RuntimeInstance.Description), ConfigurationManager.GenericConfig.HttpCLientTimeOUtSeconds);
            }
            return sourceAdapter;
        }
        private void PopulateRuntimeInstances()
        {
            cboRuntimeInstances.Items.Clear();

            if (Debugger.IsAttached)
            {
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "Stub1", UrlRuntimeLog = "Stubs/Runtime1.txt" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "Stub2", UrlRuntimeLog = "Stubs/Runtime2.txt" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "Stub3", UrlRuntimeLog = "Stubs/Runtime3.txt" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "JSONInvertedExample", UrlRuntimeLog = "Stubs/JSONInvertedExample.log" });
                cboRuntimeInstances.Items.Add(new RuntimeInstance() { Description = "BasicTest", UrlRuntimeLog = "http://localhost/runtime1.txt" });
            }
            else
            {
                cboRuntimeInstances.Items.AddRange(RuntimeInstances.ToArray());
            }
        }

        // Method to raise the custom SourceSelectionChanged event.
        protected virtual void OnSourceSelectionChanged(EventArgs e)
        {
            SourceSelectionChanged?.Invoke(this, e);
        }

        private void CboRuntimeInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUrl.Text = ((RuntimeInstance)cboRuntimeInstances.SelectedItem).UrlRuntimeLog;
            try
            {
                HttpResponseMessage httpResponseMessage = ((HttpSourceAdapter)GetSourceAdapter()).TestConnectionAndAskForAuthorisation();
                if (httpResponseMessage != null)
                {
                    if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        OnStatusUpdate("Ok", true);
                    }
                    else
                    {
                        OnStatusUpdate(HttpSourceAdapter.ConvertHttpStatusCodeToString(httpResponseMessage), false);
                    }
                }
            }
            catch (Exception ex)
            {
                OnStatusUpdate(ex.Message, false);
            }

            OnSourceSelectionChanged(EventArgs.Empty);
        }
        protected virtual void OnStatusUpdate(string message, bool isSucces)
        {
            StatusUpdate?.Invoke(message, isSucces);
        }
    }
}
