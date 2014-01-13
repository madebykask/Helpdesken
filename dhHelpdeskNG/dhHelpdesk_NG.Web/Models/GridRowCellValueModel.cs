namespace dhHelpdesk_NG.Web.Models
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class GridRowCellValueModel
    {
        public GridRowCellValueModel(string fieldName, string value)
        {
            this.FieldName = fieldName;
            this.Value = value;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        public string Value { get; private set; }
    }
}