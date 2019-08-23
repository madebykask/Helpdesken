using System.Web.Mvc;
using System.Web.SessionState;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    //its important to keep session readonly to prevent simaltenous ajax requests from blocking
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class GDPRProgressController : Controller  // keep Controller as a base class for faster performance
    {
        private readonly IGDPRTasksService _gdprTasksService;
        private readonly IGDPROperationsService _gdprOperationsService;

        #region ctor()

        public GDPRProgressController(IGDPRTasksService  gdprTasksService)
        {
            _gdprTasksService = gdprTasksService;
        }

        #endregion

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        [AllowAnonymous]
        public JsonResult GetTaskProgress(int id)
        {
            var data = _gdprTasksService.GetById(id);
            return Json(new
            {
                isComplete = data.Status == GDPRTaskStatus.Complete,
                Progress = data.Progress,
                Success = data.Success,
                Error = data.Error
            }, JsonRequestBehavior.AllowGet);
        }
    }
}