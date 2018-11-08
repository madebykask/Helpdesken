using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime.TimeZones;

namespace DH.Helpdesk.Web.Common.Converters
{
    public static class TimeZoneConverter
    {
        public static string IanaToWindows(this string ianaZoneId)
        {
            var utcZones = new[] { "Etc/UTC", "Etc/UCT", "Etc/GMT" };
            if (utcZones.Contains(ianaZoneId, StringComparer.Ordinal))
                return "UTC";

            var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;

            // resolve any link, since the CLDR doesn't necessarily use canonical IDs
            var links = tzdbSource.CanonicalIdMap
                .Where(x => x.Value.Equals(ianaZoneId, StringComparison.Ordinal))
                .Select(x => x.Key);

            // resolve canonical zones, and include original zone as well
            var possibleZones = tzdbSource.CanonicalIdMap.ContainsKey(ianaZoneId)
                ? links.Concat(new[] { tzdbSource.CanonicalIdMap[ianaZoneId], ianaZoneId })
                : links;

            // map the windows zone
            var mappings = tzdbSource.WindowsMapping.MapZones;
            var item = mappings.FirstOrDefault(x => x.TzdbIds.Any(possibleZones.Contains));
            return item?.WindowsId;
        }

        // This will return the "primary" IANA zone that matches the given windows zone.
        // If the primary zone is a link, it then resolves it to the canonical ID.
        public static string WindowsToIana(this string windowsZoneId)
        {
            // Avoid UTC being mapped to Etc/GMT, which is the mapping in CLDR
            if (windowsZoneId == "UTC")
            {
                return "Etc/UTC";
            }
            var source = TzdbDateTimeZoneSource.Default;
            // If there's no such mapping, result will be null.
            source.WindowsMapping.PrimaryMapping.TryGetValue(windowsZoneId, out var result);
            // Canonicalize
            if (result != null)
            {
                result = source.CanonicalIdMap[result];
            }
            return result;
        }
    }
}
