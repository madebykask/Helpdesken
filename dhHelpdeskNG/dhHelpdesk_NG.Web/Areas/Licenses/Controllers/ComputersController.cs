namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

    public class ComputersController : BaseController
    {
        private readonly IWorkContext workContext;

        private readonly IComputersService computersService;

        private readonly IComputersModelFactory computersModelFactory;

        public ComputersController(
                IMasterDataService masterDataService, 
                IWorkContext workContext, 
                IComputersService computersService, 
                IComputersModelFactory computersModelFactory)
            : base(masterDataService)
        {
            this.workContext = workContext;
            this.computersService = computersService;
            this.computersModelFactory = computersModelFactory;
        }

        [HttpGet]
        public ViewResult Index(int productId)
        {
            var data = this.computersService.GetComputerOverviews(this.workContext.Customer.CustomerId, productId);
            var model = this.computersModelFactory.GetContentModel(data);

            return this.View("Computers", model);
        }
    }
}
