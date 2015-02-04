namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.UI.DataVisualization.Charting;
    using System.Xml;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class ReportsBuilder : IReportsBuilder
    {
        public byte[] GetRegistratedCasesDayReport(RegistratedCasesDayData data, DateTime period, ReportTheme theme = null)
        {
            var days = DateTime.DaysInMonth(period.Year, period.Month);
            var x = new List<int>();
            var y = new List<int>();
            for (var day = 1; day < days + 1; day++)
            {
                x.Add(day);
                y.Add(data.RegisteredCases.Count(c => c.Date.Day == day));
            }

            var chart = new Chart
                        {
                            Width = 1000, 
                            Height = 300
                        };

            var serie = new Series();
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

            chart.Titles.Add(new Title(string.Format("{0}: {1}", Translation.Get("Antal ärenden"), data.RegisteredCases.Count)));

            SetTheme(chart, theme != null ? theme.Theme : ReportTheme.DefaultTheme.Theme);

            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                return ms.ToArray();                
            }
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
    }
}