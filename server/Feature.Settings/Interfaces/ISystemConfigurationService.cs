using Feature.Settings.Models;

namespace Feature.Settings.Interfaces
{
    public interface ISystemConfigurationService
    {
        Task<SystemConfigurationsDto> GetSystemConfigurationsAsync();
        Task UpdateSystemConfigurationsAsync(UpdateSystemConfigurationsRequest request);
        Task<ConfigurationItemDto?> GetByKeyAsync(string key);
        Task<string?> GetValueAsync(string key, string? defaultValue = null);
        Task<T?> GetValueAsync<T>(string key, T? defaultValue = default);
        Task SetValueAsync(string key, string value, string? description = null);
        Task InitializeDefaultConfigurationsAsync();
    }
}
