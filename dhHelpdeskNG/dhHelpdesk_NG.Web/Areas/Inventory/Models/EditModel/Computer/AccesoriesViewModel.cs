namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccesoriesViewModel : BaseViewEditWorkstationModel
    {
        private AccesoriesViewModel(
            int computerId,
            int? inventoryTypeId,
            SelectList inventoryTypes,
            SelectList inventories,
            List<InventoryGridModel> inventoryGridModels)
            : base(computerId)
        {
            this.InventoryTypeId = inventoryTypeId;
            this.InventoryTypes = inventoryTypes;
            this.Inventories = inventories;
            this.InventoryGridModels = inventoryGridModels;
        }

        [IsId]
        public int? InventoryTypeId { get; private set; }

        [IsId]
        public int? InventoryId { get; set; }

        [NotNull]
        public SelectList InventoryTypes { get; private set; }

        [NotNull]
        public SelectList Inventories { get; private set; }

        [NotNull]
        public List<InventoryGridModel> InventoryGridModels { get; private set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public override WorkstationEditTabs Tab
        {
            get
            {
                return WorkstationEditTabs.Accessories;
            }
        }

        public static AccesoriesViewModel BuildViewModel(
            int computerId,
            int? inventoryId,
            List<ItemOverview> inventoryTypes,
            List<ItemOverview> inventories,
            List<InventoryGridModel> inventoryGridModels)
        {
            var typeSelectList = new SelectList(inventoryTypes, "Value", "Name");
            var inventorySelectList = new SelectList(inventories, "Value", "Name");

            return new AccesoriesViewModel(
                computerId,
                inventoryId,
                typeSelectList,
                inventorySelectList,
                inventoryGridModels);
        }
    }
}