namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    public class StateFields
    {
        public StateFields(DateTime syncChangeDate, string createdBy)
        {
            this.SyncChangeDate = syncChangeDate;
            this.CreatedBy = createdBy;
        }

        public DateTime SyncChangeDate { get; set; }

        public string CreatedBy { get; set; }
    }
}