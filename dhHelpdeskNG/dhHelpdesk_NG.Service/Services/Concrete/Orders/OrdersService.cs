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
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Common;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Orders;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.User;
    using DH.Helpdesk.Services.Services.Orders;
    using System;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Email;
    using System.Configuration;
    

    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IOrderFieldSettingsService orderFieldSettingsService;

        private readonly IUserRepository userRepository;

        private readonly IUserWorkingGroupRepository userWorkingGroupRepository;

        private readonly IWorkingGroupRepository workingGroupRepository;

        private readonly IEmailGroupEmailRepository emailGroupEmailRepository;

        private readonly IEmailGroupRepository emailGroupRepository;

        private readonly IOrderRestorer orderRestorer;

        private readonly IUpdateOrderRequestValidator updateOrderRequestValidator;

        private readonly List<IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>> orderAuditors;

        private readonly IOrdersLogic ordersLogic;

        private readonly IMailTemplateService mailTemplateService;

        private readonly IEmailService emailService;

        private readonly IOrderEMailLogRepository _orderEMailLogRepsoitory;

        private readonly ICustomerRepository _customerRepository;

        private readonly IOrderTypeRepository _orderTypeRepository;

        private readonly ICaseService _caseService;

        public OrdersService(
                IUnitOfWorkFactory unitOfWorkFactory, 
                IOrderFieldSettingsService orderFieldSettingsService, 
                IWorkingGroupRepository workingGroupRepository, 
                IUserRepository userRepository, 
                IUserWorkingGroupRepository userWorkingGroupRepository, 
                IEmailGroupEmailRepository emailGroupEmailRepository, 
                IEmailGroupRepository emailGroupRepository, 
                IOrderRestorer orderRestorer, 
                IUpdateOrderRequestValidator updateOrderRequestValidator, 
                List<IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>> orderAuditors, 
                IOrdersLogic ordersLogic,
                IMailTemplateService mailTemplateService,
                IEmailService emailService,
                IOrderEMailLogRepository orderEMailLogRepository,
                ICustomerRepository customerRepository,
                IOrderTypeRepository orderTypeRepository,
                ICaseService caseService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.orderFieldSettingsService = orderFieldSettingsService;
            this.workingGroupRepository = workingGroupRepository;
            this.userRepository = userRepository;
            this.userWorkingGroupRepository = userWorkingGroupRepository;
            this.emailGroupEmailRepository = emailGroupEmailRepository;
            this.emailGroupRepository = emailGroupRepository;
            this.orderRestorer = orderRestorer;
            this.updateOrderRequestValidator = updateOrderRequestValidator;
            this.orderAuditors = orderAuditors;
            this.ordersLogic = ordersLogic;
            this.mailTemplateService = mailTemplateService;
            this.emailService = emailService;
            this._orderEMailLogRepsoitory = orderEMailLogRepository;
            this._customerRepository = customerRepository;
            this._orderTypeRepository = orderTypeRepository;
            this._caseService = caseService;
        }

        public OrdersFilterData GetOrdersFilterData(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var orderTypeRep = uow.GetRepository<OrderType>();
                var administratorRep = uow.GetRepository<User>();
                var statusRep = uow.GetRepository<OrderState>();

                var orderTypes = orderTypeRep.GetAll()
                                    .GetOrderTypes(customerId).ToList();

                //var orderTypesInRow = this.GetChildrenInRow(orderTypes, true).ToList();



                var administrators = administratorRep.GetAll()
                                    .GetAdministrators(customerId);

                var statuses = statusRep.GetAll()
                                    .GetOrderStatuses(customerId);

                return OrderMapper.MapToFilterData(orderTypes, administrators, statuses);
            }
        }


        public IList<OrderType> GetChildrenInRow(IList<OrderType> orderTypes, bool isTakeOnlyActive = false)
        {
            var childOrderTypes = new List<OrderType>();
            var parentOrderTypes = orderTypes.Where(ot => !ot.Parent_OrderType_Id.HasValue && (isTakeOnlyActive ? ot.IsActive == 1 : true) && ot.SubOrderTypes.Count == 0).ToList();
            
            //var parentOrderTypes = orderTypes.Where(ot => !ot.Parent_OrderType_Id.HasValue && (isTakeOnlyActive ? ot.IsActive == 1 : true)).ToList();
            var parentOrderTypesWithChild = orderTypes.Where(ot => !ot.Parent_OrderType_Id.HasValue && (isTakeOnlyActive ? ot.IsActive == 1 : true)).ToList();

            foreach (var p in parentOrderTypesWithChild)
            {
                childOrderTypes.AddRange(GetChilds(p.Name, p.IsActive, p.SubOrderTypes.ToList(), isTakeOnlyActive, p.Id));
            }

            return parentOrderTypes.Union(childOrderTypes).OrderBy(c => c.Name).ToList();
        }

        private IList<OrderType> GetChilds(string parentName, int parentState, IList<OrderType> subOrderTypes, bool isTakeOnlyActive = false, int Parent_OrderType_Id = 0)
        {
            var ret = new List<OrderType>();
            var newSubOrderTypes = subOrderTypes.Where(ct => (isTakeOnlyActive ? ct.IsActive == 1 : true)).ToList();
            var newInactiveSubOrderTypes = subOrderTypes.Where(ct => (isTakeOnlyActive ? ct.IsActive == 0 : true)).ToList();

            foreach (var s in newSubOrderTypes)
            {
                var newParentName = string.Format("{0} - {1}", parentName, s.Name);
                var newCT = new OrderType()
                {
                    Id = s.Id,
                    Name = newParentName,
                    IsActive = parentState,
                    Parent_OrderType_Id = s.Id
                };

                if (!s.SubOrderTypes.Any())
                    ret.Add(newCT);

                if (s.SubOrderTypes.Any())
                    ret.AddRange(GetChilds(newParentName, parentState, s.SubOrderTypes.ToList(), isTakeOnlyActive, Parent_OrderType_Id));

                //ret.Add(newCT);
            }


            if (newInactiveSubOrderTypes.Any())
            {
                var newParentName = string.Format("{0}", parentName);
                var newCT = new OrderType()
                {
                    Id = Parent_OrderType_Id,
                    Name = parentName,
                    IsActive = parentState,
                    Parent_OrderType_Id = null
                };

                ret.Add(newCT);
            }


            return ret;
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

        public NewOrderEditData GetNewOrderEditData(int customerId, int orderTypeId, int? lowestchildordertypeid)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var settings = this.orderFieldSettingsService.GetOrderEditSettings(customerId, orderTypeId, uow);
                var options = this.GetEditOptions(customerId, orderTypeId, settings, uow, lowestchildordertypeid);

                return new NewOrderEditData(settings, options);
            }
        }

        public FindOrderResponse FindOrder(int orderId, int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var ordersRep = uow.GetRepository<Order>();
                var orderHistoryRep = uow.GetRepository<OrderHistoryEntity>();
                //var orderLogRep = uow.GetRepository<OrderLog>();
                var orderEmailLogRep = uow.GetRepository<OrderEMailLog>();

                var order = ordersRep.GetAll()
                                .GetById(orderId)
                                .MapToFullOrderEditFields();

                var firstLevelParentId = 0;
               
                // check if ordertype has parent
                var ordertype = this._orderTypeRepository.GetById(order.OrderTypeId.Value);
                    
                if (ordertype.Parent_OrderType_Id.HasValue)
                {
                    if (ordertype.ParentOrderType.Parent_OrderType_Id.HasValue)
                    {
                        if (ordertype.ParentOrderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                        {
                            firstLevelParentId = ordertype.ParentOrderType.ParentOrderType.Parent_OrderType_Id.Value;
                        }
                        else
                        {
                            firstLevelParentId = ordertype.ParentOrderType.Parent_OrderType_Id.Value;
                        }
                    }
                    else
                    {
                        firstLevelParentId = ordertype.Parent_OrderType_Id.Value;
                    }
                }
                else
                {
                    firstLevelParentId = order.OrderTypeId.Value;
                }

                var settings = this.orderFieldSettingsService.GetOrderEditSettings(customerId, firstLevelParentId, uow);
                var options = this.GetEditOptions(customerId, firstLevelParentId, settings, uow, order.OrderTypeId);
                //var options = this.GetEditOptions(customerId, order.OrderTypeId, settings, uow, firstLevelParentId);

                var histories = orderHistoryRep.GetAll()
                                .GetByOrder(orderId)
                                .MapToOverviews();
                var historyIds = histories.Select(i => i.Id).ToArray();
                //var logOverviews = orderLogRep.GetAll()
                //                .GetByHistoryIds(historyIds)
                //                .MapToOverviews();
                var emailLogs = orderEmailLogRep.GetAll()
                                .GetByHistoryIds(historyIds)
                                .MapToOverviews();
                var historyDifferences = this.ordersLogic.AnalyzeHistoriesDifferences(histories, emailLogs, settings);

                var data = new OrderEditData(order, historyDifferences);

                return new FindOrderResponse(data, settings, options);
            }
        }

        public int AddOrUpdate(UpdateOrderRequest request)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var ordersRep = uow.GetRepository<Order>();
                var orderLogsRep = uow.GetRepository<OrderLog>();
                var orderHistoryRep = uow.GetRepository<OrderHistoryEntity>();
                
                Order entity;
                FullOrderEditFields existingOrder = null;
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

                    existingOrder = OrderEditMapper.MapToFullOrderEditFields(entity);
                    // check if ordertype has parent
                    var ordertype = this._orderTypeRepository.GetById(request.Order.OrderTypeId.Value);

                    var firstLevelParentId = 0;
                    if (ordertype.Parent_OrderType_Id.HasValue)
                    {
                        if (ordertype.ParentOrderType.Parent_OrderType_Id.HasValue)
                        {
                            if (ordertype.ParentOrderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                            {
                                firstLevelParentId = ordertype.ParentOrderType.ParentOrderType.Parent_OrderType_Id.Value;
                            }
                            else
                            {
                                firstLevelParentId = ordertype.ParentOrderType.Parent_OrderType_Id.Value;
                            }
                        }
                        else
                        {
                            firstLevelParentId = ordertype.Parent_OrderType_Id.Value;
                        }
                    }
                    else
                    {
                        firstLevelParentId = entity.OrderType_Id.Value;
                    }

                    var settings = this.orderFieldSettingsService.GetOrderEditSettings(request.CustomerId, firstLevelParentId, uow);
                    this.orderRestorer.Restore(request.Order, existingOrder, settings);
                    this.updateOrderRequestValidator.Validate(request.Order, existingOrder, settings);

                    OrderUpdateMapper.MapToEntity(entity, request.Order, request.CustomerId);
                    entity.ChangedDate = request.DateAndTime;
                    ordersRep.Update(entity);
                }

                
                var history = OrderHistoryMapper.MapToBusinessModel(request);
                var historyEntity = OrderHistoryMapper.MapToEntity(history);
                orderHistoryRep.Add(historyEntity);

                orderLogsRep.DeleteWhere(l => request.DeletedLogIds.Contains(l.Id));

                uow.Save();
                var orderId = entity.Id;

                foreach (var newLog in request.NewLogs)
                {
                    newLog.OrderId = orderId;
                    newLog.OrderHistoryId = historyEntity.Id;
                    newLog.CreatedByUserId = request.UserId;
                    newLog.CreatedDateAndTime = request.DateAndTime;
                    var logEntity = OrderLogMapper.MapToEntity(newLog);
                    orderLogsRep.Add(logEntity);
                }

                uow.Save();

                if (request.InformOrderer == true)
                {
                    var currentUser = this.userRepository.GetById(request.UserId);
                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId);
                    // get list of fields to replace [#1] tags in the subjcet and body texts
                    List<Field> fields = GetOrderFieldsForEmail(request, string.Empty, userTimeZone);
                    var customer = this._customerRepository.GetById(request.CustomerId);

                    var customEmailSender1 = customer.HelpdeskEmail;
                    MailTemplateLanguageEntity m = this.mailTemplateService.GetMailTemplateLanguageForCustomer(40, request.CustomerId, request.LanguageId, request.Order.OrderTypeId);
                    if (m != null)
                    {
                        if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                        {
                            var to = request.Order.Orderer.OrdererEmail;

                            var curMail = to.ToString();
                            if (!string.IsNullOrWhiteSpace(curMail) && emailService.IsValidEmail(curMail))
                            {

                                var el = new OrderEMailLog(orderId, historyEntity.Id, 40, curMail, customEmailSender1);
                                fields = GetOrderFieldsForEmail(request, el.OrderEMailLogGUID.ToString(), userTimeZone);
                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.OrderEMailLogGUID.ToString();

                                //var AbsoluteUrl = RequestExtension.GetAbsoluteUrl();
                                var AbsoluteUrl = "";

                                var siteHelpdesk = AbsoluteUrl + "Areas/Orders/edit/" + request.Order.Id.ToString();
                                var e_res = this.emailService.SendEmail(customEmailSender1, el.EMailAddress, m.Subject, m.Body, null, EmailResponse.GetEmptyEmailResponse(), el.MessageId, false, null, siteSelfService, siteHelpdesk);

                                //el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                this._orderEMailLogRepsoitory.Add(el);
                                this._orderEMailLogRepsoitory.Commit();
                            }
                        }

                    }

                }

                if (request.InformReceiver == true)
                {
                    var currentUser = this.userRepository.GetById(request.UserId);
                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId);
                    // get list of fields to replace [#1] tags in the subjcet and body texts
                    List<Field> fields = GetOrderFieldsForEmail(request, string.Empty, userTimeZone);
                    var customer = this._customerRepository.GetById(request.CustomerId);

                    var customEmailSender1 = customer.HelpdeskEmail;
                    MailTemplateLanguageEntity m = this.mailTemplateService.GetMailTemplateLanguageForCustomer(40, request.CustomerId, request.LanguageId, request.Order.OrderTypeId);
                    if (m != null)
                    {
                        if (!String.IsNullOrEmpty(m.Body) && !String.IsNullOrEmpty(m.Subject))
                        {
                            var to = request.Order.Receiver.ReceiverEmail;

                            var curMail = to.ToString();
                            if (!string.IsNullOrWhiteSpace(curMail) && emailService.IsValidEmail(curMail))
                            {

                                var el = new OrderEMailLog(orderId, historyEntity.Id, 40, curMail, customEmailSender1);
                                fields = GetOrderFieldsForEmail(request, el.OrderEMailLogGUID.ToString(), userTimeZone);
                                string siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.OrderEMailLogGUID.ToString();

                                //var AbsoluteUrl = RequestExtension.GetAbsoluteUrl();
                                var AbsoluteUrl = "";

                                var siteHelpdesk = AbsoluteUrl + "Areas/Orders/edit/" + request.Order.Id.ToString();
                                var e_res = this.emailService.SendEmail(customEmailSender1, el.EMailAddress, m.Subject, m.Body, null, EmailResponse.GetEmptyEmailResponse(), el.MessageId, false, null, siteSelfService, siteHelpdesk);

                                //el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                                var now = DateTime.Now;
                                el.CreatedDate = now;
                                this._orderEMailLogRepsoitory.Add(el);
                                this._orderEMailLogRepsoitory.Commit();
                            }
                        }

                    }

                }

                //if (request.CreateCase == true)
                //{
                //    var caseTypeId = "";
                //    var departmentId = "";
                //    var case_ = new CaseEditInput();

                //    // save case and case history
                //    int caseHistoryId = this._caseService.SaveCase(
                //                case_,
                //                caseLog,
                //                caseMailSetting,
                //                SessionFacade.CurrentUser.Id,
                //                this.User.Identity.Name,
                //                ei,
                //                out errors,
                //                parentCase);    
                //}
                this.orderAuditors.ForEach(a => a.Audit(request, new OrderAuditData(historyEntity.Id, existingOrder)));

                return entity.Id;
            }
        }

        private List<Field> GetOrderFieldsForEmail(UpdateOrderRequest o, string emailLogGuid, TimeZoneInfo userTimeZone)
        {
            List<Field> ret = new List<Field>();

            //var userLocal_RegTime = TimeZoneInfo.ConvertTimeFromUtc(o.DateAndTime, userTimeZone);

            ret.Add(new Field { Key = "[#61]", StringValue = o.Order.Id.ToString() });
            //ret.Add(new Field { Key = "[#16]", StringValue = userLocal_RegTime.ToString() });

            return ret;
        }

        public void Delete(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var ordersRep = uow.GetRepository<Order>();
                var orderLogsRep = uow.GetRepository<OrderLog>();
                var orderHistoryRep = uow.GetRepository<OrderHistoryEntity>();

                var order = ordersRep.GetById(id);
                order.Programs.Clear();

                var orderLogIds = order.Logs.Select(l => l.Id);
                orderLogsRep.DeleteWhere(l => orderLogIds.Contains(l.Id));

                var orderHistoryIds = order.Histories.Select(h => h.Id);
                orderHistoryRep.DeleteWhere(h => orderHistoryIds.Contains(h.Id));

                ordersRep.DeleteById(id);

                uow.Save();
            }
        }

        public List<BusinessData.Models.Orders.Order.OrderEditFields.Log> FindLogsExcludeSpecified(int orderId, List<int> excludeLogIds)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var logsRep = uow.GetRepository<OrderLog>();
                var usersRep = uow.GetRepository<User>();

                return logsRep.GetAll()
                            .GetExcludeSpecified(orderId, excludeLogIds)
                            .MapToLogs(usersRep.GetAll());                         
            }            
        }

        private OrderEditOptions GetEditOptions(
                                int customerId, 
                                int? orderTypeId,
                                FullOrderEditSettings settings,
                                IUnitOfWork uow,
                                int? lowestchildordertypeid)
        {
            var statusesRep = uow.GetRepository<OrderState>();
            var administratorsRep = uow.GetRepository<User>();
            var domainsRep = uow.GetRepository<Domain>();
            var departmentsRep = uow.GetRepository<Department>();
            var ousRep = uow.GetRepository<OU>();
            var propertiesRep = uow.GetRepository<OrderPropertyEntity>();
            var orderTypeRep = uow.GetRepository<OrderType>();

            var statuses = statusesRep.GetAll().GetByCustomer(customerId).OrderBy(x => x.SortOrder);
            var administrators = administratorsRep.GetAll().GetByCustomer(customerId).GetActiveUsers(customerId);
            var domains = domainsRep.GetAll().GetByCustomer(customerId);
            var departments = departmentsRep.GetAll().GetByCustomer(customerId);
            var units = ousRep.GetAll();
            var properties = propertiesRep.GetAll().GetByOrderType(orderTypeId);
            var deliveryDepartments = departmentsRep.GetAll().GetByCustomer(customerId);
            var deliveryOuIds = ousRep.GetAll();
            var administratorsWithEmails = administratorsRep.GetAll().GetAdministratorsWithEmails(customerId);
            var orderType = orderTypeId.HasValue ? orderTypeRep.GetAll().GetById(orderTypeId.Value).MapToName() : null;

            // get parentordertypename
            if (orderTypeId != lowestchildordertypeid.Value)
            {
                if (lowestchildordertypeid.HasValue)
                {
                    var level2Name = "";
                    var level3Name = "";
                    var level4Name = "";

                    //get
                    var lowestchild = this._orderTypeRepository.GetById(lowestchildordertypeid.Value);

                    if (lowestchild.Parent_OrderType_Id.HasValue)
                    {
                        if (lowestchild.ParentOrderType.Parent_OrderType_Id.HasValue)
                        {
                            level2Name = lowestchild.ParentOrderType.Parent_OrderType_Id.HasValue ? orderTypeRep.GetAll().GetById(lowestchild.ParentOrderType.Parent_OrderType_Id.Value).MapToName() : null;
                            orderType = orderType + " - " + level2Name;
                        }

                        if (orderTypeId != lowestchild.Parent_OrderType_Id.Value)
                        {
                            level3Name = lowestchild.Parent_OrderType_Id.HasValue ? orderTypeRep.GetAll().GetById(lowestchild.Parent_OrderType_Id.Value).MapToName() : null;
                            orderType = orderType + " - " + level3Name;
                        }
                    }

                    level4Name = lowestchildordertypeid.HasValue ? orderTypeRep.GetAll().GetById(lowestchildordertypeid.Value).MapToName() : null;

                    orderType = orderType + " - " + level4Name;
                }
            }

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