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
            this._workingGroupService = workingGroupService;
            this._stateSecondaryService = stateSecondaryService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var workingGroups = this._workingGroupService.GetWorkingGroupsForIndexPage(customer.Id);

            var model = new WorkingGroupIndexViewModel { WorkingGroup = workingGroups, Customer = customer };

            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var workingGroup = new WorkingGroupEntity { Customer_Id = customer.Id, IsActive = 1 };
           
            var model = this.CreateInputViewModel(workingGroup, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(WorkingGroupEntity workingGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._workingGroupService.SaveWorkingGroup(workingGroup, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "workinggroup", new { customerid = workingGroup.Customer_Id });

            var customer = this._customerService.GetCustomer(workingGroup.Customer_Id);
            var model = this.CreateInputViewModel(workingGroup, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var workingGroup = this._workingGroupService.GetWorkingGroup(id);
            //var usersForWorkingGroup = _workingGroupService.GetUsersForWorkingGroup(workingGroup.Id);

            if (workingGroup == null)
                return new HttpNotFoundResult("No working group found...");

            var customer = this._customerService.GetCustomer(workingGroup.Customer_Id);
            var model = this.CreateInputViewModel(workingGroup, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(WorkingGroupEntity workingGroup)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();


            this._workingGroupService.SaveWorkingGroup(workingGroup, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "workinggroup", new { customerid = workingGroup.Customer_Id });

            var customer = this._customerService.GetCustomer(workingGroup.Customer_Id);
            var model = this.CreateInputViewModel(workingGroup, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var workingGroup = this._workingGroupService.GetWorkingGroup(id);

            if (this._workingGroupService.DeleteWorkingGroup(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "workinggroup", new { customerid = workingGroup.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "workinggroup", new { acustomerid = workingGroup.Customer_Id, id = id });
            }
        }

        private WorkingGroupInputViewModel CreateInputViewModel(WorkingGroupEntity workinggroup, Customer customer)
        {
            var model = new WorkingGroupInputViewModel
            {
                WorkingGroup = workinggroup,
                Customer = customer,
                UsersForWorkingGroup = this._workingGroupService.GetUsersForWorkingGroup(workinggroup.Id),
                StateSecondaries = this._stateSecondaryService.GetStateSecondaries(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    
    }
}
