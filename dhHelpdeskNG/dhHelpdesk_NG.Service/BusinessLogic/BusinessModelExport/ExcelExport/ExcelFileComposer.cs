namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using OfficeOpenXml;

    public sealed class ExcelFileComposer : IExcelFileComposer
    {
        #region Public Methods and Operators

        public byte[] Compose(List<ExcelTableHeader> headers, List<BusinessItem> items, string worksheetName)
        {
            var outputMatrix = CreateMatrix(headers, items);
            var excelPackage = FlushMatrixToExcelPackage(outputMatrix, worksheetName);

            AutoFitColumnsWidth(excelPackage, worksheetName, headers.Count);

            return excelPackage.GetAsByteArray();
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

        private static string[,] CreateMatrix(List<ExcelTableHeader> headers, List<BusinessItem> items)
        {
            var outputMatrix = new string[items.Count + 1, headers.Count];

            for (var i = 0; i < headers.Count; i++)
            {
                outputMatrix[0, i] = headers[i].Caption;
            }

            var rowIndex = 1;

            foreach (var item in items)
            {
                var columnIndex = 0;

                foreach (var header in headers)
                {
                    var field = item.Fields.SingleOrDefault(f => f.FieldName == header.FieldName);
                    if (field != null)
                    {
                        outputMatrix[rowIndex, columnIndex] = field.Value.GetDisplayValue();
                    }

                    columnIndex++;
                }

                rowIndex++;
            }

            return outputMatrix;
        }

        private static ExcelPackage FlushMatrixToExcelPackage(string[,] outputMatrix, string worksheetName)
        {
            var memoryStream = new MemoryStream();
            var excelPackage = new ExcelPackage(memoryStream);
            var worksheet = excelPackage.Workbook.Worksheets.Add(worksheetName);

            for (var i = 0; i < outputMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < outputMatrix.GetLength(1); j++)
                {
                    worksheet.SetValue(i + 1, j + 1, outputMatrix[i, j]);
                }
            }

            return excelPackage;
        }

        #endregion
    }
}