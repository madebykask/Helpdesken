using System;

namespace dhHelpdesk_NG.Domain
{
    public class ContractFieldSettings : Entity
    {
        public int Customer_Id { get; set; }
        public int Required { get; set; }
        public int Show { get; set; }
        public int ShowExternal { get; set; }
        public int ShowInList { get; set; }
        public string FieldHelp { get; set; }
        public string ContractField { get; set; }
        public string Label { get; set; }
        public string Label_ENG { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
