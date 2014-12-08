namespace DH.Helpdesk.Domain.Accounts
{
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;

    public class AccountFieldSettings : Entity, ICustomerEntity, INamedObject
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
        public int MultiValue { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual AccountActivity AccountActivity { get; set; }
        public virtual Customer Customer { get; set; }

        public string GetName()
        {
            return this.AccountField;
        }
    }
}
