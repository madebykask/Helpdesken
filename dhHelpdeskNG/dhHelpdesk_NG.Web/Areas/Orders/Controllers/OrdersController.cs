using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.Web.Infrastructure.Order;

namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web;
	using System.Web.Mvc;
	using BusinessData.Enums.Orders;
	using BusinessData.Models.Orders.Index;
	using DH.Helpdesk.Common.Tools;
	using Dal.Enums;
	using Dal.Infrastructure.Context;
	using Services.BusinessLogic.OtherTools.Concrete;
	using Services.Services;
	using Services.Services.Orders;
	using Infrastructure.ModelFactories;
	using Models.Index;
	using Models.Order.FieldModels;
	using Models.Order.OrderEdit;
	using Enums;
	using Web.Infrastructure;
	using Web.Infrastructure.ActionFilters;
	using Web.Infrastructure.Tools;
	using Services.BusinessLogic.Admin.Users;
	using Services.BusinessLogic.Mappers.Users;
	using BusinessData.Enums.Admin.Users;
	using BusinessData.Models.Case;
	using Web.Infrastructure.Extensions;
	using Services.Services.Concrete;
	using System.IO;

	public class OrdersController : BaseController
    {
        private readonly IOrdersService _ordersService;
        private readonly IWorkContext _workContext;
        private readonly IOrdersModelFactory _ordersModelFactory;
        private readonly TemporaryIdProvider _temporaryIdProvider;
        private readonly INewOrderModelFactory _newOrderModelFactory;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly ITemporaryFilesCache _filesStore;
        private readonly IEditorStateCache _filesStateStore;
        private readonly IUpdateOrderModelFactory _updateOrderModelFactory;
        private readonly ILogsModelFactory _logsModelFactory;
        private readonly IEmailService _emailService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly IOrderTypeService _orderTypeService;
        private readonly ICustomerService _customerService;
        private readonly ISettingService _settingService;
        private readonly IDocumentService _documentService;
        private readonly IOrganizationService _organizationService;
		private readonly IOrganizationJsonService _organizationJsonService;
		private readonly IGlobalSettingService _globalSettingService;

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
                ISettingService settingService,
                IDocumentService documentService,
                IOrganizationService organizationService,
				IOrganizationJsonService organizationJsonService,
				IGlobalSettingService globalSettingService)
            : base(masterDataService)
        {
            _ordersService = ordersService;
            _workContext = workContext;
            _ordersModelFactory = ordersModelFactory;
            _temporaryIdProvider = temporaryIdProvider;
            _newOrderModelFactory = newOrderModelFactory;
            _orderModelFactory = orderModelFactory;
            _updateOrderModelFactory = updateOrderModelFactory;
            _logsModelFactory = logsModelFactory;
            _emailService = emailService;
            _userPermissionsChecker = userPermissionsChecker;
            _orderTypeService = orderTypeService;
            _customerService = customerService;
            _settingService = settingService;
            _documentService = documentService;
            _organizationService = organizationService;
			_organizationJsonService = organizationJsonService;

            _filesStateStore = editorStateCacheFactory.CreateForModule(ModuleName.Orders);
            _filesStore = temporaryFilesCacheFactory.CreateForModule(ModuleName.Orders);
			_globalSettingService = globalSettingService;

		}

        [HttpGet]
        public ActionResult Index()
        {
            int[] selectedStatuses;
            var data = _ordersService.GetOrdersFilterData(_workContext.Customer.CustomerId, _workContext.User.UserId, out selectedStatuses);

            var filters = SessionFacade.FindPageFilters<OrdersFilterModel>(PageName.OrdersOrders);
            if (filters == null)
            {
                filters = OrdersFilterModel.CreateDefault(selectedStatuses);
                SessionFacade.SavePageFilters(PageName.OrdersOrders, filters);
            }

            var filledFilters = new OrdersFilterModel(filters.OrderTypeId, filters.AdministratiorIds, filters.StartDate, filters.EndDate, filters.StatusIds, filters.Text, filters.RecordsOnPage, filters.SortField);

            var model = _ordersModelFactory.GetIndexModel(data, filledFilters);
            return View(model);
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
                                    _workContext.Customer.CustomerId,
                                    filters.OrderTypeId,
                                    filters.AdministratiorIds,
                                    filters.StartDate,
                                    filters.EndDate,
                                    filters.StatusIds,
                                    filters.Text,
                                    filters.RecordsOnPage,
                                    filters.SortField,
                                    null);

            var response = _ordersService.Search(parameters, _workContext.User.UserId);
            var ordersModel = _ordersModelFactory.Create(response, filters.SortField, filters.OrderTypeId == null);


            return PartialView(ordersModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult CreateOrder(int orderTypeForCteateOrderId)
        {
            var lowestchildordertypeid = orderTypeForCteateOrderId;
            //check if ordertype has a parent
            var orderType = _orderTypeService.GetOrderType(orderTypeForCteateOrderId);
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

            var data = _ordersService.GetNewOrderEditData(_workContext.Customer.CustomerId, orderTypeForCteateOrderId, lowestchildordertypeid, false);
            var temporaryId = _temporaryIdProvider.ProvideTemporaryId();

            var model = 
                _newOrderModelFactory.Create(temporaryId, data, _workContext, orderTypeForCteateOrderId);

            model.OrderTypeId = lowestchildordertypeid;

            return View("New", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(FullOrderEditModel model)
        {
            var currentCustomer = _customerService.GetCustomer(model.CustomerId);
            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);

            int intId;
            int.TryParse(model.Id, out intId);
            model.NewFiles = _filesStore.FindFiles(model.Id, Subtopic.FileName.ToString());
            model.DeletedFiles = _filesStateStore.FindDeletedFileNames(intId, Subtopic.FileName.ToString());

            var request = _updateOrderModelFactory.Create(
                                                model,
                                                _workContext.Customer.CustomerId,
                                                DateTime.Now,
                                                _emailService,
                                                _workContext.User.UserId,
                                                SessionFacade.CurrentLanguageId);

            var caseMailSetting = new CaseMailSetting(
                                                         currentCustomer.NewCaseEmailList,
                                                         currentCustomer.HelpdeskEmail,
                                                         RequestExtension.GetAbsoluteUrl(),
                                                         cs.DontConnectUserToWorkingGroup
                                                       );
            var id = _ordersService.AddOrUpdate(request, SessionFacade.CurrentUser.UserId, caseMailSetting, SessionFacade.CurrentLanguageId, false);

            foreach (var newFile in model.NewFiles)
            {
                _filesStore.AddFile(newFile.Content, newFile.Name, id, Subtopic.FileName.ToString());
            }

            _filesStore.ResetCacheForObject(model.Id);

            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int id, bool retToCase = false)
        {
            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            var response = _ordersService.FindOrder(id, _workContext.Customer.CustomerId, false);
            if (response == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var filesInDb = response.EditData.Order.Other.FileName != null ? new List<string> { response.EditData.Order.Other.FileName } : new List<string>();
            var filesOnDisc = _filesStore.FindFiles(id, Subtopic.FileName.ToString()).Select(f => f.Name).ToArray();
            foreach (var fileOnDisc in filesOnDisc)
            {
                if (!filesInDb.Contains(fileOnDisc))
                {
                    _filesStore.DeleteFile(fileOnDisc, id, Subtopic.FileName.ToString());
                }
            }

            _filesStateStore.ClearObjectDeletedFiles(id);

            var userHasAdminOrderPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.AdministerOrderPermission);
            var cs = _settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var userHasCreateWorkstationPermission = cs.CreateComputerFromOrder.ToBool() &&
                                                     cs.ModuleInventory.ToBool() &&
                                                     _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);

            var model = _orderModelFactory.Create(response, _workContext.Customer.CustomerId, _globalSettingService);
            model.UserHasAdminOrderPermission = userHasAdminOrderPermission;
            model.IsReturnToCase = retToCase;
            model.UserHasCreateWorkstationPermission = userHasCreateWorkstationPermission;


            return View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(FullOrderEditModel model)
        {
            var currentCustomer = _customerService.GetCustomer(model.CustomerId);
            var cs = _settingService.GetCustomerSetting(currentCustomer.Id);

            var id = int.Parse(model.Id);
            var filesInDb = model.Other?.FileName?.Value != null ? model.Other.FileName.Value.Files : new List<string>();
            var deletedFiles = _filesStateStore.FindDeletedFileNames(id, Subtopic.FileName.ToString()) ?? new List<string>();
            model.NewFiles = _filesStore.FindFiles(model.Id, Subtopic.FileName.ToString()).Where(f => !filesInDb.Contains(f.Name) && !deletedFiles.Contains(f.Name)).ToList();
            model.DeletedFiles = deletedFiles;

            model.DeletedLogIds = _filesStateStore.GetDeletedItemIds(id, OrderDeletedItem.Logs);

            var request = _updateOrderModelFactory.Create(
                                                model,
                                                _workContext.Customer.CustomerId,
                                                DateTime.Now,
                                                _emailService,
                                                _workContext.User.UserId,
                                                SessionFacade.CurrentLanguageId);

            var caseMailSetting = new CaseMailSetting(
                                                        currentCustomer.NewCaseEmailList,
                                                        currentCustomer.HelpdeskEmail,
                                                        RequestExtension.GetAbsoluteUrl(),
                                                        cs.DontConnectUserToWorkingGroup
                                                    );

            _ordersService.AddOrUpdate(request, SessionFacade.CurrentUser.UserId, caseMailSetting, SessionFacade.CurrentLanguageId, false);

            foreach (var deletedFile in model.DeletedFiles)
            {
                _filesStore.DeleteFile(deletedFile, model.Id, Subtopic.FileName.ToString());
            }

            _filesStateStore.ClearObjectDeletedFiles(id);

            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            _ordersService.Delete(id);
            _filesStore.ResetCacheForObject(id);
            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult AttachedFiles(string entityId, Subtopic subtopic)
        {
            List<string> fileNames;

            if (GuidHelper.IsGuid(entityId))
            {
                fileNames = _filesStore.FindFileNames(entityId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(entityId);
                var savedFiles = _filesStore.FindFileNames(id, subtopic.ToString());
                var deletedFiles = _filesStateStore.FindDeletedFileNames(id, subtopic.ToString());

                fileNames = new List<string>();
                fileNames.AddRange(savedFiles.Where(f => !deletedFiles.Contains(f)));
            }

            var model = new AttachedFilesModel(entityId, subtopic, fileNames);
            return PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string entityId, Subtopic subtopic, string name)
        {
            var uploadedFile = Request.Files[0];
            if (uploadedFile == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

			var extension = Path.GetExtension(name);
			if (!_globalSettingService.IsExtensionInWhitelist(extension))
			{
				throw new ArgumentException($"File extension not valid: {name}");
			}

            var fileContent = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(fileContent, 0, fileContent.Length);

            if (_filesStore.FileExists(name, entityId, subtopic.ToString()))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            _filesStore.AddFile(fileContent, name, entityId, subtopic.ToString());

            return RedirectToAction("AttachedFiles", new { entityId, subtopic });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string entityId, Subtopic subtopic, string fileName)
        {
            if (!_filesStore.FileExists(fileName, entityId, subtopic.ToString()))
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var fileContent = _filesStore.GetFileContent(fileName, entityId, subtopic.ToString());

            return File(fileContent, MimeType.BinaryFile, fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string entityId, Subtopic subtopic, string fileName)
        {
            if (GuidHelper.IsGuid(entityId))
            {
                _filesStore.DeleteFile(fileName, entityId, subtopic.ToString());
            }
            else
            {
                var id = int.Parse(entityId);
                _filesStateStore.AddDeletedFile(fileName, id, subtopic.ToString());
            }

            return RedirectToAction("AttachedFiles", new { entityId, subtopic });
        }

        [HttpGet]
        public PartialViewResult Logs(int orderId, Subtopic subtopic)
        {
            var deletedLogIds = _filesStateStore.GetDeletedItemIds(orderId, OrderDeletedItem.Logs);
            var logs = _ordersService.FindLogsExcludeSpecified(orderId, deletedLogIds);

            var response = _ordersService.FindOrder(orderId, _workContext.Customer.CustomerId, false);

            var model = _logsModelFactory.Create(orderId, subtopic, logs, response.EditOptions);

            return PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteLog(int orderId, Subtopic subtopic, int logId)
        {
            _filesStateStore.AddDeletedItem(logId, OrderDeletedItem.Logs, orderId);
            return RedirectToAction("Logs", new { orderId, subtopic });
        }

        [HttpGet]
        public FileContentResult DownloadDocument(int documentId)
        {
            var file = _documentService.GetDocumentFile(documentId);
            if (file == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }
            return File(file.File, MimeType.BinaryFile, file.FileName);
        }

        [HttpGet]
        public JsonResult SearchDepartmentsByRegionId(int? regionId)
        {
            var models = _organizationService.GetDepartments(
                SessionFacade.CurrentCustomer.Id,
                regionId);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileContentResult ExportOrder(int id)
        {
            var response = _ordersService.FindOrder(id, _workContext.Customer.CustomerId, false);
            if (response == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }
            var model = _orderModelFactory.Create(response, _workContext.Customer.CustomerId, _globalSettingService);
            var helper = new OrderExportHelper();
            var fileContent = helper.GetOrderExportText(model);
            var fileName = string.Format("O-{0}_{1}.txt", model.General.OrderNumber.Value, DateTime.Now.ToShortDateString());
            return File(fileContent, MimeType.BinaryFile, fileName);
        }

		[HttpGet]
		public JsonResult GetUnits(int? departmentId)
		{
			var ous = _organizationJsonService.GetActiveOUForDepartmentAsIdName(departmentId, _workContext.Customer.CustomerId)
				.ToList();
			return Json(ous, JsonRequestBehavior.AllowGet);
		}
    }
}
