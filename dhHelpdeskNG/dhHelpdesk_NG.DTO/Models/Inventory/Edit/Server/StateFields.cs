namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    using DH.Helpdesk.Common.Types;

    public class StateFields
    {
        public StateFields(DateTime? syncChangeDate, UserName createdBy)
        {
            this.SyncChangeDate = syncChangeDate;
            this.CreatedBy = createdBy;
        }

        public DateTime? SyncChangeDate { get; set; }

        public UserName CreatedBy { get; set; }
    }
}