namespace DH.Helpdesk.Web.Models.Inventory
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class InventoryOverviewModel
    {
        public InventoryOverviewModel(int id, List<GridRowCellValueModel> fieldValues)
        {
            this.Id = id;
            this.FieldValues = fieldValues;
        }

        [IsId]
        public int Id { get; set; }

        [NotNull]
        public List<GridRowCellValueModel> FieldValues { get; set; }
    }
}