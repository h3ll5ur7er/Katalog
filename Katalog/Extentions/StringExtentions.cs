using System.Text.RegularExpressions;

namespace Katalog
{
    public static class StringExtentions
    {
        public static bool Matches(this string s, string pattern)
        {
            return new Regex(pattern).Matches(s).Count>0;
        }
    }
}