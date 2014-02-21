namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Server
{
    public class StorageFieldsSettings
    {
        public StorageFieldsSettings(string capasity)
        {
            this.Capasity = capasity;
        }

        public string Capasity { get; set; }
    }
}