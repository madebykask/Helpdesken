namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class InventoryTypeModel
    {
        public InventoryTypeModel()
        {
        }

        public InventoryTypeModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; set; }

        [NotNull]
        [LocalizedRequired]
        [LocalizedDisplay("Inventory Type")]
        public string Name { get; set; }

        public static InventoryTypeModel CreateDefault()
        {
            return new InventoryTypeModel();
        }
    }
}