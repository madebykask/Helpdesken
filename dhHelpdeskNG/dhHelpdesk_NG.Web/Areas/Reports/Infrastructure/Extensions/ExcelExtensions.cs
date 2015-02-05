namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Extensions
{
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
    }
}