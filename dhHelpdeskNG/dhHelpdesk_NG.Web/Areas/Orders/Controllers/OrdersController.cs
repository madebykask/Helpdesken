namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Web.Infrastructure;

    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(
                IMasterDataService masterDataService, 
                IOrdersService ordersService)
            : base(masterDataService)
        {
            this.ordersService = ordersService;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
