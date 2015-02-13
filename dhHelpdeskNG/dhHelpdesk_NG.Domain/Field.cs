
namespace DH.Helpdesk.Domain
{
    using global::System;
    using DH.Helpdesk.Common.Enums.Cases;

    public class Field : Entity
    {
        public string Key { get; set; }
        public string StringValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public FieldTypes FieldType { get; set; }
        public bool TranslateThis { get; set; }
    }
}
