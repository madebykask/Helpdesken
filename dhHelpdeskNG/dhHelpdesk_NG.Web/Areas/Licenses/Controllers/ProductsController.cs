namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Infrastructure;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        private readonly IWorkContext workContext;

        public ProductsController(
                IMasterDataService masterDataService, 
                IProductsService productsService, 
                IWorkContext workContext)
            : base(masterDataService)
        {
            this.productsService = productsService;
            this.workContext = workContext;
        }

        public ViewResult Index()
        {
            return this.View();
        }
    }
}
