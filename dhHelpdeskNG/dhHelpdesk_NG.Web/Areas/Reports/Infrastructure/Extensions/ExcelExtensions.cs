namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Extensions
{
    using System;

    using OfficeOpenXml;

    public static class ExcelExtensions
    {
        public static void SetHeader(this ExcelWorksheet worksheet, int row, int column, object value)
        {
            worksheet.SetValue(row, column, value);
            worksheet.SetBold(row, column);
        }       

        public static void SetRowHeader(this ExcelWorksheet worksheet, int row, int column, object value, int? level = null)
        {
            var offset = string.Empty;
            if (level.HasValue)
            {
                for (var i = 0; i < level; i++)
                {
                    offset += "  ";
                }
            }

            worksheet.SetValue(row, column, string.Format("{0}{1}", offset, value));
            worksheet.SetBold(row, column);
        }       

        public static void SetFooter(this ExcelWorksheet worksheet, int row, int column, object value)
        {
            worksheet.SetValue(row, column, value);
            worksheet.SetBold(row, column);
        }       
        
        public static void SetEmpty(this ExcelWorksheet worksheet, int row, int column)
        {
            worksheet.SetValue(row, column, string.Empty);
        }   
    
        public static void SetBold(this ExcelWorksheet worksheet, int row, int column)
        {
            worksheet.Cells[row, column].Style.Font.Bold = true;
        }

        public static void AutoFit(this ExcelWorksheet worksheet, int columns)
        {
            for (var i = 1; i <= columns; i++)
            {
                try
                {
                    worksheet.Column(i).AutoFit();
                }
                catch(Exception)
                {
                   //continue
                }

            }
        }

        public static ExcelWorksheet AddReportWorksheet(this ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Report");
            return ws;
        }

        public static string PrepareForExcel(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Replace("<br/>", Environment.NewLine).Replace("&nbsp;", " ");  
        }
    }
}