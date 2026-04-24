using CNLib.Services.Logs;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Storage.Services
{
    public class ImageStorageService : IStorageService
    {
        private readonly ILogService<ImageStorageService> _logService;

        public ImageStorageService(ILogService<ImageStorageService> logService)
        {
            _logService = logService;
        }

        public Task DeleteAsync(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                return Task.CompletedTask;
            }
            
            var uploadFolder = Path.Combine(AppContext.BaseDirectory, "uploads");
            var errors = new List<string>();

            foreach (var path in paths)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                var fileName = path.TrimStart('/', '\\').Replace("uploads/", "").Replace("uploads\\", "");
                var filePath = Path.Combine(uploadFolder, fileName);

                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        _logService.LogError($"Delete failed: {fileName}", ex.Message);
                        throw new ServerErrorException($"Cannot delete file {fileName}: {ex.Message}");
                    }
                }
                else
                {
                    errors.Add(fileName);
                }
            }

            if (errors.Any())
            {
                _logService.LogError($"Files not found: {string.Join(", ", errors)}");
                //throw new NotFoundException($"Files not found: {string.Join(", ", errors)}");
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logService.LogError("Save failed: Invalid file");
                throw new BadRequestException("Invalid file");
            }

            const long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
            {
                _logService.LogError($"Save failed: File too large - {file.Length} bytes");
                throw new BadRequestException("File too large (max 5MB)");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                _logService.LogError($"Save failed: Invalid format - {extension}");
                throw new BadRequestException("Invalid image format");
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadFolder = Path.Combine(AppContext.BaseDirectory, "uploads");
            
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            return $"/uploads/{fileName}";
        }
    }
}
