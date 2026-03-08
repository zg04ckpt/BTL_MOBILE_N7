using Feature.Settings.Enums;
using Feature.Settings.Interfaces;

namespace Feature.Settings.Helpers
{
    public static class ConfigHelper
    {
        public static async Task<bool> IsMaintenanceModeAsync(ISystemConfigurationService service)
        {
            return await service.GetValueAsync<bool>(ConfigurationKey.MaintenanceMode.ToString(), false);
        }

        public static async Task<string[]> GetWhitelistIPsAsync(ISystemConfigurationService service)
        {
            var ips = await service.GetValueAsync(ConfigurationKey.WhitelistIPs.ToString(), "127.0.0.1,::1");
            return ips?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(ip => ip.Trim())
                       .ToArray() ?? Array.Empty<string>();
        }

        public static async Task<string> GetRequiredAppVersionAsync(ISystemConfigurationService service)
        {
            return await service.GetValueAsync(ConfigurationKey.RequiredAppVersion.ToString(), "1.0.0") ?? "1.0.0";
        }

        public static async Task<int> GetQuestionTimeLimitAsync(ISystemConfigurationService service)
        {
            return await service.GetValueAsync<int>(ConfigurationKey.QuestionTimeLimit.ToString(), 30);
        }

        public static async Task<int> GetQuestionsPerMatchAsync(ISystemConfigurationService service)
        {
            return await service.GetValueAsync<int>(ConfigurationKey.QuestionsPerMatch.ToString(), 10);
        }

        public static async Task<int> GetBaseWinPointsAsync(ISystemConfigurationService service)
        {
            return await service.GetValueAsync<int>(ConfigurationKey.BaseWinPoints.ToString(), 25);
        }

        public static async Task<int> GetBaseLosePointsAsync(ISystemConfigurationService service)
        {
            return await service.GetValueAsync<int>(ConfigurationKey.BaseLosePoints.ToString(), 10);
        }

        public static async Task<double> GetEloKFactorAsync(ISystemConfigurationService service)
        {
            return await service.GetValueAsync<double>(ConfigurationKey.EloKFactor.ToString(), 32.0);
        }

        public static int CompareVersions(string version1, string version2)
        {
            var v1Parts = version1.Split('.').Select(int.Parse).ToArray();
            var v2Parts = version2.Split('.').Select(int.Parse).ToArray();

            for (int i = 0; i < Math.Max(v1Parts.Length, v2Parts.Length); i++)
            {
                var v1 = i < v1Parts.Length ? v1Parts[i] : 0;
                var v2 = i < v2Parts.Length ? v2Parts[i] : 0;

                if (v1 > v2) return 1;
                if (v1 < v2) return -1;
            }

            return 0;
        }

        public static bool IsVersionAllowed(string clientVersion, string requiredVersion)
        {
            try
            {
                return CompareVersions(clientVersion, requiredVersion) >= 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
