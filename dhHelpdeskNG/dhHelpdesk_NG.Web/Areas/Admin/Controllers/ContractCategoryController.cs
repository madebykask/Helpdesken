using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class ContractCategoryController : BaseController
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly IContractCategoryService _contractCategoryService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICustomerService _customerService;

        public ContractCategoryController(
            ICaseTypeService caseTypeService,
            IContractCategoryService contractCategoryService,
            IStateSecondaryService stateSecondaryService,
            ICustomerService customerService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _caseTypeService = caseTypeService;
            _contractCategoryService = contractCategoryService;
            _stateSecondaryService = stateSecondaryService;
            _customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var contractCategories = _contractCategoryService.GetContractCategories(customer.Id);

            var model = new ContractCategoryIndexViewModel { ContractCategories = contractCategories, Customer = customer };
            return View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var contractCategory = new ContractCategory { Customer_Id = customer.Id };
            var model = CreateInputViewModel(contractCategory, customer);
           
            return View(model);
        }

        [HttpPost]
        public ActionResult New(ContractCategory contractCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _contractCategoryService.SaveContractCategory(contractCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "contractcategory", new { customerId = contractCategory.Customer_Id });

            var customer = _customerService.GetCustomer(contractCategory.Customer_Id);
            var model = CreateInputViewModel(contractCategory, customer);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var contractCategory = _contractCategoryService.GetContractCategory(id);

            if (contractCategory == null)
                return new HttpNotFoundResult("No contract category found");

            var customer = _customerService.GetCustomer(contractCategory.Customer_Id);
            var model = CreateInputViewModel(contractCategory, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ContractCategory contractCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _contractCategoryService.SaveContractCategory(contractCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "contractcategory", new { customerId = contractCategory.Customer_Id });

            var customer = _customerService.GetCustomer(contractCategory.Customer_Id);
            var model = CreateInputViewModel(contractCategory, customer);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var contractCategory = _contractCategoryService.GetContractCategory(id);

            if (_contractCategoryService.DeleteContractCategory(id) == DeleteMessage.Success)
                return RedirectToAction("index", "contractcategory", new { customerId = contractCategory.Customer_Id });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "contractcategory", new { area = "admin", id = contractCategory.Id });
            }
        }

        private ContractCategoryInputViewModel CreateInputViewModel(ContractCategory contractCategory, Customer customer)
        {
            var model = new ContractCategoryInputViewModel
            {
                ContractCategory = contractCategory,
                Customer = customer,
                CaseType = _caseTypeService.GetCaseTypes(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                StateSecondary = _stateSecondaryService.GetStateSecondaries(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            return model;
        }
    }
}
