namespace DH.Helpdesk.BusinessData.Models.Operation
{
    using System;

    public class OperationObjectForUpdate : OperationObject
    {
        public OperationObjectForUpdate(int id, DateTime changeDate, string name, string description)
            : base(name, description)
        {
            this.Id = id;
            this.ChangeDate = changeDate;
        }

        public DateTime ChangeDate { get; private set; }
    }
}