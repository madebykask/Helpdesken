using Ninject;
using System.Linq;

using DH.Helpdesk.SelfService.Models.Case;
using DH.Helpdesk.BusinessData.Models.SelfService.Case;


namespace DH.Helpdesk.SelfService.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using System;


    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;

        private readonly ILogService _logService;

        public CaseController(ICaseService caseService,
                              ILogService logService)            
        {
            this._caseService = caseService;
            this._logService = logService;
        }

        [HttpGet]
        public ActionResult Index(string id)        
        {
            var guid = new Guid(id);
            var model = GetCaseOverview(guid);

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

        private CaseOverviewModel GetCaseOverview(Guid GUID)
        {
            var currentCase = _caseService.GetCaseByGUID(GUID);            

            CaseOverviewModel model = null;

            if (currentCase != null) 
            {
                var caselogs = _logService.GetLogsByCaseId(currentCase.caseId)
                                          .Select(l => new SelfServiceCaseLog
                                          (
                                              l.Id,
                                              l.LogDate,
                                              l.Text_External,
                                              l.Text_Internal
                                          )).ToList();

                model = new CaseOverviewModel
                {
                    caseId = currentCase.caseId,
                    Department = currentCase.Department,
                    Notifier = currentCase.PersonName,
                    PCNumber = currentCase.PCNumber,
                    Phone = currentCase.PersonPhone,
                    ProductArea = currentCase.ProductArea,
                    RegistrationDate = currentCase.RegistrationDate,
                    WatchDate = currentCase.WatchDate,
                    CaseLogs = caselogs
                };
            }

            return model;                     
        }

    }
}
