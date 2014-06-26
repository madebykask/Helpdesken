using DH.Helpdesk.Domain;
using DH.Helpdesk.NewSelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.NewSelfService.Models;
    using DH.Helpdesk.NewSelfService.Models.Case;

    public class StartController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICaseService _caseService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingService;
        private readonly ICaseSearchService _caseSearchService;

        public StartController(IMasterDataService masterDataService,
                               ICaseFieldSettingService caseFieldSettingService,
                               ICustomerService customerService,
                               ICaseService caseService,
                               ICaseSettingsService caseSettingService,
                               ICaseSearchService caseSearchService,
                               ICaseSolutionService caseSolutionService
                              ):base(masterDataService)
        {
            this._caseService = caseService;            
            this._caseFieldSettingService = caseFieldSettingService;
            this._customerService = customerService;
            this._caseSolutionService = caseSolutionService;
            this._caseSettingService = caseSettingService;
            this._caseSearchService = caseSearchService;
        }

        //
        // GET: /Start/

        public ActionResult Index(int customerId)
        {
            if (!CheckAndUpdateGlobalValues(customerId))
                return null;

            var currentCustomer = this._customerService.GetCustomer(customerId);
            var languageId = currentCustomer.Language_Id;
         
            UserCasesModel model = null;

            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (identity != null)
            {
                string adUser = identity.Name; // "DATAHALLAND\\mg"
                string regUser = adUser.GetUserFromAdPath();
                SessionFacade.CurrentSystemUser = regUser;
                if (regUser != string.Empty)
                {
                    model = this.GetUserCasesModel(currentCustomer.Id, languageId, regUser, "", 10, "2");
                }
            }

            return this.View("Index", model);            
        }

        private UserCasesModel GetUserCasesModel(int customerId, int languageId, string curUser,
                                                string pharasSearch, int maxRecords, string progressId = "",
                                                string sortBy = "", bool ascending = false)
        {
            var cusId = customerId;
            
            var model = new UserCasesModel
            {
                CustomerId = cusId,
                LanguageId = languageId,
                UserId = curUser,
                MaxRecords = maxRecords,
                PharasSearch = pharasSearch,
                ProgressId = progressId
            };

            var srm = new CaseSearchResultModel();
            var sm = new CaseSearchModel();
            var cs = new CaseSearchFilter();
            var search = new Search();

            if (string.IsNullOrEmpty(sortBy)) sortBy = "Casenumber";

            cs.CustomerId = cusId;
            cs.FreeTextSearch = pharasSearch;
            cs.CaseProgress = progressId;

            search.SortBy = sortBy;
            search.Ascending = ascending;

            sm.Search = search;
            sm.caseSearchFilter = cs;

            // 1: User in Customer Setting
            srm.CaseSettings = this._caseSettingService.GetCaseSettingsByUserGroup(cusId, 1);
                                              

            srm.CaseSettings = srm.CaseSettings.Where(s =>
                                                        s.Name.ToLower() == "casenumber" ||
                                                        s.Name.ToLower() == "persons_name" ||
                                                        s.Name.ToLower() == "persons_phone" ||
                                                        s.Name.ToLower() == "productarea_id").ToList();

            var manualSetting = _caseSettingService.GetCaseSettings(cusId).Where(s => s.Name.ToLower() == "regtime").Take(1).SingleOrDefault();

            if (manualSetting != null)
                srm.CaseSettings.Add(manualSetting);

            srm.Cases = this._caseSearchService.Search(
                sm.caseSearchFilter,
                srm.CaseSettings,
                -1,
                curUser,
                1,
                1,
                1,
                search,
                1,
                1,
                null).Take(5).ToList(); // 

            model.CaseSearchResult = srm;
            SessionFacade.CurrentCaseSearch = sm;

            var dynamicCases = _caseService.GetAllDynamicCases().Take(5).ToList();
            model.DynamicCases = dynamicCases;

            return model;
        }
        private bool CheckAndUpdateGlobalValues(int customerId)
        {
            if ((SessionFacade.CurrentCustomer != null && SessionFacade.CurrentCustomer.Id != customerId) ||
                (SessionFacade.CurrentCustomer == null))
            {
                var newCustomer = _customerService.GetCustomer(customerId);
                if (newCustomer == null)
                    return false;

                SessionFacade.CurrentCustomer = newCustomer;
            }

            SessionFacade.CurrentLanguageId = SessionFacade.CurrentCustomer.Language_Id;
            ViewBag.PublicCustomerId = customerId;

            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (identity != null)
            {
                SessionFacade.CurrentSystemUser = identity.Name.GetUserFromAdPath();
                ViewBag.PublicCaseTemplate = _caseSolutionService.GetCaseSolutions(customerId).ToList();
            }
            return true;
        }

    }
}
