using Models.Settings.DTOs;
using Models.Settings.Enums;
using Models.Settings.Requests;

namespace Feature.Settings.Interfaces
{
    public interface ISettingsService
    {
        Task<SettingsDto> GetAllSettingsAsync();
        Task UpdateSettingsAsync(UpdateSettingsRequest request);
        Task InitializeDefaultSettingsAsync();
    }
}
