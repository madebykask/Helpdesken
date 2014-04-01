namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer
{
    using System;

    public class StateFields
    {
        public StateFields(DateTime createdDate, DateTime changedDate)
        {
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
        }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}