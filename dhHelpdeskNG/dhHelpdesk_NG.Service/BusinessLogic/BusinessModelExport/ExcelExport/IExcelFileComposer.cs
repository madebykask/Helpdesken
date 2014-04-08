namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using System.Collections.Generic;

    public interface IExcelFileComposer
    {
        byte[] Compose(List<ExcelTableHeader> headers, List<BusinessItem> items, string worksheetName);
    }
}