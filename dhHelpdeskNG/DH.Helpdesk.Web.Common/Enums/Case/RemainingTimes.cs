namespace DH.Helpdesk.Web.Common.Enums.Case
{
    public enum RemainingTimes
    {
        Delayed = int.MinValue,
        OneHour = 1,
        TwoHours = 2,
        FourHours = 3,
        EightHours = 4,
        OneDay = 5,
        TwoDays = 6,
        ThreeDays = 7,
        FourDays = 8,
        FiveDays = 9,
        MaxDays = int.MaxValue        
    }

    public static class RemainingTimesMapper
    {
        public static string ToStr(this RemainingTimes it)
        {
            var intIt = (int) it;
            return intIt.ToString();
        }
    }
}