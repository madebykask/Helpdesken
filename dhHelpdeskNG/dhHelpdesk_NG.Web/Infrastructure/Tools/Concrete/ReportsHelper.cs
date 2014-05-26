namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System;
    using System.Web.Helpers;

    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Web.Models.Reports;

    public sealed class ReportsHelper : IReportsHelper
    {
        private readonly ITemporaryFilesCache temporaryFilesCache;

        public ReportsHelper(ITemporaryFilesCacheFactory temporaryFilesCacheFactory)
        {
            this.temporaryFilesCache = temporaryFilesCacheFactory.CreateForModule(ModuleName.Reports);
        }

        public void CreateRegistratedCasesCaseTypeReport(
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
    }
}