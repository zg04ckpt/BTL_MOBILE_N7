using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Core.Utilities
{
    public class StringUtil
    {
        public class PolicyNames
        {
            public const string OnlyAdmin = nameof(OnlyAdmin);
        }

        public class ExceptionMessages
        {
            public const string NOT_EXIST = "An error occurred because the resource does not exist";
        }

        public class ApiMessages
        {
            public const string MaxDevice = "Maximum number of signed-in devices reached";
            public const string UserNotFound = "User not found";
            public const string UserIsBanned = "Account is banned";
            public const string PasswordNotSet = "Password is not set. Please sign in with a linked account";
            public const string PasswordIncorrect = "Incorrect password";
            public const string InvalidCredential = "Invalid credentials";
            public const string UnknownError = "Unknown error";
            public const string DataInitialError = "Data initialization failed";
            public const string LockReasonNotSet = "Please provide a reason for locking the account";
            public const string Updated = "Updated successfully";
            public const string Deleted = "Deleted successfully";

            public const string FileNotExist = "File does not exist";
            public const string DownloadProgressNotExist = "Download progress does not exist";
            public const string ProgressNotCompleted = "Download progress is not completed";
            public const string RequiredFormatId = "Please provide the format ID for video/audio";
        }

        public class LogMessages
        {
            public const string UserCreatedFromGoogleAccount = "User created from Google account";
            public const string UserLoggedIn = "User logged in";
            public const string UserLoggedOut = "User logged out";
            public static Func<string, string> UserIsLocked = reason => "Account locked: " + reason;
            public const string UserIsUnlocked = "Account unlocked";
        }

        public static int GetUserIdFromClaim(ClaimsPrincipal claims)
        {
            return int.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        /// <summary>
        /// Chuyển đổi chuỗi thành dạng tên hợp lệ (In hoa chữ cái đầu và cách nhau 1 space), VD: Hoàng Cao Nguyên
        /// </summary>
        /// <param name="name">Chuỗi cần convert</param>
        /// <returns>Tên hợp lệ</returns>
        public static string ToValidVietnameseFullName(string name)
        {
            return string.Join(" ", name.Trim().Split("\\s+").Select(e =>
            {
                return e[..1].ToUpper() + e[1..].ToLower();
            }));
        }


        /// <summary>
        /// Thêm các tham số query vào url
        /// </summary>
        public static string AppendParamsToUrl(string url, params (string Key, string Value)[] queryParams)
        {
            if (queryParams == null || !queryParams.Any())
                return url;
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var (k, v) in queryParams)
            {
                query[k] = v;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }



        /// <summary>
        /// Sinh một chuỗi ngẫu nhiên bất kì có độ dài cho trước
        /// </summary>
        public static string GetRandomString(int length)
        {
            byte[] bytes = new byte[length];
            using var rand = RandomNumberGenerator.Create();
            rand.GetBytes(bytes);
            return Encoding.UTF8.GetString(bytes);
        }


        /// <summary>
        /// Chuyển đổi chuỗi tiếng Việt thành in thường không dấu, cách nhau bằng dấu -
        /// </summary>
        public static string ToSlug(string str, int maxLen = 50)
        {
            // Xóa khoảng trắng đầu/cuối
            // Chuyển về chữ thường
            // Thay "đ"/"Đ" thành "d"
            // Thay dấu gạch ngang thành khoảng trắng (để tránh double "-")
            // Normalize sang FormD để tách dấu ra khỏi ký tự gốc (VD: "á" → "a" + dấu)    
            str = str
                .Trim().ToLowerInvariant()
                .Replace("đ", "d")
                .Replace("Đ", "d")
                .Replace("-", " ")
                .Normalize(NormalizationForm.FormD);

            // Chỉ giữ lại ký tự không phải là dấu (non-spacing mark)
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            // Chuẩn hóa lại chuỗi về dạng FormC (ghép ký tự lại bình thường)
            str = sb.ToString().Normalize(NormalizationForm.FormC);

            // Thay mọi ký tự không hợp lệ (không phải a-z hoặc 0-9) bằng dấu gạch ngang "-"
            // Xóa dấu "-" ở đầu/cuối
            str = Regex.Replace(str, @"[^a-z0-9]+", "-").Trim('-');

            // Nếu vượt quá maxLen, cắt bớt để tránh slug quá dài
            if (str.Length > maxLen)
                str = str[..maxLen].Trim('-');
            return str;
        }
    }
}
