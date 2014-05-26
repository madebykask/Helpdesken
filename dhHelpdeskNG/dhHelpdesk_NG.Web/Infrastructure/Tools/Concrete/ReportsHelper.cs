namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Web.Helpers;

    using DH.Helpdesk.Common.Extensions.DateTime;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Web.Models.Reports;

    public sealed class ReportsHelper : IReportsHelper
    {
        private readonly ITemporaryFilesCache temporaryFilesCache;

        public ReportsHelper(ITemporaryFilesCacheFactory temporaryFilesCacheFactory)
        {
            this.temporaryFilesCache = temporaryFilesCacheFactory.CreateForModule(ModuleName.Reports);
        }

        public bool CreateRegistratedCasesCaseTypeReport(
                            RegistratedCasesCaseTypeModel model,
                            out string objectId,
                            out string fileName)
        {
            var chart = new Chart(500, 400)
                           .AddTitle("Chart Title")
                           .AddSeries(
                               "Employee",
                               xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
                               yValues: new[] { "2", "6", "4", "5", "3" });

            this.SaveToCache(chart, out objectId, out fileName);


//            objectId = null;
//            fileName = null;
//            var from = model.PeriodFrom.RoundToMonth();
//            var until = model.PeriodUntil.RoundToMonth();
//            if (from > until)
//            {
//                return false;
//            }
//
//            var x = new List<string>();
//            while (from <= until)
//            {
//                x.Add(from.ToMonthYear());
//                from = from.AddMonths(1);
//            }
//
//            var y = new List<string>();
//
//            var chart = this.CreateChart()
//                .AddSeries(
//                    xValue: x,
//                    yValues: y)
//                .AddSeries(
//                    xValue: x,
//                    yValues: y);
//
//            this.SaveToCache(chart, out objectId, out fileName);
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