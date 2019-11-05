//namespace DH.Helpdesk.Services.Services
//{
//    using DH.Helpdesk.Dal.Repositories;
//    using DH.Helpdesk.Domain;
//    using System.Collections.Generic;
//    using System.Linq;

//    public interface IOrderService
//    {
//        Order GetOrderByCase(int caseid);
//        Order GetOrder(int orderId);
//        IList<OrderFieldSettings> GetOrderFieldSettingsForMailTemplate(int customerId, int? orderTypeId);
//    }

//    public class OrderService : IOrderService
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly IOrderFieldSettingsRepository _orderFieldSettingsRepository;

//        public OrderService(
//            IOrderRepository orderRepository,
//            IOrderFieldSettingsRepository orderFieldSettingsRepository)
//        {
//            this._orderRepository = orderRepository;
//            this._orderFieldSettingsRepository = orderFieldSettingsRepository;
//        }

//        public Order GetOrderByCase(int caseid)
//        {
//            return this._orderRepository.GetOrder(caseid);
//        }

//        public Order GetOrder(int orderId)
//        {
//            return this._orderRepository.GetById(orderId);
//        }

//        public IList<OrderFieldSettings> GetOrderFieldSettingsForMailTemplate(int customerId, int? orderTypeId)
//        {
//            return this._orderFieldSettingsRepository.GetOrderFieldSettingsForMailTemplate(customerId, orderTypeId).ToList();
//        }
//    }
//}
