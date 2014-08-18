namespace DH.Helpdesk.BusinessData.Models.Operation
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperationObjectForInsert : OperationObject
    {
        public OperationObjectForInsert(int customerId, DateTime createdDate, string name, string description)
            : base(name, description)
        {
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}