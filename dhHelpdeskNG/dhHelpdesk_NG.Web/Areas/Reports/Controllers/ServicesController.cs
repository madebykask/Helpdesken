namespace DH.Helpdesk.Web.Areas.Reports.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class ServicesController : BaseController
    {
        private readonly IUserService userService;

        private readonly IWorkContext workContext;

        public ServicesController(
            IMasterDataService masterDataService, 
            IUserService userService, 
            IWorkContext workContext)
            : base(masterDataService)
        {
            this.userService = userService;
            this.workContext = workContext;
        }

        [HttpGet]
        public JsonResult GetWorkingGroupUsers(int? workingGroupId)
        {
            var users = this.userService.GetWorkingGroupUsers(this.workContext.Customer.CustomerId, workingGroupId);

            return this.Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
