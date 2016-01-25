namespace DH.Helpdesk.Web.Infrastructure.CaseOverview
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Grid;
    using DH.Helpdesk.Web.Models.Case;

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
        /// <returns></returns>
        public CaseColumnsSettingsModel GetSettings(int customerId, int userGroupId, int userId)
        {
            var gridSettings = this.gridSettingsService.GetForCustomerUserGrid(customerId, userGroupId, userId, GridSettingsService.CASE_OVERVIEW_GRID_ID);
            var colSettingModel = new CaseColumnsSettingsModel
                                      {
                                          CustomerId = customerId,
                                          SelectedFontStyle = gridSettings.cls,
                                          UserId = userId,
                                          AvailableColumns =
                                              this.caseSettingService
                                              .GetAvailableCaseOverviewGridColumnSettings(customerId).Where(c=> !c.Name.ToLower().Contains("isabout_")).OrderBy(it => Translation.Get(it.Name, Enums.TranslationSource.CaseTranslation, customerId)),
                                          SelectedColumns =
                                              this.caseSettingService
                                              .GetSelectedCaseOverviewGridColumnSettings(
                                                  customerId,
                                                  userId)
                                      };
            IList<CaseFieldSetting> userCaseFieldSettings = this.caseFieldSettingService.GetCaseFieldSettings(customerId);
            colSettingModel.CaseFieldSettings = userCaseFieldSettings;
            var li = new List<SelectListItem>
                         {
                             new SelectListItem()
                                 {
                                     Text =
                                         Translation.Get(
                                             "Info",
                                             Enums.TranslationSource
                                         .TextTranslation),
                                     Value = "1",
                                     Selected = false
                                 }
                         };
            colSettingModel.LineList = li;
            return colSettingModel;
        }
        
    }
}