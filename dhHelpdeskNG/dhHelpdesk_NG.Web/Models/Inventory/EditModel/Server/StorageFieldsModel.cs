namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    public class StorageFieldsModel
    {
        public StorageFieldsModel(ConfigurableFieldModel<string> capasity)
        {
            this.Capasity = capasity;
        }

        public ConfigurableFieldModel<string> Capasity { get; set; }
    }
}