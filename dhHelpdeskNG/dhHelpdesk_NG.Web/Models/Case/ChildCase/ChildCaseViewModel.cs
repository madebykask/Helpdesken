namespace DH.Helpdesk.Web.Models.Case.ChildCase
{
    using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
    using DH.Helpdesk.Web.Infrastructure.CaseOverview;

    public class ChildCaseViewModel
    {
        public OutputFormatter Formatter { get; set; }

        public ChildCaseOverview[] ChildCaseList { get; set; }
    }
}