namespace DH.Helpdesk.Web.Infrastructure.Services.Case
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Grid;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Web.Models.Case.Input;

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

        public void UpdateSettings(CaseOverviewSettingsInput model, int customerId, int userId, int userGroupId)
        {
            if (model.FieldStyle == null)
            {
                throw new ArgumentException("Feild styles can not be null");
            }
            var gridSettings = this.gridSettingsService.GetForCustomerUserGrid(customerId, userId, GRID_ID);
            gridSettings.Parameters["font_style"] = model.SelectedFontStyle;
            this.gridSettingsService.Save(gridSettings, customerId, userId);
            var i = 0;
            var inputSetting =
                model.FieldStyle.Select(
                    it => new CaseOverviewGridColumnSetting { Name = it.ColumnName, Style = it.Style, Order = i++ });
            var filteredInput =
                this.caseSettingService.GetAvailableCaseOverviewGridColumnSettings(customerId, userId)
                    .Join(
                        inputSetting,
                        availableFiled => availableFiled.Name,
                        incoming => incoming.Name,
                        (availableFiled, incoming) => incoming)
                    .ToArray();
            this.caseSettingService.SyncSettings(filteredInput, customerId, userId, userGroupId);
        }

        public const string GRID_ID = "case_overview";
        public const string DEFAULT_FONT_STYLE = "normal";

        public CaseColumnsSettingsModel GetSettings(int customerId, int userId)
        {
            var gridSettings = this.gridSettingsService.GetForCustomerUserGrid(customerId, userId, GRID_ID);
            var colSettingModel = new CaseColumnsSettingsModel
                                      {
                                          CustomerId = customerId,
                                          SelectedFontStyle =
                                              gridSettings.Parameters.ContainsKey("font_style")
                                                  ? gridSettings.Parameters["font_style"]
                                                  : DEFAULT_FONT_STYLE,
                                          UserId = userId,
                                          AvailableColumns =
                                              this.caseSettingService
                                              .GetAvailableCaseOverviewGridColumnSettings(
                                                  customerId,
                                                  userId),
                                          SelectedColumns =
                                              this.caseSettingService
                                              .GetSelectedCaseOverviewGridColumnSettings(
                                                  customerId,
                                                  userId)
                                      };

            if (colSettingModel.SelectedColumns.Count() == 0)
            {
                colSettingModel.SelectedColumns = colSettingModel.AvailableColumns;
            }

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