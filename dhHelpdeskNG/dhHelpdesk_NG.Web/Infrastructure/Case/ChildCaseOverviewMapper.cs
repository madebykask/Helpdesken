namespace DH.Helpdesk.Web.Infrastructure.Case
{
    using System.Linq;

    using DH.Helpdesk.Web.Infrastructure.CaseOverview;
    using DH.Helpdesk.Web.Models.Case.ChildCase;
    
    public static class ChildCaseOverviewMapper
    {
        public static ParentCaseInfo MapBusinessToWebModel(
            this BusinessData.Models.Case.ChidCase.ParentCaseInfo bm,
            OutputFormatter formatter)
        {
            if (bm != null)
            {
                return new ParentCaseInfo()
                           {
                               ParentId = bm.ParentId,
                               CaseNumber = bm.CaseNumber,
                               Administrator = formatter.FormatUserName(bm.CaseAdministrator),
                               IsCaseClosed = bm.FinishingDate.HasValue,
							   IsChildIndependent = bm.IsChildIndependent,
                                RelationType = bm.RelationType
                           };
            }

            return null;
        }
    }
}