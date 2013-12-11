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
            _stateSecondaryService = stateSecondaryService;
            _workingGroupService = workingGroupService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var statesecondaries = _stateSecondaryService.GetStateSecondaries(customer.Id).ToList();

            var model = new StateSecondaryIndexViewModel { StateSecondaries = statesecondaries, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var statesecondary = new StateSecondary { Customer_Id = customer.Id, IsActive = 1 };

            var model = CreateInputViewModel(statesecondary, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(StateSecondary stateSecondary)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _stateSecondaryService.SaveStateSecondary(stateSecondary, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "statesecondary", new { customerId = stateSecondary.Customer_Id });

            var customer = _customerService.GetCustomer(stateSecondary.Customer_Id);
            var model = CreateInputViewModel(stateSecondary, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var statesecondary = _stateSecondaryService.GetStateSecondary(id);

            if (statesecondary == null)                
                return new HttpNotFoundResult("No state secondary found...");

            var customer = _customerService.GetCustomer(statesecondary.Customer_Id);
            var model = CreateInputViewModel(statesecondary, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(StateSecondary stateSecondary)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _stateSecondaryService.SaveStateSecondary(stateSecondary, out errors);

            if (errors.Count == 0)                
                return RedirectToAction("index", "statesecondary", new { customerId = stateSecondary.Customer_Id });

            var customer = _customerService.GetCustomer(stateSecondary.Customer_Id);
            var model = CreateInputViewModel(stateSecondary, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var statesecondary = _stateSecondaryService.GetStateSecondary(id);

            if (_stateSecondaryService.DeleteStateSecondary(id) == DeleteMessage.Success)               
                return RedirectToAction("index", "statesecondary", new { customerId = statesecondary.Customer_Id });            
            else
            {
                TempData.Add("Error", "");                
                return RedirectToAction("edit", "statesecondary", new { area = "admin", id = statesecondary.Id });
            }
        }

        private StateSecondaryInputViewModel CreateInputViewModel(StateSecondary statesecondary, Customer customer)
        {
            var model = new StateSecondaryInputViewModel
            {
                StateSecondary = statesecondary,
                Customer = customer,
                WorkingGroups = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}

