using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.SelfService.Models.Orders.FieldModels
{
    public sealed class FieldDifferencesModel
    {
        public FieldDifferencesModel(string fieldName, string oldValue, string newValue)
        {
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        [NotNullAndEmpty]
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}