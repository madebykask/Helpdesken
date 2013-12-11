using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class ChecklistServiceController : BaseController
    {
        private readonly IChecklistServiceService _checklistServiceService;

        public ChecklistServiceController(
            IChecklistServiceService checklistServiceService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _checklistServiceService = checklistServiceService;
        }

        public ActionResult Index()
        {
            var checklistservices = _checklistServiceService.GetChecklistServices(SessionFacade.CurrentCustomer.Id).ToList();
            
            return View(checklistservices);
        }

        public ActionResult New()
        {
            return View(new Domain.ChecklistService() { Customer_Id = SessionFacade.CurrentCustomer.Id, IsActive = 1 });
        }

        [HttpPost]
        public ActionResult New(Domain.ChecklistService checklistService)
        {
            if (ModelState.IsValid)
            {
                _checklistServiceService.NewChecklistService(checklistService);
                _checklistServiceService.Commit();

                return RedirectToAction("index", "checklistservice", new { area = "admin" });
            }

            return View(checklistService);
        }

        public ActionResult Edit(int id)
        {
            var checklistService = _checklistServiceService.GetChecklistService(id, SessionFacade.CurrentCustomer.Id);

            if (checklistService == null)                
                return new HttpNotFoundResult("No checklist service found...");

            return View(checklistService);
        }

        [HttpPost]
        public ActionResult Edit(Domain.ChecklistService checklistService)
        {
            if (ModelState.IsValid)
            {
                _checklistServiceService.UpdateChecklistService(checklistService);
                _checklistServiceService.Commit();

                return RedirectToAction("index", "checklistservice", new { area = "admin" });
            }

            return View(checklistService);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var checklistService = _checklistServiceService.GetChecklistService(id, SessionFacade.CurrentCustomer.Id);

            if (checklistService != null)
            {
                _checklistServiceService.DeleteChecklistService(checklistService);
                _checklistServiceService.Commit();
            }

            return RedirectToAction("index", "checklistservice", new { area = "admin" });
        }
    }
}
