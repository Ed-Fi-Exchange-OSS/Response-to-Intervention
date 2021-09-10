using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class DateMappers
    {
        private static readonly string DateFormat = "yyyy-MM-dd";
        private static readonly string DefaultDateStr = "1753-01-01";

        public static DateTime MapToDateTime(this string str)
        {
            try
            {
                return DateTime.Parse(str);
            }
            catch
            {
                return DateTime.Parse(DefaultDateStr);
            }
        }

        public static string MapToYYYYMMdd(this DateTime a)
        {
            return IsMinimumAllowedDate(a)
                ? a.ToString(DateFormat)
                : DefaultDateStr;
        }

        public static string MapToYYYYMMdd(this DateTime? a)
        {
            if (!a.HasValue)
                return DefaultDateStr;

            return MapToYYYYMMdd(a.Value);
        }

        private static bool IsMinimumAllowedDate(DateTime a)
        {
            return a >= DateTime.Parse(DefaultDateStr);
        }
    }
}
