namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings
{
    public sealed class OrderFieldSettingsFilterModel
    {
        public OrderFieldSettingsFilterModel(int? orderTypeId)
        {
            this.OrderTypeId = orderTypeId;
        }

        private OrderFieldSettingsFilterModel()
        {            
        }

        public int? OrderTypeId { get; private set; }

        public static OrderFieldSettingsFilterModel CreateDefault()
        {
            return new OrderFieldSettingsFilterModel();
        }
    }
}