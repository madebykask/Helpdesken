using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.SelfService.Infrastructure.Common.Concrete
{
    using DH.Helpdesk.BusinessData.Models.ActionSetting;    
    using DH.Helpdesk.SelfService.Models.Case;    
    using Models.Shared;
    using Helpers;
    using Helpdesk.Common.Enums;
    using Models.CaseTemplate;

    public sealed class CommonFunctions : ICommonFunctions
    {                
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IActionSettingService _actionSettingService;
        private readonly ILogService _logService;
        private readonly IOrderTypeService _orderTypeService;
        private readonly ISettingService _settingService;

        public CommonFunctions(ICaseSolutionService caseSolutionService,
                               ILogService logService,
                               IActionSettingService actionSettingService,
                               IOrderTypeService orderTypeService,
                               ISettingService settingService)
        {
            _caseSolutionService = caseSolutionService;
            _actionSettingService = actionSettingService;
            _logService = logService;
            _orderTypeService = orderTypeService;
            _settingService = settingService;
        }

        public List<CaseSolution> GetCaseTemplates(int customerId)
        {
            return _caseSolutionService.GetCaseSolutions(customerId)
                                       .Where(t => t.ShowInSelfService && t.Status != 0)
                                       .OrderBy(t => (t.OrderNum == null) ? 9999 : t.OrderNum)
                                       .ThenBy(t => t.Name).ToList();           
        }

        public List<CaseTemplateTreeViewModel> GetCaseTemplateTreeModel(int customerId, bool groupedView)
        {
            var ret = new List<CaseTemplateTreeViewModel>();
            if (groupedView)
            {
                var allTemplates = _caseSolutionService.GetCaseSolutions(customerId)
                                       .Where(t => t.ShowInSelfService && t.Status != 0)
                                       .ToList();
                var allCategories = allTemplates.Select(a => new { Id = a.CaseSolutionCategory_Id, Name = a.CaseSolutionCategory?.Name }).Distinct();
                foreach (var cat in allCategories)
                {
                    var newGroup = new CaseTemplateTreeViewModel
                    {
                        CategoryId = cat.Id.HasValue? cat.Id.Value : 0,
                        CategoryName = cat.Name
                    };
                    
                    newGroup.Templates = 
                        allTemplates.Where(t => t.CaseSolutionCategory_Id == cat.Id)
                                    .Select(t => new CaseTemplateViewModel
                                            {
                                                Id = t.Id,
                                                Name = t.Name,
                                                ShortDescription = t.ShortDescription,
                                                TemplatePath = t.TemplatePath,
                                                TemplateCategory_Id = t.CaseSolutionCategory_Id,
                                                OrderNum = t.OrderNum,
                                                ContainsExtendedForm = t.ExtendedCaseForms.Any()
                                            })
                                    .OrderBy(t => (t.OrderNum == null) ? 9999 : t.OrderNum)
                                    .ThenBy(t => t.Name).ToList();                       
                    
                    ret.Add(newGroup);
                }                
            }
            else
            {
                var root = new CaseTemplateTreeViewModel();
                root.CategoryId = null;
                var templates = _caseSolutionService.GetCaseSolutions(customerId)
                                       .Where(t => t.ShowInSelfService && t.Status != 0 )
                                       .Select(t=> new CaseTemplateViewModel
                                       {
                                           Id = t.Id,
                                           Name = t.Name,
                                           ShortDescription = t.ShortDescription,
                                           TemplatePath = t.TemplatePath,
                                           TemplateCategory_Id = t.CaseSolutionCategory_Id,
                                           OrderNum = t.OrderNum,
                                           ContainsExtendedForm = t.ExtendedCaseForms.Any()
                                       })
                                       .OrderBy(t => (t.OrderNum == null) ? 9999 : t.OrderNum)
                                       .ThenBy(t => t.Name).ToList();
                root.Templates = templates;
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

        public CaseLogModel GetCaseLogs(int caseId)
        {            
            var caseLogs = _logService.GetLogsByCaseId(caseId).OrderByDescending(l=> l.RegTime).ToList();
            
            var caseLogModel = new CaseLogModel { CaseId = caseId, CaseLogs = caseLogs };
            
            return caseLogModel;                            
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

        public LayoutViewModel GetLayoutViewModel(int? currentCase_Id, object tmpDataLanguge)
        {
            var model = new LayoutViewModel();
            model.AppType = AppConfigHelper.GetAppSetting(AppSettingsKey.CurrentApplicationType);

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

            model.CustomerId = pCustomerId;
            model.CurrentCase_Id = currentCase_Id;
            model.CurrentCustomer = SessionFacade.CurrentCustomer;
            model.OrderModuleIsEnabled = IsOrderModuleEnabled(pCustomerId);            
            model.UserHasOrderTypes = (SessionFacade.CurrentLocalUser != null)? 
                                        IsUserHasOrderTypes(SessionFacade.CurrentLocalUser.Id, pCustomerId) : false;
            model.HideMenu = !SessionFacade.UserHasAccess;            
            model.ShowLanguage = tmpDataLanguge != null && tmpDataLanguge.ToString().ToLower() == "true";
            model.CaseLog = (currentCase_Id.HasValue)? GetCaseLogs(currentCase_Id.Value) : null;
            model.HasError = SessionFacade.LastError != null && !string.IsNullOrEmpty(SessionFacade.LastError.Message);
            if (!model.HasError && !model.HideMenu &&
                model.CurrentCustomer != null && model.CurrentCustomer.ShowCaseOnExternalPage != 0)
                model.CaseTemplatesGroups = GetCaseTemplateTreeModel(pCustomerId, model.CurrentCustomer.GroupCaseTemplates != 0);
            else
                model.CaseTemplatesGroups = null;

            return model;
        }
    }
}