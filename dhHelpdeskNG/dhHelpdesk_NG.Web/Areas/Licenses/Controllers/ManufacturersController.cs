namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

    public class ManufacturersController : BaseController
    {
        private readonly IManufacturersService manufacturersService;

        private readonly IWorkContext workContext;

        private readonly IManufacturersModelFactory manufacturersModelFactory;

        public ManufacturersController(
                IMasterDataService masterDataService, 
                IManufacturersService manufacturersService, 
                IWorkContext workContext, 
                IManufacturersModelFactory manufacturersModelFactory)
            : base(masterDataService)
        {
            this.manufacturersService = manufacturersService;
            this.workContext = workContext;
            this.manufacturersModelFactory = manufacturersModelFactory;
        }

        public ViewResult Index()
        {
            var model = this.manufacturersModelFactory.GetIndexModel();
            return this.View(model);
        }
    }
}
