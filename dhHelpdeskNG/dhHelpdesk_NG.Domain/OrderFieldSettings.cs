namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Common.Collections;

    using global::System;

    using DH.Helpdesk.Domain.Interfaces;

    public class OrderFieldSettings : Entity, ICustomerEntity, INamedObject
    {
        public int Customer_Id { get; set; }

        public int? OrderType_Id { get; set; }

        public int Required { get; set; }

        public int Show { get; set; }

        public int ShowExternal { get; set; }

        public int ShowInList { get; set; }

        public string DefaultValue { get; set; }

        public string EMailIdentifier { get; set; }

        public string Label { get; set; }

        public string OrderField { get; set; }

        public string FieldHelp { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual OrderType OrderType { get; set; }

        public string GetName()
        {
            return this.OrderField;
        }
    }
}
