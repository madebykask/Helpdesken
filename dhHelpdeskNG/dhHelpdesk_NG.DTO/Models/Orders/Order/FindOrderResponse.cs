namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FindOrderResponse
    {
        public FindOrderResponse(
                OrderEditData editData,
                FullOrderEditSettings editSettings, 
                OrderEditOptions editOptions)
        {
            this.EditData = editData;
            this.EditOptions = editOptions;
            this.EditSettings = editSettings;
        }

        [NotNull]
        public OrderEditData EditData { get; private set; }

        [NotNull]
        public FullOrderEditSettings EditSettings { get; private set; }

        [NotNull]
        public OrderEditOptions EditOptions { get; private set; }
    }
}