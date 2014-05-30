namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System;
    using System.Collections.Generic;

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
                            out List<ReportFile> files);

        byte[] GetReportImageFromCache(string objectId, string fileName);

        string GetReportPathFromCache(string objectId, string fileName);
    }
}