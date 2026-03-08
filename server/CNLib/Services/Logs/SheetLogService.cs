using CNLib.Services.External.Sheets;
using Microsoft.Extensions.Options;

namespace CNLib.Services.Logs
{
    public class SheetLogService<T> : ILogService<T>
    {
        private readonly IGgSheetService _sheetService;
        private readonly SheetLogConfig _config;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public SheetLogService(
            IGgSheetService sheetService, 
            IOptions<SheetLogConfig> config)
        {
            _sheetService = sheetService;
            _config = config.Value;
        }

        public async Task LogError(string message)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _sheetService.InsertNewRowsAtAsync(_config.SpreadsheetId, _config.SheetId, 1, 2);
                await _sheetService.WriteRowAsync(_config.SpreadsheetId, $"{_config.SheetName}!A2", new[]
                {
                    "ERR",
                    DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                    message
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task LogError(string message, string detail)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _sheetService.InsertNewRowsAtAsync(_config.SpreadsheetId, _config.SheetId, 1, 2);
                await _sheetService.WriteRowAsync(_config.SpreadsheetId, $"{_config.SheetName}!A2", new []{
                        "ERR",
                        DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                        message,
                        detail
                    });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task LogInfo(string message)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _sheetService.InsertNewRowsAtAsync(_config.SpreadsheetId, _config.SheetId, 1, 2);
                await _sheetService.WriteRowAsync(_config.SpreadsheetId, $"{_config.SheetName}!A2", new []{
                        "INF",
                        DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                        message
                    });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task LogSuccess(string message)
        {
            await _semaphore.WaitAsync();
            try
            {
                await _sheetService.InsertNewRowsAtAsync(_config.SpreadsheetId, _config.SheetId, 1, 2);
                await _sheetService.WriteRowAsync(_config.SpreadsheetId, $"{_config.SheetName}!A2", new []{
                        "SUC",
                        DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                        message
                    });
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
