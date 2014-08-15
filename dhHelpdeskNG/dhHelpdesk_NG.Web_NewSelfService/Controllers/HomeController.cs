namespace H.Helpdesk.NewSelfService.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.NewSelfService.Infrastructure;

    public class HomeController : BaseController
    {                
        public HomeController( IMasterDataService masterDataService,           
                               ICaseSolutionService caseSolutionService,
                               ISSOService ssoService
                              ):base(masterDataService, ssoService, caseSolutionService)
            
        {
            //this._customerService = customerService;
        }
        
        public RedirectToRouteResult Index(int customerId=-1)
        {     
           return RedirectToAction("Index", "Start", new { customerId = customerId });                      
        }
    }
}
