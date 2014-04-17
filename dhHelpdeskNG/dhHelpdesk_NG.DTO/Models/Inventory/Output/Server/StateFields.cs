namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Server
{
    using System;

    public class StateFields
    {
        public StateFields(DateTime? syncChangeDate)
        {
            this.SyncChangeDate = syncChangeDate;
        }

        public DateTime? SyncChangeDate { get; set; }
    }
}