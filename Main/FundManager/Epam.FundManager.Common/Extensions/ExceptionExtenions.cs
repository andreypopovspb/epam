using System;

namespace Epam.FundManager.Common.Extensions
{
    public static class ExceptionExtenions
    {
        public static string ToTraceString(this Exception ex, bool addNewLine = true)
        {
            return Utility.ToTraceString(ex, addNewLine);
        }
    }
}