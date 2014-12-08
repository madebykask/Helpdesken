namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public class OrderModelFactory : IOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public OrderModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }

        public FullOrderEditModel Create(FindOrderResponse response, int customerId)
        {
            throw new System.NotImplementedException();
        }
    }
}