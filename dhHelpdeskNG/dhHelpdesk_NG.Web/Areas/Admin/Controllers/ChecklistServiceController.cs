namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class ChecklistServiceController : BaseAdminController
    {
        private readonly IChecklistServiceService _checklistServiceService;

        public ChecklistServiceController(
            IChecklistServiceService checklistServiceService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._checklistServiceService = checklistServiceService;
        }

        public ActionResult Index()
        {
            var checklistservices = this._checklistServiceService.GetChecklistServices(SessionFacade.CurrentCustomer.Id).ToList();
            
            return this.View(checklistservices);
        }

        public ActionResult New()
        {
            return this.View(new Domain.ChecklistService() { Customer_Id = SessionFacade.CurrentCustomer.Id, IsActive = 1 });
        }

        [HttpPost]
        public ActionResult New(Domain.ChecklistService checklistService)
        {
            if (this.ModelState.IsValid)
            {
                //this._checklistServiceService.NewChecklistService(checklistService);
                this._checklistServiceService.Commit();

                return this.RedirectToAction("index", "checklistservice", new { area = "admin" });
            }

            return this.View(checklistService);
        }

        public ActionResult Edit(int id)
        {
            var checklistService = this._checklistServiceService.GetChecklistService(id, SessionFacade.CurrentCustomer.Id);

            if (checklistService == null)                
                return new HttpNotFoundResult("No checklist service found...");

            return this.View(checklistService);
        }

        [HttpPost]
        public ActionResult Edit(Domain.ChecklistService checklistService)
        {
            if (this.ModelState.IsValid)
            {
                this._checklistServiceService.UpdateChecklistService(checklistService);
                this._checklistServiceService.Commit();

                return this.RedirectToAction("index", "checklistservice", new { area = "admin" });
            }

            return this.View(checklistService);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var checklistService = this._checklistServiceService.GetChecklistService(id, SessionFacade.CurrentCustomer.Id);

            if (checklistService != null)
            {
                this._checklistServiceService.DeleteChecklistService(checklistService);
                this._checklistServiceService.Commit();
            }

            return this.RedirectToAction("index", "checklistservice", new { area = "admin" });
        }
    }
}
