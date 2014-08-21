namespace DH.Helpdesk.BusinessData.Models.Operation
{
    using System;

    public class OperationObjectForRead : OperationObject
    {
        public OperationObjectForRead(
            int id,
            string name,
            string description,
            DateTime changedDate,
            DateTime createdDate)
            : base(name, description)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}