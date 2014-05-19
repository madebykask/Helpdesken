namespace DH.Helpdesk.NewSelfService.Models.Common
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GridColumnHeaderModel
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