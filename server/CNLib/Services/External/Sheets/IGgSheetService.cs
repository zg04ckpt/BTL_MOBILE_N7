namespace CNLib.Services.External.Sheets
{
    public interface IGgSheetService
    {
        Task WriteRowAsync(string spreadsheetId, string range, IList<object> values);
        Task InsertNewRowsAtAsync(string spreadsheetId, int sheetIndex, int startIndex, int endIndex);
    }
}
