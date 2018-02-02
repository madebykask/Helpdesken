using DH.Helpdesk.Web.Infrastructure.Attributes;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    [CustomAuthorize(Roles = "3,4")]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }
    }
}
