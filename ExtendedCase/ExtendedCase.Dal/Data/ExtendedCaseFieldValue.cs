namespace ExtendedCase.Dal.Data
{
    public class ExtendedCaseFieldValue
    {
        public int Id { get; set; }
        public int ExtendedCaseDataId { get; set; }
        public string FieldId { get; set; }
        public string Value { get; set; }
        public string SecondaryValue { get; set; }
        public string Properties { get; set; }
    }
}