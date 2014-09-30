namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        private readonly IWorkContext workContext;

        private readonly IProductsModelFactory productsModelFactory;

        public ProductsController(
                IMasterDataService masterDataService, 
                IProductsService productsService, 
                IWorkContext workContext, 
                IProductsModelFactory productsModelFactory)
            : base(masterDataService)
        {
            this.productsService = productsService;
            this.workContext = workContext;
            this.productsModelFactory = productsModelFactory;
        }

        public ViewResult Index()
        {
            var model = this.productsModelFactory.GetIndexModel();
            return this.View(model);
        }
    }
}
