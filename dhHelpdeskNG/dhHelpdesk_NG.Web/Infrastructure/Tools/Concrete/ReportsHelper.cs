namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Helpers;

    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Extensions.DateTime;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;

    public sealed class ReportsHelper : IReportsHelper
    {
        private readonly ITemporaryFilesCache temporaryFilesCache;

        public ReportsHelper(ITemporaryFilesCacheFactory temporaryFilesCacheFactory)
        {
            this.temporaryFilesCache = temporaryFilesCacheFactory.CreateForModule(ModuleName.Reports);
        }

        public bool CreateRegistratedCasesCaseTypeReport(
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
                            out List<ReportFile> files)
        {
            files = new List<ReportFile>();
            foreach (var caseType in caseTypes)
            {
                var from = periodFrom.RoundToMonth();
                var until = periodUntil.RoundToMonth();
                if (from > until)
                {
                    return false;
                }

                var caseTypeItems = items.Where(i => i.CaseTypeId == int.Parse(caseType.Value));

                var x = new List<string>();
                var y = new List<string>();
                var needChart = false;
                while (from <= until)
                {
                    x.Add(from.ToMonthYear());
                    var casesCount = caseTypeItems.Count(i => i.RegistrationDate.RoundToMonth() == from);
                    if (casesCount > 0)
                    {
                        needChart = true;
                    }

                    y.Add(casesCount.ToString(CultureInfo.InvariantCulture));
                    from = from.AddMonths(1);
                }

                if (!needChart)
                {
                    continue;
                }

                var chart = this.CreateChart()
                    .AddTitle(caseType.Name, caseType.Name)
                    .AddSeries(
                        caseType.Name,
                        xValue: x,
                        yValues: y);

                string objectId;
                string fileName;
                this.SaveToCache(chart, out objectId, out fileName);
                var file = new ReportFile(
                                objectId,
                                isPrint ? this.GetReportPathFromCache(objectId, fileName) : fileName);
                files.Add(file);
            }

            return true;
        }

        public byte[] GetReportImageFromCache(string objectId, string fileName)
        {
            return this.temporaryFilesCache.GetFileContent(fileName, objectId);
        }

        public string GetReportPathFromCache(string objectId, string fileName)
        {
            return this.temporaryFilesCache.FindFilePath(fileName, objectId);
        }

        private void SaveToCache(
                        Chart chart,
                        out string objectId,
                        out string fileName)
        {
            objectId = Guid.NewGuid().ToString();
            fileName = string.Format("{0}.png", objectId);
            this.temporaryFilesCache.AddFile(chart.GetBytes("png"), fileName, objectId);
        }

        private Chart CreateChart()
        {
            return new Chart(500, 400);
        }
    }
}