using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Helpers;
    using System.Web.UI.DataVisualization.Charting;
    using System.Xml;

    using DH.Helpdesk.BusinessData.Models.Reports.Data.CasesInProgressDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ClosedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    using Chart = System.Web.UI.DataVisualization.Charting.Chart;

    public sealed class ReportsBuilder : IReportsBuilder
    {
        private readonly ITemporaryFilesCache temporaryFilesCache;

        public ReportsBuilder(ITemporaryFilesCacheFactory temporaryFilesCacheFactory)
        {
            this.temporaryFilesCache = temporaryFilesCacheFactory.CreateForModule(ModuleName.Reports);
        }

        public byte[] GetRegistratedCasesDayReport(RegistratedCasesDayData data, DateTime period, ReportTheme theme = null)
        {
            return GetMonthReport(data.RegisteredCases.Select(c => c.Date).ToList(), period, theme);
        }

        public void CreateCaseSatisfactionReport(int goodVotes, int normalVotes, int badVotes, int count, out ReportFile file)
        {
            var y = new List<string>
                        {
                            goodVotes.ToString(CultureInfo.InvariantCulture), 
                            normalVotes.ToString(CultureInfo.InvariantCulture), 
                            badVotes.ToString(CultureInfo.InvariantCulture), 
                            count.ToString(CultureInfo.InvariantCulture)
                        };
            var x = new List<string> { Translation.Get("Good"), Translation.Get("Normal"), Translation.Get("Bad"), Translation.Get("Count") };

            var chart = CreateChart()
                .AddSeries(
                        xValue: x,
                        yValues: y);
            string objectId;
            string fileName;
            this.SaveToCache(chart, out objectId, out fileName);
            file = new ReportFile(objectId, fileName);
        }

        public byte[] GetReportImageFromCache(string objectId, string fileName)
        {
            return this.temporaryFilesCache.GetFileContent(fileName, objectId);
        }

        public string GetReportPathFromCache(string objectId, string fileName)
        {
            return this.temporaryFilesCache.FindFilePath(fileName, objectId);
        }

        public byte[] GetClosedCasesDayReport(ClosedCasesDayData data, DateTime period, ReportTheme theme = null)
        {
            return GetMonthReport(data.ClosedCases.Select(c => c.Date).ToList(), period, theme);
        }

        public byte[] GetCasesInProgressDayReport(CasesInProgressDayData data, DateTime period, ReportTheme theme = null)
        {
            return GetMonthReport(data.Cases.Select(c => c.Date).ToList(), period, theme, SeriesChartType.Line);
        }

        private static byte[] GetMonthReport(List<DateTime> data, DateTime period, ReportTheme theme = null, SeriesChartType type = SeriesChartType.Column)
        {
            var days = DateTime.DaysInMonth(period.Year, period.Month);
            var x = new List<int>();
            var y = new List<int>();
            for (var day = 1; day < days + 1; day++)
            {
                x.Add(day);
                y.Add(data.Count(c => c.Date.Day == day));
            }

            var chart = new Chart
            {
                Width = 1000,
                Height = 300
            };

            var serie = new Series();
            serie.ChartType = type;
            var daysArr = x.ToArray();
            var numberOfCasesArr = y.ToArray();
            for (int i = 0; i < daysArr.Length; i++)
            {
                var numberOfCasesVal = numberOfCasesArr[i];
                var point = new DataPoint(daysArr[i], numberOfCasesVal)
                {
                    IsValueShownAsLabel = numberOfCasesVal > 0
                };
                serie.Points.Add(point);
            }

            chart.Series.Add(serie);
            var area = new ChartArea
            {
                AxisX = { Interval = 1, Minimum = 1, Maximum = days, Title = Translation.Get("days") },
                AxisY = { Interval = 1, Minimum = 0, Maximum = numberOfCasesArr.Max(), Title = Translation.Get("Ärenden") }
            };
            chart.ChartAreas.Add(area);

            chart.Titles.Add(new Title(string.Format("{0}: {1}", Translation.Get("Antal ärenden"), data.Count)));

            SetTheme(chart, theme != null ? theme.Theme : ReportTheme.DefaultTheme.Theme);

            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                return ms.ToArray();
            }                        
        }

        private static System.Web.Helpers.Chart CreateChart(int width = 800, int height = 300, string theme = ChartTheme.Green)
        {
            return new System.Web.Helpers.Chart(width, height, theme);
        }

        private static void SetTheme(Chart chart, string theme)
        {
            using (var ms = new MemoryStream())
            {
                byte[] themeContent = Encoding.UTF8.GetBytes(theme);
                ms.Write(themeContent, 0, themeContent.Length);
                ms.Seek(0, SeekOrigin.Begin);

                chart.Serializer.Content = SerializationContents.All;
                chart.Serializer.SerializableContent = string.Empty;
                chart.Serializer.IsTemplateMode = true;
                chart.Serializer.IsResetWhenLoading = false;

                XmlReader reader = XmlReader.Create(ms, new XmlReaderSettings { IgnoreComments = true });
                chart.Serializer.Load(reader);
            }
        }

        private void SaveToCache(
                        System.Web.Helpers.Chart chart,
                        out string objectId,
                        out string fileName)
        {
            objectId = Guid.NewGuid().ToString();
            fileName = string.Format("{0}.png", objectId);
            this.temporaryFilesCache.AddFile(chart.GetBytes("png"), fileName, objectId);
        }        
    }
}