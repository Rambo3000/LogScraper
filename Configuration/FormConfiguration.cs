using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogScraper.LogProviders.Kubernetes;

namespace LogScraper.Configuration
{
    public partial class FormConfiguration : Form
    {
        public FormConfiguration()
        {
            InitializeComponent();
        }

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            userControlKubernetesConfig1.SetKubernetesConfig(ConfigurationManager.LogProvidersConfig.KubernetesConfig);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
