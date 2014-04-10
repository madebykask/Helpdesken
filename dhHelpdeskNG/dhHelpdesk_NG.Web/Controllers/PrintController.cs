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
        /// <param name="ouService"></param>
        public PrintController(
            IMasterDataService masterDataService, 
            ICaseService caseService,
            ICaseFieldSettingService caseFieldSettingService,
            IOUService ouService)
            : base(masterDataService)
        {
            this.caseService = caseService;
            this.caseFieldSettingService = caseFieldSettingService;
            this.ouService = ouService;
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
                        };

            return new RazorPDF.PdfResult(model, "Case");
        }
    }
}
