using Microsoft.Extensions.DependencyInjection;

namespace CNLib.Services.Logs
{
    public class ConsoleLogService<T> : ILogService<T>
    {
        public Task LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {message}");
            Console.ForegroundColor = ConsoleColor.White;
            return Task.CompletedTask;
        }

        public Task LogError(string message, string detail)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {message} ({detail})");
            Console.ForegroundColor = ConsoleColor.White;
            return Task.CompletedTask;
        }

        public Task LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {message}");
            return Task.CompletedTask;
        }

        public Task LogSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS] {message}");
            Console.ForegroundColor = ConsoleColor.White;
            return Task.CompletedTask;
        }
    }
}
