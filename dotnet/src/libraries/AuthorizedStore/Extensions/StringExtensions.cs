using System;
using System.Text.RegularExpressions;

namespace AuthorizedStore.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex s_invalidKeywordValidator
            = new Regex("[!@#$%^&*()`~\\-_=+\\[\\]\\{\\}\\\\|;:'\",.<>/?]+", RegexOptions.Compiled);

        public static bool ContainsInvalidKeywords(this string input)
            => s_invalidKeywordValidator.IsMatch(input ?? string.Empty);

        public static bool CheckNullOrWhiteSpace(string value, string paramName)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException(paramName, $"{paramName} is required.")
                : false;
        }
    }
}
