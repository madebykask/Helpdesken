namespace DH.Helpdesk.Web.Infrastructure.Case
{
    using System.Linq;

    using DH.Helpdesk.Web.Infrastructure.CaseOverview;
    using DH.Helpdesk.Web.Models.Case.ChildCase;
    
    public static class ChildCaseOverviewMapper
    {
        public static ChildCaseOverview[] MapBusinessToWebModel(this BusinessData.Models.Case.ChidCase.ChildCaseOverview[] list, OutputFormatter formatter)
        {
            if (list != null)
            {
                return
                    list.Select(
                        it =>
                        new ChildCaseOverview()
                            {
                                Id = it.Id,
                                CaseNo = it.CaseNo,
                                Subject = it.Subject,
                                CasePerformer = formatter.FormatUserName(it.CasePerformer),
                                CaseType = it.CaseType,
                                SubStatus = it.SubStatus,
                                RegistrationDate = formatter.FormatDate(it.RegistrationDate),
                                ClosingDate = formatter.FormatNullableDate(it.ClosingDate)
                            }).ToArray();
            }

            return null;
        }

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
                               IsCaseClosed = bm.FinishingDate.HasValue
                           };
            }

            return null;
        }
    }
}