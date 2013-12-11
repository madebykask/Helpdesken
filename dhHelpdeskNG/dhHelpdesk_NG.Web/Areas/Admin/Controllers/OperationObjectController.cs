using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class OperationObjectController : BaseController
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
            _caseService = caseService;
            _operationObjectService = operationObjectService;
            _workingGroupService = workingGroupService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var operationObjects = _operationObjectService.GetOperationObjects(customer.Id);

            var model = new OperationObjectIndexViewModel { OperationObjects = operationObjects, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var operationObject = new OperationObject { Customer_Id = customer.Id, IsActive = 1 };
            var model = CreateInputViewModel(operationObject, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult New(OperationObject operationObject, OperationObjectInputViewModel vmodel)
        {
            operationObject.ShowOnStartPage = returnOperationObjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _operationObjectService.SaveOperationObject(operationObject, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "operationobject", new { customerId = operationObject.Customer_Id });

            var customer = _customerService.GetCustomer(operationObject.Customer_Id);
            var model = CreateInputViewModel(operationObject, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var operationObject = _operationObjectService.GetOperationObject(id);

            if (operationObject == null)
                return new HttpNotFoundResult("No operation object found...");

            var customer = _customerService.GetCustomer(operationObject.Customer_Id);
            var model = CreateInputViewModel(operationObject, customer);
            model.StartPageShow = model.OperationObject.ShowOnStartPage;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, OperationObjectInputViewModel vmodel)
        {
            var operationObjectToSave = _operationObjectService.GetOperationObject(id);
            var b = TryUpdateModel(operationObjectToSave, "operationobject");

            operationObjectToSave.ShowOnStartPage = returnOperationObjectForSave(vmodel);

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _operationObjectService.SaveOperationObject(operationObjectToSave, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "operationobject", new { customerId = operationObjectToSave.Customer_Id });

            var customer = _customerService.GetCustomer(operationObjectToSave.Customer_Id);
            var model = CreateInputViewModel(operationObjectToSave, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var operationObject = _operationObjectService.GetOperationObject(id);
            if (_operationObjectService.DeleteOperationObject(id) == DeleteMessage.Success)
                return RedirectToAction("index", "operationobject", new { customerId = operationObject.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "operationobject", new { area = "admin", id = operationObject.Id });
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
                WorkingGroups = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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
