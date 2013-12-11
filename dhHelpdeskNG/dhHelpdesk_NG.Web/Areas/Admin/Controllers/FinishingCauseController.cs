using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "4")]
    public class FinishingCauseController : BaseController
    {
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICustomerService _customerService;

        public FinishingCauseController(
            IFinishingCauseService finishingCauseService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _finishingCauseService = finishingCauseService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            var finishingCauses = _finishingCauseService.GetFinishingCauses(customer.Id);

            var model = new FinishingCauseIndexViewModel { FinishingCauses = finishingCauses, Customer = customer };
            return View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (_finishingCauseService.GetFinishingCause(parentId.Value) == null)
                    return new HttpNotFoundResult("No parent finishing cause found...");
            }

            var finishingCause = new FinishingCause { Customer_Id = customer.Id, Parent_FinishingCause_Id = parentId, IsActive = 1 };
         
            var model = CreateInputViewModel(finishingCause, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(FinishingCause finishingCause)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _finishingCauseService.SaveFinishingCause(finishingCause, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "finishingcause", new { customerId = finishingCause.Customer_Id });

            var model = CreateInputViewModel(finishingCause, null);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var finishingCause = _finishingCauseService.GetFinishingCause(id);

            if (finishingCause == null)
                return new HttpNotFoundResult("No finishing cause found...");

            var customer = _customerService.GetCustomer(finishingCause.Customer_Id);
            var model = CreateInputViewModel(finishingCause, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FinishingCause finishingCause)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _finishingCauseService.SaveFinishingCause(finishingCause, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "finishingcause", new { customerId = finishingCause.Customer_Id });

            var customer = _customerService.GetCustomer(finishingCause.Customer_Id);
            var model = CreateInputViewModel(finishingCause, customer);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var finishingCause = _finishingCauseService.GetFinishingCause(id);

            if (_finishingCauseService.DeleteFinishingCause(id) == DeleteMessage.Success)
                return RedirectToAction("index", "finishingcause", new { customerId = finishingCause.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "finishingcause", new { area = "admin", id = finishingCause.Id });
            }
        }

        private FinishingCauseInputViewModel CreateInputViewModel(FinishingCause finishingCause, Customer customer)
        {
            var model = new FinishingCauseInputViewModel
            {
                FinishingCause = finishingCause,
                Customer = customer,
                FinishingCauseCategories = _finishingCauseService.GetFinishingCauseCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
