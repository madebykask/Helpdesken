using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class BusinessRuleController : BaseAdminController
    {
        private readonly ICustomerService _customerService;

        public BusinessRuleController(IMasterDataService masterDataService,
                                      ICustomerService customerService)
            : base(masterDataService)
        {
            _customerService = customerService;
        }

        #region Public Methods 
        
        [HttpGet]
        public ActionResult Index(int customerId, int moduleId)
        {
            var model = new BusinessRuleIndexModel();
            model.Customer = _customerService.GetCustomer(customerId);
            model.Module = new BRModuleModel(moduleId);               
            return View(model);
        }

        [HttpGet]
        public ActionResult NewRule(int customerId, int moduleId)
        {
            var model = new BusinessRuleIndexModel();
            model.Customer = _customerService.GetCustomer(customerId);
            model.Module = new BRModuleModel(moduleId);
            return View(model);
        }

        #endregion



    }
}
