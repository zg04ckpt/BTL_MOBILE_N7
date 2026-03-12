using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace CNLib.Services.External.Sheets
{
    public class GgSheetService : IGgSheetService
    {
        private readonly SheetsService _service;

        public GgSheetService()
        {
            using var stream = new FileStream(
                Path.Combine(AppContext.BaseDirectory, "hoangcn-16a345ddbd16.json"), 
                FileMode.Open, FileAccess.Read);
            
            var credential = CredentialFactory
                .FromStream<ServiceAccountCredential>(stream)
                .ToGoogleCredential();
            
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "tool.hoangcn.com",
            });
        }

        public async Task InsertNewRowsAtAsync(string spreadsheetId, int sheetIndex, int startIndex, int endIndex)
        {
            var insertRequest = new Request
            {
                InsertDimension = new InsertDimensionRequest
                {
                    Range = new DimensionRange
                    {
                        SheetId = sheetIndex,
                        Dimension = "ROWS",
                        StartIndex = startIndex,
                        EndIndex = endIndex
                    },
                    InheritFromBefore = false
                }
            };

            var batchRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new []{insertRequest}
            };

            await _service.Spreadsheets.BatchUpdate(batchRequest, spreadsheetId).ExecuteAsync();
        }

        public async Task WriteRowAsync(string sheetId, string range, IList<object> values)
        {
            var valueRange = new ValueRange
            {
                Values = new []{values}
            };

            var request = _service.Spreadsheets.Values.Append(valueRange, sheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource
                .AppendRequest.ValueInputOptionEnum.RAW;

            await request.ExecuteAsync();
        }
    }
}
