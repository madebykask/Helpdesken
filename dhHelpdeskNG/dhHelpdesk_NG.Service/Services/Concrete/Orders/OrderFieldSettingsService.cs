namespace DH.Helpdesk.Services.Services.Concrete.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Attributes.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.Services.Orders;

    using OrderFieldSettings = DH.Helpdesk.Domain.OrderFieldSettings;

    public sealed class OrderFieldSettingsService : IOrderFieldSettingsService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public OrderFieldSettingsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        [CreateMissingOrderFieldSettings("customerId", "orderTypeId")]
        public FullFieldSettingsOverview GetOrdersFieldSettingsOverview(int customerId, int? orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();

                return fieldSettingsRep.GetAll()
                        .GetByType(customerId, orderTypeId)
                        .MapToOrdersFieldSettingsOverview();
            }
        }

        public OrderFieldSettingsFilterData GetFilterData(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var orderTypeRep = uow.GetRepository<OrderType>();

                var orderTypes = orderTypeRep.GetAll()
                                    .GetOrderTypes(customerId);

                return OrderFieldSettingsMapper.MapToFilterData(orderTypes);
            }
        }

        [CreateMissingOrderFieldSettings("customerId", "orderTypeId")]
        public GetSettingsResponse GetOrderFieldSettings(int customerId, int? orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();

                return fieldSettingsRep.GetAll()
                        .GetByType(customerId, orderTypeId)
                        .MapToFullFieldSettings();
            }
        }

        public void UpdateSettings(FullFieldSettings settings)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();

                fieldSettingsRep.GetAll()
                        .GetByType(settings.CustomerId, settings.OrderTypeId)
                        .MapToEntitiesForUpdate(settings);

                uow.Save();
            }
        }

        [CreateMissingOrderFieldSettings("customerId", "orderTypeId")]
        public FullOrderEditSettings GetOrderEditSettings(int customerId, int? orderTypeId, IUnitOfWork uow)
        {
            var fieldSettingsRep = uow.GetRepository<OrderFieldSettings>();

            return fieldSettingsRep.GetAll()
                        .GetByType(customerId, orderTypeId)
                        .MapToFullOrderEditSettings();
        }
    }
}