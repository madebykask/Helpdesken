namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

    public class VendorsController : BaseController
    {
        private readonly IVendorsService vendorsService;

        private readonly IWorkContext workContext;

        private readonly IVendorsModelFactory vendorsModelFactory;

        public VendorsController(
                IMasterDataService masterDataService, 
                IVendorsService vendorsService, 
                IWorkContext workContext, 
                IVendorsModelFactory vendorsModelFactory)
            : base(masterDataService)
        {
            this.vendorsService = vendorsService;
            this.workContext = workContext;
            this.vendorsModelFactory = vendorsModelFactory;
        }

        public ViewResult Index()
        {
            var model = this.vendorsModelFactory.GetIndexModel();
            return this.View();
        }
    }
}
