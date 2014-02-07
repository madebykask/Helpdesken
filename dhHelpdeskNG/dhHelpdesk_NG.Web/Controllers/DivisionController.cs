namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class DivisionController : BaseController
    {
        private readonly IDivisionService _divisionService;

        public DivisionController(
            IDivisionService divisionService,
            IMasterDataService masterDataService)
            : base (masterDataService)
        {
            this._divisionService = divisionService;
        }

        public ActionResult Index()
        {
            var division = this._divisionService.GetDivisions(SessionFacade.CurrentCustomer.Id);

            return this.View(division);
        }

        public ActionResult New()
        {
            return this.View(new Division() { Customer_Id = SessionFacade.CurrentCustomer.Id });
        }

        [HttpPost]
        public ActionResult New(Division division)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._divisionService.SaveDivision(division, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "division");

            return this.View(division);
        }

        public ActionResult Edit(int id)
        {
            var division = this._divisionService.GetDivision(id);

            if (division == null)
                return new HttpNotFoundResult("No division found...");

            return this.View(division);
        }

        [HttpPost]
        public ActionResult Edit(Division division)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._divisionService.SaveDivision(division, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "division");

            return this.View(division);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._divisionService.DeleteDivision(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "division");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "division", new { id = id });
            }
        }
    }
}
