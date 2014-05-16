namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InventoryTypeWithInventories
    {
        public InventoryTypeWithInventories(int id, string name, List<ItemOverview> inventories)
        {
            this.Id = id;
            this.Name = name;
            this.Inventories = inventories;
        }

        public int Id { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [NotNull]
        public List<ItemOverview> Inventories { get; private set; }
    }
}