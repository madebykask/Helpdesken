using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.BusinessData.Models.Logs.Output
{
    public class EmailLogOverview
    {
        public int Id { get; set; }
        public int CaseHistoryId { get; set; }
        public GlobalEnums.MailTemplates MailTemplate { get; set; }
        public string Email { get; set; }
        public string CcEmail { get; set; }
    }
}