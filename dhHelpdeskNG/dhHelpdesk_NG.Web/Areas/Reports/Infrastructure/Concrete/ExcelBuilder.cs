using DH.Helpdesk.Services.DisplayValues.Report;

namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports;
    using DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator;
    using DH.Helpdesk.Web.Infrastructure;
    using HtmlAgilityPack;
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
                var ws = excelPackage.AddReportWorksheet();

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

                ws.AutoFit(column);

                return excelPackage.GetAsByteArray();
            }
        }

        public byte[] GetReportGeneratorExcel(ReportGeneratorModel data)
        {
            using (var memoryStream = new MemoryStream())
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var ws = excelPackage.AddReportWorksheet();
                var row = 1;
                var column = 1;

                foreach (var header in data.Headers)
                {
                    ws.SetHeader(row, column, Translation.GetCoreTextTranslation(header.Caption));
                    column++;
                }

                foreach (var c in data.Cases)
                {
                    column = 1;
                    row++;
                    foreach (var value in c.FieldValues)
                    {

                        var placeHolder = value.Value.GetDisplayValue().PrepareForExcel();

                        if (value.FieldName == "Description" || value.FieldName == "tblLog.Text_Internal" || value.FieldName == "tblLog.Text_External")
                        {
                            //Clear HTML
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(value.Value.GetDisplayValue());

                            string text = doc.DocumentNode.InnerText;  // This will get text but may have &nbsp; and other HTML entities
                            string decodedText = WebUtility.HtmlDecode(text); // This will decode HTML entities

                            // This regex pattern should match most emojis
                            string pattern = @"[\p{L}\p{M}\p{Z}\p{N}\p{P}\p{S}\p{C}\p{Pd}]+";

                            // Strip out emojis
                            string strippedText = Regex.Replace(decodedText, pattern, match =>
                            {
                                foreach (char ch in match.Value)
                                {
                                    UnicodeCategory unicodeCategory = char.GetUnicodeCategory(ch);
                                    if (unicodeCategory == UnicodeCategory.OtherSymbol)
                                    {
                                        return "";
                                    }
                                }
                                return match.Value;
                            });

                            strippedText = strippedText.Length > 1000 ? strippedText.Substring(0, 1000) : strippedText;

                            placeHolder = strippedText.PrepareForExcel();
                        }
            

                        var tempValue = value.Value as TimeDisplayValue;
                        ws.SetValue(row, column,
                            tempValue != null
                                ? tempValue.Value.ToString().PrepareForExcel()
                                : placeHolder);
                        column++;
                    }
                }

                ws.AutoFit(column);

                return excelPackage.GetAsByteArray();                            
            }
        }
        static string StripHtmlTags(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        static string RemoveSpecialCharacters(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                // You can extend this condition to keep the characters you want
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == ' ' || c == '.' || c == ',')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public byte[] GetFinishingCauseCustomerExcel(FinishingCauseCustomerModel data)
        {
            using (var memoryStream = new MemoryStream())
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var ws = excelPackage.AddReportWorksheet();
                var row = 1;
                var column = 1;

                ws.SetHeader(row, column, string.Format("{0} / {1}", Translation.Get("Avslutsorsak"), Translation.Get("Avdelning")));
                foreach (var department in data.Data.Departments)
                {
                    column++;
                    ws.SetHeader(row, column, department.DepartmentName);
                }

                foreach (var r in data.Data.Rows)
                {
                    row++;
                    column = 1;
                    var level = r.FinishingCause.GetLevel();
                    ws.SetRowHeader(row, column, r.FinishingCause.Name, level);
                    foreach(var c in r.Columns)
                    {
                        column++;
                        if (c.CasesNumber > 0)
                        {
                            ws.SetValue(row, column, string.Format("{0} ({1}%)", c.CasesNumber, c.CaseNumberPercents));
                        }
                    }
                }

                ws.AutoFit(column);

                return excelPackage.GetAsByteArray();  
            }
        }
    }
}