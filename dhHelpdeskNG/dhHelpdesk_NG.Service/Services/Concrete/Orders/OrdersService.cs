using DH.Helpdesk.BusinessData.Enums.Orders.FieldNames;
using DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
using DH.Helpdesk.Services.BusinessLogic.Specifications.User;

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
    using DH.Helpdesk.Services.Services.Orders;
    using System;
    using DH.Helpdesk.BusinessData.Models.Email;
    using System.Configuration;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums;
    using Infrastructure;

    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private readonly IOrderFieldSettingsService _orderFieldSettingsService;

        private readonly IUserRepository _userRepository;

        private readonly IUserWorkingGroupRepository _userWorkingGroupRepository;

        private readonly IWorkingGroupRepository _workingGroupRepository;

        private readonly IEmailGroupEmailRepository _emailGroupEmailRepository;

        private readonly IEmailGroupRepository _emailGroupRepository;

        private readonly IOrderRestorer _orderRestorer;

        private readonly IUpdateOrderRequestValidator _updateOrderRequestValidator;

        private readonly List<IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>> _orderAuditors;

        private readonly IOrdersLogic _ordersLogic;

        private readonly IMailTemplateService _mailTemplateService;

        private readonly IEmailService _emailService;

        private readonly IOrderEMailLogRepository _orderEMailLogRepsoitory;

        private readonly ICustomerRepository _customerRepository;

        private readonly IOrderTypeRepository _orderTypeRepository;

        private readonly ICaseService _caseService;

        private readonly ICaseTypeRepository _caseTypeRepository;

        private readonly ISettingService _settingService;

        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;

        private readonly IOrderRepository _orderRepository;

        private readonly IComputerUsersRepository _computerUsersRepository;

        private readonly ICaseExtraFollowersService _caseExtraFollowersService;

        private readonly IPriorityService _priorityService;

        private readonly IMasterDataService _masterDataService;

        private readonly IWorkingGroupService _workingGroupService;

        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly IOrderFieldSettingsRepository _orderFieldSettingsRepository;

        private readonly IOrderStateRepository _orderStateRepository;

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
                ICaseService caseService,
                ICaseTypeRepository caseTypeRepository,
                ISettingService settingService,
                IOrderRepository orderRepository,
                IEmailSendingSettingsProvider emailSendingSettingsProvider,
                IComputerUsersRepository computerUsersRepository,
                ICaseExtraFollowersService caseExtraFollowersService,
                IPriorityService priorityService,
                IMasterDataService masterDataService,
                IWorkingGroupService workingGroupService,
                IRegistrationSourceCustomerService registrationSourceCustomerService,
                IOrderFieldSettingsRepository orderFieldSettingsRepository,
                IOrderStateRepository orderStateRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _orderFieldSettingsService = orderFieldSettingsService;
            _workingGroupRepository = workingGroupRepository;
            _userRepository = userRepository;
            _userWorkingGroupRepository = userWorkingGroupRepository;
            _emailGroupEmailRepository = emailGroupEmailRepository;
            _emailGroupRepository = emailGroupRepository;
            _orderRestorer = orderRestorer;
            _updateOrderRequestValidator = updateOrderRequestValidator;
            _orderAuditors = orderAuditors;
            _ordersLogic = ordersLogic;
            _mailTemplateService = mailTemplateService;
            _emailService = emailService;
            _orderEMailLogRepsoitory = orderEMailLogRepository;
            _customerRepository = customerRepository;
            _orderTypeRepository = orderTypeRepository;
            _caseService = caseService;
            _caseTypeRepository = caseTypeRepository;
            _settingService = settingService;
            _emailSendingSettingsProvider = emailSendingSettingsProvider;
            _orderRepository = orderRepository;
            _computerUsersRepository = computerUsersRepository;
            _caseExtraFollowersService = caseExtraFollowersService;
            _priorityService = priorityService;
            _masterDataService = masterDataService;
            _workingGroupService = workingGroupService;
            _registrationSourceCustomerService = registrationSourceCustomerService;
            _orderFieldSettingsRepository = orderFieldSettingsRepository;
            _orderStateRepository = orderStateRepository;
        }

        public OrdersFilterData GetOrdersFilterData(int customerId, int userId, out int[] selectedStatuses)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var orderTypeRep = uow.GetRepository<OrderType>();
                var administratorRep = uow.GetRepository<User>();
                var statusRep = uow.GetRepository<OrderState>();
                var userRep = uow.GetRepository<User>();

                var orderTypes = new List<OrderType>();
                var user = userRep.GetById(userId);
                if (user != null &&
                    (user.UserGroup_Id == UserGroups.Administrator ||
                     user.UserGroup_Id == UserGroups.CustomerAdministrator ||
                     user.UserGroup_Id == UserGroups.SystemAdministrator))
                {
                    orderTypes = orderTypeRep.GetAll().GetRootOrderTypes(customerId).ToList();
                }
                else
                {
                    orderTypes = orderTypeRep.GetAll(m => m.Users)
                        .GetRootOrderTypes(customerId)
                        .GetByUsers(userId)
                        .ToList();
                }

                var orderTypesInRow = this.GetChildrenInRow(orderTypes, true).ToList();

                var administrators = administratorRep.GetAll().GetAdministrators(customerId);

                var statuses = statusRep.GetAll()
                                    .GetOrderStatuses(customerId);
                selectedStatuses = statuses.Where(x => x.SelectedInSearchCondition == 1).Select(x => x.Id).ToArray();
                return OrderMapper.MapToFilterData(orderTypes, orderTypesInRow, administrators, statuses);
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

        public SearchResponse Search(SearchParameters parameters, int userId, bool isSelfService = false)
        {
            var settings = this._orderFieldSettingsService.GetOrdersFieldSettingsOverview(parameters.CustomerId, parameters.OrderTypeId);
            var user = _userRepository.GetUser(userId);
            using (var uow = this._unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var orderTypeRep = uow.GetRepository<OrderType>();
                var orderFieldTypesRep = uow.GetRepository<OrderFieldType>();
                var orderTypes = orderTypeRep.GetAll(x => x.Users).GetOrderTypes(parameters.CustomerId).ToList();
                var filteredOrderTypes = orderTypes;
                if (isSelfService && user.UserGroupId < UserGroups.Administrator)
                {
                    filteredOrderTypes = FilterOrderTypeByUser(orderTypes, userId);
                }
                var rootOrderType = filteredOrderTypes.Where(ot => parameters.OrderTypeId == null || ot.Id == parameters.OrderTypeId).ToList();
                var orderTypeDescendants = GetChildrenInRow(rootOrderType, true).ToList();
                orderTypeDescendants.AddRange(rootOrderType);
                var orderRep = uow.GetRepository<Order>();

                var overviews = orderRep.GetAll().Search(
                                    parameters.CustomerId,
                                    orderTypeDescendants.Select(ot => ot.Id).ToArray(),
                                    parameters.AdministratorIds,
                                    parameters.StartDate,
                                    parameters.EndDate,
                                    parameters.StatusIds,
                                    parameters.Phrase,
                                    parameters.SortField,
                                    parameters.SelectCount,
                                    parameters.CreatedBy);

                var caseNumbers = overviews.Where(o => o.CaseNumber.HasValue).Select(o => o.CaseNumber).ToList();
                var caseRep = uow.GetRepository<Case>();
                var caseEntities = caseRep.GetAll().Where(c => caseNumbers.Contains(c.CaseNumber)).ToList();
                var orderFieldTypes = orderFieldTypesRep.GetAll().GetByType(parameters.OrderTypeId).ActiveOnly().ToList();

                var orderData = Sort(overviews.MapToFullOverviews(orderTypes, caseEntities, orderFieldTypes), parameters.SortField);

                var searchResult = new SearchResult(orderData.Count(), orderData);
                return new SearchResponse(settings, searchResult);
            }
        }

        public Order GetOrderByCase(int caseid)
        {
            return this._orderRepository.GetOrder(caseid);
        }

        public Order GetOrder(int orderId)
        {
            return this._orderRepository.GetById(orderId);
        }

        public IList<OrderFieldSettings> GetOrderFieldSettingsForMailTemplate(int customerId, int? orderTypeId)
        {
            return this._orderFieldSettingsRepository.GetOrderFieldSettingsForMailTemplate(customerId, orderTypeId).ToList();
        }

        public NewOrderEditData GetNewOrderEditData(int customerId, int orderTypeId, int? lowestchildordertypeid, bool useExternal)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var settings = this._orderFieldSettingsService.GetOrderEditSettings(customerId, orderTypeId, uow, useExternal);
                var options = this.GetEditOptions(customerId, orderTypeId, settings, uow, lowestchildordertypeid);

                return new NewOrderEditData(settings, options);
            }
        }

        public FindOrderResponse FindOrder(int orderId, int customerId, bool useExternal)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var ordersRep = uow.GetRepository<Order>();
                var orderHistoryRep = uow.GetRepository<OrderHistoryEntity>();
                var orderFieldTypesRep = uow.GetRepository<OrderFieldType>();
                //var orderLogRep = uow.GetRepository<OrderLog>();
                var orderEmailLogRep = uow.GetRepository<OrderEMailLog>();
                var caseRep = uow.GetRepository<Case>();

                var order = ordersRep.GetAll()
                                .GetById(orderId)
                                .MapToFullOrderEditFields();
                if (order.Other.CaseNumber.HasValue)
                {
                    order.Other.CaseId = caseRep.GetAll()
                        .GetByCaseNumber(order.Other.CaseNumber.Value).FirstOrDefault()?.Id;
                }

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

                var settings = this._orderFieldSettingsService.GetOrderEditSettings(customerId, firstLevelParentId, uow, useExternal);
                var options = this.GetEditOptions(customerId, firstLevelParentId, settings, uow, order.OrderTypeId);
                //var options = this.GetEditOptions(customerId, order.OrderTypeId, settings, uow, firstLevelParentId);
                var orderFieldTypes = orderFieldTypesRep.GetAll().GetByType(order.OrderTypeId).ToList();

                var histories = orderHistoryRep.GetAll()
                                .GetByOrder(orderId)
                                .MapToOverviews(orderFieldTypes);
                var historyIds = histories.Select(i => i.Id).ToArray();
                //var logOverviews = orderLogRep.GetAll()
                //                .GetByHistoryIds(historyIds)
                //                .MapToOverviews();
                var emailLogs = orderEmailLogRep.GetAll()
                                .GetByHistoryIds(historyIds)
                                .MapToOverviews();
                var historyDifferences = this._ordersLogic.AnalyzeHistoriesDifferences(histories, emailLogs, settings);

                var data = new OrderEditData(order, historyDifferences);

                return new FindOrderResponse(data, settings, options);
            }
        }

        public int AddOrUpdate(UpdateOrderRequest request, string userId, CaseMailSetting caseMailSetting, int languageId, bool useExternal)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var ordersRep = uow.GetRepository<Order>();
                var orderLogsRep = uow.GetRepository<OrderLog>();
                var orderHistoryRep = uow.GetRepository<OrderHistoryEntity>();
                var programRep = uow.GetRepository<Program>();

                var orderState = new OrderState();

                var customerSetting = _settingService.GetCustomerSetting(request.CustomerId);
                var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

                if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
                {
                    var info = _emailSendingSettingsProvider.GetSettings();
                    smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
                }

                var programs = new List<Program>();
                var programIds = request.Order.Program.Programs;
                if (programIds.Any())
                {
                    programs = programRep.GetAll().Where(x => programIds.Contains(x.Id)).ToList();
                }

                Order entity;
                FullOrderEditFields existingOrder = null;
                if (request.Order.IsNew())
                {
                    entity = new Order();
                    OrderUpdateMapper.MapToEntity(entity, request.Order, request.CustomerId, programs);
                    entity.CreatedDate = request.DateAndTime;
                    entity.CreatedByUser_Id = request.UserId;
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
                   
                    if (request.Order.General.StatusId != existingOrder.General.StatusId)
                    {
                        var statusId = request.Order.General.StatusId ?? default(int);
                        orderState = _orderStateRepository.GetById(statusId);
                    }

                    var settings = this._orderFieldSettingsService.GetOrderEditSettings(request.CustomerId, firstLevelParentId, uow, useExternal);
                    this._orderRestorer.Restore(request.Order, existingOrder, settings);
                    this._updateOrderRequestValidator.Validate(request.Order, existingOrder, settings);

                    OrderUpdateMapper.MapToEntity(entity, request.Order, request.CustomerId, programs);
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

                if (request.InformOrderer)
                {
                    SendOrderNotification(request, historyEntity, entity, smtpInfo, request.Order.Orderer.OrdererEmail);
                }

                if (request.InformReceiver)
                {
                    SendOrderNotification(request, historyEntity, entity, smtpInfo, request.Order.Receiver.ReceiverEmail);
                }

                if(orderState != null)
                {
                    var orderFollowers = orderState.EMailList ?? string.Empty;
                    var valuesSplitter = ",";
                    var pairSplitter = ";";
                    var eMails = orderFollowers
                        .Split(new[] { pairSplitter }, StringSplitOptions.None)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s =>
                        {
                            var values = s.Split(new[] { valuesSplitter }, StringSplitOptions.None);
                            return values[0];
                        }).ToList();
                    foreach(var eMail in eMails)
                    {
                        SendOrderNotification(request, historyEntity, entity, smtpInfo, eMail);
                    }                    
                }

                if (request.CreateCase)
                {
                    IDictionary<string, string> errors;

                    //get createcasetype by ordertype
                    var orderType = this._orderTypeRepository.GetById(entity.OrderType_Id.Value);

                    var newCase = new Case();

                    newCase.Customer_Id = entity.Customer_Id;
                    if (orderType.CreateCase_CaseType_Id.HasValue)
                    {
                        newCase.CaseType_Id = orderType.CreateCase_CaseType_Id.Value;
                    }
                    else
                    {
                        //get customer casetype
                        var customerId = entity.Customer_Id;
                        var casetype = this._caseTypeRepository.Get(x => x.Customer_Id == customerId && x.IsActive == 1);
                        newCase.CaseType_Id = casetype.Id;                    //get another id
                    }

                    var registrationSource = _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(request.CustomerId)
                            .FirstOrDefault(x => x.SystemCode == (int)CaseRegistrationSource.Order);
                    if (registrationSource != null)
                        newCase.RegistrationSourceCustomer_Id = registrationSource.Id;

                    if (!string.IsNullOrEmpty(entity.UserId))
                    {
                        var userIdForCase = entity.UserId;
                        if (userIdForCase.Contains("\\"))
                        {
                            userIdForCase = userIdForCase.Split('\\').Last();
                        }
                        var user = _userRepository.GetUserByLogin(userIdForCase, request.CustomerId);
                        if (user != null)
                        {
                            newCase.User_Id = user.Id;
                        }
                        var cUser = _computerUsersRepository.GetComputerUserByUserId(userIdForCase, request.CustomerId,
                            entity.Domain_Id);
                        if (cUser != null)
                        {
                            newCase.ReportedBy = cUser.UserId;
                            newCase.PersonsName = cUser.FirstName + " " + cUser.SurName;
                            newCase.PersonsEmail = cUser.Email;
                            newCase.PersonsPhone = cUser.Phone;
                            newCase.PersonsCellphone = cUser.Cellphone;
                            newCase.Department_Id = cUser.Department_Id;
                            newCase.OU_Id = cUser.OU_Id;
                            newCase.CostCentre = cUser.CostCentre;
                            newCase.UserCode = cUser.UserCode;
                        }
                    }

                    newCase.Priority_Id = _priorityService.GetDefaultId(request.CustomerId);

                    if (!newCase.Department_Id.HasValue)
                        newCase.Department_Id = entity.Department_Id;
                    //newCase.Priority_Id = entity.OrderPropertyId;
                    
                    newCase.Caption = orderType.Name;
                    newCase.RegLanguage_Id = languageId;
                    newCase.StateSecondary_Id = null;
                    newCase.Performer_User_Id = null;
                    newCase.CaseGUID = Guid.NewGuid();

                    var ei = new CaseExtraInfo() { CreatedByApp = CreatedByApplications.Helpdesk5, LeadTimeForNow = 0, ActionLeadTime = 0, ActionExternalTime = 0 };

                    #region Select Case Followers

                    var toFollowers = entity.OrdererID ?? string.Empty;
                    var valuesSplitter = "&,";
                    var pairSplitter = "&;";
                    var userIds = toFollowers
                        .Split(new[] { pairSplitter }, StringSplitOptions.None)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s =>
                        {
                            var values = s.Split(new[] { valuesSplitter }, StringSplitOptions.None);
                            return values[0];
                        })
                        .Where(x => !x.Equals(newCase.ReportedBy)).ToList();
                    var emails = _computerUsersRepository.GetEmailByUserIds(userIds, request.CustomerId);

                    #endregion

                    var orderRows = new List<string>
                    {
                        entity.OrderRow,
                        entity.OrderRow2,
                        entity.OrderRow3,
                        entity.OrderRow4,
                        entity.OrderRow5,
                        entity.OrderRow6,
                        entity.OrderRow7,
                        entity.OrderRow8,
                        entity.OrderInfo
                    };
                    var description = string.Empty;
                    foreach (var orderRow in orderRows)
                    {
                        if (!string.IsNullOrEmpty(orderRow))
                            description = description + orderRow + Environment.NewLine;
                    }
                    newCase.Description = string.IsNullOrEmpty(description) ? "." : description;

                    var caseHistoryId = this._caseService.SaveCase(newCase, null, 0, userId, ei, out errors);

                    //get casenumber
                    var newcase = this._caseService.GetCaseById(newCase.Id);

                    entity.CaseNumber = newcase.CaseNumber;

                    if (emails.Any())
                        _caseExtraFollowersService.SaveExtraFollowers(newCase.Id, emails, entity.User_Id);

                    #region Send New Case Emails

                    var basePath = _masterDataService.GetFilePath(newCase.Customer_Id);
                    var currentUser = _userRepository.GetById(request.UserId);
                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId);

                    var mailSenders = new MailSenders();
                    caseMailSetting.CustomeMailFromAddress = mailSenders;

                    mailSenders.SystemEmail = caseMailSetting.HelpdeskMailFromAdress;

                    if (newCase.WorkingGroup_Id.HasValue)
                    {
                        var curWG = _workingGroupService.GetWorkingGroup(newCase.WorkingGroup_Id.Value);
                        if (curWG != null)
                            if (!string.IsNullOrWhiteSpace(curWG.EMail) && _emailService.IsValidEmail(curWG.EMail))
                                mailSenders.WGEmail = curWG.EMail;
                    }

                    if (newCase.DefaultOwnerWG_Id.HasValue && newCase.DefaultOwnerWG_Id.Value > 0)
                    {
                        var defaultWGEmail = _workingGroupService.GetWorkingGroup(newCase.DefaultOwnerWG_Id.Value).EMail;
                        mailSenders.DefaultOwnerWGEMail = defaultWGEmail;
                    }
                    _caseService.SendCaseEmail(newCase.Id, caseMailSetting, caseHistoryId, basePath, userTimeZone);

                    #endregion

                    ordersRep.Update(entity);
                    uow.Save();
                }

                entity = _orderRepository.GetById(entity.Id);
                existingOrder = OrderEditMapper.MapToFullOrderEditFields(entity);
                this._orderAuditors.ForEach(a => a.Audit(request, new OrderAuditData(historyEntity.Id, existingOrder)));

                return entity.Id;
            }
        }

        private void SendOrderNotification(UpdateOrderRequest request, 
            OrderHistoryEntity historyEntity, Order entity,
            MailSMTPSetting smtpInfo, string mailto)
        {
            var currentUser = _userRepository.GetById(request.UserId);
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentUser.TimeZoneId);
            var customer = _customerRepository.GetById(request.CustomerId);
            var customerSetting = _settingService.GetCustomerSetting(request.CustomerId);
            var customEmailSender1 = customer.HelpdeskEmail;

            var m = _mailTemplateService.GetMailTemplateForCustomerAndLanguage(request.CustomerId, request.LanguageId, 40, request.Order.OrderTypeId);

            if (!string.IsNullOrEmpty(m?.Body) && !string.IsNullOrEmpty(m.Subject) &&
                !string.IsNullOrWhiteSpace(mailto) && _emailService.IsValidEmail(mailto))
            {
                var el = new OrderEMailLog(entity.Id, historyEntity.Id, 40, mailto, customEmailSender1);
                var fields = GetOrderFieldsForEmail(entity, el.OrderEMailLogGUID.ToString(), userTimeZone);
                var siteSelfService = ConfigurationManager.AppSettings["dh_selfserviceaddress"].ToString() + el.OrderEMailLogGUID.ToString();

                //var AbsoluteUrl = RequestExtension.GetAbsoluteUrl();
                var AbsoluteUrl = "";
                var mailResponse = EmailResponse.GetEmptyEmailResponse();
                var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
                var siteHelpdesk = AbsoluteUrl + "Areas/Orders/edit/" + entity.Id;
                var e_res = _emailService.SendEmail(customEmailSender1, el.EMailAddress, null, m.Subject, m.Body, fields, mailSetting,
                    el.MessageId, false, null, siteSelfService, siteHelpdesk);

                //el.SetResponse(e_res.SendTime, e_res.ResponseMessage);
                var now = DateTime.Now;
                el.CreatedDate = now;
                _orderEMailLogRepsoitory.Add(el);
                _orderEMailLogRepsoitory.Commit();
            }
        }

        private List<Field> GetOrderFieldsForEmail(Order o, string emailLogGuid, TimeZoneInfo userTimeZone)
        {
            List<Field> ret = new List<Field>();

            //var userLocal_RegTime = TimeZoneInfo.ConvertTimeFromUtc(o.DateAndTime, userTimeZone);

            ret.Add(new Field { Key = "[#61]", StringValue = o.Id.ToString() });
            //ret.Add(new Field { Key = "[#16]", StringValue = userLocal_RegTime.ToString() });

            return ret;
        }

        public void Delete(int id)
        {
            using (var uow = this._unitOfWorkFactory.Create())
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
            using (var uow = this._unitOfWorkFactory.Create())
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
            var employmentTypeRep = uow.GetRepository<EmploymentType>();
            var regionsRep = uow.GetRepository<Region>();
            var orderFieldTypesRep = uow.GetRepository<OrderFieldType>();
            var programsRep = uow.GetRepository<Program>();

            var statuses = statusesRep.GetAll().GetByCustomer(customerId).OrderBy(x => x.SortOrder);
            var administrators = administratorsRep.GetAll().GetByCustomer(customerId).GetActiveUsers(customerId).GetPerformers();
            var domains = domainsRep.GetAll().GetByCustomer(customerId);
            var departments = departmentsRep.GetAll().GetActiveByCustomer(customerId);
            var units = ousRep.GetAll().Where(o => o.Department.Customer_Id == customerId && o.IsActive == 1 && o.Show == 1);
            var properties = propertiesRep.GetAll().GetByOrderType(orderTypeId);
            var deliveryDepartments = departmentsRep.GetAll().GetActiveByCustomer(customerId);
            var deliveryOuIds = ousRep.GetAll().Where(o => o.Department.Customer_Id == customerId && o.IsActive == 1 && o.Show == 1); ;
            var administratorsWithEmails = administratorsRep.GetAll().GetAdministratorsWithEmails(customerId);
            var orderType = orderTypeId.HasValue ? orderTypeRep.GetAll().GetById(orderTypeId.Value) : null;
            var orderTypeName = orderType != null ? orderType.MapToName() : null;
            var orderTypeDesc = orderType?.MapToDescription();
            var orderTypeDoc = orderType?.MapToDocument();
            var employmentTypes = employmentTypeRep.GetAll();
            var regions = regionsRep.GetAll().GetByCustomer(customerId);
            var accountTypes = orderFieldTypesRep.GetAll().GetByType(orderTypeId).ActiveOnly();
            var allPrograms = programsRep.GetAll().Where(x => x.IsActive != 0 && x.Customer_Id == customerId && x.ShowOnStartPage > 0);

            // get parentordertypename
            if (lowestchildordertypeid.HasValue && orderTypeId != lowestchildordertypeid.Value)
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
                        level2Name = orderTypeRep.GetAll().GetById(lowestchild.ParentOrderType.Parent_OrderType_Id.Value).MapToName();
                        orderTypeName = orderTypeName + " - " + level2Name;
                    }

                    if (orderTypeId != lowestchild.Parent_OrderType_Id.Value)
                    {
                        level3Name = orderTypeRep.GetAll().GetById(lowestchild.Parent_OrderType_Id.Value).MapToName();
                        orderTypeName = orderTypeName + " - " + level3Name;
                    }
                }

                var ordType = orderTypeRep.GetAll().GetById(lowestchildordertypeid.Value);
                level4Name = ordType.MapToName();
                orderTypeName = orderTypeName + " - " + level4Name;
                orderTypeDesc = ordType.MapToDescription();
                orderTypeDoc = ordType.MapToDocument();
            }

            var workingGroupsWithEmails = new List<GroupWithEmails>();
            var emailGroupsWithEmails = new List<GroupWithEmails>();
            if (settings.Log.Log.Show)
            {
                var workingGroupOverviews = this._workingGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var workingGroupIds = workingGroupOverviews.Select(g => g.Id).ToList();
                var workingGroupsUserIds = this._userWorkingGroupRepository.FindWorkingGroupsUserIds(workingGroupIds);
                var userIds = workingGroupsUserIds.SelectMany(g => g.UserIds).ToList();
                var userIdsWithEmails = this._userRepository.FindUsersEmails(userIds);

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

                var emailGroups = this._emailGroupRepository.FindActiveIdAndNameOverviews(customerId);
                var emailGroupIds = emailGroups.Select(g => g.Id).ToList();
                var emailGroupsEmails = this._emailGroupEmailRepository.FindEmailGroupsEmails(emailGroupIds);

                emailGroupsWithEmails = new List<GroupWithEmails>(emailGroups.Count);

                foreach (var emailGroup in emailGroups)
                {
                    var groupEmails = emailGroupsEmails.Single(e => e.ItemId == emailGroup.Id).Emails;
                    var groupWithEmails = new GroupWithEmails(emailGroup.Id, emailGroup.Name, groupEmails);

                    emailGroupsWithEmails.Add(groupWithEmails);
                }
            }
            
            return OrderEditMapper.MapToOrderEditOptions(
                                    orderTypeName,
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
                                    settings,
                                    employmentTypes,
                                    regions,
                                    accountTypes,
                                    allPrograms,
                                    orderTypeDesc,
                                    orderTypeDoc);
        }

        private List<OrderType> FilterOrderTypeByUser(List<OrderType> orderTypes, int userId)
        {
            return orderTypes
                .Select(orderType => GetParentOrChildAssignedToUser(userId, orderType))
                .Where(type => type != null).ToList();
        }

        private static OrderType GetParentOrChildAssignedToUser(int userId, OrderType orderType)
        {
            for (var i = 0; i < 100000; i++)
            {
                if (orderType != null)
                {
                    if (orderType.Users.Any(u => u.Id == userId))
                    {
                        return orderType;
                    }
                    if (orderType.Parent_OrderType_Id.HasValue)
                    {
                        orderType = orderType.ParentOrderType;
                        continue;
                    }
                }
                return null;
            }
            return null;
        }

        private FullOrderOverview[] Sort(FullOrderOverview[] items, SortField sort)
        {
            var orderSort = items.OrderBy(o => o.OrderType);

            if (sort == null) return orderSort.ToArray();

            switch (sort.SortBy)
            {
                case SortBy.Ascending:
                    switch (sort.Name)
                    {
                        // Delivery
                        case DeliveryFieldNames.DeliveryDate:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryDate);
                            break;
                        case DeliveryFieldNames.InstallDate:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.InstallDate);
                            break;
                        case DeliveryFieldNames.DeliveryDepartment:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryDepartment);
                            break;
                        case DeliveryFieldNames.DeliveryOu:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryOu);
                            break;
                        case DeliveryFieldNames.DeliveryAddress:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryAddress);
                            break;
                        case DeliveryFieldNames.DeliveryPostalCode:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryPostalCode);
                            break;
                        case DeliveryFieldNames.DeliveryPostalAddress:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryPostalAddress);
                            break;
                        case DeliveryFieldNames.DeliveryLocation:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryLocation);
                            break;
                        case DeliveryFieldNames.DeliveryInfo1:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryInfo1);
                            break;
                        case DeliveryFieldNames.DeliveryInfo2:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryInfo2);
                            break;
                        case DeliveryFieldNames.DeliveryInfo3:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryInfo3);
                            break;
                        case DeliveryFieldNames.DeliveryOuId:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryOuId);
                            break;
                        case DeliveryFieldNames.DeliveryName:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryName);
                            break;
                        case DeliveryFieldNames.DeliveryPhone:
                            orderSort = orderSort.ThenBy(o => o.Delivery?.DeliveryPhone);
                            break;

                        // General
                        case GeneralFieldNames.OrderNumber:
                            orderSort = orderSort.ThenBy(o => o.General?.OrderNumber);
                            break;
                        case GeneralFieldNames.Customer:
                            orderSort = orderSort.ThenBy(o => o.General?.Customer);
                            break;
                        case GeneralFieldNames.Administrator:
                            orderSort = orderSort.ThenBy(o => o.General?.Administrator);
                            break;
                        case GeneralFieldNames.Domain:
                            orderSort = orderSort.ThenBy(o => o.General?.Domain);
                            break;
                        case GeneralFieldNames.OrderDate:
                            orderSort = orderSort.ThenBy(o => o.General?.OrderDate);
                            break;
                        //Log
                        case LogFieldNames.Log:
                            orderSort = orderSort.ThenBy(l => l.Log);
                            break;
                        //Orderer
                        case OrdererFieldNames.OrdererId:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererId);
                            break;
                        case OrdererFieldNames.OrdererName:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererName);
                            break;
                        case OrdererFieldNames.OrdererLocation:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererLocation);
                            break;
                        case OrdererFieldNames.OrdererEmail:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererEmail);
                            break;
                        case OrdererFieldNames.OrdererPhone:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererPhone);
                            break;
                        case OrdererFieldNames.OrdererCode:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererCode);
                            break;
                        case OrdererFieldNames.Department:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.Department);
                            break;
                        case OrdererFieldNames.Unit:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.Unit);
                            break;
                        case OrdererFieldNames.OrdererAddress:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererAddress);
                            break;
                        case OrdererFieldNames.OrdererInvoiceAddress:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererInvoiceAddress);
                            break;
                        case OrdererFieldNames.OrdererReferenceNumber:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.OrdererReferenceNumber);
                            break;
                        case OrdererFieldNames.AccountingDimension1:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.AccountingDimension1);
                            break;
                        case OrdererFieldNames.AccountingDimension2:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.AccountingDimension2);
                            break;
                        case OrdererFieldNames.AccountingDimension3:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.AccountingDimension3);
                            break;
                        case OrdererFieldNames.AccountingDimension4:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.AccountingDimension4);
                            break;
                        case OrdererFieldNames.AccountingDimension5:
                            orderSort = orderSort.ThenBy(o => o.Orderer?.AccountingDimension5);
                            break;
                        //Order
                        case OrderFieldNames.Property:
                            orderSort = orderSort.ThenBy(o => o.Order?.Property);
                            break;
                        case OrderFieldNames.OrderRow1:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow1);
                            break;
                        case OrderFieldNames.OrderRow2:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow2);
                            break;
                        case OrderFieldNames.OrderRow3:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow3);
                            break;
                        case OrderFieldNames.OrderRow4:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow4);
                            break;
                        case OrderFieldNames.OrderRow5:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow5);
                            break;
                        case OrderFieldNames.OrderRow6:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow6);
                            break;
                        case OrderFieldNames.OrderRow7:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow7);
                            break;
                        case OrderFieldNames.OrderRow8:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderRow8);
                            break;
                        case OrderFieldNames.Configuration:
                            orderSort = orderSort.ThenBy(o => o.Order?.Configuration);
                            break;
                        case OrderFieldNames.OrderInfo:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderInfo);
                            break;
                        case OrderFieldNames.OrderInfo2:
                            orderSort = orderSort.ThenBy(o => o.Order?.OrderInfo2);
                            break;
                        // Other
                        case OtherFieldNames.FileName:
                            orderSort = orderSort.ThenBy(o => o.Other?.FileName);
                            break;
                        case OtherFieldNames.CaseNumber:
                            orderSort = orderSort.ThenBy(o => o.Other?.CaseNumber);
                            break;
                        case OtherFieldNames.Info:
                            orderSort = orderSort.ThenBy(o => o.Other?.Info);
                            break;
                        case OtherFieldNames.Status:
                            orderSort = orderSort.ThenBy(o => o.Other?.Status);
                            break;
                        //Programm
                        case ProgramFieldNames.Program:
                            orderSort = orderSort.ThenBy(o => o.Program?.Programs?.Length);
                            break;
                        case ProgramFieldNames.InfoProduct:
                            orderSort = orderSort.ThenBy(o => o.Program?.InfoProduct);
                            break;
                        //Reciever
                        case ReceiverFieldNames.ReceiverId:
                            orderSort = orderSort.ThenBy(o => o.Receiver?.ReceiverId);
                            break;
                        case ReceiverFieldNames.ReceiverName:
                            orderSort = orderSort.ThenBy(o => o.Receiver?.ReceiverName);
                            break;
                        case ReceiverFieldNames.ReceiverEmail:
                            orderSort = orderSort.ThenBy(o => o.Receiver?.ReceiverEmail);
                            break;
                        case ReceiverFieldNames.ReceiverPhone:
                            orderSort = orderSort.ThenBy(o => o.Receiver?.ReceiverPhone);
                            break;
                        case ReceiverFieldNames.ReceiverLocation:
                            orderSort = orderSort.ThenBy(o => o.Receiver?.ReceiverLocation);
                            break;
                        case ReceiverFieldNames.MarkOfGoods:
                            orderSort = orderSort.ThenBy(o => o.Receiver?.MarkOfGoods);
                            break;
                        //Supplier
                        case SupplierFieldNames.SupplierOrderNumber:
                            orderSort = orderSort.ThenBy(o => o.Supplier?.SupplierOrderNumber);
                            break;
                        case SupplierFieldNames.SupplierOrderDate:
                            orderSort = orderSort.ThenBy(o => o.Supplier?.SupplierOrderDate);
                            break;
                        case SupplierFieldNames.SupplierOrderInfo:
                            orderSort = orderSort.ThenBy(o => o.Supplier?.SupplierOrderInfo);
                            break;
                        //User
                        case UserFieldNames.UserId:
                            orderSort = orderSort.ThenBy(o => o.User?.UserId);
                            break;
                        case UserFieldNames.UserFirstName:
                            orderSort = orderSort.ThenBy(o => o.User?.UserFirstName);
                            break;
                        case UserFieldNames.UserLastName:
                            orderSort = orderSort.ThenBy(o => o.User?.UserLastName);
                            break;
                        case UserFieldNames.UserPhone:
                            orderSort = orderSort.ThenBy(o => o.User?.UserPhone);
                            break;
                        case UserFieldNames.UserEMail:
                            orderSort = orderSort.ThenBy(o => o.User?.UserEMail);
                            break;
                        case UserFieldNames.UserInitials:
                            orderSort = orderSort.ThenBy(o => o.User?.UserInitials);
                            break;
                        case UserFieldNames.UserPersonalIdentityNumber:
                            orderSort = orderSort.ThenBy(o => o.User?.UserPersonalIdentityNumber);
                            break;
                        case UserFieldNames.UserExtension:
                            orderSort = orderSort.ThenBy(o => o.User?.UserExtension);
                            break;
                        case UserFieldNames.UserTitle:
                            orderSort = orderSort.ThenBy(o => o.User?.UserTitle);
                            break;
                        case UserFieldNames.UserLocation:
                            orderSort = orderSort.ThenBy(o => o.User?.UserLocation);
                            break;
                        case UserFieldNames.UserRoomNumber:
                            orderSort = orderSort.ThenBy(o => o.User?.UserRoomNumber);
                            break;
                        case UserFieldNames.UserPostalAddress:
                            orderSort = orderSort.ThenBy(o => o.User?.UserPostalAddress);
                            break;
                        case UserFieldNames.Responsibility:
                            orderSort = orderSort.ThenBy(o => o.User?.Responsibility);
                            break;
                        case UserFieldNames.Activity:
                            orderSort = orderSort.ThenBy(o => o.User?.Activity);
                            break;
                        case UserFieldNames.Manager:
                            orderSort = orderSort.ThenBy(o => o.User?.Manager);
                            break;
                        case UserFieldNames.ReferenceNumber:
                            orderSort = orderSort.ThenBy(o => o.User?.ReferenceNumber);
                            break;
                        case UserFieldNames.InfoUser:
                            orderSort = orderSort.ThenBy(o => o.User?.InfoUser);
                            break;
                        case UserFieldNames.UserOU_Id:
                            orderSort = orderSort.ThenBy(o => o.User?.UserOU_Id);
                            break;
                        case UserFieldNames.EmploymentType:
                            orderSort = orderSort.ThenBy(o => o.User?.EmploymentType);
                            break;
                        case UserFieldNames.UserDepartment_Id1:
                            orderSort = orderSort.ThenBy(o => o.User?.UserDepartment_Id1);
                            break;
                        case UserFieldNames.UserDepartment_Id2:
                            orderSort = orderSort.ThenBy(o => o.User?.UserDepartment_Id2);
                            break;

                        //Account Info
                        case AccountInfoFieldNames.StartedDate:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.StartedDate);
                            break;
                        case AccountInfoFieldNames.FinishDate:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.FinishDate);
                            break;
                        case AccountInfoFieldNames.HomeDirectory:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.HomeDirectory);
                            break;
                        case AccountInfoFieldNames.Profile:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.Profile);
                            break;
                        case AccountInfoFieldNames.InventoryNumber:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.InventoryNumber);
                            break;
                        case AccountInfoFieldNames.Info:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.Info);
                            break;
                        case AccountInfoFieldNames.AccountTypeId:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.AccountTypeId);
                            break;
                        //case AccountInfoFieldNames.AccountTypeId2:
                        //    orderSort = orderSort.ThenBy(o => o.AccountInfo?.OrderFieldType2.Name);
                        //    break;
                        case AccountInfoFieldNames.AccountTypeId3:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.AccountTypeId3);
                            break;
                        case AccountInfoFieldNames.AccountTypeId4:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.AccountTypeId4);
                            break;
                        case AccountInfoFieldNames.AccountTypeId5:
                            orderSort = orderSort.ThenBy(o => o.AccountInfo?.AccountTypeId5);
                            break;
                        //Contact
                        case ContactFieldNames.Id:
                            orderSort = orderSort.ThenBy(o => o.Contact?.Id);
                            break;
                        case ContactFieldNames.EMail:
                            orderSort = orderSort.ThenBy(o => o.Contact?.EMail);
                            break;
                        case ContactFieldNames.Name:
                            orderSort = orderSort.ThenBy(o => o.Contact?.Name);
                            break;
                        case ContactFieldNames.Phone:
                            orderSort = orderSort.ThenBy(o => o.Contact?.Phone);
                            break;
                    }
                    break;

                case SortBy.Descending:
                    switch (sort.Name)
                    {
                        // Delivery
                        case DeliveryFieldNames.DeliveryDate:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryDate);
                            break;
                        case DeliveryFieldNames.InstallDate:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.InstallDate);
                            break;
                        case DeliveryFieldNames.DeliveryDepartment:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryDepartment);
                            break;
                        case DeliveryFieldNames.DeliveryOu:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryOu);
                            break;
                        case DeliveryFieldNames.DeliveryAddress:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryAddress);
                            break;
                        case DeliveryFieldNames.DeliveryPostalCode:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryPostalCode);
                            break;
                        case DeliveryFieldNames.DeliveryPostalAddress:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryPostalAddress);
                            break;
                        case DeliveryFieldNames.DeliveryLocation:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryLocation);
                            break;
                        case DeliveryFieldNames.DeliveryInfo1:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryInfo1);
                            break;
                        case DeliveryFieldNames.DeliveryInfo2:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryInfo2);
                            break;
                        case DeliveryFieldNames.DeliveryInfo3:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryInfo3);
                            break;
                        case DeliveryFieldNames.DeliveryOuId:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryOuId);
                            break;
                        case DeliveryFieldNames.DeliveryName:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryName);
                            break;
                        case DeliveryFieldNames.DeliveryPhone:
                            orderSort = orderSort.ThenByDescending(o => o.Delivery?.DeliveryPhone);
                            break;

                        // General
                        case GeneralFieldNames.OrderNumber:
                            orderSort = orderSort.ThenByDescending(o => o.General?.OrderNumber);
                            break;
                        case GeneralFieldNames.Customer:
                            orderSort = orderSort.ThenByDescending(o => o.General?.Customer);
                            break;
                        case GeneralFieldNames.Administrator:
                            orderSort = orderSort.ThenByDescending(o => o.General?.Administrator);
                            break;
                        case GeneralFieldNames.Domain:
                            orderSort = orderSort.ThenByDescending(o => o.General?.Domain);
                            break;
                        case GeneralFieldNames.OrderDate:
                            orderSort = orderSort.ThenByDescending(o => o.General?.OrderDate);
                            break;
                        //Log
                        case LogFieldNames.Log:
                            orderSort = orderSort.ThenByDescending(l => l.Log);
                            break;
                        //Orderer
                        case OrdererFieldNames.OrdererId:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererId);
                            break;
                        case OrdererFieldNames.OrdererName:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererName);
                            break;
                        case OrdererFieldNames.OrdererLocation:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererLocation);
                            break;
                        case OrdererFieldNames.OrdererEmail:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererEmail);
                            break;
                        case OrdererFieldNames.OrdererPhone:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererPhone);
                            break;
                        case OrdererFieldNames.OrdererCode:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererCode);
                            break;
                        case OrdererFieldNames.Department:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.Department);
                            break;
                        case OrdererFieldNames.Unit:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.Unit);
                            break;
                        case OrdererFieldNames.OrdererAddress:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererAddress);
                            break;
                        case OrdererFieldNames.OrdererInvoiceAddress:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererInvoiceAddress);
                            break;
                        case OrdererFieldNames.OrdererReferenceNumber:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.OrdererReferenceNumber);
                            break;
                        case OrdererFieldNames.AccountingDimension1:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.AccountingDimension1);
                            break;
                        case OrdererFieldNames.AccountingDimension2:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.AccountingDimension2);
                            break;
                        case OrdererFieldNames.AccountingDimension3:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.AccountingDimension3);
                            break;
                        case OrdererFieldNames.AccountingDimension4:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.AccountingDimension4);
                            break;
                        case OrdererFieldNames.AccountingDimension5:
                            orderSort = orderSort.ThenByDescending(o => o.Orderer?.AccountingDimension5);
                            break;
                        //Order
                        case OrderFieldNames.Property:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.Property);
                            break;
                        case OrderFieldNames.OrderRow1:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow1);
                            break;
                        case OrderFieldNames.OrderRow2:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow2);
                            break;
                        case OrderFieldNames.OrderRow3:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow3);
                            break;
                        case OrderFieldNames.OrderRow4:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow4);
                            break;
                        case OrderFieldNames.OrderRow5:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow5);
                            break;
                        case OrderFieldNames.OrderRow6:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow6);
                            break;
                        case OrderFieldNames.OrderRow7:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow7);
                            break;
                        case OrderFieldNames.OrderRow8:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderRow8);
                            break;
                        case OrderFieldNames.Configuration:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.Configuration);
                            break;
                        case OrderFieldNames.OrderInfo:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderInfo);
                            break;
                        case OrderFieldNames.OrderInfo2:
                            orderSort = orderSort.ThenByDescending(o => o.Order?.OrderInfo2);
                            break;
                        // Other
                        case OtherFieldNames.FileName:
                            orderSort = orderSort.ThenByDescending(o => o.Other?.FileName);
                            break;
                        case OtherFieldNames.CaseNumber:
                            orderSort = orderSort.ThenByDescending(o => o.Other?.CaseNumber);
                            break;
                        case OtherFieldNames.Info:
                            orderSort = orderSort.ThenByDescending(o => o.Other?.Info);
                            break;
                        case OtherFieldNames.Status:
                            orderSort = orderSort.ThenByDescending(o => o.Other?.Status);
                            break;
                        //Programm
                        case ProgramFieldNames.Program:
                            orderSort = orderSort.ThenByDescending(o => o.Program?.Programs?.Length);
                            break;
                        case ProgramFieldNames.InfoProduct:
                            orderSort = orderSort.ThenByDescending(o => o.Program?.InfoProduct);
                            break;
                        //Reciever
                        case ReceiverFieldNames.ReceiverId:
                            orderSort = orderSort.ThenByDescending(o => o.Receiver?.ReceiverId);
                            break;
                        case ReceiverFieldNames.ReceiverName:
                            orderSort = orderSort.ThenByDescending(o => o.Receiver?.ReceiverName);
                            break;
                        case ReceiverFieldNames.ReceiverEmail:
                            orderSort = orderSort.ThenByDescending(o => o.Receiver?.ReceiverEmail);
                            break;
                        case ReceiverFieldNames.ReceiverPhone:
                            orderSort = orderSort.ThenByDescending(o => o.Receiver?.ReceiverPhone);
                            break;
                        case ReceiverFieldNames.ReceiverLocation:
                            orderSort = orderSort.ThenByDescending(o => o.Receiver?.ReceiverLocation);
                            break;
                        case ReceiverFieldNames.MarkOfGoods:
                            orderSort = orderSort.ThenByDescending(o => o.Receiver?.MarkOfGoods);
                            break;
                        //Supplier
                        case SupplierFieldNames.SupplierOrderNumber:
                            orderSort = orderSort.ThenByDescending(o => o.Supplier?.SupplierOrderNumber);
                            break;
                        case SupplierFieldNames.SupplierOrderDate:
                            orderSort = orderSort.ThenByDescending(o => o.Supplier?.SupplierOrderDate);
                            break;
                        case SupplierFieldNames.SupplierOrderInfo:
                            orderSort = orderSort.ThenByDescending(o => o.Supplier?.SupplierOrderInfo);
                            break;
                        //User
                        case UserFieldNames.UserId:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserId);
                            break;
                        case UserFieldNames.UserFirstName:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserFirstName);
                            break;
                        case UserFieldNames.UserLastName:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserLastName);
                            break;
                        case UserFieldNames.UserPhone:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserPhone);
                            break;
                        case UserFieldNames.UserEMail:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserEMail);
                            break;
                        case UserFieldNames.UserInitials:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserInitials);
                            break;
                        case UserFieldNames.UserPersonalIdentityNumber:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserPersonalIdentityNumber);
                            break;
                        case UserFieldNames.UserExtension:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserExtension);
                            break;
                        case UserFieldNames.UserTitle:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserTitle);
                            break;
                        case UserFieldNames.UserLocation:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserLocation);
                            break;
                        case UserFieldNames.UserRoomNumber:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserRoomNumber);
                            break;
                        case UserFieldNames.UserPostalAddress:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserPostalAddress);
                            break;
                        case UserFieldNames.Responsibility:
                            orderSort = orderSort.ThenByDescending(o => o.User?.Responsibility);
                            break;
                        case UserFieldNames.Activity:
                            orderSort = orderSort.ThenByDescending(o => o.User?.Activity);
                            break;
                        case UserFieldNames.Manager:
                            orderSort = orderSort.ThenByDescending(o => o.User?.Manager);
                            break;
                        case UserFieldNames.ReferenceNumber:
                            orderSort = orderSort.ThenByDescending(o => o.User?.ReferenceNumber);
                            break;
                        case UserFieldNames.InfoUser:
                            orderSort = orderSort.ThenByDescending(o => o.User?.InfoUser);
                            break;
                        case UserFieldNames.UserOU_Id:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserOU_Id);
                            break;
                        case UserFieldNames.EmploymentType:
                            orderSort = orderSort.ThenByDescending(o => o.User?.EmploymentType);
                            break;
                        case UserFieldNames.UserDepartment_Id1:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserDepartment_Id1);
                            break;
                        case UserFieldNames.UserDepartment_Id2:
                            orderSort = orderSort.ThenByDescending(o => o.User?.UserDepartment_Id2);
                            break;

                        //Account Info
                        case AccountInfoFieldNames.StartedDate:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.StartedDate);
                            break;
                        case AccountInfoFieldNames.FinishDate:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.FinishDate);
                            break;
                        case AccountInfoFieldNames.HomeDirectory:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.HomeDirectory);
                            break;
                        case AccountInfoFieldNames.Profile:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.Profile);
                            break;
                        case AccountInfoFieldNames.InventoryNumber:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.InventoryNumber);
                            break;
                        case AccountInfoFieldNames.Info:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.Info);
                            break;
                        case AccountInfoFieldNames.AccountTypeId:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.AccountTypeId);
                            break;
                        //case AccountInfoFieldNames.AccountTypeId2:
                        //    orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.OrderFieldType2.Name);
                        //    break;
                        case AccountInfoFieldNames.AccountTypeId3:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.AccountTypeId3);
                            break;
                        case AccountInfoFieldNames.AccountTypeId4:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.AccountTypeId4);
                            break;
                        case AccountInfoFieldNames.AccountTypeId5:
                            orderSort = orderSort.ThenByDescending(o => o.AccountInfo?.AccountTypeId5);
                            break;
                        //Contact
                        case ContactFieldNames.Id:
                            orderSort = orderSort.ThenByDescending(o => o.Contact?.Id);
                            break;
                        case ContactFieldNames.EMail:
                            orderSort = orderSort.ThenByDescending(o => o.Contact?.EMail);
                            break;
                        case ContactFieldNames.Name:
                            orderSort = orderSort.ThenByDescending(o => o.Contact?.Name);
                            break;
                        case ContactFieldNames.Phone:
                            orderSort = orderSort.ThenByDescending(o => o.Contact?.Phone);
                            break;
                    }
                    break;
            }

            return orderSort.ToArray();
        }
    }
}