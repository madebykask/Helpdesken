namespace DH.Helpdesk.SelfService.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.SelfService.Infrastructure;

    public class HomeController : BaseController
    {                
        public HomeController( IMasterDataService masterDataService,           
                               ICaseSolutionService caseSolutionService                               
                              ):base(masterDataService, caseSolutionService)
            
        {
            //this._customerService = customerService;
        }
        
        public RedirectToRouteResult Index(int customerId = -1)
        {     
           return RedirectToAction("Index", "Start", new { customerId });                      
        }
    }
}
