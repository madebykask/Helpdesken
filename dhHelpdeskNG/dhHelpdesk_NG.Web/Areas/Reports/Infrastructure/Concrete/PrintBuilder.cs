namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Concrete
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;

    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Print;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Areas.Reports.Infrastructure.Utils;
    using DH.Helpdesk.Web.Infrastructure;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class PrintBuilder : IPrintBuilder
    {
        public string GetPrintFileName(ReportType report)
        {
            return string.Format("{0} Report.pdf", DateTime.Now).Replace(" ", "_");
        }

        public byte[] GetCaseTypeArticleNoPrint(
            CaseTypeArticleNoPrintData data,
            DateTime? periodFrom,
            DateTime? periodUntil,
            ShowCases showCases,
            bool isShowCaseTypeDetails,
            bool isShowPercents)
        {
            using (var ms = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                var writer = PdfWriter.GetInstance(document, ms);
                writer.PageEvent = new ReportPrintEvents(GetHeader(), null);
                document.Open();

                document.AddReportDate();
                document.AddReportTitle(ReportUtils.GetReportName(ReportType.CaseTypeArticleNo));
                document.AddLine();
                document.AddReportParams(
                        Translation.Get("Avdelning påverkad"),
                        data.Departments.ToValuesList(),
                        Translation.Get("Driftgrupp"), 
                        data.WorkingGroups.ToValuesList(),
                        Translation.Get("Ärendetyp"), 
                        data.CaseTypes.ToValuesList(),
                        Translation.Get("Produktområde"), 
                        data.ProductAreas.ToValuesList(),
                        Translation.Get("Period från"), 
                        periodFrom.HasValue ? periodFrom.Value.ToShortDateString() : string.Empty,
                        Translation.Get("Period till"), 
                        periodUntil.HasValue ? periodUntil.Value.ToShortDateString() : string.Empty,
                        Translation.Get("Visa"), 
                        showCases == ShowCases.AllCases ? Translation.Get("Alla ärenden") : Translation.Get("Pågående ärenden"));

                var table = new PdfPTable(data.Data.CaseTypes.Count + 2)
                                {
                                    WidthPercentage = 100, 
                                    SpacingBefore = 20f
                                };

                table.AddHeader(Translation.Get("Produktområde"));
                foreach (var caseType in data.Data.CaseTypes)
                {
                    table.AddHeader(caseType.Name);
                }

                table.AddHeader(Translation.Get("Totalt"));

                if (isShowCaseTypeDetails && data.Data.HasCaseTypeDetails())
                {
                    table.AddEmpty();
                    foreach (var caseType in data.Data.CaseTypes)
                    {
                        table.AddCell(caseType.Details);
                    }

                    table.AddEmpty();
                }

                foreach (var productArea in data.Data.GetLineProductAreas())
                {
                    var level = productArea.GetLevel();
                    table.AddRowHeader(productArea.ProductArea.Name, level);
                    for (var i = 0; i < productArea.Cases.Count; i++)
                    {
                        var cases = productArea.Cases[i];
                        table.AddCell(cases.Number > 0 ?
                                      (isShowPercents ? 
                                           string.Format("{0} ({1}%)", cases.Number, data.Data.GetTotalPercentsForCaseType(cases, data.Data.CaseTypes[i])) 
                                           : cases.Number.ToString(CultureInfo.InvariantCulture)) : string.Empty);
                    }

                    var total = productArea.GetTotal();
                    table.AddCell(total > 0 ?
                                      (isShowPercents ?
                                           string.Format("{0} ({1}%)", total, data.Data.GetTotalPercentsForProductArea(productArea))
                                           : total.ToString(CultureInfo.InvariantCulture)) : string.Empty);
                }

                table.AddFooter(Translation.Get("Totalt"));
                foreach (var caseType in data.Data.CaseTypes)
                {
                    var caseTypeTotal = data.Data.GetTotalForCaseType(caseType);
                    table.AddFooter(caseTypeTotal > 0 ?
                                    (isShowPercents ?
                                        string.Format("{0} (100%)", caseTypeTotal)
                                         : caseTypeTotal.ToString(CultureInfo.InvariantCulture)) : string.Empty);
                }

                var totalAll = data.Data.GetTotal();
                table.AddFooter(totalAll > 0 ?
                                    (isShowPercents ?
                                        string.Format("{0} (100%)", totalAll)
                                         : totalAll.ToString(CultureInfo.InvariantCulture)) : string.Empty);

                document.Add(table);                                                                                
                document.Close();
                writer.Close();
                ms.Close();
                return ms.GetBuffer();
            }
        }

        private static IElement GetHeader()
        {
            return Image.GetInstance(HttpContext.Current.Server.MapPath("~/img-profile/print-logo.png"));
        }
    }
}