namespace H.Helpdesk.NewSelfService.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;

    public class HomeController : Controller
    {
        private readonly ICustomerService _customerService;

        
        public HomeController(
            ICustomerService customerService,
            IMasterDataService masterDataService)
            
        {
            this._customerService = customerService;
        }
        
        public RedirectToRouteResult Index(int customerId=-1)
        {     
           return RedirectToAction("Index", "Start", new { customerId = customerId });                      
        }
    }
}
