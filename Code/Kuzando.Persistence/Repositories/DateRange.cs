using System;
using Kuzando.Common;

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

        public static DateRange CreateWeekRange(DateTime start)
        {
            // zero hours/miunutes/etc...
            start = start.GetLastMidnight();

            var lastSunday = start;

            while (lastSunday.DayOfWeek != DayOfWeek.Sunday)
            {
                lastSunday = lastSunday.Subtract(TimeSpan.FromDays(1));
            }
            lastSunday = lastSunday.GetLastMidnight();
            var nextSunday = lastSunday.AddDays(7);
            return new DateRange(lastSunday, nextSunday);
        }
    }
}