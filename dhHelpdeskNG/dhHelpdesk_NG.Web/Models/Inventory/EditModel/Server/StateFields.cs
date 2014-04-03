namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    using System;

    public class StateFields
    {
        public StateFields(ConfigurableFieldModel<DateTime> syncChangeDate, ConfigurableFieldModel<string> createdBy)
        {
            this.SyncChangeDate = syncChangeDate;
            this.CreatedBy = createdBy;
        }

        public ConfigurableFieldModel<DateTime> SyncChangeDate { get; set; }

        public ConfigurableFieldModel<string> CreatedBy { get; set; }
    }
}