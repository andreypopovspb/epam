namespace Epam.FundManager.Common
{
    static class FormatUtility
    {
        public static decimal? TryParseToDecimal(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim();
                decimal result;
                if (decimal.TryParse(value, out result))
                {
                    return result;
                }
                if (decimal.TryParse(value.Replace(".", ","), out result))
                {
                    return result;
                }
                if (decimal.TryParse(value.Replace(",", "."), out result))
                {
                    return result;
                }
            }
            return null;
        }

        public static bool TryParse(string value, out decimal result)
        {
            decimal? decimalValue = TryParseToDecimal(value);
            if (decimalValue != null)
            {
                result = decimalValue.Value;
                return true;
            }
            result = 0;
            return false;
        }

        public static int? TryParseToInt(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim();
                int result;
                if (int.TryParse(value, out result))
                {
                    return result;
                }
            }
            return null;
        }

        public static bool TryParse(string value, out int result)
        {
            int? intValue = TryParseToInt(value);
            if (intValue != null)
            {
                result = intValue.Value;
                return true;
            }
            result = 0;
            return false;
        }
    }
}
