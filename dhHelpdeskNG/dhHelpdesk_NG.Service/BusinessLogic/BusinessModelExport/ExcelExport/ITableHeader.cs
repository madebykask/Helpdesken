namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    public interface ITableHeader
    {
        string FieldName { get; }

        string Caption { get; }
    }
}