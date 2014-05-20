namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class TypeGroupModel
    {
        public TypeGroupModel(int id, int sortOrder, string name)
        {
            this.Id = id;
            this.SortOrder = sortOrder;
            this.Name = name;
        }

        [IsId]
        public int Id { get; set; }

        [MinValue(0)]
        public int SortOrder { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }
    }
}