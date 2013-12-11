using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class WorkingGroupController : BaseController
    {
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICustomerService _customerService;

        public WorkingGroupController(
            IWorkingGroupService workingGroupService,
            IStateSecondaryService stateSecondaryService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _workingGroupService = workingGroupService;
            _stateSecondaryService = stateSecondaryService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var workingGroups = _workingGroupService.GetWorkingGroups(customer.Id);

            var model = new WorkingGroupIndexViewModel { WorkingGroup = workingGroups, Customer = customer };

            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var workingGroup = new WorkingGroup { Customer_Id = customer.Id, IsActive = 1 };
           
            var model = CreateInputViewModel(workingGroup, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(WorkingGroup workingGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _workingGroupService.SaveWorkingGroup(workingGroup, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "workinggroup", new { customerid = workingGroup.Customer_Id });

            var customer = _customerService.GetCustomer(workingGroup.Customer_Id);
            var model = CreateInputViewModel(workingGroup, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var workingGroup = _workingGroupService.GetWorkingGroup(id);

            if (workingGroup == null)
                return new HttpNotFoundResult("No working group found...");

            var customer = _customerService.GetCustomer(workingGroup.Customer_Id);
            var model = CreateInputViewModel(workingGroup, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(WorkingGroup workingGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _workingGroupService.SaveWorkingGroup(workingGroup, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "workinggroup", new { customerid = workingGroup.Customer_Id });

            var customer = _customerService.GetCustomer(workingGroup.Customer_Id);
            var model = CreateInputViewModel(workingGroup, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var workingGroup = _workingGroupService.GetWorkingGroup(id);

            if (_workingGroupService.DeleteWorkingGroup(id) == DeleteMessage.Success)
                return RedirectToAction("index", "workinggroup", new { customerid = workingGroup.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "workinggroup", new { acustomerid = workingGroup.Customer_Id, id = id });
            }
        }

        private WorkingGroupInputViewModel CreateInputViewModel(WorkingGroup workinggroup, Customer customer)
        {
            var model = new WorkingGroupInputViewModel
            {
                WorkingGroup = workinggroup,
                Customer = customer,
                StateSecondaries = _stateSecondaryService.GetStateSecondaries(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    
    }
}
