using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogScraper.Configuration;
using LogScraper.Utilities.Extensions;

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
                catch (Exception ex)
                {
                    ex.LogStackTraceToFile("Error during update check.");
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
            (string latestVersion, string releaseUrl, bool isPrerelease) = await GetLatestGitHubReleaseAsync(githubUser, githubRepo, ConfigAppState.Instance.GenericConfig.Value.IncludeBetaUpdates);

            if (latestVersion == null)
            {
                return; // Exit if the latest version could not be retrieved
            }

            if (IsNewerVersion(latestVersion, currentVersion))
            {
                string prereleaseText = isPrerelease ? Environment.NewLine + Environment.NewLine + "Please note: this is a pre-release version and may be unstable." : "";
                // Notify the user about the new version and provide an option to open the download page
                DialogResult result = MessageBox.Show(
                    "A new version " + latestVersion + " is available. Do you want to open the download page?" + prereleaseText,
                    "Update Available",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    OpenReleasePage(releaseUrl); // Open the GitHub release page
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
        private static async Task<(string Version, string HtmlUrl, bool IsPrerelease)> GetLatestGitHubReleaseAsync(string user, string repository, bool includePrereleases)
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
                    JsonElement root = document.RootElement;

                    return (
                        root.GetProperty("tag_name").GetString(), root.GetProperty("html_url").GetString(), root.GetProperty("prerelease").GetBoolean());
                }

                foreach (JsonElement release in document.RootElement.EnumerateArray())
                {
                    if (!release.GetProperty("prerelease").GetBoolean())
                    {
                        continue;
                    }

                    return (release.GetProperty("tag_name").GetString(), release.GetProperty("html_url").GetString(), true);
                }
            }
            catch (Exception ex)
            {
                // Intentionally ignored
                ex.LogStackTraceToFile("Error fetching latest release information from GitHub.");
            }

            return (null, null, false);
        }

        /// <summary>
        /// Determines whether the latest version is newer than the current version.
        /// Handles semver pre-release labels (e.g. 4.0.0-alpha.1): a stable release is
        /// always considered newer than a pre-release of the same base version.
        /// </summary>
        /// <param name="latest">The latest version string.</param>
        /// <param name="current">The current version string.</param>
        /// <returns>True if the latest version is newer; otherwise, false.</returns>
        private static bool IsNewerVersion(string latest, string current)
        {
            string cleanLatest = StripSemVerMetadata(latest);
            string cleanCurrent = StripSemVerMetadata(current);

            (Version baseLatest, string preLatest, int preNumLatest) = ParseSemVer(cleanLatest);
            (Version baseCurrent, string preCurrent, int preNumCurrent) = ParseSemVer(cleanCurrent);

            if (baseLatest == null || baseCurrent == null) return false;

            if (baseLatest != baseCurrent) return baseLatest > baseCurrent;

            // Same base version: stable beats any pre-release; otherwise compare pre-release numbers.
            bool latestIsStable = string.IsNullOrEmpty(preLatest);
            bool currentIsStable = string.IsNullOrEmpty(preCurrent);

            if (latestIsStable && !currentIsStable) return true;  // stable update available for pre-release user
            if (!latestIsStable && currentIsStable) return false;  // pre-release is never newer than stable
            if (latestIsStable && currentIsStable) return false;   // identical stable versions

            // Both pre-release: compare label alphabetically (beta > alpha), then number
            int labelCompare = string.Compare(preLatest, preCurrent, StringComparison.OrdinalIgnoreCase);
            if (labelCompare != 0) return labelCompare > 0;
            return preNumLatest > preNumCurrent;
        }

        /// <summary>
        /// Parses a cleaned semver string (no metadata, no leading 'v') into its base version,
        /// pre-release label, and pre-release number (e.g. "4.0.0-alpha.2" → 4.0.0, "alpha", 2).
        /// </summary>
        private static (Version BaseVersion, string PrereleaseLabel, int PrereleaseNumber) ParseSemVer(string version)
        {
            string baseStr = version;
            string label = "";
            int number = 0;

            int dashIndex = version.IndexOf('-');
            if (dashIndex >= 0)
            {
                baseStr = version[..dashIndex];
                string prerelease = version[(dashIndex + 1)..];
                int dotIndex = prerelease.LastIndexOf('.');
                if (dotIndex >= 0 && int.TryParse(prerelease[(dotIndex + 1)..], out int n))
                {
                    label = prerelease[..dotIndex];
                    number = n;
                }
                else
                {
                    label = prerelease;
                }
            }

            return Version.TryParse(baseStr, out Version v) ? (v, label, number) : (null, null, 0);
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
        /// Opens the GitHub release page for the specified release Url in the default web browser.
        /// </summary>
        /// <param name="releaseUrl">The URL of the GitHub release page.</param>
        private static void OpenReleasePage(string releaseUrl)
        {
            if (!string.IsNullOrEmpty(releaseUrl))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = releaseUrl,
                    UseShellExecute = true
                });
            }
        }
    }

}
