using CNLib.Services.Logs;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Storage.Services
{
    public class ImageCleanupService : BackgroundService
    {
        private readonly ILogService<ImageCleanupService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);

        public ImageCleanupService(
            ILogService<ImageCleanupService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo("Image Cleanup Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInfo($"Starting cleanup of unused images at {DateTime.Now}");
                    await CleanupUnusedImagesAsync();
                    _logger.LogInfo($"Completed cleanup of unused images at {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while cleaning up unused images", ex.Message);
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInfo("Image Cleanup Service stopped");
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

            _logger.LogInfo($"Found {usedFileNames.Count} images in use");

            // Get all files in uploads folder
            var uploadFolder = Path.Combine(AppContext.BaseDirectory, "uploads");
            if (!Directory.Exists(uploadFolder))
            {
                _logger.LogError("Uploads folder does not exist");
                return;
            }

            var allFiles = Directory.GetFiles(uploadFolder);
            _logger.LogInfo($"Found {allFiles.Length} files in uploads folder");

            var deletedCount = 0;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var filePath in allFiles)
            {
                var fileName = Path.GetFileName(filePath);
                var extension = Path.GetExtension(fileName).ToLowerInvariant();

                // Only process image files
                if (!allowedExtensions.Contains(extension))
                {
                    continue;
                }

                // If file is not in use, delete it
                if (!usedFileNames.Contains(fileName))
                {
                    try
                    {
                        File.Delete(filePath);
                        deletedCount++;
                        _logger.LogInfo($"Deleted unused file: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Cannot delete file {fileName}", ex.Message);
                    }
                }
            }

            if (deletedCount > 0)
            {
                _logger.LogInfo($"Deleted {deletedCount} unused image files");
            }
            else
            {
                _logger.LogInfo("No image files need to be deleted");
            }
        }
    }
}
