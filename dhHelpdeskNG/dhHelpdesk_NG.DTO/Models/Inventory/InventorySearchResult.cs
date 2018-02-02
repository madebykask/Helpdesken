namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    public class InventorySearchResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TypeDescription { get; set; }

        public string Location { get; set; }

        public string TypeName { get; set; }

        public bool NeedTranslate { get; set; }
    }
}
