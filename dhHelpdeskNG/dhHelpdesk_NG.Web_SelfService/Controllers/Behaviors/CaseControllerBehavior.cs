using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Domain;
using DH.Helpdesk.SelfService.Entites;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.SelfService.Infrastructure.Helpers;
using DH.Helpdesk.SelfService.Models;
using DH.Helpdesk.SelfService.Models.Case;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.SelfService.Controllers.Behaviors
{
    public class CaseControllerBehavior
    {
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseService _caseService;
        private readonly ICaseSearchService _caseSearchService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly IProductAreaService _productAreaService;
        private readonly ISelfServiceConfigurationService _configurationService;
        private readonly IComputerService _computerService;
        private readonly IFeatureToggleService _featureToggleService;

        #region ctor()

        public CaseControllerBehavior(
            IMasterDataService masterDataService,
            ICaseService caseService,
            ICaseSearchService caseSearchService,
            ICaseSettingsService caseSettingsService,
            ICaseFieldSettingService caseFieldSettingService,
            IProductAreaService productAreaService,
            ISelfServiceConfigurationService configurationService, 
            IComputerService computerService,
            IFeatureToggleService featureToggleService)
        {
            _masterDataService = masterDataService;
            _caseService = caseService;
            _caseSearchService = caseSearchService;
            _caseSettingsService = caseSettingsService;
            _caseFieldSettingService = caseFieldSettingService;
            _productAreaService = productAreaService;
            _configurationService = configurationService;
            _computerService = computerService;
            _featureToggleService = featureToggleService;

        }

        #endregion

        public CaseOverviewCriteriaModel GetCaseOverviewCriteria()
        {
            var curUser = SessionFacade.CurrentSystemUser ?? SessionFacade.CurrentUserIdentity.UserId;
            var userEmployeeNumber = SessionFacade.CurrentUserIdentity.EmployeeNumber;
            var initiator = string.IsNullOrEmpty(curUser) ? null : _computerService.GetComputerUserByUserID(curUser, SessionFacade.CurrentCustomer.Id);
            var showOnExtPageDepartmentCases = initiator?.ShowOnExtPageDepartmentCases ?? false;
            if (_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_SELFSERVICE_SETTING_VIEW_DEPARTMENT_CASES))
            {
                 showOnExtPageDepartmentCases = false;
            }
            var criteria = new CaseOverviewCriteriaModel()
            {
                MyCasesRegistrator = SessionFacade.CurrentCustomer.MyCasesRegistrator,
                MyCasesInitiator = SessionFacade.CurrentCustomer.MyCasesInitiator,
                MyCasesFollower = SessionFacade.CurrentCustomer.MyCasesFollower,
                MyCasesRegarding = SessionFacade.CurrentCustomer.MyCasesRegarding,
                MyCasesUserGroup = SessionFacade.CurrentCustomer.MyCasesUserGroup,
                MyCasesInitiatorDepartmentId = showOnExtPageDepartmentCases ? initiator.Department_Id : null,
                UserId = curUser,
                UserEmployeeNumber = userEmployeeNumber,
                GroupMember = new List<string>()
            };

            if (criteria.MyCasesUserGroup)
            {
                var employees = SessionFacade.CurrentCoWorkers;
                if (employees == null)
                {
                    try
                    {
                        var useApi = SessionFacade.CurrentCustomer.FetchDataFromApiOnExternalPage;
                        var customerId = SessionFacade.CurrentCustomer.Id;
                        var apiCredential = WebApiConfig.GetAmApiInfo();
                        var employee = _masterDataService.GetEmployee(customerId, userEmployeeNumber, useApi, apiCredential);

                        if (employee != null)
                        {
                            SessionFacade.CurrentCoWorkers = employee.Subordinates;
                            employees = employee.Subordinates;
                        }
                    }
                    catch { }
                }

                if (employees != null)
                    criteria.GroupMember.AddRange(employees.Where(e => !string.IsNullOrEmpty(e.EmployeeNumber))
                                                           .Select(e => e.EmployeeNumber).ToList());
            }

            return criteria;
        }

        public IList<CaseSearchResult> CaseSearchTranslate(IList<CaseSearchResult> cases, int customerId)
        {
            var ret = cases;
            var productareaCache = _productAreaService.GetProductAreasForCustomer(customerId).ToDictionary(it => it.Id, it => true);
            foreach (CaseSearchResult r in ret)
            {
                foreach (var c in r.Columns)
                {
                    if (c.TreeTranslation)
                    {
                        switch (c.Key.ToLower())
                        {
                            case "productarea_id":
                                if (productareaCache.ContainsKey(c.Id))
                                {
                                    var names = _productAreaService.GetParentPath(c.Id, customerId).Select(name => Translation.Get(name, Enums.TranslationSource.TextTranslation));
                                    c.StringValue = string.Join(" - ", names);
                                }

                                break;
                        }
                    }
                }
            }

            return ret;
        }

        public CaseSearchResultModel GetCaseSearchResultsModel(CaseSearchInputParameters searchParams, Customer customer)
        {
            var customerId = customer.Id;
            
            const int userGroupId = 1;
            var userId = SessionFacade.CurrentLocalUser?.Id ?? -1;

            var caseSettings = _caseSettingsService.GetCaseSettingsByUserGroup(customerId, userGroupId);
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);
            var appSettings = _configurationService.AppSettings;
            var appType = appSettings.ApplicationType;

            //search cases
            var cases = DoCustomerCasesSearch(searchParams, customerId, appType, caseSettings, caseFieldSettings, userGroupId, userId);

            var dynamicCases = IsLineManagerApplication()
                ? _caseService.GetAllDynamicCases(customerId, cases.Select(c => c.Id).ToArray())
                : null;

            var model = new CaseSearchResultModel
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                SortOrder = searchParams.SortAscending ? Enums.SortOrder.Asc : Enums.SortOrder.Desc,
                SortBy = searchParams.SortBy,
                /////////////////////////////
                CaseSettings = caseSettings,
                Cases = CaseSearchTranslate(cases, customerId),
                DynamicCases = dynamicCases
            };

            return model;
        }

        public List<SelectListItem> PrepareProgressSelectItems()
        {
            var isLM = IsLineManagerApplication();

            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = CaseProgressFilter.CasesInProgress,
                    Text = isLM ? Translation.Get("Ongoing Service Requests") : Translation.Get("Ongoing Cases")
                },
                new SelectListItem
                {
                    Value = CaseProgressFilter.ClosedCases,
                    Text = isLM ? Translation.Get("Finished Service Requests") : Translation.Get("Closed Cases")
                },
                new SelectListItem
                {
                    Value = CaseProgressFilter.None,
                    Text = isLM ? Translation.Get("All Service Requests") : Translation.Get("All Cases")
                }
            };

            return items;
        }

        private IList<CaseSearchResult> DoCustomerCasesSearch(CaseSearchInputParameters searchParams,
                                                        int customerId,
                                                        string appType,
                                                        IList<CaseSettings> caseSettings,
                                                        IList<CaseFieldSetting> caseFieldSettings,
                                                        int userGroupId,
                                                        int userId,
                                                        bool showRemainingTime = false,
                                                        bool restrictedCasePermission = true,
                                                        int showNotAssignedWorkingGroups = 1)
        {
            var currentUser = searchParams.IdentityUser;

            var search = new Search
            {
                SortBy = searchParams.SortBy,
                Ascending = searchParams.SortAscending
            };

            var searchFilter = new CaseSearchFilter
            {
                CustomerId = customerId,
                FreeTextSearch = searchParams.PharasSearch,
                CaseProgress = searchParams.ProgressId,
                ReportedBy = "",
                UserId =  userId,
                RegUserId = currentUser,
                CaseOverviewCriteria = GetCaseOverviewCriteria()
            };

            CaseRemainingTimeData remainingTimeData;
            CaseAggregateData aggregateData;
            var workTimeCalc = TimeZoneInfo.Local;

            var caseSearchModel = new CaseSearchModel
            {
                Search = search,
                caseSearchFilter = searchFilter
            };

            //save search parameters to session
            //todo: update values without create new
            SessionFacade.CurrentCaseSearch = caseSearchModel;

            var cases = _caseSearchService.Search(
                searchFilter,
                caseSettings,
                caseFieldSettings?.ToArray(),
                userId,
                currentUser,
                showNotAssignedWorkingGroups,
                userGroupId,
                restrictedCasePermission,
                search,
                SessionFacade.CurrentCustomer.WorkingDayStart,
                SessionFacade.CurrentCustomer.WorkingDayEnd,
                workTimeCalc,
                appType,
                showRemainingTime,
                out remainingTimeData,
                out aggregateData).Items.ToList(); // Take(maxRecords)

            return cases;
        }

        protected bool IsLineManagerApplication()
        {
            var applicationType = _configurationService.AppSettings.ApplicationType;
            return ApplicationTypes.LineManager.Equals(applicationType, StringComparison.OrdinalIgnoreCase);
        }

        #region Validation Methods

        public RequestValidationResult ValidateSearchParameters(CaseSearchInputParameters parameters)
        {
            var progressId = parameters.ProgressId;
            if (progressId != CaseProgressFilter.ClosedCases &&
                progressId != CaseProgressFilter.CasesInProgress &&
                progressId != CaseProgressFilter.None)
            {
                return RequestValidationResult.Error("Process is not valid!", 202);
            }
            return RequestValidationResult.Ok();
        }

        public RequestValidationResult ValidateCustomer()
        {
            if (SessionFacade.CurrentCustomer == null)
            {
                return RequestValidationResult.Error("Customer is not valid!");
            }
            return RequestValidationResult.Ok();
        }

        public RequestValidationResult ValidateCurrentUserIdentity()
        {
            if (SessionFacade.CurrentUserIdentity == null)
            {
                return RequestValidationResult.Error("You don't have access to cases, please login again.", 203);
            }
            return RequestValidationResult.Ok();
        }

        public RequestValidationResult ValidateLocalUser()
        {
            if (SessionFacade.CurrentLocalUser == null)
            {
                return RequestValidationResult.Error("There's no associated customer user for logged in user.", 203);
            }
            return RequestValidationResult.Ok();
        }

        #endregion
    }

    #region RequestValidationResult class

    public class RequestValidationResult
    {
        public bool Valid { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }

        #region Factory Methods

        public static RequestValidationResult Error(string msg, int errorCode = 0)
        {
            return new RequestValidationResult
            {
                Valid = false,
                ErrorMessage = msg,
                ErrorCode = errorCode
            };
        }

        public static RequestValidationResult Ok()
        {
            return new RequestValidationResult { Valid = true };
        }

        #endregion
    }

    #endregion

    
}