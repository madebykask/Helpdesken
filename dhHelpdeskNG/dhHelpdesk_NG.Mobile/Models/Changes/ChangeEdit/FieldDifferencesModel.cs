namespace DH.Helpdesk.Mobile.Models.Changes.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldDifferencesModel
    {
        public FieldDifferencesModel(string fieldName, string oldValue, string newValue)
        {
            this.FieldName = fieldName;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        [NotNullAndEmpty]
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}