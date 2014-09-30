namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Areas.Licenses.Infrastructure.ModelFactories;
    using DH.Helpdesk.Web.Infrastructure;

    public class LicensesController : BaseController
    {
        private readonly ILicensesService licensesService;

        private readonly IWorkContext workContext;

        private readonly ILicensesModelFactory licensesModelFactory;

        public LicensesController(
                IMasterDataService masterDataService, 
                ILicensesService licensesService, 
                IWorkContext workContext, 
                ILicensesModelFactory licensesModelFactory)
            : base(masterDataService)
        {
            this.licensesService = licensesService;
            this.workContext = workContext;
            this.licensesModelFactory = licensesModelFactory;
        }

        public ViewResult Index()
        {
            var model = this.licensesModelFactory.GetIndexModel();
            return this.View(model);
        }
    }
}
