using DH.Helpdesk.Common.Enums.Cases;

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
        public const string DEFAULT_FONT_STYLE = "normal";

        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly GridSettingsService _gridSettingsService;

        public CaseOverviewGridSettingsService(ICaseSettingsService caseSettingService, ICaseFieldSettingService caseFieldSettingService, GridSettingsService gridSettingsService)
        {
            _caseSettingService = caseSettingService;
            _caseFieldSettingService = caseFieldSettingService;
            _gridSettingsService = gridSettingsService;
        }

        /// <summary>
        /// Content of this method was taken from CaseOverviewController
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userGroupId"></param>
        /// <param name="userId"></param>
        /// <param name="gridId"></param>
        /// <param name="customerCaseFieldSettings"></param>
        /// <returns></returns>
        public CaseColumnsSettingsModel GetSettings(int customerId, 
            int userGroupId, 
            int userId,
            int gridId = GridSettingsService.CASE_OVERVIEW_GRID_ID,
            IList<CaseFieldSetting> customerCaseFieldSettings = null)
        {
            // this is done for performance optimisation
            if (customerCaseFieldSettings == null)
            {
               customerCaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            }

            var gridSettings = 
                _gridSettingsService.GetForCustomerUserGrid(customerId, userGroupId, userId, gridId);

            var exceptedList = new []
            {
                UserFields.IsAbout_CostCentre.ToLower(),
                UserFields.IsAbout_Department.ToLower(),
                UserFields.IsAbout_OU.ToLower(),
                //UserFields.IsAbout_Persons_CellPhone.ToLower(),
                UserFields.IsAbout_Persons_Email.ToLower(),
                UserFields.IsAbout_Persons_Phone.ToLower(),
                UserFields.IsAbout_Place.ToLower(),
                UserFields.IsAbout_Region.ToLower(),
                UserFields.IsAbout_UserCode.ToLower(),
                LogFields.InternalLogNote.ToLower(),
                LogFields.ExternalLogNote.ToLower(),
                "tblProblem.ResponsibleUser_Id".ToLower()
            };

            var availableColumns = 
                _caseSettingService.GetAvailableCaseOverviewGridColumnSettings(customerId, customerCaseFieldSettings)
                    .Where(c => !exceptedList.Contains(c.Name.ToLower()))
                    .OrderBy(it => Translation.GetForCase(it.Name, customerId));

            var selectedColumns = 
                _caseSettingService.GetSelectedCaseOverviewGridColumnSettings(customerId, userId, CaseSettingTypes.CaseOverview, customerCaseFieldSettings);

            var colSettingModel = new CaseColumnsSettingsModel
            {
                CustomerId = customerId,
                SelectedFontStyle = gridSettings.cls,
                SelectedPageSize = gridSettings.pageOptions.recPerPage,
                UserId = userId,
                AvailableColumns = availableColumns,
                SelectedColumns = selectedColumns,
                CaseFieldSettings = customerCaseFieldSettings,
                LineList = new[] { CreateSelectListItem(Translation.GetCoreTextTranslation("Info"), "1") }
            };

            return colSettingModel;
        }

        private static SelectListItem CreateSelectListItem(string text, string value, bool selected = false)
        {
            return new SelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selected
            };
        }

    }
}