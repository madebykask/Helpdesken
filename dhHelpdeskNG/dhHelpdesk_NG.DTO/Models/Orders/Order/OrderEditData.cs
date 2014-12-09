namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;
    
    public sealed class OrderEditData
    {
        public OrderEditData(
                FullOrderEditFields order, 
                List<HistoriesDifference> histories)
        {
            this.Histories = histories;
            this.Order = order;
        }

        [NotNull]
        public FullOrderEditFields Order { get; private set; }

        [NotNull]
        public List<HistoriesDifference> Histories { get; private set; }
    }
}