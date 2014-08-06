using DH.Helpdesk.NewSelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.NewSelfService.Controllers
{
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.NewSelfService.Models.CoWorkers;
    using DH.Helpdesk.NewSelfService.WebServices;
    using DH.Helpdesk.NewSelfService.WebServices.Common;
    using System.Web.Script.Serialization;

    public class CoWorkersController : BaseController
    {        
        private readonly ICustomerService _customerService;        

        public CoWorkersController(IMasterDataService masterDataService,
                                   ICustomerService customerService,
                                   ICaseSolutionService caseSolutionService,
                                   ISSOService ssoService
                                  ):base(masterDataService, ssoService, caseSolutionService)
        {
            this._customerService = customerService;            
        }

        //
        // GET: /CoWorkers/

        public ActionResult Index(int customerId)
        {
            var model = new CoWorkersModel();            
            
            var curIdentity = SessionFacade.CurrentUserIdentity;
            List<string> coWorkers = new List<string>() ;
            if (curIdentity != null && curIdentity.EmployeeNumber != "")
            {
                var _amAPIService = new AMAPIService();
                var employeeList = AsyncHelpers.RunSync<string>(() => _amAPIService.GetEmployeeFor(curIdentity.EmployeeNumber));
                
                var strEmployees = employeeList.ToString();
                strEmployees = strEmployees.Replace("[",string.Empty).Replace("]",string.Empty).Replace("\"",string.Empty);                
                coWorkers = strEmployees.Split(',').ToList();
                SessionFacade.CurrentCoWorkers = coWorkers;
                TempData["EmployeeList"] = coWorkers;
            }

            var allCoWorkers = new List<CoWorker>();

            foreach (var cw in coWorkers)
            {
               var cwr = new CoWorker
               {
                   EmployeeNumber = cw,
                   FirstName = "Majid",
                   LastName = "Ghanadi",
                   JobTitle="---", 
                   JobKey ="000",
                   Email = "majidco18@gmail.com"
               };
               allCoWorkers.Add(cwr);
            }

            model.CoWorkers = allCoWorkers;
            return View(model);
        }
       
    }
}
