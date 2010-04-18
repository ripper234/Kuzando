using System;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Util
{
    internal class Randomizer
    {
        private readonly Random _random;

        public Randomizer(Random random)
        {
            _random = random;
        }

        public DateTime RandomDate()
        {
            int daysBefore = 10;
            int daysRange = 13;

            int minutesInRange = daysRange * 24 * 60;
            int minutes = _random.Next(minutesInRange);
            return DateTime.Now.Subtract(TimeSpan.FromDays(daysBefore)).Add(TimeSpan.FromMinutes(minutes));
        }

        public T RandomEnum<T>()
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return values[_random.Next(values.Length)];
        }

        public int RandomInt(int n)
        {
            return _random.Next(n);
        }
    }
}