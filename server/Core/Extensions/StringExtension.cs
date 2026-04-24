namespace Core.Extensions
{
    public static class StringExtension
    {
        public static bool EqualIgnoreCase(this string s1, string s2)
            => s1.ToLower() == s2.ToLower();
    }
}
