namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    public class StorageFields
    {
        public StorageFields(ConfigurableFieldModel<string> capasity)
        {
            this.Capasity = capasity;
        }

        public ConfigurableFieldModel<string> Capasity { get; set; }
    }
}