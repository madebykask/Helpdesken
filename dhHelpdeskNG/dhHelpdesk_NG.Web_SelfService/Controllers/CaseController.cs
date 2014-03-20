using Ninject;
using System.Linq;

using DH.Helpdesk.SelfService.Models.Case;
using DH.Helpdesk.BusinessData.Models.SelfService.Case;



namespace DH.Helpdesk.SelfService.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using System;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;


    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;

        private readonly ILogService _logService;
        
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        

        public CaseController(ICaseService caseService,
                              ICaseFieldSettingService caseFieldSettingService,
                              ILogService logService)            
        {
            this._caseService = caseService;
            this._logService = logService;
            this._caseFieldSettingService = caseFieldSettingService;
        }

        [HttpGet]
        public ActionResult Index(string id, int languageId = 1)        
        {
            var guid = new Guid(id);
            var model = GetCaseOverview(guid, languageId);

            return this.View(model);
        }

        public ActionResult Search()
        {
            return this.View();
        }

        public ActionResult New()
        {
            return this.View();
        }

        private CaseOverviewModel GetCaseOverview(Guid GUID, int languageId)
        {
            var currentCase = _caseService.GetCaseByGUID(GUID);

            var caseFieldSetting = _caseFieldSettingService.ListToShowOnCasePage(currentCase.Customer_Id, languageId)
                                                           .Where(c => c.ShowExternal == 1)                                                           
                                                           .ToList();   
            
            CaseOverviewModel model = null;
            
            if (currentCase != null) 
            {
                var caselogs = _logService.GetLogsByCaseId(currentCase.Id).ToList();

                model = new CaseOverviewModel
                {
                    CasePreview = currentCase,
                    CaseLogs = caselogs                    
                };
            }

            return model;                     
        }

    }
}
