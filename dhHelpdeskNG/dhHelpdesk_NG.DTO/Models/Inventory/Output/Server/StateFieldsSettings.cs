namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Server
{
    using System;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(DateTime createdDate, DateTime changedDate, DateTime syncChangeDate, string createdBy)
        {
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SyncChangeDate = syncChangeDate;
            this.CreatedBy = createdBy;
        }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime SyncChangeDate { get; set; }

        public string CreatedBy { get; set; }
    }
}