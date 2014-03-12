namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.BusinessData.Models.Common;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryType : BusinessModel
    {
        private InventoryType(string name, BusinessModelStates businessModelStates)
            : base(businessModelStates)
        {
            this.Name = name;
        }

        [IsId]
        [CanGet(BusinessModelStates.Created)]
        public int CustomerId { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [CanGet(BusinessModelStates.Updated)]
        public DateTime ChangedDate { get; private set; }

        [CanGet(BusinessModelStates.Created)]
        public DateTime CreatedDate { get; private set; }

        public static InventoryType CreateNew(int customerId, string name, DateTime createdDate)
        {
            var businessModel = new InventoryType(name, BusinessModelStates.Created) { CustomerId = customerId, CreatedDate = createdDate };

            return businessModel;
        }

        public static InventoryType CreateUpdated(string name, int id, DateTime changedDate)
        {
            var businessModel = new InventoryType(name, BusinessModelStates.Updated) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static InventoryType CreateForEdit(string name, int id, DateTime createdDate, DateTime changedDate)
        {
            var businessModel = new InventoryType(name, BusinessModelStates.ForEdit) { Id = id, CreatedDate = createdDate, ChangedDate = changedDate };

            return businessModel;
        }
    }
}
