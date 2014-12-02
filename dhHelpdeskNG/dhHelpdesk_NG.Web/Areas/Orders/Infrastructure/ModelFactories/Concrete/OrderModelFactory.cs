namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    public class OrderModelFactory : IOrderModelFactory
    {
        private readonly IConfigurableFieldModelFactory configurableFieldModelFactory;

        public OrderModelFactory(IConfigurableFieldModelFactory configurableFieldModelFactory)
        {
            this.configurableFieldModelFactory = configurableFieldModelFactory;
        }
    }
}