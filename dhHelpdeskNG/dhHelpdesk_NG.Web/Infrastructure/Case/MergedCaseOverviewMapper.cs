namespace DH.Helpdesk.Web.Infrastructure.Case
{
    using System.Linq;

    using DH.Helpdesk.Web.Infrastructure.CaseOverview;
    using DH.Helpdesk.Web.Models.Case.ChildCase;

    public static class MergedCaseOverviewMapper
    {
        public static MergedParentInfo MapBusinessToWebModel(
            this BusinessData.Models.Case.MergedCase.MergedParentInfo bm,
            OutputFormatter formatter)
        {
            if (bm != null)
            {
                return new MergedParentInfo()
                           {
                               ParentId = bm.ParentId,
                               CaseNumber = bm.CaseNumber,
                               Administrator = formatter.FormatUserName(bm.CaseAdministrator)
                           };
            }

            return null;
        }
    }
}
