namespace CNLib.Utils
{
    public class EnvUil
    {
        public static string GetEnv(string key)
        {
            return Environment.GetEnvironmentVariable(key)
                ?? throw new InvalidOperationException($"Required environment variable '{key}' was not found.");
        }
    }
}
