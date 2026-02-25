using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Storage.Services
{
    public class ImageCleanupService : BackgroundService
    {
        private readonly ILogger<ImageCleanupService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);

        public ImageCleanupService(
            ILogger<ImageCleanupService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Image Cleanup Service đã khởi động");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Bắt đầu quét và dọn dẹp ảnh không sử dụng lúc {Time}", DateTime.UtcNow);
                    await CleanupUnusedImagesAsync();
                    _logger.LogInformation("Hoàn thành quét và dọn dẹp ảnh lúc {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi dọn dẹp ảnh không sử dụng");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("Image Cleanup Service đã dừng");
        }

        private async Task CleanupUnusedImagesAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Lấy tất cả đường dẫn ảnh đang được sử dụng từ database
            var usedImageUrls = await dbContext.Set<Feature.Users.Entities.User>()
                .Where(u => !string.IsNullOrEmpty(u.AvatarUrl))
                .Select(u => u.AvatarUrl)
                .Distinct()
                .ToListAsync();

            // Chuẩn hóa đường dẫn thành tên file
            var usedFileNames = usedImageUrls
                .Select(url => url.TrimStart('/', '\\')
                    .Replace("uploads/", "")
                    .Replace("uploads\\", ""))
                .Where(name => !string.IsNullOrEmpty(name))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            _logger.LogInformation("Tìm thấy {Count} ảnh đang được sử dụng", usedFileNames.Count);

            // Lấy tất cả file trong thư mục uploads
            var uploadFolder = Path.Combine(AppContext.BaseDirectory, "uploads");
            if (!Directory.Exists(uploadFolder))
            {
                _logger.LogWarning("Thư mục uploads không tồn tại");
                return;
            }

            var allFiles = Directory.GetFiles(uploadFolder);
            _logger.LogInformation("Tìm thấy {Count} file trong thư mục uploads", allFiles.Length);

            var deletedCount = 0;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var filePath in allFiles)
            {
                var fileName = Path.GetFileName(filePath);
                var extension = Path.GetExtension(fileName).ToLowerInvariant();

                // Chỉ xử lý file ảnh
                if (!allowedExtensions.Contains(extension))
                {
                    continue;
                }

                // Nếu file không được sử dụng, xóa nó
                if (!usedFileNames.Contains(fileName))
                {
                    try
                    {
                        File.Delete(filePath);
                        deletedCount++;
                        _logger.LogInformation("Đã xóa file không sử dụng: {FileName}", fileName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Không thể xóa file {FileName}", fileName);
                    }
                }
            }

            if (deletedCount > 0)
            {
                _logger.LogInformation("Đã xóa {Count} file ảnh không sử dụng", deletedCount);
            }
            else
            {
                _logger.LogInformation("Không có file ảnh nào cần xóa");
            }
        }
    }
}
