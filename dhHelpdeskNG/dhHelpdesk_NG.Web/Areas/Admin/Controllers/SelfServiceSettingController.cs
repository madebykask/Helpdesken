using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.BusinessData.Models.ComputerUsers;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{    

    public class SelfServiceSettingController : BaseAdminController
    {

        private readonly ICustomerService _customerService;
        private readonly ISettingService _settingService;
        private readonly IDocumentService _documentService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;
        private readonly IComputerService _computerService;
        private readonly IFeatureToggleService _featureToggleService;

        public SelfServiceSettingController(
                ICustomerService customerService,
                ISettingService settingService,
                IDocumentService documentService,
                ICaseTypeService caseTypeService,
                IProductAreaService productAreaService,
                IMasterDataService masterDataService,
                IFeatureToggleService featureToggleService,
                IComputerService computerService)
            : base(masterDataService)
        {
            this._customerService = customerService;
            this._settingService = settingService;
            this._documentService = documentService;
            this._caseTypeService = caseTypeService;
            this._productAreaService = productAreaService;
            _featureToggleService = featureToggleService;
            _computerService = computerService;
        }
        //
        // GET: /Admin/SelfServiceSetting/

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var setting = _settingService.GetCustomerSetting(customerId);
            var allCategories = _documentService.GetDocumentCategories(customerId);
            var availableCats = allCategories.Where(c=> c.ShowOnExternalPage == false).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            var selectedCats = allCategories.Where(c => c.ShowOnExternalPage).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var allInitiators = new List<ComputerUserShort>();
            var availableInitiators = new List<SelectListItem>();
            var selectedInitiators = new List<SelectListItem>();
            if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_SELFSERVICE_SETTING_VIEW_DEPARTMENT_CASES))
            {
                allInitiators = _computerService.GetComputerUsersShort(customerId)
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.SurName)
                .ThenBy(a => a.UserId)
                .ToList();

                 availableInitiators = allInitiators.Where(c => !c.ShowOnExtPageDepartmentCases)
                        .Select(x => new SelectListItem
                        {
                            Text = string.Format("{1} {2} - {0}", x.UserId, x.FirstName, x.SurName),
                            Value = x.Id.ToString()
                        })
                        .ToList();


                selectedInitiators = allInitiators.Where(c => c.ShowOnExtPageDepartmentCases)
                       .Select(x => new SelectListItem
                       {
                           Text = string.Format("{1} {2} - {0}", x.UserId, x.FirstName, x.SurName),
                           Value = x.Id.ToString()
                       }).OrderBy(a => a.Text)
                       .ToList();
            }

            var allCaseTypes = _caseTypeService.GetCaseTypesForSetting(customerId);
            var availableCaseTypes = allCaseTypes.Where(c => c.ShowOnExtPageCases == 0).Select(x => new SelectListItem
            {
                Text = x.getCaseTypeParentPath(),
                Value = x.Id.ToString(),
                Disabled = x.IsActive == 0
            }).OrderBy(a => a.Text).ToList();

            var selectedCaseTypes = allCaseTypes.Where(c => c.ShowOnExtPageCases == 1).Select(x => new SelectListItem
            {
                Text = x.getCaseTypeParentPath(),
                Value = x.Id.ToString(),
                Disabled = x.IsActive == 0
            }).OrderBy(a => a.Text).ToList();

            var allProductAreas = _productAreaService.GetProductAreasForSetting(customerId, false);
            var availableProductAreas = allProductAreas.Where(p => p.ShowOnExtPageCases == 0).Select(x => new SelectListItem
            {
                Text = x.getProductAreaParentPath(),
                Value = x.Id.ToString(),
                Disabled = x.IsActive == 0
            }).OrderBy(a => a.Text).ToList();

            var selectedProductAreas = allProductAreas.Where(p => p.ShowOnExtPageCases == 1).Select(x => new SelectListItem
            {
                Text = x.getProductAreaParentPath(),
                Value = x.Id.ToString(),
                Disabled = x.IsActive == 0
            }).OrderBy(a => a.Text).ToList();

            var selectedNum = 0;
            if (customer.ShowFAQOnExternalStartPage.HasValue)
              selectedNum = customer.ShowFAQOnExternalStartPage.Value;

            var nums = new List<int> { 1, 5, 10, 50, 100, 1000 };
            var numbers = nums.Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = x.ToString(),
                Selected = (x == selectedNum)
            }).ToList();

            var model = new SelfServiceIndexViewModel()
                {
                    Customer = customer,
                    AvailableCategories = availableCats,
                    SelectedCategories = selectedCats,
                    AvailableInitiators = availableInitiators,
                    SelectedInitiators = selectedInitiators,
                    StartPageFAQNums = numbers,
                    AvailableCaseTypes = availableCaseTypes,
                    SelectedCaseTypes = selectedCaseTypes,
                    AvailableProductAreas = availableProductAreas,
                    SelectedProductAreas = selectedProductAreas,
                    CaseComplaintDays = setting != null ? setting.CaseComplaintDays : 0
                };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, SelfServiceIndexViewModel vmodel,
            int[] selectedCategories, 
            int[] selectedCaseTypes,
            int[] selectedProductAreas,
            int[] selectedInitiators)
        {
            selectedInitiators = selectedInitiators ?? new int[]{};
            var customerToSave = this._customerService.GetCustomer(id);
            customerToSave.PasswordRequiredOnExternalPage = vmodel.Customer.PasswordRequiredOnExternalPage;
            customerToSave.ShowBulletinBoardOnExtPage = vmodel.Customer.ShowBulletinBoardOnExtPage;
            customerToSave.ShowCalenderOnExtPage = vmodel.Customer.ShowCalenderOnExtPage;
            customerToSave.ShowOperationalLogOnExtPage = vmodel.Customer.ShowOperationalLogOnExtPage;
            customerToSave.ShowDashboardOnExternalPage = vmodel.Customer.ShowDashboardOnExternalPage;
            customerToSave.ShowFAQOnExternalPage = vmodel.Customer.ShowFAQOnExternalPage;
            customerToSave.ShowDocumentsOnExternalPage = vmodel.Customer.ShowDocumentsOnExternalPage;
            customerToSave.ShowFAQOnExternalStartPage = vmodel.Customer.ShowFAQOnExternalStartPage;
            customerToSave.ShowCoWorkersOnExternalPage = vmodel.Customer.ShowCoWorkersOnExternalPage;
            customerToSave.ShowHelpOnExternalPage = vmodel.Customer.ShowHelpOnExternalPage;
            customerToSave.UseInitiatorAutocomplete = vmodel.Customer.UseInitiatorAutocomplete;
            customerToSave.UseInternalLogNoteOnExternalPage = vmodel.Customer.UseInternalLogNoteOnExternalPage;
            customerToSave.MyCasesFollower = vmodel.Customer.MyCasesFollower;
            customerToSave.MyCasesInitiator = vmodel.Customer.MyCasesInitiator;
            customerToSave.MyCasesRegarding = vmodel.Customer.MyCasesRegarding;
            customerToSave.MyCasesRegistrator = vmodel.Customer.MyCasesRegistrator;
            customerToSave.MyCasesUserGroup = vmodel.Customer.MyCasesUserGroup;
            customerToSave.RestrictUserToGroupOnExternalPage = vmodel.Customer.RestrictUserToGroupOnExternalPage;
            customerToSave.FetchDataFromApiOnExternalPage = vmodel.Customer.FetchDataFromApiOnExternalPage;
            customerToSave.ShowCasesOnExternalPage = vmodel.Customer.ShowCasesOnExternalPage;
            customerToSave.ShowCaseOnExternalPage = vmodel.Customer.ShowCaseOnExternalPage;
            customerToSave.GroupCaseTemplates = vmodel.Customer.GroupCaseTemplates;
            customerToSave.FetchPcNumber = vmodel.Customer.FetchPcNumber;

            customerToSave.ShowCaseActionsPanelOnTop = vmodel.Customer.ShowCaseActionsPanelOnTop;
            customerToSave.ShowCaseActionsPanelAtBottom = vmodel.Customer.ShowCaseActionsPanelAtBottom;

            //keep at least one selected
            if (!customerToSave.ShowCaseActionsPanelOnTop && !customerToSave.ShowCaseActionsPanelAtBottom)
                customerToSave.ShowCaseActionsPanelOnTop = true;

            var caseType_Id = 0;
            if (customerToSave == null)
                throw new Exception("No customer found...");

            IDictionary<string, string> errors = new Dictionary<string, string>();
            
            this._customerService.SaveEditCustomer(customerToSave, out errors);

            var allCategories = _documentService.GetDocumentCategories(id);

            foreach (var cat in allCategories)
            {
                if (selectedCategories != null && selectedCategories.Contains(cat.Id))
                    cat.ShowOnExternalPage = true;
                else
                    cat.ShowOnExternalPage = false;

                _documentService.SaveDocumentCategory(cat, out errors);
            }

            var allCaseTypes = _caseTypeService.GetCaseTypesForSetting(id);

            foreach (var type in allCaseTypes)
            {
                if (selectedCaseTypes != null && selectedCaseTypes.Contains(type.Id))
                    type.ShowOnExtPageCases = 1;
                else
                    type.ShowOnExtPageCases = 0;

                _caseTypeService.SaveCaseType(type, out errors);
            }

            var allProductAreas = _productAreaService.GetProductAreasForSetting(id, false);
            foreach (var prod in allProductAreas)
            {
                if (selectedProductAreas != null && selectedProductAreas.Contains(prod.Id))
                    prod.ShowOnExtPageCases = 1;
                else
                    prod.ShowOnExtPageCases = 0;
                
                var prodareawgs = prod.WorkingGroups.Any() ? prod.WorkingGroups.Select(wg => wg.Id).ToArray() : new int[0];

                CaseTypeProductArea connectedCaseType = null;
                if (prod.Id > 0)
                {
                    connectedCaseType = prod.CaseTypeProductAreas?.FirstOrDefault(x => x.ProductArea_Id == prod.Id);
                }
                caseType_Id = connectedCaseType?.CaseType_Id ?? 0;

                _productAreaService.SaveProductArea(prod, prodareawgs, caseType_Id, out errors);
            }

            if (!_featureToggleService.IsActive(FeatureToggleTypes.DISABLE_SELFSERVICE_SETTING_VIEW_DEPARTMENT_CASES))
            {
                var oldSelectedInitiators = _computerService.GetComputerUsers(id, true)
                .Select(i => i.Id).ToArray();
                var toSelectInitiators = selectedInitiators.Where(i => !oldSelectedInitiators.Contains(i))
                    .ToArray();
                _computerService.UpdateNotifierShowOnExtPageDepartmentCases(toSelectInitiators, true);

                var toUnselectInitiators = oldSelectedInitiators.Except(selectedInitiators)
                    .ToArray();
                _computerService.UpdateNotifierShowOnExtPageDepartmentCases(toUnselectInitiators, false);
            }

            var setting = _settingService.GetCustomerSetting(id);
            if (setting != null)
            {
                setting.CaseComplaintDays = vmodel.CaseComplaintDays;
                _settingService.SaveSetting(setting, out errors);
            }
            else
            {
                throw new Exception("No customer settings found...");
            }

            if (errors.Count == 0)
                return this.RedirectToAction("Index", "SelfServiceSetting", new { customerId = id });

            

            var availableCats = allCategories.Where(c => c.ShowOnExternalPage == false).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var selectedCats = allCategories.Where(c => c.ShowOnExternalPage).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            var model = new SelfServiceIndexViewModel()
            {
                Customer = customerToSave,
                AvailableCategories = availableCats,
                SelectedCategories = selectedCats,
                CaseComplaintDays = vmodel.CaseComplaintDays
            };

            return this.View(model);
        }
                                
    }
}
