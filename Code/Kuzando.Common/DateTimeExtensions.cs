using System;

namespace Kuzando.Common
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long TotalTimeMillis(this DateTime date)
        {
            return (long)((date - Jan1st1970).TotalMilliseconds);
        }

        public static DateTime GetLastMidnight(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 
                                                      DateTimeKind.Utc);

        public static DateTime DaysSince1970ToDateTime(long daysSince1970)
        {
            return Epoch.AddDays(daysSince1970);
        }

        public static int GetDaysSince1970(this DateTime date)
        {
            return (int)date.Subtract(Epoch).TotalDays;
        }
    }
}
