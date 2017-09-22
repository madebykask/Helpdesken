using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Controllers
{
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Classes.ServiceAPI.AMAPI.Output;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.SelfService.Models.CoWorkers;
    using DH.Helpdesk.SelfService.WebServices;
    using DH.Helpdesk.SelfService.WebServices.Common;
    using System.Web.Script.Serialization;

    public class CoWorkersController : BaseController
    {        
        private readonly ICustomerService _customerService;        

        public CoWorkersController(IMasterDataService masterDataService,
                                   ICustomerService customerService,
                                   ICaseSolutionService caseSolutionService                                   
                                  ):base(masterDataService, caseSolutionService)
        {
            this._customerService = customerService;            
        }

        //
        // GET: /CoWorkers/

        public ActionResult Index(int customerId)
        {
            if (!SessionFacade.CurrentCustomer.FetchDataFromApiOnExternalPage)
            {
                SessionFacade.UserHasAccess = false;
                ErrorGenerator.MakeError("You don't have access to the portal.", 401);
                return RedirectToAction("Index", "Error");
            }

            var model = new CoWorkersModel();                        
            var curIdentity = SessionFacade.CurrentUserIdentity;

            var allCoWorkers = new List<CoWorker>();

            if (curIdentity != null && curIdentity.EmployeeNumber != "")
            {                
                try
                {
                    var _amAPIService = new AMAPIService();
                    var employee = AsyncHelpers.RunSync<APIEmployee>(() => _amAPIService.GetEmployeeFor(curIdentity.EmployeeNumber));
                    if (employee.IsManager)
                    {
                        foreach (var cw in employee.Subordinates)
                        {
                            var emailAddress = "";
                            if (cw.ExtraInfo.ContainsKey("Email"))
                            {
                                var mail = cw.ExtraInfo["Email"];
                                if (mail.ContainsKey("comm"))
                                {
                                    emailAddress = mail["comm"];
                                }
                            }
                            var cwr = new CoWorker
                            {
                                EmployeeNumber = cw.EmployeeNumber,
                                FirstName = cw.FirstName,
                                LastName = cw.LastName,
                                JobTitle = cw.JobName,
                                JobKey = cw.JobCode,
                                Email = emailAddress
                            };
                            allCoWorkers.Add(cwr);
                        }

                        //SessionFacade.CurrentCoWorkers = employee.Subordinates;
                        SessionFacade.UserHasAccess = true;
                    }
                    else
                    {
                        SessionFacade.UserHasAccess = false;
                        ErrorGenerator.MakeError("You don't have access to the portal.", 401);
                        return RedirectToAction("Index", "Error");
                    }
                }
                catch (Exception ex)
                {
                    SessionFacade.UserHasAccess = false;
                    ErrorGenerator.MakeError("Portal is not accessible. \n " + ex.Message , 402);
                    return RedirectToAction("Index", "Error");
                }                                
            }
            
            model.CoWorkers = allCoWorkers;
            return View(model);
        }
       
    }
}
