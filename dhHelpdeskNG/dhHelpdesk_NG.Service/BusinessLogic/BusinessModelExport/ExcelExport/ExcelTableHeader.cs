namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ExcelTableHeader : ITableHeader
    {
        public ExcelTableHeader(string caption, string fieldName)
        {
            this.Caption = caption;
            this.FieldName = fieldName;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}