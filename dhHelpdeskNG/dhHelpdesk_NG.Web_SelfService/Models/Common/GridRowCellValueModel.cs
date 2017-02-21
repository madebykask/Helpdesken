namespace DH.Helpdesk.SelfService.Models.Common
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.DisplayValues;

    public sealed class GridRowCellValueModel
    {
        public GridRowCellValueModel(string fieldName, string value)
        {
            FieldName = fieldName;
            Value = value;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        public string Value { get; private set; }
    }

    public sealed class NewGridRowCellValueModel
    {
        public NewGridRowCellValueModel(string fieldName, DisplayValue value)
        {
            FieldName = fieldName;
            Value = value;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        public DisplayValue Value { get; private set; }
    }
}