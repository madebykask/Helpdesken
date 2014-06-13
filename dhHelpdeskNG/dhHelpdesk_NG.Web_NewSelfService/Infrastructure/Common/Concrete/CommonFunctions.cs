using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.NewSelfService.Infrastructure.Common.Concrete
{
    using DH.Helpdesk.BusinessData.Models.ActionSetting;

    using Microsoft.Ajax.Utilities;

    public sealed class CommonFunctions : ICommonFunctions
    {                
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly IActionSettingService _actionSettingService;

        public CommonFunctions(ICaseSolutionService caseSolutionService,
                               IActionSettingService actionSettingService)
        {
            this._caseSolutionService = caseSolutionService;
            this._actionSettingService = actionSettingService;
        }

        public List<CaseSolution> GetCaseTemplates(int customerId)
        {            
            return _caseSolutionService.GetCaseSolutions(customerId).Where(t=> t.ShowInSelfService).ToList();           
        }

        public List<ActionSetting> GetActionSettings(int customerId)
        {
            return this._actionSettingService.GetActionSettings(customerId);
        }
    }
}