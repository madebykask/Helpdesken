namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.BusinessData.Models.Common;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryType : BusinessModel
    {
        private InventoryType(string name, ModelStates businessModelStates)
            : base(businessModelStates)
        {
            this.Name = name;
        }

        [IsId]
        [AllowRead(ModelStates.Created)]
        public int CustomerId { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; private set; }

        [AllowRead(ModelStates.Created)]
        public DateTime CreatedDate { get; private set; }

        public static InventoryType CreateNew(int customerId, string name, DateTime createdDate)
        {
            var businessModel = new InventoryType(name, ModelStates.Created) { CustomerId = customerId, CreatedDate = createdDate };

            return businessModel;
        }

        public static InventoryType CreateUpdated(string name, int id, DateTime changedDate)
        {
            var businessModel = new InventoryType(name, ModelStates.Updated) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static InventoryType CreateForEdit(string name, int id, DateTime createdDate, DateTime changedDate)
        {
            var businessModel = new InventoryType(name, ModelStates.ForEdit) { Id = id, CreatedDate = createdDate, ChangedDate = changedDate };

            return businessModel;
        }
    }
}
