namespace DH.Helpdesk.Services.Services.Concrete.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Common;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;
    using DH.Helpdesk.Services.Services.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IOrderFieldSettingsService orderFieldSettingsService;

        private readonly IUserRepository userRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IEmailGroupEmailRepository emailGroupEmailRepository;

        private readonly IEmailGroupRepository emailGroupRepository;

        public OrdersService(
                IUnitOfWorkFactory unitOfWorkFactory, 
                IOrderFieldSettingsService orderFieldSettingsService, 
                IWorkingGroupRepository workingGroupRepository, 
                IUserRepository userRepository, 
                IUserWorkingGroupRepository userWorkingGroupRepository, 
                IEmailGroupEmailRepository emailGroupEmailRepository, 
                IEmailGroupRepository emailGroupRepository)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.orderFieldSettingsService = orderFieldSettingsService;
            this.workingGroupRepository = workingGroupRepository;
            this.userRepository = userRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.emailGroupEmailRepository = emailGroupEmailRepository;
            this.emailGroupRepository = emailGroupRepository;
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

        public NewOrderEditData GetNewOrderEditData(int customerId, int orderTypeId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var settings = this.orderFieldSettingsService.GetOrderEditSettings(customerId, orderTypeId, uow);
                var options = this.GetEditOptions(customerId, orderTypeId, settings, uow);

                return new NewOrderEditData(settings, options);
            }
        }

        public FindOrderResponse FindOrder(int orderId, int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var ordersRep = uow.GetRepository<Order>();
                var order = ordersRep.GetAll()
                                .GetById(orderId)
                                .MapToFullOrderEditFields();

                var history = new List<HistoriesDifference>();
                var data = new OrderEditData(order, history);

                var settings = this.orderFieldSettingsService.GetOrderEditSettings(customerId, order.OrderTypeId, uow);
                var options = this.GetEditOptions(customerId, order.OrderTypeId, settings, uow);

                return new FindOrderResponse(data, settings, options);
            }
        }

        public int AddOrUpdate(UpdateOrderRequest request)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var ordersRep = uow.GetRepository<Order>();

                Order entity;
                if (request.Order.IsNew())
                {
                    entity = new Order();
                    OrderUpdateMapper.MapToEntity(entity, request.Order, request.CustomerId);
                    entity.CreatedDate = request.DateAndTime;
                    entity.ChangedDate = request.DateAndTime;
                    ordersRep.Add(entity);
                }
                else
                {
                    entity = ordersRep.GetById(request.Order.Id);
                    OrderUpdateMapper.MapToEntity(entity, request.Order, request.CustomerId);
                    entity.ChangedDate = request.DateAndTime;
                    ordersRep.Update(entity);
                }

                uow.Save();
                return entity.Id;
            }
        }

        private OrderEditOptions GetEditOptions(
                                int customerId, 
                                int? orderTypeId,
                                FullOrderEditSettings settings,
                                IUnitOfWork uow)
        {
            var statusesRep = uow.GetRepository<OrderState>();
            var administratorsRep = uow.GetRepository<User>();
            var domainsRep = uow.GetRepository<Domain>();
            var departmentsRep = uow.GetRepository<Department>();
            var ousRep = uow.GetRepository<OU>();
            var propertiesRep = uow.GetRepository<OrderPropertyEntity>();
            var orderTypeRep = uow.GetRepository<OrderType>();

            var statuses = statusesRep.GetAll().GetByCustomer(customerId);
            var administrators = administratorsRep.GetAll().GetByCustomer(customerId);
            var domains = domainsRep.GetAll().GetByCustomer(customerId);
            var departments = departmentsRep.GetAll().GetByCustomer(customerId);
            var units = ousRep.GetAll();
            var properties = propertiesRep.GetAll().GetByOrderType(orderTypeId);
            var deliveryDepartments = departmentsRep.GetAll().GetByCustomer(customerId);
            var deliveryOuIds = ousRep.GetAll();
            var administratorsWithEmails = administratorsRep.GetAll().GetAdministratorsWithEmails(customerId);
            var orderType = orderTypeId.HasValue ? orderTypeRep.GetAll().GetById(orderTypeId.Value).MapToName() : null;

            var workingGroupsWithEmails = new List<GroupWithEmails>();
            var emailGroupsWithEmails = new List<GroupWithEmails>();
            if (settings.Log.Log.Show)
            {
                var workingGroupOverviews = this.workingGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var workingGroupIds = workingGroupOverviews.Select(g => g.Id).ToList();
                var workingGroupsUserIds = this.userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
                var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
                var userIdsWithEmails = this.userRepository.FindUsersEmails(userIds);

                workingGroupsWithEmails = new List<GroupWithEmails>(workingGroupOverviews.Count);

                foreach (var workingGroupOverview in workingGroupOverviews)
                {
                    var groupUserIdsWithEmails =
                        workingGroupsUserIds.Single(g => g.WorkingGroupId == workingGroupOverview.Id);

                    var groupEmails =
                        userIdsWithEmails.Where(e => groupUserIdsWithEmails.UserIds.Contains(e.ItemId))
                            .Select(e => e.Email)
                            .ToList();

                    var groupWithEmails = new GroupWithEmails(
                        workingGroupOverview.Id,
                        workingGroupOverview.Name,
                        groupEmails);

                    workingGroupsWithEmails.Add(groupWithEmails);
                }

                var emailGroups = this.emailGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var emailGroupIds = emailGroups.Select(g => g.Id).ToList();
                var emailGroupsEmails = this.emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

                emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

                foreach (var emailGroup in emailGroups)
                {
                    var groupEmails = emailGroupsEmails.Single(e => e.ItemId == emailGroup.Id).Emails;
                    var groupWithEmails = new GroupWithEmails(emailGroup.Id, emailGroup.Name, groupEmails);

                    emailGroupsWithEmails.Add(groupWithEmails);
                }
            }

            return OrderEditMapper.MapToOrderEditOptions(
                                    orderType,
                                    statuses,
                                    administrators,
                                    domains,
                                    departments,
                                    units,
                                    properties,
                                    deliveryDepartments,
                                    deliveryOuIds,
                                    emailGroupsWithEmails,
                                    workingGroupsWithEmails,
                                    administratorsWithEmails,
                                    settings);            
        }    
    }
}