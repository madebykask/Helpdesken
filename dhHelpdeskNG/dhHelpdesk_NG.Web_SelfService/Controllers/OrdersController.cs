using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Enums.Orders;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Orders.Index;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Orders;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders;
using DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders.Concrete;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Extensions;
using DH.Helpdesk.SelfService.Infrastructure.Tools;
using DH.Helpdesk.SelfService.Models.Orders;
using DH.Helpdesk.SelfService.Models.Orders.FieldModels;
using DH.Helpdesk.SelfService.Models.Orders.OrderEdit;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
using DH.Helpdesk.Services.BusinessLogic.OtherTools.Concrete;
using DH.Helpdesk.Services.Services.Concrete;
using static DH.Helpdesk.SelfService.Infrastructure.Enums;

namespace DH.Helpdesk.SelfService.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService _ordersService;
        private readonly IOrdersModelFactory _ordersModelFactory;
        private readonly IEditorStateCache _filesStateStore;
        private readonly ITemporaryFilesCache _filesStore;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrganizationService _organizationService;
        private readonly ICustomerService _customerService;
        private readonly ISettingService _settingService;
        private readonly IUpdateOrderModelFactory _updateOrderModelFactory;
        private readonly INewOrderModelFactory _newOrderModelFactory;
        private readonly IEmailService _emailService;
        private readonly TemporaryIdProvider _temporaryIdProvider;
        private readonly IOrderTypeService _orderTypeService;
        private readonly IDocumentService _documentService;

        public OrdersController(IMasterDataService masterDataService,
            ISelfServiceConfigurationService configurationService,
            ICaseSolutionService caseSolutionService,
            IOrdersService ordersService,
            IOrdersModelFactory ordersModelFactory,
            IEditorStateCacheFactory editorStateCacheFactory,
            ITemporaryFilesCacheFactory temporaryFilesCacheFactory,
            IUserPermissionsChecker userPermissionsChecker,
            IOrderModelFactory orderModelFactory,
            IOrganizationService organizationService,
            ICustomerService customerService,
            ISettingService settingService,
            IUpdateOrderModelFactory updateOrderModelFactory,
            INewOrderModelFactory newOrderModelFactory,
            IEmailService emailService,
            TemporaryIdProvider temporaryIdProvider,
            IOrderTypeService orderTypeService,
            IDocumentService documentService)
            : base(configurationService, masterDataService, caseSolutionService)
        {
            _ordersService = ordersService;
            _ordersModelFactory = ordersModelFactory;
            _userPermissionsChecker = userPermissionsChecker;
            _orderModelFactory = orderModelFactory;
            _organizationService = organizationService;
            _customerService = customerService;
            _settingService = settingService;
            _updateOrderModelFactory = updateOrderModelFactory;
            _newOrderModelFactory = newOrderModelFactory;
            _emailService = emailService;
            _temporaryIdProvider = temporaryIdProvider;
            _orderTypeService = orderTypeService;
            _documentService = documentService;

            _filesStateStore = editorStateCacheFactory.CreateForModule(ModuleName.Orders);
            _filesStore = temporaryFilesCacheFactory.CreateForModule(ModuleName.Orders);
        }

        public ActionResult Index(int customerId)
        {
            int[] selectedStatuses;
            var data = _ordersService.GetOrdersFilterData(SessionFacade.CurrentCustomerID, SessionFacade.CurrentLocalUser.Id, out selectedStatuses);

            var filters = SessionFacade.FindPageFilters<OrdersFilterModel>(PageName.Orders);
            if (filters == null)
            {
                filters = OrdersFilterModel.CreateDefault(selectedStatuses);
                SessionFacade.SavePageFilters(PageName.Orders, filters);
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
                        : SessionFacade.FindPageFilters<OrdersFilterModel>(PageName.Orders);

            SessionFacade.SavePageFilters(PageName.Orders, filters);

            var parameters = new SearchParameters(
                                    SessionFacade.CurrentCustomerID,
                                    filters.OrderTypeId,
                                    filters.AdministratiorIds,
                                    filters.StartDate,
                                    filters.EndDate,
                                    filters.StatusIds,
                                    filters.Text,
                                    filters.RecordsOnPage,
                                    filters.SortField,
                                    SessionFacade.CurrentLocalUser.Id);

            var response = _ordersService.Search(parameters, SessionFacade.CurrentLocalUser.Id, true);
            var ordersModel = _ordersModelFactory.Create(response, filters.SortField, filters.OrderTypeId == null);

            return PartialView(ordersModel);
        }

        [HttpGet]
        public JsonResult SearchDepartmentsByRegionId(int? id)
        {
            var models = _organizationService.GetDepartments(SessionFacade.CurrentCustomerID,id);
            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ActionResult CreateOrder(int orderTypeForCreateOrderId, int customerId)
        {
            var lowestchildordertypeid = orderTypeForCreateOrderId;
            //check if ordertype has a parent
            var orderType = _orderTypeService.GetOrderType(orderTypeForCreateOrderId);
            if (orderType.Parent_OrderType_Id.HasValue)
            {
                if (orderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                {
                    if (orderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                    {
                        if (orderType.ParentOrderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.HasValue)
                        {
                            orderTypeForCreateOrderId = orderType.ParentOrderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.Value;
                        }
                        else
                        {
                            orderTypeForCreateOrderId = orderType.ParentOrderType.ParentOrderType.Parent_OrderType_Id.Value;
                        }
                    }
                    else
                    {
                        orderTypeForCreateOrderId = orderType.ParentOrderType.Parent_OrderType_Id.Value;
                    }
                }
                else
                {
                    orderTypeForCreateOrderId = orderType.Parent_OrderType_Id.Value;
                }
            }
            else
            {
                orderTypeForCreateOrderId = orderType.Id;
            }

            var data = _ordersService.GetNewOrderEditData(SessionFacade.CurrentCustomerID, orderTypeForCreateOrderId, lowestchildordertypeid, true);
            var temporaryId = _temporaryIdProvider.ProvideTemporaryId();
            var settings = _settingService.GetCustomerSettings(SessionFacade.CurrentCustomerID);
            var userData = new OrderUserData
            {
                Login = SessionFacade.CurrentLocalUser.UserId,
                FirstName = SessionFacade.CurrentLocalUser.FirstName,
                LastName = SessionFacade.CurrentLocalUser.SurName,
                Phone = SessionFacade.CurrentLocalUser.Phone,
                Email = SessionFacade.CurrentLocalUser.Email
            };
            var model = _newOrderModelFactory.Create(temporaryId,
                                                data,
                                                userData,
                                                settings.CreateCaseFromOrder,
                                                SessionFacade.CurrentCustomerID,
                                                orderTypeForCreateOrderId);

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

            var request = _updateOrderModelFactory.Create(model,
                                                SessionFacade.CurrentCustomerID,
                                                DateTime.Now,
                                                _emailService,
                                                SessionFacade.CurrentLocalUser.Id,
                                                SessionFacade.CurrentLanguageId);

            var caseMailSetting = new CaseMailSetting(currentCustomer.NewCaseEmailList,
                                                         currentCustomer.HelpdeskEmail,
                                                         RequestExtension.GetAbsoluteUrl(),
                                                         cs.DontConnectUserToWorkingGroup
                                                       );
            var id = _ordersService.AddOrUpdate(request, SessionFacade.CurrentLocalUser.UserId, caseMailSetting, SessionFacade.CurrentLanguageId, true);

            foreach (var newFile in model.NewFiles)
            {
                _filesStore.AddFile(newFile.Content, newFile.Name, id, Subtopic.FileName.ToString());
            }

            _filesStore.ResetCacheForObject(model.Id);

            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return RedirectToAction("Index", new { customerId = model.CustomerId });
        }

        [HttpGet]
        public ActionResult Edit(int id, int customerId)
        {
            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            var response = _ordersService.FindOrder(id, SessionFacade.CurrentCustomerID, true);
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

            var userHasAdminOrderPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentLocalUser), UserPermission.AdministerOrderPermission);

            var model = _orderModelFactory.Create(response, SessionFacade.CurrentCustomerID);
            model.UserHasAdminOrderPermission = userHasAdminOrderPermission;

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
                                                SessionFacade.CurrentCustomerID,
                                                DateTime.Now,
                                                _emailService,
                                                SessionFacade.CurrentLocalUser.Id,
                                                SessionFacade.CurrentLanguageId);

            var caseMailSetting = new CaseMailSetting(currentCustomer.NewCaseEmailList,
                                                        currentCustomer.HelpdeskEmail,
                                                        RequestExtension.GetAbsoluteUrl(),
                                                        cs.DontConnectUserToWorkingGroup);

            _ordersService.AddOrUpdate(request, SessionFacade.CurrentLocalUser.UserId, caseMailSetting, SessionFacade.CurrentLanguageId, true);

            foreach (var deletedFile in model.DeletedFiles)
            {
                _filesStore.DeleteFile(deletedFile, model.Id, Subtopic.FileName.ToString());
            }

            _filesStateStore.ClearObjectDeletedFiles(id);

            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return RedirectToAction("Index", new { customerId = model.CustomerId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id, int customerId)
        {
            _ordersService.Delete(id);
            _filesStore.ResetCacheForObject(id);
            _filesStateStore.ClearObjectDeletedItems(id, OrderDeletedItem.Logs);

            return RedirectToAction("Index", new { customerId = customerId });
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
        public FileContentResult DownloadDocument(int documentId)
        {
            var file = _documentService.GetDocumentFile(documentId);
            if (file == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }
            return File(file.File, MimeType.BinaryFile, file.FileName);
        }
    }
}