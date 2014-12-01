namespace DH.Helpdesk.Services.Services.Concrete.Orders
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Common;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.Services.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IOrderFieldSettingsService orderFieldSettingsService;

        public OrdersService(
                IUnitOfWorkFactory unitOfWorkFactory, 
                IOrderFieldSettingsService orderFieldSettingsService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.orderFieldSettingsService = orderFieldSettingsService;
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

        public SearchResponse Search(SearchParameters parameters)
        {
            var settings = this.orderFieldSettingsService.GetOrdersFieldSettingsOverview(parameters.CustomerId, parameters.OrderTypeId);
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var orderRep = uow.GetRepository<Order>();

                var overviews = orderRep.GetAll().Search(
                                    parameters.CustomerId,
                                    parameters.OrderTypeId,
                                    parameters.AdministratorIds,
                                    parameters.StartDate,
                                    parameters.EndDate,
                                    parameters.StatusIds,
                                    parameters.Phrase,
                                    parameters.SortField,
                                    parameters.SelectCount)
                                    .MapToFullOverviews();

                var searchResult = new SearchResult(overviews.Count(), overviews);
                return new SearchResponse(settings, searchResult);                
            }
        }

        public NewOrderEditData GetNewOrderEditData(int customerId, int? orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var settings = this.orderFieldSettingsService.GetOrderEditSettings(customerId, orderTypeId, uow);

                var statusesRep = uow.GetRepository<OrderState>();
                var administratorsRep = uow.GetRepository<User>();
                var domainsRep = uow.GetRepository<Domain>();
                var departmentsRep = uow.GetRepository<Department>();
                var ousRep = uow.GetRepository<OU>();
                var propertiesRep = uow.GetRepository<OrderPropertyEntity>();

                var statuses = statusesRep.GetAll().GetByCustomer(customerId);
                var administrators = administratorsRep.GetAll().GetByCustomer(customerId);
                var domains = domainsRep.GetAll().GetByCustomer(customerId);
                var departments = departmentsRep.GetAll().GetByCustomer(customerId);
                var units = ousRep.GetAll();
                var properties = propertiesRep.GetAll().GetByOrderType(orderTypeId);
                var deliveryDepartments = departmentsRep.GetAll().GetByCustomer(customerId);
                var deliveryOuIds = ousRep.GetAll();

                var options = OrderEditMapper.MapToOrderEditOptions(
                                        statuses,
                                        administrators,
                                        domains,
                                        departments,
                                        units,
                                        properties,
                                        deliveryDepartments,
                                        deliveryOuIds,
                                        settings);

                return new NewOrderEditData(settings, options);
            }
        }
    }
}