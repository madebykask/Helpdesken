namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    [CustomAuthorize(Roles = "4")]
    public class StateSecondaryController : BaseController
    {
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICustomerService _customerService;

        public StateSecondaryController(
            IStateSecondaryService stateSecondaryService,
            IWorkingGroupService workingGroupService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._stateSecondaryService = stateSecondaryService;
            this._workingGroupService = workingGroupService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var statesecondaries = this._stateSecondaryService.GetStateSecondaries(customer.Id).ToList();

            var model = new StateSecondaryIndexViewModel { StateSecondaries = statesecondaries, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var statesecondary = new StateSecondary { Customer_Id = customer.Id, IsActive = 1 };

            var model = this.CreateInputViewModel(statesecondary, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(StateSecondary stateSecondary)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._stateSecondaryService.SaveStateSecondary(stateSecondary, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "statesecondary", new { customerId = stateSecondary.Customer_Id });

            var customer = this._customerService.GetCustomer(stateSecondary.Customer_Id);
            var model = this.CreateInputViewModel(stateSecondary, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var statesecondary = this._stateSecondaryService.GetStateSecondary(id);

            if (statesecondary == null)                
                return new HttpNotFoundResult("No state secondary found...");

            var customer = this._customerService.GetCustomer(statesecondary.Customer_Id);
            var model = this.CreateInputViewModel(statesecondary, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(StateSecondary stateSecondary)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._stateSecondaryService.SaveStateSecondary(stateSecondary, out errors);

            if (errors.Count == 0)                
                return this.RedirectToAction("index", "statesecondary", new { customerId = stateSecondary.Customer_Id });

            var customer = this._customerService.GetCustomer(stateSecondary.Customer_Id);
            var model = this.CreateInputViewModel(stateSecondary, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var statesecondary = this._stateSecondaryService.GetStateSecondary(id);

            if (this._stateSecondaryService.DeleteStateSecondary(id) == DeleteMessage.Success)               
                return this.RedirectToAction("index", "statesecondary", new { customerId = statesecondary.Customer_Id });            
            else
            {
                this.TempData.Add("Error", "");                
                return this.RedirectToAction("edit", "statesecondary", new { area = "admin", id = statesecondary.Id });
            }
        }

        private StateSecondaryInputViewModel CreateInputViewModel(StateSecondary statesecondary, Customer customer)
        {
            var model = new StateSecondaryInputViewModel
            {
                StateSecondary = statesecondary,
                Customer = customer,
                WorkingGroups = this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}

