namespace DH.Helpdesk.Web.Areas.Licenses.Controllers
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Licenses;
    using DH.Helpdesk.Web.Infrastructure;

    public class LicensesController : BaseController
    {
        private readonly ILicensesService licensesService;

        public LicensesController(
                IMasterDataService masterDataService, 
                ILicensesService licensesService)
            : base(masterDataService)
        {
            this.licensesService = licensesService;
        }
    }
}
