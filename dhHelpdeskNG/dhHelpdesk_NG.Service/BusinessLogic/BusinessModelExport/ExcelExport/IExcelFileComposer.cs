namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using System.Collections.Generic;

    public interface IExcelFileComposer
    {
        byte[] Compose(IEnumerable<ITableHeader> headers, IEnumerable<IRow<ICell>> items, string worksheetName);
    }
}