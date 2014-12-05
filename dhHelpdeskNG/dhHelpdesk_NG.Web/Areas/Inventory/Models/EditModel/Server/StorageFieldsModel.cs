namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
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