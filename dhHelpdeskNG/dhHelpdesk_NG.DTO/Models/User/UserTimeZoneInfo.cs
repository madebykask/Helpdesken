namespace DH.Helpdesk.BusinessData.Models.User
{
    public class UserTimeZoneInfo
    {
        public UserTimeZoneInfo()
        {
        }

        public UserTimeZoneInfo(string timeZoneOffsetInJan1, string timeZoneOffsetInJul1)
        {
            TimeZoneOffsetInJan1 = timeZoneOffsetInJan1;
            TimeZoneOffsetInJul1 = timeZoneOffsetInJul1;
        }

        public string TimeZoneOffsetInJan1 { get; set; }
        public string TimeZoneOffsetInJul1 { get; set; }
    }
}