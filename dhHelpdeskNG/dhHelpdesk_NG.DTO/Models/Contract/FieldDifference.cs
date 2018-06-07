namespace DH.Helpdesk.BusinessData.Models.Contract
{
    public class FieldDifference
    {
        public FieldDifference(string fieldName, string prevValue, string newValue)
        {
            FieldName = fieldName;
            PrevValue = prevValue;
            NewValue = newValue;
        }

        public string FieldName { get; set; }
        public string Label { get; set; }
        public string LabelTranslation { get; set; }
        public string PrevValue { get; set; }
        public string NewValue { get; set; }
    }
}