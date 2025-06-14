﻿using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.Services.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;

namespace DH.Helpdesk.SelfService.Controllers
{
    using BusinessData.Models.Employee;
    using DH.Helpdesk.SelfService.Infrastructure.Common.Concrete;
    using DH.Helpdesk.SelfService.Models.CoWorkers;
    using Infrastructure.Helpers;

    public class CoWorkersController : BaseController
    {
        private readonly IMasterDataService _masterDataService;
        private readonly ICustomerService _customerService;        

        public CoWorkersController(IMasterDataService masterDataService,
                                   ISelfServiceConfigurationService configurationService,
                                   ICustomerService customerService,
                                   ICaseSolutionService caseSolutionService                                   
                                  ):base(configurationService, masterDataService, caseSolutionService)
        {
            _customerService = customerService;
            _masterDataService = masterDataService;
        }

        //
        // GET: /CoWorkers/

        public ActionResult Index(int customerId)
        {
            if (SessionFacade.CurrentUserIdentity == null)
            {
                SessionFacade.UserHasAccess = false;
                ErrorGenerator.MakeError("Session expired! Please refresh the page.", 400);
                return RedirectToAction("Index", "Error");
            }

            var curIdentity = SessionFacade.CurrentUserIdentity;
            var model = new CoWorkersModel();                        
            var allCoWorkers = new List<CoWorker>();

            if (SessionFacade.CurrentCoWorkers == null)
            {              
                try
                {
                    /*This is IKEA specific condition*/
                    if (!UserBelongedToCurrentCustomer(curIdentity.EmployeeNumber))
                        SessionFacade.CurrentCoWorkers = new List<SubordinateResponseItem>();
                    else
                    {
                        var useApi = SessionFacade.CurrentCustomer.FetchDataFromApiOnExternalPage;
                        var apiCredential = WebApiConfig.GetAmApiInfo();
                        var employee = _masterDataService.GetEmployee(customerId, curIdentity.EmployeeNumber, useApi, apiCredential);

                        if (employee != null)                        
                            SessionFacade.CurrentCoWorkers = employee.Subordinates;                        
                    }
                }
                catch (Exception ex)
                {
                    SessionFacade.UserHasAccess = false;
                    ErrorGenerator.MakeError("Error in access to members data. \n " + ex.Message, 402);
                    return RedirectToAction("Index", "Error");
                }               
            }

            if (SessionFacade.CurrentCoWorkers != null)
            {
                var employees = SessionFacade.CurrentCoWorkers;
                foreach (var cw in employees)
                {                    
                    var cwr = new CoWorker
                    {
                        EmployeeNumber = cw.EmployeeNumber,
                        FirstName = cw.FirstName,
                        LastName = cw.LastName,
                        JobTitle = cw.JobName,
                        JobKey = cw.JobCode,
                        Email = cw.Email
                    };
                    allCoWorkers.Add(cwr);
                }                
                SessionFacade.UserHasAccess = true;
            }
            else
            {
                SessionFacade.UserHasAccess = false;
                ErrorGenerator.MakeError("No information found about your member(s).", 401);
                return RedirectToAction("Index", "Error");
            }

            
            model.CoWorkers = allCoWorkers;
            return View(model);
        }
       
    }
}
