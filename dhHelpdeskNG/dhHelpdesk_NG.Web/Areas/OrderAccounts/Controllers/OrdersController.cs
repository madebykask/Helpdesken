using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Areas.OrderAccounts.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Accounts.Read.Overview;
    using DH.Helpdesk.BusinessData.Models.Accounts.Write;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Services.Response.Account;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.BusinessModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Infrastructure.ModelMappers;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order;
    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public class OrdersController : UserInteractionController
    {
        private const string FilterName = "OrderAccountFilter";

        private readonly IOrderAccountProxyService orderAccountProxyService;

        private readonly IOrderAccountSettingsProxyService orderAccountSettingsProxyService;

        private readonly IUserService userService;

        private readonly IOrganizationService organizationService;

        private readonly IOrderModelMapper orderModelMapper;

        private readonly IAccountDtoMapper accountDtoMapper;

        private readonly ITemporaryFilesCache userTemporaryFilesStorage;

        private readonly IEditorStateCache userEditorValuesStorage;

        public OrdersController(
            IMasterDataService masterDataService,
            IOrderAccountProxyService orderAccountProxyService,
            IOrderAccountSettingsProxyService orderAccountSettingsProxyService,
            IUserService userService,
            IOrganizationService organizationService,
            IOrderModelMapper orderModelMapper,
            IAccountDtoMapper accountDtoMapper,
            IEditorStateCacheFactory userEditorValuesStorageFactory,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory)
            : base(masterDataService)
        {
            this.orderAccountProxyService = orderAccountProxyService;
            this.orderAccountSettingsProxyService = orderAccountSettingsProxyService;
            this.userService = userService;
            this.organizationService = organizationService;
            this.orderModelMapper = orderModelMapper;
            this.accountDtoMapper = accountDtoMapper;

            this.userEditorValuesStorage = userEditorValuesStorageFactory.CreateForModule(ModuleName.OrderAccounts);
            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.OrderAccounts);
        }

        public ViewResult Index(int? activityType)
        {
            var currentFilter = SessionFacade.FindPageFilters<Models.Order.Filter>(FilterName)
                                ?? Models.Order.Filter.CreateDefault();

            List<ItemOverview> activities = this.orderAccountProxyService.GetAccountActivities();
            List<ItemOverview> users = this.userService.FindActiveOverviews(OperationContext.CustomerId);

            OrdersIndexModel viewModel = OrdersIndexModel.BuildViewModel(activityType, activities, users, currentFilter);

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Grid(Models.Order.Filter filter, int? activityType)
        {
            SessionFacade.SavePageFilters(FilterName, filter);

            List<AccountOverview> models = this.orderAccountProxyService.GetOverviews(
                filter.CreateRequest(activityType),
                OperationContext);

            var settings = new List<AccountFieldsSettingsOverviewWithActivity>();

            if (activityType.HasValue)
            {
                AccountFieldsSettingsOverview setting =
                    this.orderAccountSettingsProxyService.GetFieldsSettingsOverview(
                        activityType.Value,
                        this.OperationContext);
                var settingOverview = new AccountFieldsSettingsOverviewWithActivity(activityType.Value, setting);

                settings.Add(settingOverview);
            }
            else
            {
                settings = this.orderAccountSettingsProxyService.GetFieldsSettingsOverviews(this.OperationContext);
            }

            List<GridModel> gridModels = GridModel.BuildGrid(models, settings, filter.SortField);

            return this.PartialView("Grids", gridModels);
        }

        [HttpGet]
        public ViewResult Edit(int id, int accountActivityId, int caseId)
        {
            this.userTemporaryFilesStorage.ResetCacheForObject(id);
            this.userEditorValuesStorage.ClearObjectDeletedFiles(id);

            AccountForEdit model = this.orderAccountProxyService.Get(id);
            AccountFieldsSettingsForModelEdit settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForModelEdit(accountActivityId, OperationContext);
            AccountOptionsResponse options = this.orderAccountProxyService.GetOptions(accountActivityId, OperationContext);
            HeadersFieldSettings headers = this.orderAccountSettingsProxyService.GetHeadersFieldSettings(accountActivityId);

            AccountModel viewModel = this.orderModelMapper.BuildViewModel(model, options, settings, headers);
            viewModel.CaseId = caseId;

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(AccountModel model, string clickedButton)
        {
            WebTemporaryFile file = null;
            List<string> deletedRegistrationFiles = this.userEditorValuesStorage.FindDeletedFileNames(model.Id);
            List<WebTemporaryFile> newRegistrationFiles = this.userTemporaryFilesStorage.FindFiles(model.Id);

            if (!deletedRegistrationFiles.Any())
            {
                if (newRegistrationFiles.Any())
                {
                    file = newRegistrationFiles.SingleOrDefault();
                }
            }

            this.userTemporaryFilesStorage.ResetCacheForObject(model.Id);
            this.userEditorValuesStorage.ClearObjectDeletedFiles(model.Id);

            AccountForUpdate dto = this.accountDtoMapper.BuidForUpdate(model, file, OperationContext);
            this.orderAccountProxyService.Update(dto, this.OperationContext);

            if (clickedButton == ClickedButton.Save)
            {
                return this.RedirectToAction("Edit", new { dto.Id, activityType = dto.ActivityId });
            }

            return this.RedirectToAction("Index", new { activityType = dto.ActivityId });
        }

        [HttpPost]
        public RedirectToRouteResult RedirectToNew(int activityTypeForEdit)
        {
            return this.RedirectToAction("New", new { activityTypeForEdit });
        }

        [HttpGet]
        public ViewResult New(int activityTypeForEdit)
        {
            AccountFieldsSettingsForModelEdit settings =
                this.orderAccountSettingsProxyService.GetFieldsSettingsForModelEdit(
                    activityTypeForEdit,
                    OperationContext);
            AccountOptionsResponse options = this.orderAccountProxyService.GetOptions(
                activityTypeForEdit,
                OperationContext);
            IdAndNameOverview activity =
                this.orderAccountProxyService.GetAccountActivityItemOverview(activityTypeForEdit);

            HeadersFieldSettings headers =
                this.orderAccountSettingsProxyService.GetHeadersFieldSettings(activityTypeForEdit);

            AccountModel viewModel = this.orderModelMapper.BuildViewModel(
                activityTypeForEdit,
                options,
                settings,
                SessionFacade.CurrentUser,
                headers);
            viewModel.ActivityName = activity.Name;

            return this.View("New", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(AccountModel model, string clickedButton)
        {
            string guid = model.Guid;

            WebTemporaryFile registrationFile = this.userTemporaryFilesStorage.FindFiles(guid).SingleOrDefault();

            AccountForInsert dto = this.accountDtoMapper.BuidForInsert(model, registrationFile, OperationContext);
            int id = this.orderAccountProxyService.Add(dto, this.OperationContext);

            if (clickedButton == ClickedButton.Save)
            {
                return this.RedirectToAction("Edit", new { id, activityType = dto.ActivityId });
            }

            this.userTemporaryFilesStorage.ResetCacheForObject(guid);

            return this.RedirectToAction("Index", new { activityType = dto.ActivityId });
        }

        [HttpGet]
        public RedirectToRouteResult Delete(int id)
        {
            this.orderAccountProxyService.Delete(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult SearchDepartmentsByRegionId(int? selected)
        {
            List<ItemOverview> models = this.organizationService.GetDepartments(
                SessionFacade.CurrentCustomer.Id,
                selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AttachedFile(string orderId)
        {
            string fileName;

            if (GuidHelper.IsGuid(orderId))
            {
                fileName = this.userTemporaryFilesStorage.FindFileNames(orderId).SingleOrDefault();
            }
            else
            {
                string savedFile = this.userTemporaryFilesStorage.FindFileNames(orderId).SingleOrDefault();

                if (!string.IsNullOrWhiteSpace(savedFile))
                {
                    fileName = savedFile;
                }
                else
                {
                    List<string> deletedFileNames = this.userEditorValuesStorage.FindDeletedFileNames(
                        int.Parse(orderId));
                    fileName = deletedFileNames.Any()
                                   ? null
                                   : this.orderAccountProxyService.GetFileName(int.Parse(orderId));
                }
            }

            var model = new FilesModel(orderId, fileName);
            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult UploadFile(string orderId, string name)
        {
            var uploadedFile = this.Request.Files[0];

            if (uploadedFile == null)
            {
                throw new HttpException((int)HttpStatusCode.NoContent, null);
            }

            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (this.userTemporaryFilesStorage.FileExists(name, orderId))
            {
                throw new HttpException((int)HttpStatusCode.Conflict, null);
            }

            if (this.userTemporaryFilesStorage.FindFileNames(orderId).Any())
            {
                this.userTemporaryFilesStorage.DeleteFile(name, orderId);
            }

            this.userTemporaryFilesStorage.AddFile(uploadedData, name, orderId);

            return this.RedirectToAction("AttachedFile", new { orderId });
        }

        [HttpGet]
        public FileContentResult DownloadFile(string orderId, string fileName)
        {
            byte[] fileContent;

            if (GuidHelper.IsGuid(orderId))
            {
                fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, orderId);
            }
            else
            {
                var fileInWebStorage = this.userTemporaryFilesStorage.FileExists(fileName, orderId);

                fileContent = fileInWebStorage
                                  ? this.userTemporaryFilesStorage.GetFileContent(fileName, orderId)
                                  : this.orderAccountProxyService.GetFileContent(int.Parse(orderId));
            }

            return this.File(fileContent, MimeType.BinaryFile, fileName);
        }

        [HttpPost]
        public RedirectToRouteResult DeleteFile(string orderId, string fileName)
        {
            if (GuidHelper.IsGuid(orderId))
            {
                this.userTemporaryFilesStorage.DeleteFile(fileName, orderId);
            }
            else
            {
                if (this.userTemporaryFilesStorage.FileExists(fileName, orderId))
                {
                    this.userTemporaryFilesStorage.DeleteFile(fileName, orderId);
                }
                else
                {
                    this.userEditorValuesStorage.AddDeletedFile(fileName, int.Parse(orderId));
                }
            }

            return this.RedirectToAction("AttachedFile", new { orderId });
        }
    }
}