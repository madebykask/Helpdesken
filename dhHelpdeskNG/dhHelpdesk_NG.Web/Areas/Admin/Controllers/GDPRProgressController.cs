using System.Web.Mvc;
using System.Web.SessionState;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    //its important to keep session readonly to prevent simaltenous ajax requests from blocking
    [SessionState(SessionStateBehavior.Disabled)]
    public class GDPRProgressController : Controller  // keep Controller as a base class for faster performance
    {
        private readonly IGDPROperationsService _gdprOperationsService;

        #region ctor()

        public GDPRProgressController(IGDPROperationsService gdprOperationsService)
        {
            _gdprOperationsService = gdprOperationsService;
        }

        #endregion

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        [AllowAnonymous]
        public JsonResult GetOperationProgress(int id)
        {
            var data = _gdprOperationsService.GetDataPrivacyOperationAuditData(id);
            return Json(new
            {
                isComplete = data.Status == GDPROperationStatus.Complete,
                Success = data.Success,
                Error = data.Error
            }, JsonRequestBehavior.AllowGet);
        }
    }
}