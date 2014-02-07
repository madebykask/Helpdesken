namespace DH.Helpdesk.Web.Models.Changes
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FieldDifferenceModel
    {
        public FieldDifferenceModel(string fieldName, string oldValue, string newValue)
        {
            this.FieldName = fieldName;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        public string OldValue { get; private set; }

        public string NewValue { get; private set; }
    }
}