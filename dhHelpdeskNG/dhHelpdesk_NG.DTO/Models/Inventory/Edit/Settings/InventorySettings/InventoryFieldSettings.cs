namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;

    public class InventoryFieldSettings : BusinessModel
    {
        private InventoryFieldSettings(ModelStates state, DefaultFieldSettings defaultSettings)
            : base(state)
        {
            this.DefaultSettings = defaultSettings;
        }

        [AllowRead(ModelStates.Updated)]
        public int InventoryTypeId { get; private set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; private set; }

        public DefaultFieldSettings DefaultSettings { get; private set; }

        public static InventoryFieldSettings CreateForEdit(DefaultFieldSettings defaultSettings)
        {
            var businessModel = new InventoryFieldSettings(ModelStates.ForEdit, defaultSettings);

            return businessModel;
        }

        public static InventoryFieldSettings CreateForUpdate(
            int inventoryTypeId,
            DefaultFieldSettings defaultSettings,
            DateTime changedDate)
        {
            var businessModel = new InventoryFieldSettings(ModelStates.Updated, defaultSettings)
                                    {
                                        InventoryTypeId =
                                            inventoryTypeId,
                                        ChangedDate =
                                            changedDate
                                    };

            return businessModel;
        }
    }
}