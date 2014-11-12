namespace DH.Helpdesk.Services.Services.Concrete.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.Services.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public OrdersService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public OrderFieldSettingsOverview[] GetFieldSettingsOverviews(int customerId, int? orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();

                return fieldSettingsRep.GetAll()
                        .GetByTypeForList(customerId, orderTypeId)
                        .MapToListOverviews();
            }
        }
    }
}