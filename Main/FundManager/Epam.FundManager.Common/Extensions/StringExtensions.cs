using System.Text;

namespace Epam.FundManager.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Hash SHA1
        /// </summary>
        public static byte[] Hash1(this string data, Encoding encoding = null)
        {
            return Utility.Hash1(data, encoding);
        }

        /// <summary>
        /// Hash SHA256
        /// </summary>
        public static byte[] Hash256(this string data, Encoding encoding = null)
        {
            return Utility.Hash256(data, encoding);
        }

        public static string GetChecksum(this string value)
        {
            return Utility.GetChecksum(value);
        }

        public static byte[] ToByteArray(this string data)
        {
            return Utility.ToByteArray(data);
        }

        public static decimal? TryParseToDecimal(this string value)
        {
            return FormatUtility.TryParseToDecimal(value);
        }

        public static bool TryParse(this string value, out decimal result)
        {
            return FormatUtility.TryParse(value, out result);
        }

        public static int? TryParseToInt(this string value)
        {
            return FormatUtility.TryParseToInt(value);
        }

        public static bool TryParse(this string value, out int result)
        {
            return FormatUtility.TryParse(value, out result);
        }
    }
}