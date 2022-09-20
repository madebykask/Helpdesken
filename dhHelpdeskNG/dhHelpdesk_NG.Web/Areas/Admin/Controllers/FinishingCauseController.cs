namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class FinishingCauseController : BaseAdminController
    {
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICustomerService _customerService;

        public FinishingCauseController(
            IFinishingCauseService finishingCauseService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._finishingCauseService = finishingCauseService;
            this._customerService = customerService;
        }

        public JsonResult SetShowOnlyActiveFinishingCausesInAdmin(bool value)
        {
            SessionFacade.ShowOnlyActiveFinishingCausesInAdmin = value;
            return this.Json(new { result = "success" });
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            var finishingCauses = this._finishingCauseService.GetFinishingCauses(customer.Id).Where(x => x.Merged == false).ToList(); 

            var model = new FinishingCauseIndexViewModel { FinishingCauses = finishingCauses, Customer = customer, IsShowOnlyActive = SessionFacade.ShowOnlyActiveFinishingCausesInAdmin };
            return this.View(model);
        }

        public ActionResult New(int? parentId, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            if (parentId.HasValue)
            {
                if (this._finishingCauseService.GetFinishingCause(parentId.Value) == null)
                    return new HttpNotFoundResult("No parent finishing cause found...");
            }

            var finishingCause = new FinishingCause { Customer_Id = customer.Id, Parent_FinishingCause_Id = parentId, IsActive = 1 };
         
            var model = this.CreateInputViewModel(finishingCause, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(FinishingCause finishingCause)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._finishingCauseService.SaveFinishingCause(finishingCause, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "finishingcause", new { customerId = finishingCause.Customer_Id });

            var model = this.CreateInputViewModel(finishingCause, null);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var finishingCause = this._finishingCauseService.GetFinishingCause(id);

            if (finishingCause == null)
                return new HttpNotFoundResult("No finishing cause found...");

            var customer = this._customerService.GetCustomer(finishingCause.Customer_Id);
            var model = this.CreateInputViewModel(finishingCause, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(FinishingCause finishingCause)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._finishingCauseService.SaveFinishingCause(finishingCause, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "finishingcause", new { customerId = finishingCause.Customer_Id });

            var customer = this._customerService.GetCustomer(finishingCause.Customer_Id);
            var model = this.CreateInputViewModel(finishingCause, customer);

            return this.View(model);
        }

        public ActionResult Delete(int id)
        {

            var finishingCause = this._finishingCauseService.GetFinishingCause(id);

            if (this._finishingCauseService.DeleteFinishingCause(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "finishingcause", new { customerId = finishingCause.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "finishingcause", new { area = "admin", id = finishingCause.Id });
            }

            //var finishingCause = this._finishingCauseService.GetFinishingCause(id);

            ////check if there is subfinishingcause
            //var subfinishingcauses = this._finishingCauseService.GetSubFinishingCauses(finishingCause.Id);

            //if (this._finishingCauseService.DeleteFinishingCause(id) == DeleteMessage.Success)
            //{
            //    if (subfinishingcauses != null)
            //    {
            //        foreach (var sfc in subfinishingcauses)
            //        {
            //            this._finishingCauseService.DeleteFinishingCause(sfc.Id);
            //        }
            //    }
            //    return this.RedirectToAction("index", "finishingcause", new { customerId = finishingCause.Customer_Id });
            //}
            //else
            //{
            //    this.TempData.Add("Error", "");
            //    return this.RedirectToAction("edit", "finishingcause", new { area = "admin", id = finishingCause.Id });
            //}
        }

        private FinishingCauseInputViewModel CreateInputViewModel(FinishingCause finishingCause, Customer customer)
        {
            var model = new FinishingCauseInputViewModel
            {
                FinishingCause = finishingCause,
                Customer = customer,
                FinishingCauseCategories = this._finishingCauseService.GetFinishingCauseCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
