namespace DH.Helpdesk.Web.Infrastructure.CaseOverview
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Grid;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.Enums.Case.Fields;

    public class CaseOverviewGridSettingsService
    {
        private readonly ICaseSettingsService caseSettingService;

        private readonly ICaseFieldSettingService caseFieldSettingService;

        private readonly GridSettingsService gridSettingsService;

        public CaseOverviewGridSettingsService(ICaseSettingsService caseSettingService, ICaseFieldSettingService caseFieldSettingService, GridSettingsService gridSettingsService)
        {
            this.caseSettingService = caseSettingService;
            this.caseFieldSettingService = caseFieldSettingService;
            this.gridSettingsService = gridSettingsService;
        }
        
        public const string DEFAULT_FONT_STYLE = "normal";

        /// <summary>
        /// Content of this method was taken from CaseOverviewController
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="gridId"></param>
        /// <param name="customerCaseFieldSettings"></param>
        /// <returns></returns>
        public CaseColumnsSettingsModel GetSettings(int customerId, int userGroupId, int userId,
            int gridId = GridSettingsService.CASE_OVERVIEW_GRID_ID,
            IList<CaseFieldSetting> customerCaseFieldSettings = null)
        {
            // this is done for performance optimisation
            if (customerCaseFieldSettings == null)
            {
               customerCaseFieldSettings = this.caseFieldSettingService.GetCaseFieldSettings(customerId);
            }

            var gridSettings = this.gridSettingsService.GetForCustomerUserGrid(customerId, userGroupId, userId, gridId);
            var exceptedList = new string[]
                { 
                    UserFields.IsAbout_CostCentre.ToLower(), 
                    UserFields.IsAbout_Department.ToLower(),
                    UserFields.IsAbout_OU.ToLower(),
                    UserFields.IsAbout_Persons_CellPhone.ToLower(),
                    UserFields.IsAbout_Persons_Email.ToLower(),
                    UserFields.IsAbout_Persons_Phone.ToLower(),
                    UserFields.IsAbout_Place.ToLower(),
                    UserFields.IsAbout_Region.ToLower(),
                    UserFields.IsAbout_UserCode.ToLower(),
                    LogFields.InternalLogNote.ToLower(),
                    LogFields.ExternalLogNote.ToLower()
                };


            var avaialbleColumnds = 
                this.caseSettingService.GetAvailableCaseOverviewGridColumnSettings(customerId, customerCaseFieldSettings)
                                       .Where(c => !exceptedList.Contains(c.Name.ToLower()))
                                       .OrderBy(it => Translation.Get(it.Name, Enums.TranslationSource.CaseTranslation, customerId));

            var selectedColumns = this.caseSettingService.GetSelectedCaseOverviewGridColumnSettings(customerId, userId, customerCaseFieldSettings);

            var colSettingModel = new CaseColumnsSettingsModel
            {
                CustomerId = customerId,
                SelectedFontStyle = gridSettings.cls,
                SelectedPageSize = gridSettings.pageOptions.recPerPage,
                UserId = userId,
                AvailableColumns = avaialbleColumnds,
                SelectedColumns = selectedColumns
            };

            colSettingModel.CaseFieldSettings = customerCaseFieldSettings;
            colSettingModel.LineList = new[]
            {
                new SelectListItem
                {
                    Text = Translation.Get("Info", Enums.TranslationSource.TextTranslation),
                    Value = "1",
                    Selected = false
                }
            };
             
            return colSettingModel;
        }
    }
}