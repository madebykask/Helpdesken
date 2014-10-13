namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Server
{
    using System;

    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsModel
    {
        public StateFieldsModel()
        {
        }

        public StateFieldsModel(ConfigurableFieldModel<DateTime?> syncChangeDate, UserName changedByUserName)
        {
            this.SyncChangeDate = syncChangeDate;
            this.ChangedByUserName = changedByUserName;
        }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> SyncChangeDate { get; set; }

        public UserName ChangedByUserName { get; set; }
    }
}