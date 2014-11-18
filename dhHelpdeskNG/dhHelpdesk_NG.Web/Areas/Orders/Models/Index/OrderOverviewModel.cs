namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class OrderOverviewModel
    {
        public OrderOverviewModel(
                int id, 
                List<NewGridRowCellValueModel> fieldValues)
        {
            this.FieldValues = fieldValues;
            this.Id = id;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public List<NewGridRowCellValueModel> FieldValues { get; private set; }
    }
}