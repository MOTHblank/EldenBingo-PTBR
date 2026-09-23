using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EldenBingo.Util
{
    public class GitHubVersionChecker
    {
        private const string GITHUB_API_URL = "https://api.github.com/repos/MOTHblank/EldenBingo-PTBR/releases";
        private readonly Version _currentVersion;
        private readonly HttpClient _httpClient;

        public GitHubVersionChecker(Version currentVersion)
        {
            _currentVersion = currentVersion;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("EldenBingo-VersionChecker", "1.0"));
        }

        public async Task<GitHubRelease?> CheckForNewerVersionAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(GITHUB_API_URL);
                var releases = JsonConvert.DeserializeObject<GitHubRelease[]>(response);

                if (releases == null || releases.Length == 0)
                    return null;

                var latestVersion = _currentVersion;
                GitHubRelease? latestRelease = null;

                foreach (var release in releases.Where(r => !r.Prerelease))
                {
                    if (!TryParseReleaseVersion(release.Tag_Name, out var releaseVersion))
                        continue;

                    if (releaseVersion > latestVersion)
                    {
                        latestVersion = releaseVersion;
                        latestRelease = release;
                    }
                }

                return latestRelease;
            }
            catch (Exception ex)
            {
                MainForm.Instance?.PrintToConsole($"Error checking for new version: {ex.Message}", Color.Red, true);
                return null;
            }
        }

        private static bool TryParseReleaseVersion(string? tagName, out Version version)
        {
            version = new Version();

            if (string.IsNullOrWhiteSpace(tagName))
                return false;

            var normalizedTag = tagName.Trim();
            if (normalizedTag.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                normalizedTag = normalizedTag[1..];

            if (!Version.TryParse(normalizedTag, out var parsedVersion) || parsedVersion == null)
                return false;

            version = parsedVersion;
            return true;
        }

        public record struct GitHubRelease(string Name, string Tag_Name, string Html_Url, bool Prerelease, GitHubAsset[]? Assets);

        public record struct GitHubAsset(string Name, string Browser_Download_Url);
    }
}
