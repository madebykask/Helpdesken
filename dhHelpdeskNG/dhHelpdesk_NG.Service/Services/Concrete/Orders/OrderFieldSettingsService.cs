namespace DH.Helpdesk.Services.Services.Concrete.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Attributes.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.Services.Orders;

    public sealed class OrderFieldSettingsService : IOrderFieldSettingsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public OrderFieldSettingsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        [CreateMissingOrderFieldSettings("customerId", "orderTypeId")]
        public OrdersFieldSettingsOverview GetOrdersFieldSettingsOverview(int customerId, int? orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();

                return fieldSettingsRep.GetAll()
                        .GetByType(customerId, orderTypeId)
                        .MapToOrdersFieldSettingsOverview();
            }
        }
    }
}