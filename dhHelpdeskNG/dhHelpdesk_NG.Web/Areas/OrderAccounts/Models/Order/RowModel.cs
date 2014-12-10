namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class RowModel : IRow<NewGridRowCellValueModel>
    {
        public RowModel(int id, IEnumerable<NewGridRowCellValueModel> fields)
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