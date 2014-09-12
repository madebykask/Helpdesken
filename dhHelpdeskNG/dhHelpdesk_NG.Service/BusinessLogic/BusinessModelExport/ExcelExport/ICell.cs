namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using DH.Helpdesk.Services.DisplayValues;
        
    public interface ICell
    {
        string FieldName { get; }

        DisplayValue Value { get; }

        bool IsBold { get; set; }
    }
}