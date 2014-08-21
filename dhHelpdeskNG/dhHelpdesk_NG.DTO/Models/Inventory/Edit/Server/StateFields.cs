namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    using DH.Helpdesk.Common.Types;

    public class StateFields
    {
        public StateFields(DateTime? syncChangeDate)
        {
            this.SyncChangeDate = syncChangeDate;
        }

        public StateFields(DateTime? syncChangeDate, UserName changedByUserName)
        {
            this.SyncChangeDate = syncChangeDate;
            this.ChangedByUserName = changedByUserName;
        }

        public DateTime? SyncChangeDate { get; set; }

        public UserName ChangedByUserName { get; set; }

        public static StateFields CreateDefault()
        {
            return new StateFields(null);
        }
    }
}