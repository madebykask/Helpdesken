namespace DH.Helpdesk.Mobile.Models.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Mobile.Models.Shared;

    public sealed class InventoryOverviewModel : IRow<NewGridRowCellValueModel>
    {
        public InventoryOverviewModel(int id, IEnumerable<NewGridRowCellValueModel> fields)
        {
            this.Id = id;
            this.Fields = fields;
        }

        [IsId]
        public int Id { get; set; }

        [NotNull]
        public IEnumerable<NewGridRowCellValueModel> Fields { get; set; }
    }
}