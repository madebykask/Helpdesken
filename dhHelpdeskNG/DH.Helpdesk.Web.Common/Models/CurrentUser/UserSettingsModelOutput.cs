using DH.Helpdesk.Web.Common.Converters;

namespace DH.Helpdesk.Web.Common.Models.CurrentUser
{
    public class UserSettingsModelOutput
    {
        public int Id { get; set; }
        public string UserGuid { get; set; }
        public int UserRole { get; set; }
        public int CustomerId { get; set; }
        public int LanguageId { get; set; }
        public string TimeZone { get; set; }
        public MomentJsTimeZoneInfo TimeZoneMoment { get; set; }
        public bool OwnCasesOnly { get; set; }
        public bool CreateCasePermission { get; set; }
        public bool DeleteAttachedFiles { get; set; }
    }
}
