using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CNLib.Utils
{
    public class StringUtil
    {
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
