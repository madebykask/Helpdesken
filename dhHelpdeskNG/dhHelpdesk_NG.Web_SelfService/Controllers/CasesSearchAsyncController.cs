using System;
using System.Net;
using System.Web.Mvc;
using System.Web.SessionState;
using DH.Helpdesk.SelfService.Controllers.Behaviors;
using DH.Helpdesk.SelfService.Entites;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Models;
using DH.Helpdesk.SelfService.Models.Error;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;
using DH.Helpdesk.SelfService.Models.Case;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService.Controllers
{
    //NOTE: its important to keep search logic in this separate controller with Session readonly attribute to allow ajax requests running in parallel!
    // if session is not readonly requests will be blocked in queue by session lock and executed one by one...
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CasesSearchAsyncController : Controller
    {
        private readonly IMasterDataService _masterDataService;
        private readonly CaseControllerBehavior _caseControllerBehavior;

        #region ctor()

        public CasesSearchAsyncController(IMasterDataService masterDataService,
            ICaseService caseService,
            ICaseSearchService caseSearchService,
            ICaseSettingsService caseSettingsService,
            ICaseFieldSettingService caseFieldSettingService,
            IProductAreaService productAreaService,
            ISelfServiceConfigurationService configurationService)
        {
            _masterDataService = masterDataService;
            _caseControllerBehavior = new CaseControllerBehavior(masterDataService, caseService, caseSearchService,
                caseSettingsService, caseFieldSettingService,
                productAreaService, configurationService);
        }
        
        #endregion

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public PartialViewResult SearchCustomerCases(CaseSearchInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.GetErrors();
                return CreateErrorResult(new Error(error));
            }

            var searchParams = CaseSearchInputParameters.Create(inputModel);

            if (string.IsNullOrEmpty(searchParams.SortBy))
            {
                searchParams.SortBy = "Casenumber";
                searchParams.SortAscending = false;
            }

            var res = _caseControllerBehavior.ValidateSearchParameters(searchParams);
            if (!res.Valid)
            {
                return CreateErrorResult(new Error(res.ErrorMessage));
            }

            var sessionCustomer = SessionFacade.CurrentCustomer;
            var customer = sessionCustomer?.Id != searchParams.CustomerId ?
                _masterDataService.GetCustomer(inputModel.CustomerId) :
                sessionCustomer;

            Exception ex = null;
            CaseSearchResultModel model = null;
            try
            {
                model = _caseControllerBehavior.GetCaseSearchResultsModel(searchParams, customer);
            }
            catch (Exception e)
            {
                ex = e;
            }

            if (ex != null)
            {
                return CreateErrorResult(new Error(ex.Message));
            }

            return PartialView("~/Views/Case/_CaseSearchResults.cshtml", model);
        }

        #region Helper Methods

        private PartialViewResult CreateErrorResult(Error err)
        {
            ControllerContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return PartialView("~/Views/Case/_SearchError.cshtml", err);
        }

        #endregion
    }
}