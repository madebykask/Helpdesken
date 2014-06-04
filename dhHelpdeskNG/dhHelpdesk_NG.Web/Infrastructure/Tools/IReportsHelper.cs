namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure.Tools.Concrete;

    public interface IReportsHelper
    {
       bool CreateRegistratedCasesCaseTypeReport(
                            ItemOverview customer,
                            ItemOverview report,
                            IEnumerable<ItemOverview> workingGroups,
                            IEnumerable<ItemOverview> caseTypes,
                            ProductArea productArea,
                            DateTime periodFrom,
                            DateTime periodUntil,
                            bool showDetails,
                            bool isPrint,
                            RegistratedCasesCaseTypeItem[] items,
                            out List<ReportFile> files);

       bool CreateRegistratedCasesDayReport(
                            ItemOverview customer,
                            ItemOverview report,
                            ItemOverview department,
                            IEnumerable<ItemOverview> caseTypes,
                            ItemOverview workingGroup,
                            ItemOverview administrator,
                            DateTime period,
                            bool isPrint,
                            RegistratedCasesDayItem[] items,
                            out ReportFile file);

        byte[] GetReportImageFromCache(string objectId, string fileName);

        string GetReportPathFromCache(string objectId, string fileName);
    }
}