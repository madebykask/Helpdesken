namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Models.Reports;

    public sealed class ReportsHelper : IReportsHelper
    {
        public void CreateRegistratedCasesCaseTypeReport(RegistratedCasesCaseTypeModel model, out string cachedReportKey)
        {
            var chart = new Chart(600, 400)
                .AddTitle("Chart Title")
                .AddSeries(
                    "Employee",
                    xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
                    yValues: new[] { "2", "6", "4", "5", "3" });

            this.SaveToCache(chart, out cachedReportKey);
        }

        public FileContentResult GetReportImageFromCache(string cacheKey)
        {
            var chart = Chart.GetFromCache(cacheKey);
            WebCache.Remove(cacheKey);
            return new FileContentResult(chart.GetBytes(), @"image/png");            
        }

        private void SaveToCache(Chart chart, out string cachedReportKey)
        {
            cachedReportKey = Guid.NewGuid().ToString();
            chart.SaveToCache(cachedReportKey, 1, false);            
        }
    }
}