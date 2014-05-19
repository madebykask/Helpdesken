namespace DH.Helpdesk.NewSelfService.Controllers
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
        
        public ActionResult Index()
        {
            var customer = this._customerService.GetCustomer(1);

            return this.View(customer);

        }
    }
}
