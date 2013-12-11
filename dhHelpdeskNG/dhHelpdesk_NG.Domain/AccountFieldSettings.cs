using System;

namespace dhHelpdesk_NG.Domain
{
    public class AccountFieldSettings : Entity
    {
        public int AccountActivity_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Required { get; set; }
        public int Show { get; set; }
        public int ShowExternal { get; set; }
        public int ShowInList { get; set; }
        public string AccountField { get; set; }
        public string EMailIdentifier { get; set; }
        public string FieldHelp { get; set; }
        public string Label { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual AccountActivity AccountActivity { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
