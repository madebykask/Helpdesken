namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer
{
    using System;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(DateTime createdDate, DateTime changedDate)
        {
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
        }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}