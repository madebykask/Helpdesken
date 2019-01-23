using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Web.Common.Converters
{
 /// <summary>
    ///     Tool to generates JavaScript that adds MomentJs timezone into moment.tz store.
    ///     As per http://momentjs.com/timezone/docs/
    /// </summary>
    public static class TimeZoneToMomentConverter
    {
        private static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero);

        private static readonly ConcurrentDictionary<Tuple<string, int, int, string>, MomentJsTimeZoneInfo> Cache =
            new ConcurrentDictionary<Tuple<string, int, int, string>, MomentJsTimeZoneInfo>(); //TODO: use icacheservice instead

        /// <summary>
        ///     Generates timezone class for adding MomentJs timezone into moment.tz store.
        ///     It caches the result by TimeZoneInfo.Id
        ///     Usage in momentjs: moment.tz._zones[result.name.toLowerCase().replace(/\//g, '_')] = z;
        /// </summary>
        /// <param name="timeZoneId">Windows standart TimeZone id</param>
        /// <param name="yearFrom">Minimum year</param>
        /// <param name="yearTo">Maximum year (inclusive)</param>
        /// <param name="overrideName">Name of the generated MomentJs Zone; TimeZoneInfo.Id by default</param>
        /// <returns></returns>
        public static MomentJsTimeZoneInfo GenerateAddMomentZoneScript(string timeZoneId, int yearFrom, int yearTo,
            string overrideName = null)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            if (tz == null) return null;
            var key = new Tuple<string, int, int, string>(tz.Id, yearFrom, yearTo, overrideName);

            return Cache.GetOrAdd(key, x =>
            {
                var untils = EnumerateUntils(tz, yearFrom, yearTo).ToArray();

                return new MomentJsTimeZoneInfo
                {
                    Name = overrideName ?? tz.Id,
                    Abbrs = untils.Select(u => "-").ToArray(),
                    Untils = untils.Select(u => u.Item1).ToArray(),
                    Offsets = untils.Select(u => u.Item2).ToArray()
                };
            });
        }

        private static IEnumerable<Tuple<long, int>> EnumerateUntils(TimeZoneInfo timeZone, int yearFrom, int yearTo)
        {
            // return until-offset pairs
            var maxStep = (int) TimeSpan.FromDays(7).TotalMinutes;
            Func<DateTimeOffset, int> offset = t => (int) TimeZoneInfo.ConvertTime(t, timeZone).Offset.TotalMinutes;

            var t1 = new DateTimeOffset(yearFrom, 1, 1, 0, 0, 0, TimeSpan.Zero);

            while (t1.Year <= yearTo)
            {
                var step = maxStep;

                var t2 = t1.AddMinutes(step);
                while (offset(t1) != offset(t2) && step > 1)
                {
                    step = step/2;
                    t2 = t1.AddMinutes(step);
                }

                if (step == 1 && offset(t1) != offset(t2))
                {
                    yield return new Tuple<long, int>((long) (t2 - UnixEpoch).TotalMilliseconds, -offset(t1));
                }
                t1 = t2;
            }

            yield return new Tuple<long, int>((long) (t1 - UnixEpoch).TotalMilliseconds, -offset(t1));
        }
    }
    public class MomentJsTimeZoneInfo
    {
        public string Name { get; set; }
        public string[] Abbrs { get; set; }
        public long[] Untils { get; set; } 
        public int[] Offsets { get; set; }
    }

}
