using System;

namespace GlobalNavService.Utils.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Determines whether [is absolute URL] [the specified URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool IsAbsoluteUrl(this string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        /// <summary>
        /// Trims the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string TrimUrl(this string url)
        {
            return System.Text.RegularExpressions.Regex.Replace(url, "([^:]/)/+", "$1");
        }
    }
}