namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using OfficeOpenXml;

    public sealed class ExcelFileComposer : IExcelFileComposer
    {
        #region Public Methods and Operators

        public byte[] Compose(IEnumerable<ITableHeader> headers, IEnumerable<IRow<ICell>> items, string worksheetName)
        {
            List<ITableHeader> tempHeaders = headers.ToList();
            List<IRow<ICell>> tempItems = items.ToList();

            using (var excelPackage = CreateExcelPackage(tempHeaders, tempItems, worksheetName))
            {
                AutoFitColumnsWidth(excelPackage, worksheetName, tempHeaders.Count);
                var bytes = excelPackage.GetAsByteArray();
                return bytes;
            }
        }

        #endregion

        #region Methods

        private static void AutoFitColumnsWidth(ExcelPackage excelPackage, string worksheetName, int columnCount)
        {
            var worksheet = excelPackage.Workbook.Worksheets[worksheetName];

            for (var i = 1; i < columnCount + 1; i++)
            {
                worksheet.Column(i).AutoFit();
            }
        }

        private static ExcelPackage CreateExcelPackage(List<ITableHeader> headers, List<IRow<ICell>> items, string worksheetName)
        {
            var memoryStream = new MemoryStream();
            var excelPackage = new ExcelPackage(memoryStream);
            
            var worksheet = excelPackage.Workbook.Worksheets.Add(worksheetName);

            for (var i = 0; i < headers.Count; i++)
            {
                worksheet.SetValue(1, i + 1, headers[i].Caption);
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }

            int rowIndex = 1;

            foreach (IRow<ICell> item in items)
            {
                int columnIndex = 0;

                foreach (ITableHeader header in headers)
                {
                    ICell field = item.Fields.SingleOrDefault(f => f.FieldName == header.FieldName);
                    if (field != null)
                    {
                        worksheet.SetValue(rowIndex + 1, columnIndex + 1, field.Value.GetDisplayValue());
                        SetStyle(field, worksheet, rowIndex, columnIndex);
                    }

                    columnIndex++;
                }

                rowIndex++;
            }

            return excelPackage;
        }

        private static void SetStyle(ICell field, ExcelWorksheet worksheet, int rowIndex, int columnIndex)
        {
            worksheet.Cells[rowIndex + 1, columnIndex + 1].Style.Font.Bold = field.IsBold;
        }

        #endregion
    }
}   