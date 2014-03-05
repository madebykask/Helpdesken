namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Server
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