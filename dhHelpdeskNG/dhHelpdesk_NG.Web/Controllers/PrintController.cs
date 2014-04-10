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
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
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
        /// The service.
        /// </param>
        /// <param name="logService">
        /// The log service.
        /// </param>
        /// <param name="userService">
        /// The user Service.
        /// </param>
        /// <param name="caseTypeService">
        /// The case Type Service.
        /// </param>
        public PrintController(
            IMasterDataService masterDataService, 
            ICaseService caseService,
            ICaseFieldSettingService caseFieldSettingService,
            IOUService ouService,
            ILogService logService,
            IUserService userService,
            ICaseTypeService caseTypeService,
            ISystemService systemService)
            : base(masterDataService)
        {
            this.caseService = caseService;
            this.caseFieldSettingService = caseFieldSettingService;
            this.ouService = ouService;
            this.logService = logService;
            this.userService = userService;
            this.caseTypeService = caseTypeService;
            this.systemService = systemService;
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
            caseModel.Ou = ous.FirstOrDefault(o => caseModel.OuId == o.Id);
            caseModel.Logs = this.logService.GetLogsByCaseId(caseId);
            caseModel.User = this.userService.GetUserOverview(caseModel.UserId);

            var caseType = this.caseTypeService.GetCaseType(caseModel.CaseTypeId);
            if (caseType != null)
            {
                caseModel.ParentPathCaseType = caseType.getCaseTypeParentPath();
            }

            if (caseModel.SystemId.HasValue)
            {
                caseModel.System = this.systemService.GetSystemOverview(caseModel.SystemId.Value);
            }

            var model = new CasePrintModel()
                        {
                            CustomerId = customerId,
                            Case = caseModel,
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
                        };

            return new RazorPDF.PdfResult(model, "Case");
        }
    }
}
