namespace DH.Helpdesk.BusinessData.Models.Operation
{
    using System;

    public class OperationObjectForUpdate : OperationObject
    {
        public OperationObjectForUpdate(DateTime changeDate, string name, string description)
            : base(name, description)
        {
            this.ChangeDate = changeDate;
        }

        public DateTime ChangeDate { get; private set; }
    }
}