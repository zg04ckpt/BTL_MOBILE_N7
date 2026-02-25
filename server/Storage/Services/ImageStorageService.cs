using Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Storage.Services
{
    public interface IImageStorageService
    {
        Task<string> SaveAsync(IFormFile file);
        Task DeleteAsync(params string[] paths);
    }

    public class ImageStorageService : IImageStorageService
    {
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
                        throw new ServerErrorException($"Không thể xóa file {fileName}: {ex.Message}");
                    }
                }
                else
                {
                    errors.Add(fileName);
                }
            }

            if (errors.Any())
            {
                throw new NotFoundException($"Các file sau không tồn tại: {string.Join(", ", errors)}");
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("File không hợp lệ");
            }

            const long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
            {
                throw new BadRequestException("File quá lớn (tối đa 5MB)");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                throw new BadRequestException("Định dạng ảnh không hợp lệ");
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
