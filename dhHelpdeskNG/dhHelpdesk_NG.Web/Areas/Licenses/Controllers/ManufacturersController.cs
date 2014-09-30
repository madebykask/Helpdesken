namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Infrastructure;

    public class ManufacturersController : BaseController
    {
        private readonly IManufacturersService manufacturersService;

        public ManufacturersController(
                IMasterDataService masterDataService, 
                IManufacturersService manufacturersService)
            : base(masterDataService)
        {
            this.manufacturersService = manufacturersService;
        }
    }
}
