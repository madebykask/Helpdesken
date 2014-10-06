namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Areas.Admin.Models;
    using DH.Helpdesk.Mobile.Infrastructure;

    public class OperationObjectController : BaseAdminController
    {
        private readonly ICaseService _caseService;
        private readonly IOperationObjectService _operationObjectService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly ICustomerService _customerService;

        public OperationObjectController(
            ICaseService caseService,
            IOperationObjectService operationObjectService,
            IWorkingGroupService workingGroupService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._caseService = caseService;
            this._operationObjectService = operationObjectService;
            this._workingGroupService = workingGroupService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var operationObjects = this._operationObjectService.GetOperationObjects(customer.Id);

            var model = new OperationObjectIndexViewModel { OperationObjects = operationObjects, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var operationObject = new OperationObject { Customer_Id = customer.Id, IsActive = 1 };
            var model = this.CreateInputViewModel(operationObject, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(OperationObject operationObject, OperationObjectInputViewModel vmodel)
        {
            operationObject.ShowOnStartPage = this.returnOperationObjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._operationObjectService.SaveOperationObject(operationObject, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "operationobject", new { customerId = operationObject.Customer_Id });

            var customer = this._customerService.GetCustomer(operationObject.Customer_Id);
            var model = this.CreateInputViewModel(operationObject, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var operationObject = this._operationObjectService.GetOperationObject(id);

            if (operationObject == null)
                return new HttpNotFoundResult("No operation object found...");

            var customer = this._customerService.GetCustomer(operationObject.Customer_Id);
            var model = this.CreateInputViewModel(operationObject, customer);
            model.StartPageShow = model.OperationObject.ShowOnStartPage;

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, OperationObjectInputViewModel vmodel)
        {
            var operationObjectToSave = this._operationObjectService.GetOperationObject(id);
            var b = this.TryUpdateModel(operationObjectToSave, "operationobject");

            operationObjectToSave.ShowOnStartPage = this.returnOperationObjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._operationObjectService.SaveOperationObject(operationObjectToSave, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "operationobject", new { customerId = operationObjectToSave.Customer_Id });

            var customer = this._customerService.GetCustomer(operationObjectToSave.Customer_Id);
            var model = this.CreateInputViewModel(operationObjectToSave, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var operationObject = this._operationObjectService.GetOperationObject(id);
            if (this._operationObjectService.DeleteOperationObject(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "operationobject", new { customerId = operationObject.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "operationobject", new { area = "admin", id = operationObject.Id });
            }
        }

        private OperationObjectInputViewModel CreateInputViewModel(OperationObject operationObject, Customer customer)
        {
            #region selectlistitem

            List<SelectListItem> sl = new List<SelectListItem>();
            for (int i = 0; i < 10; i++)
            {
                sl.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            #endregion

            #region model

            var model = new OperationObjectInputViewModel
            {
                OperationObject = operationObject,
                Customer = customer,
                NumberToShowOnStartPage = sl,
                WorkingGroups = this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList()
            };

            #endregion

            #region ints

            if (operationObject.ShowOnStartPage == 0)
            {
                model.ShowYesNo = 0;
            }
            else
            {
                model.ShowYesNo = 1;
            }

            #endregion

            return model;
        }

        private int returnOperationObjectForSave(OperationObjectInputViewModel model)
        {
            var show = model.OperationObject.ShowOnStartPage;

            if (model.ShowYesNo == 0)
            {
                show = 0;
            }
            else
            {
                show = model.StartPageShow;
            }

            return show;
        }
    }
}
