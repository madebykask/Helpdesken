namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        private readonly IWorkContext workContext;

        private readonly IOrdersModelFactory ordersModelFactory;

        private readonly TemporaryIdProvider temporaryIdProvider;

        private readonly INewOrderModelFactory newOrderModelFactory;

        private readonly IOrderModelFactory orderModelFactory;

        private readonly ITemporaryFilesCache filesStore;

        private readonly IEditorStateCache filesStateStore;

        private readonly IUpdateOrderModelFactory updateOrderModelFactory;

        private readonly ILogsModelFactory logsModelFactory;

        private readonly IEmailService emailService;

        private readonly IUserPermissionsChecker _userPermissionsChecker;

        private readonly IOrderTypeService _orderTypeService;

        private readonly ICustomerService _customerService;

        private readonly ISettingService _settingService;

        public OrdersController(
                IMasterDataService masterDataService, 
                IOrdersService ordersService, 
                IWorkContext workContext, 
                IOrdersModelFactory ordersModelFactory, 
                TemporaryIdProvider temporaryIdProvider, 
                INewOrderModelFactory newOrderModelFactory, 
                IOrderModelFactory orderModelFactory, 
                IEditorStateCacheFactory editorStateCacheFactory,
                ITemporaryFilesCacheFactory temporaryFilesCacheFactory, 
                IUpdateOrderModelFactory updateOrderModelFactory, 
                ILogsModelFactory logsModelFactory, 
                IEmailService emailService,
                IUserPermissionsChecker userPermissionsChecker,
                IOrderTypeService orderTypeService,
                ICustomerService customerService,
                ISettingService settingService)
            : base(masterDataService)
        {
            this.ordersService = ordersService;
            this.workContext = workContext;
            this.ordersModelFactory = ordersModelFactory;
            this.temporaryIdProvider = temporaryIdProvider;
            this.newOrderModelFactory = newOrderModelFactory;
            this.orderModelFactory = orderModelFactory;
            this.updateOrderModelFactory = updateOrderModelFactory;
            this.logsModelFactory = logsModelFactory;
            this.emailService = emailService;
            this._userPermissionsChecker = userPermissionsChecker;
            this._orderTypeService = orderTypeService;
            this._customerService = customerService;
            this._settingService = settingService;

            this.filesStateStore = editorStateCacheFactory.CreateForModule(ModuleName.Orders);
            this.filesStore = temporaryFilesCacheFactory.CreateForModule(ModuleName.Orders);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var filters = SessionFacade.FindPageFilters<OrdersFilterModel>(PageName.OrdersOrders);
            if (filters == null)
            {
                filters = OrdersFilterModel.CreateDefault();
                SessionFacade.SavePageFilters(PageName.OrdersOrders, filters);
            }

			int[] selectedStatuses;
            var data = this.ordersService.GetOrdersFilterData(this.workContext.Customer.CustomerId, out selectedStatuses);

	        var filledFilters = new OrdersFilterModel(filters.OrderTypeId, filters.AdministratiorIds, filters.StartDate, filters.EndDate, selectedStatuses, filters.Text, filters.RecordsOnPage, filters.SortField);

            var model = this.ordersModelFactory.GetIndexModel(data, filledFilters);
            return this.View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [BadRequestOnNotValid]
        public PartialViewResult Orders(OrdersIndexModel model)
        {
            var filters = model != null
                        ? model.GetFilter()
                        : SessionFacade.FindPageFilters<OrdersFilterModel>(PageName.OrdersOrders);

            SessionFacade.SavePageFilters(PageName.OrdersOrders, filters);

            var parameters = new SearchParameters(
                                    this.workContext.Customer.CustomerId,
                                    filters.OrderTypeId,
                                    filters.AdministratiorIds,
                                    filters.StartDate,
                                    filters.EndDate,
                                    filters.StatusIds,
                                    filters.Text,
                                    filters.RecordsOnPage,
                                    filters.SortField);

            var response = this.ordersService.Search(parameters);
            var ordersModel = this.ordersModelFactory.Create(response, filters.SortField, filters.OrderTypeId == null);

            
            return this.PartialView(ordersModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult CreateOrder(int orderTypeForCteateOrderId)
        {
            var lowestchildordertypeid = orderTypeForCteateOrderId;
            //check if ordertype has a parent
            var orderType = this._orderTypeService.GetOrderType(orderTypeForCteateOrderId);
            if (orderType.Parent_OrderType_Id.HasValue)
            {
                if (orderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                {
                    if (orderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                    {
                        if (orderType.ParentOrderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                        {
                            orderTypeForCteateOrderId = orderType.ParentOrderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.Value;
                        }
                        else
                        {
                            orderTypeForCteateOrderId = orderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.Value;
                        }
                    }
                    else
                    {
                        orderTypeForCteateOrderId = orderType.ParentOrderType.Parent_OrderType_Id.Value;
                    }
                }
                else
                {
                    orderTypeForCteateOrderId = orderType.Parent_OrderType_Id.Value;
                }
            }
            else
            {
                orderTypeForCteateOrderId = orderType.Id;
            }

            var data = this.ordersService.GetNewOrderEditData(this.workContext.Customer.CustomerId, orderTypeForCteateOrderId, lowestchildordertypeid);
            var temporaryId = this.temporaryIdProvider.ProvideTemporaryId();

            var model = this.newOrderModelFactory.Create(
                                                temporaryId,
                                                data,
                                                this.workContext,
                                                orderTypeForCteateOrderId);

            model.OrderTypeId = lowestchildordertypeid;

            return this.View("New", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(FullOrderEditModel model)
        {
            var currentCustomer = this._customerService.GetCustomer(model.CustomerId);
            var cs = this._settingService.GetCustomerSetting(currentCustomer.Id);

            int intId;
            int.TryParse(model.Id, out intId);
            model.NewFiles = this.filesStore.FindFiles(model.Id, Subtopic.FileName.ToString());
            model.DeletedFiles = this.filesStateStore.FindDeletedFileNames(intId, Subtopic.FileName.ToString());

            var request = this.updateOrderModelFactory.Create(
                                                model, 
                                                this.workContext.Customer.CustomerId, 
                                                DateTime.Now, 
                                                this.emailService,
                                                this.workContext.User.UserId,
                                                SessionFacade.CurrentLanguageId);

            var caseMailSetting = new CaseMailSetting(
                                                         currentCustomer.NewCaseEmailList,
                                                         currentCustomer.HelpdeskEmail,
                                                         RequestExtension.GetAbsoluteUrl(),
                                                         cs.DontConnectUserToWorkingGroup
                                                       );
            var id = this.ordersService.AddOrUpdate(request, SessionFacade.CurrentUser.UserId, caseMailSetting, SessionFacade.CurrentLanguageId);

            foreach (var newFile in model.NewFiles)
            {
                this.filesStore.AddFile(newFile.Content, newFile.Name, id, Subtopic.FileName.ToString());
            }

            this.filesStore.ResetCacheForObject(model.Id);

            this.filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {            
            this.filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            var response = this.ordersService.FindOrder(id, this.workContext.Customer.CustomerId);
            if (response == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var filesInDb = response.EditData.Order.Other.FileName != null ? new List<string> { response.EditData.Order.Other.FileName } : new List<string>();
            var filesOnDisc = this.filesStore.FindFiles(id, Subtopic.FileName.ToString()).Select(f => f.Name).ToArray();
            foreach (var fileOnDisc in filesOnDisc)
            {
                if (!filesInDb.Contains(fileOnDisc))
                {
                    this.filesStore.DeleteFile(fileOnDisc, id, Subtopic.FileName.ToString());
                }
            }

            this.filesStateStore.ClearObjectDeletedFiles(id);

            var userHasAdminOrderPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.AdministerOrderPermission);

            var model = this.orderModelFactory.Create(response, this.workContext.Customer.CustomerId);
            model.UserHasAdminOrderPermission = userHasAdminOrderPermission;

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(FullOrderEditModel model)
        {
            var currentCustomer = this._customerService.GetCustomer(model.CustomerId);
            var cs = this._settingService.GetCustomerSetting(currentCustomer.Id);

            var id = int.Parse(model.Id);
            var filesInDb = model.Other?.FileName?.Value != null ? model.Other.FileName.Value.Files : new List<string>();
            model.NewFiles = this.filesStore.FindFiles(model.Id, Subtopic.FileName.ToString()).Where(f => !filesInDb.Contains(f.Name)).ToList();
            model.DeletedFiles = this.filesStateStore.FindDeletedFileNames(id, Subtopic.FileName.ToString());

            model.DeletedLogIds = this.filesStateStore.GetDeletedItemIds(id, OrderDeletedItem.Logs);

            var request = this.updateOrderModelFactory.Create(
                                                model, 
                                                this.workContext.Customer.CustomerId, 
                                                DateTime.Now,
                                                this.emailService,
                                                this.workContext.User.UserId,
                                                SessionFacade.CurrentLanguageId);

            var caseMailSetting = new CaseMailSetting(
                                                        currentCustomer.NewCaseEmailList,
                                                        currentCustomer.HelpdeskEmail,
                                                        RequestExtension.GetAbsoluteUrl(),
                                                        cs.DontConnectUserToWorkingGroup
                                                    );

            this.ordersService.AddOrUpdate(request, SessionFacade.CurrentUser.UserId, caseMailSetting, SessionFacade.CurrentLanguageId);

            foreach (var deletedFile in model.DeletedFiles)
            {
                this.filesStore.DeleteFile(deletedFile, model.Id, Subtopic.FileName.ToString());
            }

            this.filesStateStore.ClearObjectDeletedFiles(id);  

            this.filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.ordersService.Delete(id);
            this.filesStore.ResetCacheForObject(id);
            this.filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult AttachedFiles(string entityId, Subtopic subtopic)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(entityId))
            {
                fileNames = this.filesStore.FindFileNames(entityId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(entityId);
                var savedFiles = this.filesStore.FindFileNames(id, subtopic.ToString());
                var deletedFiles = this.filesStateStore.FindDeletedFileNames(id, subtopic.ToString());

                fileNames = new List<string>();
                fileNames.AddRange(savedFiles.Where(f => !deletedFiles.Contains(f)));
            }

            var model = new AttachedFilesModel(entityId, subtopic, fileNames);
            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string entityId, Subtopic subtopic, string name)
        {
            var uploadedFile = this.Request.Files[0];
            if (uploadedFile == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var fileContent = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileContent, 0, fileContent.Length);

            if (this.filesStore.FileExists(name, entityId, subtopic.ToString()))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            this.filesStore.AddFile(fileContent, name, entityId, subtopic.ToString());

            return this.RedirectToAction("AttachedFiles", new { entityId, subtopic });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string entityId, Subtopic subtopic, string fileName)
        {
            if (!this.filesStore.FileExists(fileName, entityId, subtopic.ToString()))
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var fileContent = this.filesStore.GetFileContent(fileName, entityId, subtopic.ToString());

            return this.File(fileContent, MimeType.BinaryFile, fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string entityId, Subtopic subtopic, string fileName)
        {
            if (GuidHelper.IsGuid(entityId))
            {
                this.filesStore.DeleteFile(fileName, entityId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(entityId);
                this.filesStateStore.AddDeletedFile(fileName, id, subtopic.ToString());
            }

            return this.RedirectToAction("AttachedFiles", new { entityId, subtopic });
        }

        [HttpGet]
        public PartialViewResult Logs(int orderId, Subtopic subtopic)
        {
            var deletedLogIds = this.filesStateStore.GetDeletedItemIds(orderId, OrderDeletedItem.Logs);
            var logs = this.ordersService.FindLogsExcludeSpecified(orderId, deletedLogIds);

            var response = this.ordersService.FindOrder(orderId, this.workContext.Customer.CustomerId);

            var model = this.logsModelFactory.Create(orderId, subtopic, logs, response.EditOptions);

            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int orderId, Subtopic subtopic, int logId)
        {
            this.filesStateStore.AddDeletedItem(logId, OrderDeletedItem.Logs, orderId);
            return this.RedirectToAction("Logs", new { orderId, subtopic });
        }
    }
}
