using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogScraper.Utilities
{
    /// <summary>
    /// Provides functionality to check for updates to the application by querying the latest release information from GitHub.
    /// </summary>
    public static class GitHubUpdateChecker
    {
        /// <summary>
        /// Initiates an asynchronous check for updates to the application.
        /// </summary>
        /// <remarks>
        /// This method runs the update check on a background thread to avoid blocking the UI thread.
        /// </remarks>
        public static void CheckForUpdateInSeperateThread()
        {
            Task.Run(async () =>
            {
                try
                {
                    await CheckForUpdateAsync();
                }
                catch
                {
                    // Swallow any exceptions to avoid disrupting the user experience.
                }
            });
        }

        /// <summary>
        /// Performs the actual update check by comparing the current version with the latest version available on GitHub.
        /// </summary>
        public static async Task CheckForUpdateAsync(bool ShowMessageNoNewVersionFound = false)
        {
            string githubUser = "Rambo3000"; // GitHub username
            string githubRepo = "LogScraper"; // GitHub repository name
            string currentVersion = GetCurrentVersion(); // Get the current application version
            string latestVersion = await GetLatestGitHubReleaseVersionAsync(githubUser, githubRepo); // Fetch the latest release version

            if (latestVersion == null)
            {
                return; // Exit if the latest version could not be retrieved
            }

            if (IsNewerVersion(latestVersion, currentVersion))
            {
                // Notify the user about the new version and provide an option to open the download page
                DialogResult result = MessageBox.Show(
                    "A new version " + latestVersion + " is available. Do you want to open the download page?",
                    "Update Available",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    OpenReleasePage(githubUser, githubRepo); // Open the GitHub release page
                }
            }
            else if (ShowMessageNoNewVersionFound)
            {
                MessageBox.Show("Your version is up to date.", "No Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Retrieves the current version of the application from the assembly metadata.
        /// </summary>
        /// <returns>The current version as a string.</returns>
        private static string GetCurrentVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyInformationalVersionAttribute attribute =
                (AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(
                    assembly, typeof(AssemblyInformationalVersionAttribute));

            if (attribute != null)
            {
                return attribute.InformationalVersion; // Return the informational version if available
            }

            return assembly.GetName().Version.ToString(); // Fallback to the assembly version
        }

        /// <summary>
        /// Fetches the latest release version from the GitHub API.
        /// </summary>
        /// <param name="user">The GitHub username.</param>
        /// <param name="repository">The GitHub repository name.</param>
        /// <param name="includePrereleases">Whether to include pre-release versions in the check.</param>
        /// <returns>The latest release version as a string, or null if the request fails.</returns>
        private static async Task<string> GetLatestGitHubReleaseVersionAsync(string user, string repository, bool includePrereleases = false)
        {
            string url = "https://api.github.com/repos/" + user + "/" + repository + "/releases" + (!includePrereleases ? "/latest" : "");

            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("update-checker");

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                using JsonDocument document = JsonDocument.Parse(json);

                if (!includePrereleases)
                {
                    if (document.RootElement.TryGetProperty("tag_name", out JsonElement tag))
                    {
                        return tag.GetString();
                    }

                    return null;
                }

                foreach (JsonElement release in document.RootElement.EnumerateArray())
                {
                    bool isPrerelease = release.GetProperty("prerelease").GetBoolean();

                    if (!isPrerelease)
                    {
                        continue;
                    }

                    if (release.TryGetProperty("tag_name", out JsonElement prereleaseTag))
                    {
                        return prereleaseTag.GetString();
                    }
                }
            }
            catch
            {
                // Intentionally ignored
            }

            return null;
        }


        /// <summary>
        /// Determines whether the latest version is newer than the current version.
        /// </summary>
        /// <param name="latest">The latest version string.</param>
        /// <param name="current">The current version string.</param>
        /// <returns>True if the latest version is newer; otherwise, false.</returns>
        private static bool IsNewerVersion(string latest, string current)
        {
            string cleanLatest = StripSemVerMetadata(latest); // Remove metadata from the version string
            string cleanCurrent = StripSemVerMetadata(current);

            bool parsedLatest = Version.TryParse(cleanLatest, out Version latestVersion);
            bool parsedCurrent = Version.TryParse(cleanCurrent, out Version currentVersion);

            if (parsedLatest && parsedCurrent)
            {
                return latestVersion > currentVersion; // Compare the parsed versions
            }

            return false; // Return false if parsing fails
        }

        /// <summary>
        /// Removes metadata and prefixes (e.g., "v") from a semantic version string.
        /// </summary>
        /// <param name="version">The version string to clean.</param>
        /// <returns>The cleaned version string.</returns>
        private static string StripSemVerMetadata(string version)
        {
            int plusIndex = version.IndexOf('+');
            if (plusIndex >= 0) version = version[..plusIndex]; // Remove metadata after '+'

            if (version.StartsWith('v') || version.StartsWith('V')) version = version[1..]; // Remove 'v' or 'V' prefix

            return version.Trim(); // Trim any whitespace
        }

        /// <summary>
        /// Opens the GitHub release page for the specified repository in the default web browser.
        /// </summary>
        /// <param name="user">The GitHub username.</param>
        /// <param name="repo">The GitHub repository name.</param>
        private static void OpenReleasePage(string user, string repo)
        {
            string url = "https://github.com/" + user + "/" + repo + "/releases/latest";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Use the default web browser
            });
        }
    }

}
