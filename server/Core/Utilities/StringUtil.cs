using System.Globalization;
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
            public const string NOT_EXIST = "Lỗi xảy ra do không tồn tại";
        }

        public class ApiMessages
        {
            public const string MaxDevice = "Số lượng thiết bị đăng nhập đã đạt tối đa";
            public const string UserNotFound = "Người dùng không tồn tại";
            public const string UserIsBanned = "Tài khoản đang bị khóa";
            public const string PasswordNotSet = "Mật khẩu chưa được thiết lập, vui lòng đăng nhập bằng tài khoản liên kết";
            public const string PasswordIncorrect = "Mật khẩu chưa chính xác";
            public const string InvalidCredential = "Thông tin xác thực không hợp lệ";
            public const string UnknownError = "Lỗi không xác định";
            public const string DataInitialError = "Khởi tạo dữ liệu thất bại";
            public const string LockReasonNotSet = "Vui lòng cung cấp lí do khóa tài khoản";
            public const string Updated = "Cập nhật thành công";
            public const string Deleted = "Đã xóa thành công";

            public const string FileNotExist = "File không tồn tại";
            public const string DownloadProgressNotExist = "Tiến trình tải không tồn tại";
            public const string ProgressNotCompleted = "Tiến trình tải chưa hoàn thành";
            public const string RequiredFormatId = "Vui lòng cùng cấp format ID cho video/audio";
        }

        public class LogMessages
        {
            public const string UserCreatedFromGoogleAccount = "Tạo tài khoản từ Google Account";
            public const string UserLoggedIn = "Đã đăng nhập";
            public const string UserLoggedOut = "Đã đăng xuất";
            public static Func<string, string> UserIsLocked = reason => "Bị khóa tài khoản: " + reason;
            public const string UserIsUnlocked = "Đã mở khóa tài khoản";
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
