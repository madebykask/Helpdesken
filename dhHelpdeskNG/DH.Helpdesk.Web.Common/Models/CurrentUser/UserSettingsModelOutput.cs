using DH.Helpdesk.Web.Common.Converters;

namespace DH.Helpdesk.Web.Common.Models.CurrentUser
{
    public class UserSettingsModelOutput
    {
        public int CustomerId { get; set; }
        public int LanguageId { get; set; }
        public string TimeZone { get; set; }
        public MomentJsTimeZoneInfo TimeZoneMoment { get; set; }
    }
}
