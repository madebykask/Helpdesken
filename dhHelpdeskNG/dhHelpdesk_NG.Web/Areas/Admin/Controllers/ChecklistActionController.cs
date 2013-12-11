using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class ChecklistActionController : BaseController
    {
        private readonly IChecklistActionService _checklistActionService;
        private readonly IChecklistServiceService _checklistServiceService;

        public ChecklistActionController(
            IChecklistActionService checklistActionService,
            IChecklistServiceService checklistServiceService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _checklistActionService = checklistActionService;
            _checklistServiceService = checklistServiceService;
        }

        public ActionResult Index()
        {
            var checklistactions = _checklistActionService.GetChecklistActions().ToList();

            return View(checklistactions);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new ChecklistAction { IsActive = 1 });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(ChecklistAction checklistAction)
        {
            if (ModelState.IsValid)
            {
                _checklistActionService.NewChecklistAction(checklistAction);
                _checklistActionService.Commit();

                return RedirectToAction("index", "checklistaction", new { area = "admin" });
            }

            var model = CreateInputViewModel(checklistAction);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var checklistAction = _checklistActionService.GetChecklistAction(id);

            if (checklistAction == null)
                return new HttpNotFoundResult("No checklist action found...");

            var model = CreateInputViewModel(checklistAction);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChecklistAction checklistAction)
        {
            if (ModelState.IsValid)
            {
                _checklistActionService.UpdateChecklistAction(checklistAction);
                _checklistActionService.Commit();

                return RedirectToAction("index", "checklistaction", new { area = "admin" });
            }

            var model = CreateInputViewModel(checklistAction);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var checklistAction = _checklistActionService.GetChecklistAction(id);

            if (checklistAction != null)
            {
                _checklistActionService.DeleteChecklistAction(checklistAction);
                _checklistActionService.Commit();
            }

            return RedirectToAction("index", "checklistaction", new { area = "admin" });
        }

        private ChecklistActionInputViewModel CreateInputViewModel(ChecklistAction checklistAction)
        {
            var model = new ChecklistActionInputViewModel
            {
                ChecklistAction = checklistAction,
                ChecklistServices = _checklistServiceService.GetChecklistServices(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}