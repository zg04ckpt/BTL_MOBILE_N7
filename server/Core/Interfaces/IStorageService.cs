using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveAsync(IFormFile file);
        Task DeleteAsync(params string[] paths);
    }
}
