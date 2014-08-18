namespace DH.Helpdesk.BusinessData.Models.Operation
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class OperationObjectForView : OperationObject
    {
        public OperationObjectForView(
            string name,
            string description,
            int id,
            DateTime changedDate,
            DateTime createdDate)
            : base(name, description)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int Id { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}