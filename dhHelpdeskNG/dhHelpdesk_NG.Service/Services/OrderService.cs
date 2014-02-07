namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IOrderService
    {
        Order GetOrder(int caseid);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(
            IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public Order GetOrder(int caseid)
        {
            return this._orderRepository.GetOrder(caseid);
        }
    }
}
