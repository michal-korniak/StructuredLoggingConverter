using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
