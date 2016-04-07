using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.SelfService.Infrastructure.Common.Concrete
{
    using DH.Helpdesk.BusinessData.Models.ActionSetting;
    using Microsoft.Ajax.Utilities;
    using DH.Helpdesk.SelfService.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Error;

    public sealed class CommonFunctions : ICommonFunctions
    {                
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IActionSettingService _actionSettingService;
        private readonly ILogService _logService;

        public CommonFunctions(ICaseSolutionService caseSolutionService,
                               ILogService logService,
                               IActionSettingService actionSettingService)
        {
            this._caseSolutionService = caseSolutionService;
            this._actionSettingService = actionSettingService;
            this._logService = logService;
        }

        public List<CaseSolution> GetCaseTemplates(int customerId)
        {
            return _caseSolutionService.GetCaseSolutions(customerId)
                                       .Where(t => t.ShowInSelfService && t.Status != 0)
                                       .OrderBy(t => (t.OrderNum == null) ? 9999 : t.OrderNum)
                                       .ThenBy(t => t.Name).ToList();           
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
    }
}