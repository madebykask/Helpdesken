namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    using System;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsModel
    {
        public StateFieldsModel()
        {
        }

        public StateFieldsModel(ConfigurableFieldModel<DateTime?> syncChangeDate, UserName createdBy)
        {
            this.SyncChangeDate = syncChangeDate;
            this.CreatedBy = createdBy;
        }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> SyncChangeDate { get; set; }

        public UserName CreatedBy { get; set; }
    }
}