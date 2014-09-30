namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Infrastructure;

    public class VendorsController : BaseController
    {
        private readonly IVendorsService vendorsService;

        public VendorsController(
                IMasterDataService masterDataService, 
                IVendorsService vendorsService)
            : base(masterDataService)
        {
            this.vendorsService = vendorsService;
        }
    }
}
