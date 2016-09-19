using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Changes;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;
using DH.Helpdesk.Web.Areas.Admin.Models.BusinessRules;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class DepricatedBusinessRulesController : BaseAdminController
    {
        #region Test_Data will be removed

        private static IList<BusinessRuleItemViewModel> _rules;

        static DepricatedBusinessRulesController()
        {
            _rules = new List<BusinessRuleItemViewModel>();
            for (int i = 1; i < 10; i++)
            {
                _rules.Add(new BusinessRuleItemViewModel
                {
                    Id = i,
                    RuleName = "Rule Name " + i,
                    EventName = "Event Name " + i,
                    SubjectName = "Subject Name " + i,
                    CreatedDate = DateTime.UtcNow.AddDays(-i),
                    CreatedByName = "User Name"
                });
            }
        }
        #endregion

        private readonly ICustomerService _customerService;

        public DepricatedBusinessRulesController(IMasterDataService masterDataService, ICustomerService customerService) 
            : base(masterDataService)
        {
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var model = new BusinessRulesViewModel();
            model.Customer = _customerService.GetCustomer(customerId);
            model.BusinessRules = _rules; //TODO: change to db data
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            return View("Edit");
        }

        public ActionResult Edit(int id, int customerId)
        {
            return View();
        }
    }
}
