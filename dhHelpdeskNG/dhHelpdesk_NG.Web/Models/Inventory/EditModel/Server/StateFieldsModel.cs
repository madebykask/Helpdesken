namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    using System;

    using DH.Helpdesk.Common.Types;

    public class StateFieldsModel
    {
        public StateFieldsModel(ConfigurableFieldModel<DateTime?> syncChangeDate, UserName createdBy)
        {
            this.SyncChangeDate = syncChangeDate;
            this.CreatedBy = createdBy;
        }

        public ConfigurableFieldModel<DateTime?> SyncChangeDate { get; set; }

        public UserName CreatedBy { get; set; }
    }
}