// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintController.cs" company="">
//   
// </copyright>
// <summary>
//   The print controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Controllers
{
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.Print;
    using DH.Helpdesk.Web.Models.Print.Case;

    /// <summary>
    /// The print controller.
    /// </summary>
    public class PrintController : BaseController
    {
        /// <summary>
        /// The case service.
        /// </summary>
        private readonly ICaseService caseService;

        /// <summary>
        /// The case field setting service.
        /// </summary>
        private readonly ICaseFieldSettingService caseFieldSettingService;

        /// <summary>
        /// The service.
        /// </summary>
        private readonly IOUService ouService;

        /// <summary>
        /// The log service.
        /// </summary>
        private readonly ILogService logService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// The case type service.
        /// </summary>
        private readonly ICaseTypeService caseTypeService;

        /// <summary>
        /// The system service.
        /// </summary>
        private readonly ISystemService systemService;

        /// <summary>
        /// The impact service.
        /// </summary>
        private readonly IImpactService impactService;

        /// <summary>
        /// The supplier service.
        /// </summary>
        private readonly ISupplierService supplierService;

        /// <summary>
        /// The category service.
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// The product area service.
        /// </summary>
        private readonly IProductAreaService productAreaService;

        /// <summary>
        /// The case file service.
        /// </summary>
        private readonly ICaseFileService caseFileService;

        /// <summary>
        /// The status service.
        /// </summary>
        private readonly IStatusService statusService;

        /// <summary>
        /// The project service.
        /// </summary>
        private readonly IProjectService projectService;

        /// <summary>
        /// The problem service.
        /// </summary>
        private readonly IProblemService problemService;

        /// <summary>
        /// The change service.
        /// </summary>
        private readonly IChangeService changeService;

        /// <summary>
        /// The setting service.
        /// </summary>
        private readonly ISettingService settingService;

        /// <summary>
        /// The finishing cause service.
        /// </summary>
        private readonly IFinishingCauseService finishingCauseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintController"/> class.
        /// </summary>
        /// <param name="masterDataService">
        /// The master data service.
        /// </param>
        /// <param name="caseService">
        /// The case service.
        /// </param>
        /// <param name="caseFieldSettingService">
        /// The case field setting service.
        /// </param>
        /// <param name="ouService">
        /// The ou service.
        /// </param>
        /// <param name="logService">
        /// The log service.
        /// </param>
        /// <param name="userService">
        /// The user service.
        /// </param>
        /// <param name="caseTypeService">
        /// The case type service.
        /// </param>
        /// <param name="systemService">
        /// The system service.
        /// </param>
        /// <param name="impactService">
        /// The impact service.
        /// </param>
        /// <param name="supplierService"></param>
        /// <param name="categoryService"></param>
        /// <param name="productAreaService"></param>
        /// <param name="caseFileService"></param>
        /// <param name="statusService"></param>
        /// <param name="projectService"></param>
        /// <param name="problemService"></param>
        /// <param name="changeService"></param>
        /// <param name="settingService"></param>
        /// <param name="finishingCauseService"></param>
        public PrintController(
            IMasterDataService masterDataService, 
            ICaseService caseService,
            ICaseFieldSettingService caseFieldSettingService,
            IOUService ouService,
            ILogService logService,
            IUserService userService,
            ICaseTypeService caseTypeService,
            ISystemService systemService,
            IImpactService impactService, 
            ISupplierService supplierService, 
            ICategoryService categoryService, 
            IProductAreaService productAreaService, 
            ICaseFileService caseFileService, 
            IStatusService statusService, 
            IProjectService projectService, 
            IProblemService problemService, 
            IChangeService changeService, 
            ISettingService settingService, 
            IFinishingCauseService finishingCauseService)
            : base(masterDataService)
        {
            this.caseService = caseService;
            this.caseFieldSettingService = caseFieldSettingService;
            this.ouService = ouService;
            this.logService = logService;
            this.userService = userService;
            this.caseTypeService = caseTypeService;
            this.systemService = systemService;
            this.impactService = impactService;
            this.supplierService = supplierService;
            this.categoryService = categoryService;
            this.productAreaService = productAreaService;
            this.caseFileService = caseFileService;
            this.statusService = statusService;
            this.projectService = projectService;
            this.problemService = problemService;
            this.changeService = changeService;
            this.settingService = settingService;
            this.finishingCauseService = finishingCauseService;
        }

        /// <summary>
        /// The case.
        /// </summary>
        /// <param name="caseId">
        /// The case id.
        /// </param>
        /// <param name="customerId">
        /// The customer Id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Case(int caseId, int customerId)
        {
            var caseModel = this.caseService.GetCaseOverview(caseId);
            if (caseModel == null)
            {
                return new HttpNotFoundResult();
            }

            var fields = this.caseFieldSettingService.GetCaseFieldSettings(customerId);
            
            var ous = this.ouService.GetOUs(customerId);
            var selectedOU = new Domain.OU();
            if (caseModel.OuId.HasValue)
            {
                selectedOU = this.ouService.GetOU(caseModel.OuId.Value);
                if (selectedOU.Parent_OU_Id != null)
                    selectedOU.Name = selectedOU.Parent.Name + " - " + selectedOU.Name;
            }
            caseModel.Ou = selectedOU; //ous.FirstOrDefault(o => caseModel.OuId == o.Id);
            caseModel.Logs = this.logService.GetCaseLogOverviews(caseId);

            if (caseModel.UserId.HasValue)
            {
                caseModel.User = this.userService.GetUserOverview(caseModel.UserId.Value);
            }

            var caseType = this.caseTypeService.GetCaseType(caseModel.CaseTypeId);
            if (caseType != null)
            {
                caseModel.ParentPathCaseType = caseType.getCaseTypeParentPath();
            }

            if (caseModel.SystemId.HasValue)
            {
                caseModel.System = this.systemService.GetSystemOverview(caseModel.SystemId.Value);
            }

            if (caseModel.ImpactId.HasValue)
            {
                caseModel.Impact = this.impactService.GetImpactOverview(caseModel.ImpactId.Value);                
            }

            if (caseModel.SupplierId.HasValue)
            {
                caseModel.Supplier = this.supplierService.GetSupplierOverview(caseModel.SupplierId.Value);
            }

            if (caseModel.CategoryId.HasValue)
            {
                caseModel.Category = this.categoryService.GetCategoryOverview(caseModel.CategoryId.Value);
            }

            if (caseModel.ProductAreaId.HasValue)
            {
                caseModel.ProductArea = this.productAreaService.GetProductAreaOverview(caseModel.ProductAreaId.Value);
            }

            if (caseModel.CaseResponsibleUserId.HasValue)
            {
                caseModel.CaseResponsibleUser = this.userService.GetUserOverview(caseModel.CaseResponsibleUserId.Value);
            }

            if (caseModel.PerformerUserId.HasValue)
            {
                caseModel.PerformerUser = this.userService.GetUserOverview(caseModel.PerformerUserId.Value);
            }

            if (caseModel.StatusId.HasValue)
            {
                caseModel.Status = this.statusService.GetStatusOverview(caseModel.StatusId.Value);
            }

            if (caseModel.ProjectId.HasValue)
            {
                caseModel.Project = this.projectService.GetProject(caseModel.ProjectId.Value);
            }

            if (caseModel.ProblemId.HasValue)
            {
                caseModel.Problem = this.problemService.GetProblem(caseModel.ProblemId.Value);
            }

            if (caseModel.ChangeId.HasValue)
            {
                caseModel.Change = this.changeService.GetChangeOverview(caseModel.ChangeId.Value);
            }
            var files = caseFileService.FindFileNamesByCaseId(caseId).Select(x => new LogFileModel
            {
                Name = x
            }).ToList();
            var model = new CasePrintModel()
                        {
                            CustomerId = customerId,
                            Case = caseModel,
                            CaseFilesModel = new FilesModel(caseId.ToString(CultureInfo.InvariantCulture), files, false),
                            CaseFieldSettings = fields,
                            CaseLog = this.logService.InitCaseLog(SessionFacade.CurrentUser.Id, string.Empty),
                            FinishingCauses = this.finishingCauseService.GetFinishingCauses(customerId),
                            IsDepartmentVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Department_Id),
                            IsPersonsCellPhoneVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Persons_CellPhone),
                            IsPersonsEmailVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Persons_EMail),
                            IsPersonsNameVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Persons_Name),
                            IsPersonsPhoneVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Persons_Phone),
                            IsRegionVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Region_Id),
                            IsReportedByVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.ReportedBy),
                            IsOuVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.OU_Id),
                            IsPlaceVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Place),
                            IsUserCodeVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.UserCode),
                            IsInventoryNumberVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.InventoryNumber),
                            IsInventoryTypeVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.ComputerType_Id),
                            IsInventoryLocationVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.InventoryLocation),
                            IsCaseNumberVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.CaseNumber),
                            IsUserVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.User_Id),
                            IsCaseTypeVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.CaseType_Id),
                            IsSystemVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.System_Id),
                            IsImpactVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Impact_Id),
                            IsCategoryVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Category_Id),
                            IsSupplierVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Supplier_Id),
                            IsInvoiceNumberVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.InvoiceNumber),
                            IsReferenceNumberVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.ReferenceNumber),
                            IsCaptionVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Caption),
                            IsDescriptionVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Description),
                            IsMiscellaneousVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Miscellaneous),
                            IsProductAreaVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.ProductArea_Id),
                            IsContactBeforeActionVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.ContactBeforeAction),
                            IsSmsVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.SMS),
                            IsAgreedDateVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.AgreedDate),
                            IsAvailableVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Available),
                            IsCostVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Cost),
                            IsFilesVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Filename),
                            IsWorkingGroupVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.WorkingGroup_Id),
                            IsCaseResponsibleUserVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id),
                            IsPerformerUserVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Performer_User_Id),
                            IsPriorityVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Priority_Id),
                            IsStatusVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Status_Id),
                            IsStateSecondaryVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.StateSecondary_Id),
                            IsWatchDateVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.WatchDate),
                            IsVerifiedVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.Verified),
                            IsVerifiedDescriptionVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.VerifiedDescription),
                            IsSolutionRateVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.SolutionRate),
                            IsLogTextExternalVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.tblLog_Text_External),
                            IsLogTextInternalVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal),
                            IsFinishingDescriptionVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.FinishingDescription),
                            IsFinishingDateVisible = fields.IsFieldVisible(GlobalEnums.TranslationCaseFields.FinishingDate),
                        };

            var customerSettings = this.settingService.GetCustomerSetting(customerId);
            if (customerSettings != null)
            {
                model.DepartmentFilterFormat = customerSettings.DepartmentFilterFormat;
            }

            return new PrintPdfResult(model, "CaseHtml");
        }
    }
}
