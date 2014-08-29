namespace DH.Helpdesk.Web.Models.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.DisplayValues;

    public sealed class NewGridRowCellValueModel : ICell
    {
        public NewGridRowCellValueModel(string fieldName, DisplayValue value)
        {
            this.FieldName = fieldName;
            this.Value = value;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        public DisplayValue Value { get; private set; }
    }
}