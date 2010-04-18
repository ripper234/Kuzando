using System;

namespace Kuzando.Persistence.Repositories
{
    public class DateRange
    {
        public readonly DateTime From;
        public readonly DateTime To;

        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return From + "-" + To;
        }

        public static DateRange CreateWeekRange(DateTime now)
        {
            var lastSunday = DateTime.Now;
            while (lastSunday.DayOfWeek != DayOfWeek.Sunday)
            {
                lastSunday = lastSunday.Subtract(TimeSpan.FromDays(1));
            }
            lastSunday = GetMidnight(lastSunday);
            var nextSunday = lastSunday.AddDays(7);
            return new DateRange(lastSunday, nextSunday);
        }

        private static DateTime GetMidnight(DateTime sunday)
        {
            return new DateTime(sunday.Year, sunday.Month, sunday.Day);
        }
    }
}