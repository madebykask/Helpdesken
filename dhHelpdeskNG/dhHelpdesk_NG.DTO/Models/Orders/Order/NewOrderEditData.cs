namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewOrderEditData
    {
        public NewOrderEditData(
                FullOrderEditSettings editSettings, 
                OrderEditOptions editOptions)
        {
            this.EditOptions = editOptions;
            this.EditSettings = editSettings;
        }

        [NotNull]
        public FullOrderEditSettings EditSettings { get; private set; }

        [NotNull]
        public OrderEditOptions EditOptions { get; private set; }
    }
}