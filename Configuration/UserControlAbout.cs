using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LogScraper.Properties;
using LogScraper.Utilities;

namespace LogScraper.Configuration
{
    public partial class UserControlAbout : UserControl
    {
        public UserControlAbout()
        {
            InitializeComponent();

            string version = Application.ProductVersion;
            if (version.Contains('+')) version = version[..version.IndexOf('+')];

            lblVersion.Text = "v" + version;

            LinkComponents.Links.Clear();
            LinkComponents.Links.Add(23, 13, "https://scintilla.org");
            LinkComponents.Links.Add(52, 18, "https://pictogrammers.com/library/mdi/");

            ApplyLocalization();
        }

        public void ApplyLocalization()
        {
            GrpAbout.Text = Resources.UserControlAbout_GrpAbout;
            string runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            LblRuntime.Text = string.Format(Resources.UserControlAbout_LblRuntime, runtime);
            LblAuthor.Text = Resources.UserControlAbout_LblAuthor;
            LinkGitHub.Text = Resources.UserControlAbout_LinkGitHub;
            LinkGitHub.Links.Clear();
            LinkGitHub.Links.Add(0, LinkGitHub.Text.Length, "https://github.com/Rambo3000/LogScraper");
            BtnUpdate.Text = Resources.UserControlAbout_BtnCheckForUpdates;
            LblDisclaimer.Text = Resources.UserControlAbout_LblDisclaimer;
            LblDisclaimerFullText.Text = Resources.UserControlAbout_LblDisclaimerFullText;
            LblGnuLicense.Text = Resources.UserControlAbout_LblGnuLicense;
        }

        private void LinkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Link.LinkData.ToString(),
                UseShellExecute = true
            });
        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            BtnUpdate.Enabled = false;
            Application.DoEvents();
            await GitHubUpdateChecker.CheckForUpdateAsync(true);

            Application.DoEvents();
            BtnUpdate.Enabled = true;
        }

        private void UserControlAbout_SizeChanged(object sender, EventArgs e)
        {
            CenterControl(lblVersion);
            CenterControl(LblRuntime);
            CenterControl(LblAuthor);
            CenterControl(LinkGitHub);
            CenterControl(LblOpenExecutableFolder);
            CenterControl(BtnUpdate);
        }
        private void CenterControl(Control control)
        {
            int center = GrpAbout.ClientSize.Width / 2;
            control.Left = center - control.Width / 2;
        }

        private void LinkComponents_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Link.LinkData.ToString()) { UseShellExecute = true });
        }

        private void LblOpenExecutableFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenExecutableFolder();
        }
        private static void OpenExecutableFolder()
        {
            string executablePath = AppContext.BaseDirectory;
            string folderPath = Path.GetDirectoryName(executablePath);

            if (folderPath == null)
            {
                return;
            }

            ProcessStartInfo processStartInfo = new()
            {
                FileName = "explorer.exe",
                Arguments = folderPath,
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
        }
    }
}
