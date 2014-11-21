namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderFieldSettingsFilterModel
    {
        public OrderFieldSettingsFilterModel(int? orderTypeId)
        {
            this.OrderTypeId = orderTypeId;
        }

        private OrderFieldSettingsFilterModel()
        {            
        }

        [IsId]
        public int? OrderTypeId { get; private set; }

        public static OrderFieldSettingsFilterModel CreateDefault()
        {
            return new OrderFieldSettingsFilterModel();
        }
    }
}