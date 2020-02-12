using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class ContractCategoryController : BaseAdminController
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
            this._caseTypeService = caseTypeService;
            this._contractCategoryService = contractCategoryService;
            this._stateSecondaryService = stateSecondaryService;
            this._customerService = customerService;
        }

        public ActionResult Index(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var contractCategories = this._contractCategoryService.GetContractCategories(customer.Id);

            var model = new ContractCategoryIndexViewModel { ContractCategories = contractCategories, Customer = customer };
            return this.View(model);
        }

        public ActionResult New(int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var contractCategory = new ContractCategory { Customer_Id = customer.Id };
            var model = this.CreateInputViewModel(contractCategory, customer);
           
            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(ContractCategory contractCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._contractCategoryService.SaveContractCategory(contractCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "contractcategory", new { customerId = contractCategory.Customer_Id });

            var customer = this._customerService.GetCustomer(contractCategory.Customer_Id);
            var model = this.CreateInputViewModel(contractCategory, customer);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var contractCategory = this._contractCategoryService.GetContractCategory(id);

            if (contractCategory == null)
                return new HttpNotFoundResult("No contract category found");

            var customer = this._customerService.GetCustomer(contractCategory.Customer_Id);
            var model = this.CreateInputViewModel(contractCategory, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(ContractCategory contractCategory, int? caseType_Id)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            contractCategory.CaseType_Id = caseType_Id;
            this._contractCategoryService.SaveContractCategory(contractCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "contractcategory", new { customerId = contractCategory.Customer_Id });

            var customer = this._customerService.GetCustomer(contractCategory.Customer_Id);
            var model = this.CreateInputViewModel(contractCategory, customer);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var contractCategory = this._contractCategoryService.GetContractCategory(id);

            if (this._contractCategoryService.DeleteContractCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "contractcategory", new { customerId = contractCategory.Customer_Id });
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "contractcategory", new { area = "admin", id = contractCategory.Id });
            }
        }

        private ContractCategoryInputViewModel CreateInputViewModel(ContractCategory contractCategory, Customer customer)
        {           
            var model = new ContractCategoryInputViewModel
            {
                ContractCategory = contractCategory,
                Customer = customer,
                CaseTypes = this._caseTypeService.GetCaseTypesOverviewWithChildren(contractCategory.Customer_Id, true).OrderBy(c => Translation.GetMasterDataTranslation(c.Name)).ToList(),
                CaseType_Id = contractCategory.CaseType_Id == null ? 0 : contractCategory.CaseType_Id.Value ,
                ParentPath_CaseType = "--",
                
                StateSecondary = this._stateSecondaryService.GetActiveStateSecondaries(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };


            if (model.CaseType_Id > 0)
            {
                var c = _caseTypeService.GetCaseType(contractCategory.CaseType_Id.Value);
                if (c != null)
                {
                    c = Translation.TranslateCaseType(c);
                    model.ParentPath_CaseType = c.getCaseTypeParentPath();
                    model.ContractCategory.CreateCase_CaseType = c;
                }                
            }
            return model;
        }
    }
}
