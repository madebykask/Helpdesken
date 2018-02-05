namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryEditViewModel : BaseEditCustomInventoryModel
    {
        public InventoryEditViewModel(int id,
            InventoryViewModel inventoryViewModel,
            List<DynamicFieldModel> dynamicFieldModels,
            List<TypeGroupModel> typeGroupModels) : base(id)
        {
            this.InventoryViewModel = inventoryViewModel;
            this.DynamicFieldModels = dynamicFieldModels;
            this.TypeGroupModels = typeGroupModels;
        }

        [NotNull]
        public InventoryViewModel InventoryViewModel { get; set; }

        [NotNull]
        public List<DynamicFieldModel> DynamicFieldModels { get; set; }

        [NotNull]
        public List<TypeGroupModel> TypeGroupModels { get; set; }

//        public bool UserHasInventoryAdminPermission { get; set; }
//
//        public bool IsForDialog { get; set; }

        public override CustomInventoryTabs Tab { get { return CustomInventoryTabs.CustomInventory; } }
    }
}