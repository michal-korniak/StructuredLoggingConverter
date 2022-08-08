using System.Text;
using System.Text.RegularExpressions;

namespace StructuredLoggingConverter.Extensions
{
    public static class StringExtensions
    {

        public static string CapitalizeFirst(this string str)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }

            return str.ToUpper();
        }

        public static string CapitalizeAfter(this string s, IEnumerable<char> chars)
        {
            var charsHash = new HashSet<char>(chars);
            StringBuilder sb = new StringBuilder(s);
            for (int i = 0; i < sb.Length - 2; i++)
            {
                if (charsHash.Contains(sb[i]) && sb[i + 1] == ' ')
                    sb[i + 2] = char.ToUpper(sb[i + 2]);
            }
            return sb.ToString();
        }

        public static string TrimStart(this string target, string trimString)
        {
            if (string.IsNullOrEmpty(trimString)) return target;

            string result = target;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        public static string[] SplitByUpperCase(this string str)
        {
            return Regex.Split(str, @"(?<!^)(?=[A-Z])");
        }

        public static IEnumerable<string> RemoveFollowingDuplicates(this IEnumerable<string> stringEnumerable)
        {
            List<string> result = new List<string>();
            foreach (var str in stringEnumerable)
            {
                if (result.LastOrDefault() != str)
                {
                    result.Add(str);
                }
            }
            return result;
        }

        public static string Concat(this IEnumerable<string> stringEnumerable)
        {
            return string.Concat(stringEnumerable);
        }

        public static int IndexOfNonWhitespace(this string source, int startIndex = 0)
        {
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex");
            }

            if (source != null)
            {
                for (int i = startIndex; i < source.Length; i++)
                {
                    if (!char.IsWhiteSpace(source[i]))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

    }
}
