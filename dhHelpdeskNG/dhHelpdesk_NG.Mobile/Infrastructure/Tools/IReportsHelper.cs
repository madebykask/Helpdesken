namespace DH.Helpdesk.Mobile.Infrastructure.Tools
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Mobile.Infrastructure.Tools.Concrete;

    public interface IReportsHelper
    {
       void CreateRegistratedCasesCaseTypeReport(
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

       void CreateRegistratedCasesDayReport(
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

       void CreateAverageSolutionTimeReport(
                            ItemOverview customer,
                            ItemOverview report,
                            ItemOverview department,
                            IEnumerable<ItemOverview> caseTypes,
                            ItemOverview workingGroup,
                            DateTime periodFrom,
                            DateTime periodUntil,
                            bool isPrint,
                            out ReportFile file);

        byte[] GetReportImageFromCache(string objectId, string fileName);

        string GetReportPathFromCache(string objectId, string fileName);
    }
}