namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Infrastructure;

    [CustomAuthorize(Roles = "3,4")]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }
    }
}
