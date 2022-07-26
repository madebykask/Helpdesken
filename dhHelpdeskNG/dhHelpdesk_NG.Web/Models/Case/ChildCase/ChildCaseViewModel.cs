using System.Collections.Generic;

namespace DH.Helpdesk.Web.Models.Case.ChildCase
{
    using DH.Helpdesk.BusinessData.Models.Case.ChidCase;
    using DH.Helpdesk.BusinessData.Models.Case.MergedCase;
    using DH.Helpdesk.Web.Infrastructure.CaseOverview;

    public class ChildCaseViewModel
    {
        public ChildCaseViewModel()
        {
            ChildCaseList = new List<ChildCaseOverview>();
            MergedChildList= new List<MergedChildOverview>();
        }

        public OutputFormatter Formatter { get; set; }
        public IList<ChildCaseOverview> ChildCaseList { get; set; }
        public IList<MergedChildOverview> MergedChildList { get; set; }
    }
}