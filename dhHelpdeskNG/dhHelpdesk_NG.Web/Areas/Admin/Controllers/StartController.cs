namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class StartController : BaseController
    {
        public StartController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Clear Admin Sessions. Used by close administration button.
        /// </summary>
        [HttpPost]
        public void ClearAdminSessions()
        {
            ClearUserSearchSession();
        }

        /// <summary>
        /// Clear user search session.
        /// </summary>
        private void ClearUserSearchSession()
        {
            this.Session["UserSearch"] = null;
        }
    }
}
