using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.SessionState;
using DH.Helpdesk.BusinessData.Models.Grid;
using DH.Helpdesk.BusinessData.Models.User.Input;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Grid;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Behaviors;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Web.Infrastructure.Grid;
using DH.Helpdesk.Web.Models.Case;
using System.Collections.Generic;
using DH.Helpdesk.Dal.Repositories.FileIndexing;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Web.Infrastructure.Logger;

namespace DH.Helpdesk.Web.Controllers
{
    //NOTE: its important to keep search logic in this separate controller with Session readonly attribute to allow ajax requests running in parallel!
    // if session is not readonly requests will be blocked in queue by session lock and executed one by one...
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AdvancedSearchController : Controller
    {
        private readonly ILoggerService _logger = LogManager.Error;
        private readonly AdvancedSearchBehavior _advancedSearchBehavior;
        private readonly ISettingService _settingService;
        private readonly GridSettingsService _gridSettingsService;
		private readonly ICustomerService _customerService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ICaseLockService _caseLockService;

		public AdvancedSearchController(
            ICaseFieldSettingService caseFieldSettingService, 
            ICaseSearchService caseSearchService,
            ISettingService settingService,
            IProductAreaService productAreaService,
            ICaseSettingsService caseSettingsService,
            IUserService userService, 
            ICustomerUserService customerUserService,
			ICustomerService customerService,
            GridSettingsService gridSettingsService,
            IGlobalSettingService globalSettingService,
            ICaseLockService caseLockService)
        {
            _settingService = settingService;
            _gridSettingsService = gridSettingsService;
            _globalSettingService = globalSettingService;
            _caseLockService = caseLockService;
            _customerService = customerService;
            _advancedSearchBehavior = new AdvancedSearchBehavior(caseFieldSettingService, 
                caseSearchService, 
                userService, 
                settingService, 
                productAreaService,
                customerUserService,
                globalSettingService,
                caseLockService);
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Search(AdvancedCaseSearchInput inputModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.GetErrorsText();
                //todo: return js error
                return CreateErrorResult($"Invalid state. {Environment.NewLine}{errors}"); 
            }

            var customerId = inputModel.CustomerId;
            var searchFilter = _advancedSearchBehavior.MapToCaseSearchFilter(inputModel);

            var gridSortingOptions = new GridSortOptions()
            {
                sortBy = inputModel.SortBy,
                sortDir = (SortingDirection)inputModel.SortDir
            };

            var gridSettings =
                CreateGridSettingsModel(customerId, SessionFacade.CurrentUser, gridSortingOptions);

            var extendedCustomers = _settingService.GetExtendedSearchIncludedCustomers();

            AdvancedSearchDataModel searchDataModel;
			try
			{
				searchDataModel = _advancedSearchBehavior.RunAdvancedSearchForCustomer(searchFilter,
					gridSettings,
					customerId,
					customerId,
					SessionFacade.CurrentUser,
					extendedCustomers);
            }
			catch (FileIndexingException ex)
			{
				var customer = _customerService.GetCustomer(customerId);
                _logger.Error(ex);
				//Response.StatusCode = 500;
				return Json(new { errorMsg = $"Can not search in files for customer { customer?.Name ?? "[unkown]" } ({customerId})" });
			}

			var jsonGridSettings =
                JsonGridSettingsMapper.ToJsonGridSettingsModel(
                    gridSettings, 
                    customerId,
                    1/*availableColCount?*/,
                    CaseColumnsSettingsModel.PageSizes.Select(x => x.Value).ToArray());

            var data = new
            {
                searchResults = searchDataModel.Data, 
                count = searchDataModel.CasesCount,
                gridSettings = jsonGridSettings
            };

            return Json(new { result = "success", data });
        }

        private GridSettingsModel CreateGridSettingsModel(int searchCustomerId, UserOverview currentUser, GridSortOptions sortOptions)
        {
            var gridSettings =
                _gridSettingsService.GetForCustomerUserGrid(
                    searchCustomerId,
                    0 /*currentUser.UserGroupId*/,  //NOTE: at the moment there's no support for groups and only 0 is used for new AdvancedSearch
                    currentUser.Id,
                    GridSettingsService.CASE_ADVANCED_SEARCH_GRID_ID);

            if (sortOptions != null)
                gridSettings.sortOptions = sortOptions;

            return gridSettings;
        }

        #region Helper Methods

        private PartialViewResult CreateErrorResult(string err)
        {
            ControllerContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return PartialView("~/Views/Cases/AdvancedSearch/_SearchError.cshtml", err);
        }

        #endregion
    }
}