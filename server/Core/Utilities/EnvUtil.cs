namespace Core.Utilities
{
    public class EnvUtil
    {
        public class Keys
        {
            public const string QUIZBATTLE_PGDB_CONNECTION_STRING = nameof(QUIZBATTLE_PGDB_CONNECTION_STRING);
            public const string QUIZBATTLE_PGDB_FOR_HANGFIRE_CONNECTION_STRING = nameof(QUIZBATTLE_PGDB_FOR_HANGFIRE_CONNECTION_STRING);
            public const string QUIZBATTLE_SECRET_KEY = nameof(QUIZBATTLE_SECRET_KEY);
            public const string QUIZBATTLE_ADMIN_EMAIL = nameof(QUIZBATTLE_ADMIN_EMAIL);
            public const string QUIZBATTLE_ADMIN_PASS = nameof(QUIZBATTLE_ADMIN_PASS);
            public const string QUIZBATTLE_GOOGLE_CLIENT_ID = nameof(QUIZBATTLE_GOOGLE_CLIENT_ID);
            public const string QUIZBATTLE_GOOGLE_CLIENT_SECRET = nameof(QUIZBATTLE_GOOGLE_CLIENT_SECRET);
            public const string QUIZBATTLE_CLOUDINARY_NAME = nameof(QUIZBATTLE_CLOUDINARY_NAME);
            public const string QUIZBATTLE_CLOUDINARY_API_KEY = nameof(QUIZBATTLE_CLOUDINARY_API_KEY);
            public const string QUIZBATTLE_CLOUDINARY_API_SECRET = nameof(QUIZBATTLE_CLOUDINARY_API_SECRET);
        }

        public static string GetEnv(string key)
        {
            return Environment.GetEnvironmentVariable(key)
                ?? throw new InvalidOperationException($"Required environment variable '{key}' was not found.");
        }
    }
}
