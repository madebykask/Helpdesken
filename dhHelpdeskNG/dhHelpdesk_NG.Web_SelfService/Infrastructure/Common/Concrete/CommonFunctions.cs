using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Domain;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;

namespace DH.Helpdesk.SelfService.Infrastructure.Common.Concrete
{
    using DH.Helpdesk.BusinessData.Models.ActionSetting;
    using Models.Shared;
    using Helpdesk.Common.Enums;
    using Models.CaseTemplate;

    public sealed class CommonFunctions : ICommonFunctions
    {                
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly IActionSettingService _actionSettingService;
        private readonly ILogService _logService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly ISettingService _settingService;
        private readonly ISelfServiceConfigurationService _configurationService;

        public CommonFunctions(ICaseSolutionService caseSolutionService,
                               IGlobalSettingService globalSettingService,
                               ILogService logService,
                               IActionSettingService actionSettingService,
                               IOrderTypeService orderTypeService,
                               ISettingService settingService,
                               ISelfServiceConfigurationService configurationService)
        {
            _caseSolutionService = caseSolutionService;
            _globalSettingService = globalSettingService;
            _actionSettingService = actionSettingService;
            _logService = logService;
            _orderTypeService = orderTypeService;
            _settingService = settingService;
            _configurationService = configurationService;
        }

        public List<CaseTemplateTreeViewModel> GetCaseTemplateTreeModel(int customerId, bool groupedView)
        {
            var ret = new List<CaseTemplateTreeViewModel>();
            if (groupedView)
            {
                var allTemplates = _caseSolutionService.GetSelfServiceCaseTemplates(customerId);

                var allCategories = allTemplates.Select(a => new
                {
                    Id = a.CaseSolutionCategory_Id,
                    Name = a.CaseSolutionCategoryName ?? string.Empty
                }).Distinct().ToList();

                ret = allCategories.Select(cat => new CaseTemplateTreeViewModel
                {
                    CategoryId = cat.Id ?? 0,
                    CategoryName = cat.Name,
                    Templates = 
                        allTemplates.Where(t => t.CaseSolutionCategory_Id == cat.Id)
                            .Select(t => new CaseTemplateViewModel
                            {
                                Id = t.Id,
                                Name = t.Name,
                                ShortDescription = t.ShortDescription,
                                TemplatePath = t.TemplatePath,
                                TemplateCategory_Id = t.CaseSolutionCategory_Id,
                                OrderNum = t.OrderNum,
                                ContainsExtendedForm = t.ContainsExtendedForm
                            })
                        .OrderBy(t => t.OrderNum ?? 9999)
                        .ThenBy(t => t.Name)
                        .ToList()
                }).ToList();
            }
            else
            {
                var root = new CaseTemplateTreeViewModel
                {
                    CategoryId = null,
                    Templates =  
                        _caseSolutionService.GetSelfServiceCaseTemplates(customerId)
                            .Select(t => new CaseTemplateViewModel
                            {
                                Id = t.Id,
                                Name = t.Name,
                                ShortDescription = t.ShortDescription,
                                TemplatePath = t.TemplatePath,
                                TemplateCategory_Id = t.CaseSolutionCategory_Id,
                                OrderNum = t.OrderNum,
                                ContainsExtendedForm = t.ContainsExtendedForm
                            })
                            .OrderBy(t => t.OrderNum ?? 9999)
                            .ThenBy(t => t.Name)
                            .ToList()
                };
                
                ret.Add(root);
            }

            /*Show Templates without group first then groups sorted by name*/
            var sortedView = ret.Where(r => r.CategoryId == 0).ToList();
            sortedView.AddRange(ret.Where(r => r.CategoryId != 0).OrderBy(r=> r.CategoryName).ToList());

            return sortedView;
        }

        public List<ActionSetting> GetActionSettings(int customerId)
        {
            return this._actionSettingService.GetActionSettings(customerId);
        }

        public bool IsOrderModuleEnabled(int customerId)
        {
            var settings = _settingService.GetCustomerSettings(customerId);
            return settings != null && settings.ModuleOrder;
        }

        public bool IsUserHasOrderTypes(int userId, int customerId)
        {
            var isUserHasOrderTypes = _orderTypeService.IsUserHasOrderTypes(customerId, userId);
            return isUserHasOrderTypes;
        }

        public LayoutViewModel GetLayoutViewModel(object tmpDataLanguge)
        {
            var globalSettings = _globalSettingService.GetGlobalSettings();
            string signOutUrl = System.Configuration.ConfigurationManager.AppSettings["SignOutUrl"] != null ?
                          System.Configuration.ConfigurationManager.AppSettings["SignOutUrl"] : "";
            int pCustomerId = -1;
            if (SessionFacade.CurrentCustomer != null)
            {
                pCustomerId = SessionFacade.CurrentCustomer.Id;
                SessionFacade.CurrentCustomerID = pCustomerId;
            }

            if (pCustomerId == -1 && SessionFacade.CurrentCustomerID > 0)
            {
                pCustomerId = SessionFacade.CurrentCustomerID;
                if (SessionFacade.CurrentCustomer != null)
                {
                    SessionFacade.CurrentCustomer.Id = pCustomerId;
                }
            }

            var appType = _configurationService.AppSettings.ApplicationType;
            var model = new LayoutViewModel
            {
                SignOutUrl = signOutUrl,
                AppType = appType,
                IsLineManager = ApplicationTypes.LineManager.Equals(appType, StringComparison.OrdinalIgnoreCase),
                IsMultiCustomerSearchEnabled = globalSettings.First().MultiCustomersSearch == 1,
                CustomerId = pCustomerId,
                CurrentCustomer = SessionFacade.CurrentCustomer,
                CurrentSystemUser = SessionFacade.CurrentSystemUser,
                CurrentUserIdentity = SessionFacade.CurrentUserIdentity,
                CurrentLocalUser = SessionFacade.CurrentLocalUser,
                OrderModuleIsEnabled = IsOrderModuleEnabled(pCustomerId),
                UserHasOrderTypes = SessionFacade.CurrentLocalUser != null &&
                                    IsUserHasOrderTypes(SessionFacade.CurrentLocalUser.Id, pCustomerId),
                HideMenu = !SessionFacade.UserHasAccess,
                LoginMode = _configurationService.AppSettings.LoginMode,
                ShowLanguage = tmpDataLanguge != null && tmpDataLanguge.ToString().ToLower() == "true",
                AllLanguages = SessionFacade.AllLanguages,
                CurrentLanguageId = SessionFacade.CurrentLanguageId,
                HasError = SessionFacade.LastError != null && !string.IsNullOrEmpty(SessionFacade.LastError.Message)
            };

            if (!model.HasError && !model.HideMenu && model.CurrentCustomer != null && model.CurrentCustomer.ShowCaseOnExternalPage != 0)
                model.CaseTemplatesGroups = GetCaseTemplateTreeModel(pCustomerId, model.CurrentCustomer.GroupCaseTemplates != 0);
            else
                model.CaseTemplatesGroups = null;

            return model;
        }
    }
}