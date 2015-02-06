namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete
{
    using System;
    using System.Globalization;
    using System.IO;

    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure;

    using OfficeOpenXml;

    public sealed class ExcelBuilder : IExcelBuilder
    {
        public string GetExcelFileName(ReportType report)
        {
            return string.Format("{0} Report.xlsx", DateTime.Now).Replace(" ", "_");
        }

        public byte[] GetCaseTypeArticleNoExcel(
                        CaseTypeArticleNoData data,
                        bool isShowCaseTypeDetails,
                        bool isShowPercents)
        {
            using (var memoryStream = new MemoryStream())
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var ws = excelPackage.Workbook.Worksheets.Add("Report");

                var row = 1;
                var column = 1;                
                ws.SetHeader(row, column, Translation.Get("Produktområde"));
                foreach (var caseType in data.CaseTypes)
                {
                    column++;
                    ws.SetHeader(row, column, caseType.Name);
                }

                ws.SetHeader(row, column, Translation.Get("Totalt"));                
                row++;

                if (isShowCaseTypeDetails && data.HasCaseTypeDetails())
                {
                    column = 1;
                    ws.SetEmpty(row, column);
                    foreach (var caseType in data.CaseTypes)
                    {
                        ws.SetValue(row, column, caseType.Details);
                        column++;
                    }

                    ws.SetEmpty(row, column);
                    row++;
                }

                foreach (var productArea in data.GetLineProductAreas())
                {
                    column = 1;
                    var level = productArea.GetLevel();
                    ws.SetRowHeader(row, column, productArea.ProductArea.Name, level);
                    for (var i = 0; i < productArea.Cases.Count; i++)
                    {
                        column++;
                        var cases = productArea.Cases[i];
                        ws.SetValue(row, column, cases.Number > 0 ?
                                      (isShowPercents ?
                                           string.Format("{0} ({1}%)", cases.Number, data.GetTotalPercentsForCaseType(cases, data.CaseTypes[i]))
                                           : cases.Number.ToString(CultureInfo.InvariantCulture)) : string.Empty);
                    }

                    var total = productArea.GetTotal();
                    ws.SetValue(row, column, total > 0 ?
                                      (isShowPercents ?
                                           string.Format("{0} ({1}%)", total, data.GetTotalPercentsForProductArea(productArea))
                                           : total.ToString(CultureInfo.InvariantCulture)) : string.Empty);
                    row++;
                }

                column = 1;
                ws.SetFooter(row, column, Translation.Get("Totalt"));
                foreach (var caseType in data.CaseTypes)
                {
                    column++;
                    var caseTypeTotal = data.GetTotalForCaseType(caseType);
                    ws.SetFooter(row, column, caseTypeTotal > 0 ?
                                    (isShowPercents ?
                                        string.Format("{0} (100%)", caseTypeTotal)
                                         : caseTypeTotal.ToString(CultureInfo.InvariantCulture)) : string.Empty);
                }

                var totalAll = data.GetTotal();
                ws.SetFooter(row, column, totalAll > 0 ?
                                    (isShowPercents ?
                                        string.Format("{0} (100%)", totalAll)
                                         : totalAll.ToString(CultureInfo.InvariantCulture)) : string.Empty);

                for (var i = 1; i <= column; i++)
                {
                    ws.Column(i).AutoFit();
                }

                return excelPackage.GetAsByteArray();
            }
        }
    }
}