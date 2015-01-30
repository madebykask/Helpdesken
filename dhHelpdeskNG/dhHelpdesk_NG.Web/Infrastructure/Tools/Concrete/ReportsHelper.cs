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

        public void CreateRegistratedCasesCaseTypeReport(
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
                    return;
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
        }

        public void CreateRegistratedCasesDayReport(
            ItemOverview customer,
            ItemOverview report,
            ItemOverview department,
            IEnumerable<ItemOverview> caseTypes,
            ItemOverview workingGroup,
            ItemOverview administrator,
            DateTime period,
            bool isPrint,
            RegistratedCasesDayItem[] items,
            out ReportFile file)
        {
            var days = DateTime.DaysInMonth(period.Year, period.Month);
            var x = new List<int>();
            var y = new List<int>();
            for (var day = 1; day < days + 1; day++)
            {
                x.Add(day);    
                y.Add(items.Count(i => i.RegistrationDate.Day == day));
            }

            var chart = this.CreateChart(800, 100)
                .AddSeries(
                        xValue: x,
                        yValues: y);
            string objectId;
            string fileName;
            this.SaveToCache(chart, out objectId, out fileName);
            file = new ReportFile(
                            objectId,
                            isPrint ? this.GetReportPathFromCache(objectId, fileName) : fileName);
        }

        public void CreateAverageSolutionTimeReport(
            ItemOverview customer,
            ItemOverview report,
            ItemOverview department,
            IEnumerable<ItemOverview> caseTypes,
            ItemOverview workingGroup,
            DateTime periodFrom,
            DateTime periodUntil,
            bool isPrint,
            out ReportFile file)
        {
            var from = periodFrom.RoundToMonth();
            var until = periodUntil.RoundToMonth();

            var x = new List<string>();
            var y = new List<string>();
            while (from <= until)
            {
                x.Add(from.ToMonthYear());
                y.Add("1");
                from = from.AddMonths(1);
            }

            var chart = this.CreateChart()
                .AddSeries(
                        xValue: x,
                        yValues: y);
            string objectId;
            string fileName;
            this.SaveToCache(chart, out objectId, out fileName);
            file = new ReportFile(
                            objectId,
                            isPrint ? this.GetReportPathFromCache(objectId, fileName) : fileName);
        }

        public byte[] GetReportImageFromCache(string objectId, string fileName)
        {
            return this.temporaryFilesCache.GetFileContent(fileName, objectId);
        }

        public string GetReportPathFromCache(string objectId, string fileName)
        {
            return this.temporaryFilesCache.FindFilePath(fileName, objectId);
        }

        public void CreateCaseSatisfactionReport(int goodVotes, int normalVotes, int badVotes, int count, out ReportFile file)
        {
            var y = new List<string>() { goodVotes.ToString(), normalVotes.ToString(), badVotes.ToString(), count.ToString() };
            var x = new List<string>() { Translation.Get("Good"), Translation.Get("Normal"), Translation.Get("Bad"), Translation.Get("Count") };

            var chart = this.CreateChart()
                .AddSeries(
                        xValue: x,
                        yValues: y,
                        chartType: "Column");
            string objectId;
            string fileName;
            this.SaveToCache(chart, out objectId, out fileName);
            file = new ReportFile(objectId, fileName);
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

        private Chart CreateChart(int width = 1000, int height = 100)
        {
            return new Chart(width, height);
        }
    }
}