namespace DH.Helpdesk.Web.Models.Inventory
{
    using DH.Helpdesk.Web.Models.Inventory.SearchModels;

    public class WorkstaionsViewModel
    {
        public WorkstaionsViewModel(WorkstationSearchViewModel workstationSearchViewModel, InventoryGridModel inventoryGridModel)
        {
            this.WorkstationSearchViewModel = workstationSearchViewModel;
            this.InventoryGridModel = inventoryGridModel;
        }

        public WorkstationSearchViewModel WorkstationSearchViewModel { get; set; }

        public InventoryGridModel InventoryGridModel { get; set; }
    }
}