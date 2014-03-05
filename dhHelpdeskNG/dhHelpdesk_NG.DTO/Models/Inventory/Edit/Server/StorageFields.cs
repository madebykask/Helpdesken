namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    public class StorageFields
    {
        public StorageFields(string capasity)
        {
            this.Capasity = capasity;
        }

        public string Capasity { get; set; }
    }
}