namespace DH.Helpdesk.Services.Services.Concrete.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Common;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.Services.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public OrdersService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public OrdersFilterData GetOrdersFilterData(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var orderTypeRep = uow.GetRepository<OrderType>();
                var administratorRep = uow.GetRepository<User>();
                var statusRep = uow.GetRepository<OrderState>();

                var orderTypes = orderTypeRep.GetAll()
                                    .GetOrderTypes(customerId);

                var administrators = administratorRep.GetAll()
                                    .GetAdministrators(customerId);

                var statuses = statusRep.GetAll()
                                    .GetOrderStatuses(customerId);

                return OrderMapper.MapToFilterData(orderTypes, administrators, statuses);
            }
        }
    }
}