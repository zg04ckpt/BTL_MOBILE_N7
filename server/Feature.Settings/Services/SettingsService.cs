using Core.Base;
using Core.Interfaces;
using Feature.Settings.Interfaces;
using Models.Settings.DTOs;
using Models.Settings.Entities;
using Models.Settings.Enums;
using Models.Settings.Requests;
using System.Globalization;

namespace Feature.Settings.Services
{
    public class SettingsService : BaseService, ISettingsService
    {
        public SettingsService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<SettingsDto> GetAllSettingsAsync()
        {
            var settings = await _uow.Repository<Setting>().GetAllAsync(
                predicate: e => true);

            var dic = settings.ToDictionary(
                keySelector: e => e.Key,
                elementSelector: e => e);

            return new SettingsDto
            {
                WhitelistIPs = dic[ConfigurationKey.WhitelistIPs].Value,
                BaseLosePoints = int.Parse(dic[ConfigurationKey.BaseLosePoints].Value),
                BaseWinPoints = int.Parse(dic[ConfigurationKey.BaseWinPoints].Value),
                EloKFactor = double.Parse(dic[ConfigurationKey.EloKFactor].Value, CultureInfo.InvariantCulture),
                LastUpdated = dic.Values.Max(s => s.UpdatedAt),
                LoginLiveTime = int.Parse(dic[ConfigurationKey.LoginLiveTime].Value),
                MaintenanceMode = bool.Parse(dic[ConfigurationKey.MaintenanceMode].Value),
                QuestionsPerMatch = int.Parse(dic[ConfigurationKey.QuestionsPerMatch].Value),
                QuestionTimeLimit = int.Parse(dic[ConfigurationKey.QuestionTimeLimit].Value)
            };
        }

        public async Task InitializeDefaultSettingsAsync()
        {
            var existingSettings = await _uow.Repository<Setting>().GetAllAsync(
                predicate: e => true);
            var existingKeys = existingSettings
                .Select(s => s.Key)
                .ToHashSet();

            var defaultSettings = new Setting[]
            {
                new ()
                {
                    Key = ConfigurationKey.WhitelistIPs,
                    Description = "Danh sách IP được phép truy cập khi bảo trì",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "127.0.0.1"
                },
                new ()
                {
                    Key = ConfigurationKey.LoginLiveTime,
                    Description = "Thời gian tối đa duy trì phiên đăng nhập (minutes)",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "10080" // 7 ngày
                },
                new ()
                {
                    Key = ConfigurationKey.QuestionTimeLimit,
                    Description = "Số giây suy nghĩ tối đa mỗi câu hỏi trong trận đấu",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "15"
                },
                new ()
                {
                    Key = ConfigurationKey.BaseLosePoints,
                    Description = "Số điểm bị trừ mỗi khi thua (hạng thấp nhất sau trận)",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "15"
                },
                new ()
                {
                    Key = ConfigurationKey.MaintenanceMode,
                    Description = "Bật trạng thái bảo chì, chỉ WhitelistIPs truy cập được hệ thống",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "false"
                },
                new ()
                {
                    Key = ConfigurationKey.EloKFactor,
                    Description = "Hệ số điều chỉnh tốc độ leo rank",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "32"
                },
                new ()
                {
                    Key = ConfigurationKey.BaseWinPoints,
                    Description = "Số điểm bị trừ mỗi khi thắng (hạng cao nhất sau trận)",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "20"
                },
                new ()
                {
                    Key = ConfigurationKey.QuestionsPerMatch,
                    Description = "Số câu hỏi mỗi trận đấu",
                    UpdatedAt = DateTime.UtcNow,
                    Value = "10"
                }
            };

            var missingSettings = defaultSettings
                .Where(s => !existingKeys.Contains(s.Key))
                .ToArray();

            if (!missingSettings.Any())
            {
                return;
            }

            await _uow.Repository<Setting>().AddAsync(missingSettings);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateSettingsAsync(UpdateSettingsRequest request)
        {
            var settings = await _uow.Repository<Setting>().GetAllAsync(
                predicate: e => true);

            var dic = settings.ToDictionary(
                keySelector: e => e.Key,
                elementSelector: e => e);

            dic[ConfigurationKey.WhitelistIPs].Value = request.WhitelistIPs;
            dic[ConfigurationKey.BaseLosePoints].Value = request.BaseLosePoints + "";
            dic[ConfigurationKey.BaseWinPoints].Value = request.BaseWinPoints + "";
            dic[ConfigurationKey.EloKFactor].Value = request.EloKFactor.ToString(CultureInfo.InvariantCulture);
            dic[ConfigurationKey.LoginLiveTime].Value = request.LoginLiveTime + "";
            dic[ConfigurationKey.MaintenanceMode].Value = request.MaintenanceMode + "";
            dic[ConfigurationKey.QuestionsPerMatch].Value = request.QuestionsPerMatch + "";
            dic[ConfigurationKey.QuestionTimeLimit].Value = request.QuestionTimeLimit + "";

            await _uow.Repository<Setting>().UpdateAsync(dic.Values.ToArray());
            await _uow.SaveChangesAsync();
        }
    }
}
