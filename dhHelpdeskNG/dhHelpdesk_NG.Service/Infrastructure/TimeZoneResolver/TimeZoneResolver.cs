using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Users;

namespace DH.Helpdesk.Services.Infrastructure.TimeZoneResolver
{
    public class TimeZoneResolver
    {
        public static TimeZoneAutodetectResult DetectTimeZone(int Jan1TimeZoneOffset, int Jul1TimeZoneOffset, out TimeZoneInfo[] detectedTimeZones)
        {
            Jan1TimeZoneOffset = -Jan1TimeZoneOffset;
            Jul1TimeZoneOffset = -Jul1TimeZoneOffset;

            var baseCompareOffset = Jan1TimeZoneOffset < Jul1TimeZoneOffset ? Jan1TimeZoneOffset : Jul1TimeZoneOffset;

            if (Jan1TimeZoneOffset == Jul1TimeZoneOffset)
            {
                // no daylightsaving
                detectedTimeZones =
                    TimeZoneInfo.GetSystemTimeZones()
                        .Where(tz => tz.BaseUtcOffset.TotalMinutes == baseCompareOffset)
                        .ToArray();
            }
            else
            {
                detectedTimeZones =
                    TimeZoneInfo.GetSystemTimeZones()
                        .Where(tz => tz.BaseUtcOffset.TotalMinutes == baseCompareOffset && tz.SupportsDaylightSavingTime)
                        .ToArray();
            }

            if (detectedTimeZones.Length == 1)
            {
                return TimeZoneAutodetectResult.Success;
            }

            if (detectedTimeZones.Length > 1)
            {
                return TimeZoneAutodetectResult.MoreThanOne;
            }

            return TimeZoneAutodetectResult.Failure;
        }
    }
}
