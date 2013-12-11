using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class DivisionController : BaseController
    {
        private readonly IDivisionService _divisionService;

        public DivisionController(
            IDivisionService divisionService,
            IMasterDataService masterDataService)
            : base (masterDataService)
        {
            _divisionService = divisionService;
        }

        public ActionResult Index()
        {
            var division = _divisionService.GetDivisions(SessionFacade.CurrentCustomer.Id);

            return View(division);
        }

        public ActionResult New()
        {
            return View(new Division() { Customer_Id = SessionFacade.CurrentCustomer.Id });
        }

        [HttpPost]
        public ActionResult New(Division division)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _divisionService.SaveDivision(division, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "division");

            return View(division);
        }

        public ActionResult Edit(int id)
        {
            var division = _divisionService.GetDivision(id);

            if (division == null)
                return new HttpNotFoundResult("No division found...");

            return View(division);
        }

        [HttpPost]
        public ActionResult Edit(Division division)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _divisionService.SaveDivision(division, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "division");

            return View(division);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_divisionService.DeleteDivision(id) == DeleteMessage.Success)
                return RedirectToAction("index", "division");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "division", new { id = id });
            }
        }
    }
}
