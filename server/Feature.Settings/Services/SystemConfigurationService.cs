using CNLib.Services.Logs;
using Core.Base;
using Core.Interfaces;
using Feature.Settings.Entities;
using Feature.Settings.Enums;
using Feature.Settings.Interfaces;
using Feature.Settings.Models;
using System.Globalization;

namespace Feature.Settings.Services
{
    public class SystemConfigurationService : BaseService, ISystemConfigurationService
    {
        private readonly ILogService<SystemConfigurationService> _logService;

        public SystemConfigurationService(IUnitOfWork uow, ILogService<SystemConfigurationService> logService) : base(uow)
        {
            _logService = logService;
        }

        public async Task<SystemConfigurationsDto> GetSystemConfigurationsAsync()
        {
            var configs = await _uow.Repository<SystemConfiguration>().GetAllAsync(c => true);
            var configDict = configs.ToDictionary(c => c.Key, c => c.Value);
            
            return new SystemConfigurationsDto
            {
                // General Settings
                MaintenanceMode = GetBoolValue(configDict, ConfigurationKey.MaintenanceMode.ToString(), false),
                WhitelistIPs = GetStringValue(configDict, ConfigurationKey.WhitelistIPs.ToString(), string.Empty),
                RequiredAppVersion = GetStringValue(configDict, ConfigurationKey.RequiredAppVersion.ToString(), "1.0.0"),
                LoginLiveTime = GetIntValue(configDict, ConfigurationKey.LoginLiveTime.ToString(), 10080),
                
                // Game Settings
                QuestionTimeLimit = GetIntValue(configDict, ConfigurationKey.QuestionTimeLimit.ToString(), 30),
                QuestionsPerMatch = GetIntValue(configDict, ConfigurationKey.QuestionsPerMatch.ToString(), 10),
                BaseWinPoints = GetIntValue(configDict, ConfigurationKey.BaseWinPoints.ToString(), 25),
                BaseLosePoints = GetIntValue(configDict, ConfigurationKey.BaseLosePoints.ToString(), 10),
                EloKFactor = GetDoubleValue(configDict, ConfigurationKey.EloKFactor.ToString(), 32.0),
                
                LastUpdated = configs.Any() ? configs.Max(c => c.UpdatedAt) : DateTime.UtcNow
            };
        }

        public async Task UpdateSystemConfigurationsAsync(UpdateSystemConfigurationsRequest request)
        {
            var updates = new Dictionary<string, (string value, string description)>
            {
                // General Settings
                { 
                    ConfigurationKey.MaintenanceMode.ToString(), 
                    (request.MaintenanceMode.ToString(), "Enable/disable maintenance mode")
                },
                { 
                    ConfigurationKey.WhitelistIPs.ToString(), 
                    (request.WhitelistIPs, "Comma-separated list of whitelisted IPs for maintenance mode")
                },
                { 
                    ConfigurationKey.RequiredAppVersion.ToString(), 
                    (request.RequiredAppVersion, "Minimum required app version")
                },
                { 
                    ConfigurationKey.LoginLiveTime.ToString(), 
                    (request.LoginLiveTime.ToString(), "Login session live time in minutes")
                },
                
                // Game Settings
                { 
                    ConfigurationKey.QuestionTimeLimit.ToString(), 
                    (request.QuestionTimeLimit.ToString(), "Time limit per question in seconds")
                },
                { 
                    ConfigurationKey.QuestionsPerMatch.ToString(), 
                    (request.QuestionsPerMatch.ToString(), "Number of questions per match")
                },
                { 
                    ConfigurationKey.BaseWinPoints.ToString(), 
                    (request.BaseWinPoints.ToString(), "Base points awarded for winning a match")
                },
                { 
                    ConfigurationKey.BaseLosePoints.ToString(), 
                    (request.BaseLosePoints.ToString(), "Base points deducted for losing a match")
                },
                { 
                    ConfigurationKey.EloKFactor.ToString(), 
                    (request.EloKFactor.ToString(CultureInfo.InvariantCulture), "ELO rating K-factor for rank calculation")
                }
            };

            foreach (var (key, (value, description)) in updates)
            {
                await SetValueAsync(key, value, description);
            }

            await _uow.SaveChangesAsync();
        }

        public async Task<ConfigurationItemDto?> GetByKeyAsync(string key)
        {
            var config = await _uow.Repository<SystemConfiguration>().GetFirstAsync(c => c.Key == key);
            
            if (config == null)
            {
                return null;
            }
            
            return new ConfigurationItemDto
            {
                Id = config.Id,
                Key = config.Key,
                Value = config.Value,
                Description = config.Description,
                UpdatedAt = config.UpdatedAt
            };
        }

        public async Task<string?> GetValueAsync(string key, string? defaultValue = null)
        {
            var config = await _uow.Repository<SystemConfiguration>().GetFirstAsync(c => c.Key == key);
            return config?.Value ?? defaultValue;
        }

        public async Task<T?> GetValueAsync<T>(string key, T? defaultValue = default)
        {
            var value = await GetValueAsync(key);
            
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }
        }

        public async Task SetValueAsync(string key, string value, string? description = null)
        {
            var repository = _uow.Repository<SystemConfiguration>();
            var config = await repository.GetFirstAsync(c => c.Key == key);

            if (config == null)
            {
                config = new SystemConfiguration
                {
                    Key = key,
                    Value = value,
                    Description = description,
                    UpdatedAt = DateTime.UtcNow
                };
                await repository.AddAsync(config);
            }
            else
            {
                config.Value = value;
                if (description != null)
                    config.Description = description;
                config.UpdatedAt = DateTime.UtcNow;
                await repository.UpdateAsync(config);
            }
        }

        public async Task InitializeDefaultConfigurationsAsync()
        {
            var repository = _uow.Repository<SystemConfiguration>();
            var count = await repository.CountAsync();

            if (count > 0)
            {
                return;
            }

            var defaultConfigs = new[]
            {
                new SystemConfiguration
                {
                    Key = ConfigurationKey.MaintenanceMode.ToString(),
                    Value = "false",
                    Description = "Enable/disable maintenance mode",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.WhitelistIPs.ToString(),
                    Value = "127.0.0.1,::1",
                    Description = "Comma-separated list of whitelisted IPs for maintenance mode",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.RequiredAppVersion.ToString(),
                    Value = "1.0.0",
                    Description = "Minimum required app version",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.LoginLiveTime.ToString(),
                    Value = "10080",
                    Description = "Login session live time in minutes (default: 7 days)",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.QuestionTimeLimit.ToString(),
                    Value = "30",
                    Description = "Time limit per question in seconds",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.QuestionsPerMatch.ToString(),
                    Value = "10",
                    Description = "Number of questions per match",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.BaseWinPoints.ToString(),
                    Value = "25",
                    Description = "Base points awarded for winning a match",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.BaseLosePoints.ToString(),
                    Value = "10",
                    Description = "Base points deducted for losing a match",
                    UpdatedAt = DateTime.UtcNow
                },
                new SystemConfiguration
                {
                    Key = ConfigurationKey.EloKFactor.ToString(),
                    Value = "32",
                    Description = "ELO rating K-factor for rank calculation",
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await repository.AddAsync(defaultConfigs);
            await _uow.SaveChangesAsync();
            
            _logService.LogSuccess($"Default configs initialized: {defaultConfigs.Length} items");
        }

        private static bool GetBoolValue(Dictionary<string, string> dict, string key, bool defaultValue)
        {
            if (dict.TryGetValue(key, out var value) && bool.TryParse(value, out var result))
                return result;
            return defaultValue;
        }

        private static string GetStringValue(Dictionary<string, string> dict, string key, string defaultValue)
        {
            return dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        private static int GetIntValue(Dictionary<string, string> dict, string key, int defaultValue)
        {
            if (dict.TryGetValue(key, out var value) && int.TryParse(value, out var result))
                return result;
            return defaultValue;
        }

        private static double GetDoubleValue(Dictionary<string, string> dict, string key, double defaultValue)
        {
            if (dict.TryGetValue(key, out var value) && double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return defaultValue;
        }
    }
}
