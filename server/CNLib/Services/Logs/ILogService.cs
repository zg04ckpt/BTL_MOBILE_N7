namespace CNLib.Services.Logs
{
    public interface ILogService<T>
    {
        Task LogInfo(string message);
        Task LogSuccess(string message);
        Task LogError(string message);
        Task LogError(string message, string detail);
    }
}
