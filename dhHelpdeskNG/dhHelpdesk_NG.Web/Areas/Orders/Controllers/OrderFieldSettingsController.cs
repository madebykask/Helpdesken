namespace DH.Helpdesk.Web.Areas.Orders.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Orders;
    using DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

    public class OrderFieldSettingsController : BaseController
    {
        private readonly IOrderFieldSettingsService orderFieldSettingsService;

        private readonly IOrderFieldSettingsModelFactory orderFieldSettingsModelFactory;

        private readonly IWorkContext workContext;

        public OrderFieldSettingsController(
                IMasterDataService masterDataService, 
                IOrderFieldSettingsService orderFieldSettingsService, 
                IWorkContext workContext, 
                IOrderFieldSettingsModelFactory orderFieldSettingsModelFactory)
            : base(masterDataService)
        {
            this.orderFieldSettingsService = orderFieldSettingsService;
            this.workContext = workContext;
            this.orderFieldSettingsModelFactory = orderFieldSettingsModelFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
