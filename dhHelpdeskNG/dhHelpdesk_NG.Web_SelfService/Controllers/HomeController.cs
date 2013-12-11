using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;

namespace dhHelpdesk_NG.Web_SelfService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;

        
        public HomeController(
            ICustomerService customerService,
            IMasterDataService masterDataService)
            
        {
            _customerService = customerService;
        }
        
        public ActionResult Index()
        {
            var customer = _customerService.GetCustomer(1);

            return View(customer);

        }
    }
}
