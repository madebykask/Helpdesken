namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class BusinessItemField
    {
        public BusinessItemField(string fieldName, string value)
        {
            this.FieldName = fieldName;
            this.Value = value;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        public string Value { get; private set; }
    }
}