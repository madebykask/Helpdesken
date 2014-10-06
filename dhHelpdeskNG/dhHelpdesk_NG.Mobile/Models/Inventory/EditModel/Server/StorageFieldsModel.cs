namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageFieldsModel
    {
        public StorageFieldsModel()
        {
        }

        public StorageFieldsModel(ConfigurableFieldModel<string> capasity)
        {
            this.Capasity = capasity;
        }

        [NotNull]
        public ConfigurableFieldModel<string> Capasity { get; set; }
    }
}