namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Helpers;
    using System.Web.UI.DataVisualization.Charting;
    using System.Xml;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;

    public sealed class ReportsBuilder : IReportsBuilder
    {
        private const string DefaultTheme = ChartTheme.Green;

        public byte[] GetRegistratedCasesDayReport(RegistratedCasesDayData data, DateTime period)
        {
            var days = DateTime.DaysInMonth(period.Year, period.Month);
            var x = new List<int>();
            var y = new List<int>();
            for (var day = 1; day < days + 1; day++)
            {
                x.Add(day);
                y.Add(data.RegisteredCases.Count(c => c.Date.Day == day));
            }

            var chart = new System.Web.UI.DataVisualization.Charting.Chart
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
                               AxisX = { Interval = 1, Minimum = 1, Maximum = days, Title = "Days" },
                               AxisY = { Interval = 1, Title = "Registered cases" }
                           };
            chart.ChartAreas.Add(area);   
         
            using (var memoryStream = new MemoryStream())
            {
                byte[] themeContent = Encoding.UTF8.GetBytes(DefaultTheme);
                memoryStream.Write(themeContent, 0, themeContent.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                SetTheme(chart, memoryStream);
            }

            using (var ms = new MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                return ms.ToArray();                
            }
        }

        private static void SetTheme(System.Web.UI.DataVisualization.Charting.Chart chart, Stream templateStream)
        {
            chart.Serializer.Content = SerializationContents.All;
            chart.Serializer.SerializableContent = string.Empty;
            chart.Serializer.IsTemplateMode = true;
            chart.Serializer.IsResetWhenLoading = false;

            XmlReader reader = XmlReader.Create(templateStream, new XmlReaderSettings { IgnoreComments = true });
            chart.Serializer.Load(reader);
        }
    }
}