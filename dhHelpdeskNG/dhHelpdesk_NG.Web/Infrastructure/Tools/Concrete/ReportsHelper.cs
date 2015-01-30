namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
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
    using System.Xml.Linq;

    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Extensions.DateTime;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Domain;

    using Chart = System.Web.Helpers.Chart;

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


            var c = new global::System.Web.UI.DataVisualization.Charting.Chart();
            c.Width = 1000;
            c.Height = 300;           

            var serie = new Series();
            var xArr = x.ToArray();
            var yArr = y.ToArray();
            for (int i = 0; i < xArr.Length; i++)
            {
                var yVal = yArr[i];
                var point = new DataPoint(xArr[i], yVal);
                point.IsValueShownAsLabel = yVal > 0;
                serie.Points.Add(point);
            }

            c.Series.Add(serie);
            var area = new ChartArea();
            area.AxisX.Interval = 1;
            area.AxisX.Minimum = 1;
            area.AxisX.Maximum = days;
            area.AxisX.Title = "Days";
            area.AxisY.Interval = 1;
            area.AxisY.Title = "Registered cases";
            c.ChartAreas.Add(area);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] themeContent = Encoding.UTF8.GetBytes(ChartTheme.Green);
                memoryStream.Write(themeContent, 0, themeContent.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                LoadChartThemeFromFile(c, memoryStream);
            }

            using (var imagestream = new MemoryStream())
            {
                c.SaveImage(imagestream, ChartImageFormat.Png);
                byte[] imageBytes = imagestream.ToArray();

                var objectId = Guid.NewGuid().ToString();
                var fileName = string.Format("{0}.png", objectId);
                this.temporaryFilesCache.AddFile(imageBytes, fileName, objectId);
                file = new ReportFile(
                                objectId,
                                isPrint ? this.GetReportPathFromCache(objectId, fileName) : fileName);
            }
            


            /*var theme = XDocument.Parse(ChartTheme.Green);
            var axisX = theme.Descendants("AxisX").FirstOrDefault();
            if (axisX != null)
            {
                axisX.Add(new XAttribute("Interval", "1"));
                var mg = axisX.Descendants("MajorGrid").FirstOrDefault();
                if (mg != null)
                {
                    mg.Add(new XAttribute("Interval", "1"));
                }
            }            

            var chart = this.CreateChart(1000, 300, theme.ToString())
                .AddSeries(
                        xValue: x,
                        yValues: y).SetXAxis("Days", 1, days).SetYAxis("Registered cases");


            string objectId;
            string fileName;
            this.SaveToCache(chart, out objectId, out fileName);
            file = new ReportFile(
                            objectId,
                            isPrint ? this.GetReportPathFromCache(objectId, fileName) : fileName);*/
        }


        private static void LoadChartThemeFromFile(global::System.Web.UI.DataVisualization.Charting.Chart chart, Stream templateStream)
        {
            // workarounds for Chart templating bugs mentioned in: 
            // http://social.msdn.microsoft.com/Forums/en-US/MSWinWebChart/thread/b50d5b7e-30e2-4948-af7a-370d9be1268a
            chart.Serializer.Content = global::System.Web.UI.DataVisualization.Charting.SerializationContents.All;
            chart.Serializer.SerializableContent = String.Empty; // deserialize all content
            chart.Serializer.IsTemplateMode = true;
            chart.Serializer.IsResetWhenLoading = false;
            // loading serializer with stream to avoid bug with template file getting locked in VS 


            // The default xml reader used by the serializer does not ignore comments 
            // Using the IsUnknownAttributeIgnored fixes this, but then it would give no feedback to the user
            // if member names do not match the spelling and casing of Chart properties. 
            XmlReader reader = XmlReader.Create(templateStream, new XmlReaderSettings { IgnoreComments = true });
            chart.Serializer.Load(reader);
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

        private Chart CreateChart(int width = 800, int height = 300, string theme = ChartTheme.Green)
        {
            return new Chart(width, height, theme);
        }
    }
}