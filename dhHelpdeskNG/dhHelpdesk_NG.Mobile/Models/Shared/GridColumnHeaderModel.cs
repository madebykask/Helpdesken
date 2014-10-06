namespace DH.Helpdesk.Mobile.Models.Shared
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;

    public sealed class GridColumnHeaderModel : ITableHeader
    {
        public GridColumnHeaderModel(string fieldName, string caption)
        {
            this.FieldName = fieldName;
            this.Caption = caption;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }
    }
}