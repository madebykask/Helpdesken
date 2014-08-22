namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class ChecklistActionController : BaseAdminController
    {
        private readonly IChecklistActionService _checklistActionService;
        private readonly IChecklistServiceService _checklistServiceService;

        public ChecklistActionController(
            IChecklistActionService checklistActionService,
            IChecklistServiceService checklistServiceService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._checklistActionService = checklistActionService;
            this._checklistServiceService = checklistServiceService;
        }

        public ActionResult Index()
        {
            var checklistactions = this._checklistActionService.GetChecklistActions().ToList();

            return this.View(checklistactions);
        }

        public ActionResult New()
        {
            var model = this.CreateInputViewModel(new ChecklistAction { IsActive = 1 });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ChecklistAction checklistAction)
        {
            if (this.ModelState.IsValid)
            {
                this._checklistActionService.NewChecklistAction(checklistAction);
                this._checklistActionService.Commit();

                return this.RedirectToAction("index", "checklistaction", new { area = "admin" });
            }

            var model = this.CreateInputViewModel(checklistAction);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var checklistAction = this._checklistActionService.GetChecklistAction(id);

            if (checklistAction == null)
                return new HttpNotFoundResult("No checklist action found...");

            var model = this.CreateInputViewModel(checklistAction);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChecklistAction checklistAction)
        {
            if (this.ModelState.IsValid)
            {
                this._checklistActionService.UpdateChecklistAction(checklistAction);
                this._checklistActionService.Commit();

                return this.RedirectToAction("index", "checklistaction", new { area = "admin" });
            }

            var model = this.CreateInputViewModel(checklistAction);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var checklistAction = this._checklistActionService.GetChecklistAction(id);

            if (checklistAction != null)
            {
                this._checklistActionService.DeleteChecklistAction(checklistAction);
                this._checklistActionService.Commit();
            }

            return this.RedirectToAction("index", "checklistaction", new { area = "admin" });
        }

        private ChecklistActionInputViewModel CreateInputViewModel(ChecklistAction checklistAction)
        {
            var model = new ChecklistActionInputViewModel
            {
                ChecklistAction = checklistAction,
                ChecklistServices = this._checklistServiceService.GetChecklistServices(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}