namespace DH.Helpdesk.Web.Areas.Orders.Models.Index
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class OrderOverviewModel
    {
        public OrderOverviewModel(
                int id,
                string orderType,
                List<NewGridRowCellValueModel> fieldValues)
        {
            this.FieldValues = fieldValues;
            this.Id = id;
            OrderType = orderType;
        }

        [IsId]
        public int Id { get; private set; }

        public string OrderType { get; private set; }

        [NotNull]
        public List<NewGridRowCellValueModel> FieldValues { get; private set; }
    }
}