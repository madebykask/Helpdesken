using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;
using DH.Helpdesk.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{    

    public class SelfServiceSettingController : BaseAdminController
    {

        private readonly ICustomerService _customerService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ISettingService _settingService;
        private readonly IDocumentService _documentService;

        public SelfServiceSettingController(
                ICustomerService customerService,
                ICaseFieldSettingService caseFieldSettingService,
                ISettingService settingService,
                IDocumentService documentService,
                IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._customerService = customerService;
            this._caseFieldSettingService = caseFieldSettingService;
            this._settingService = settingService;
            this._documentService = documentService;
        }
        //
        // GET: /Admin/SelfServiceSetting/

        public ActionResult Index(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

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
                    StartPageFAQNums = numbers
                };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, SelfServiceIndexViewModel vmodel, int[] SelectedCategories)
        {
            var customerToSave = this._customerService.GetCustomer(id);
            customerToSave.PasswordRequiredOnExternalPage = vmodel.Customer.PasswordRequiredOnExternalPage;
            customerToSave.ShowBulletinBoardOnExtPage = vmodel.Customer.ShowBulletinBoardOnExtPage;
            customerToSave.ShowDashboardOnExternalPage = vmodel.Customer.ShowDashboardOnExternalPage;
            customerToSave.ShowFAQOnExternalPage = vmodel.Customer.ShowFAQOnExternalPage;
            customerToSave.ShowDocumentsOnExternalPage = vmodel.Customer.ShowDocumentsOnExternalPage;
            customerToSave.ShowFAQOnExternalStartPage = vmodel.Customer.ShowFAQOnExternalStartPage;
            customerToSave.ShowCoWorkersOnExternalPage = vmodel.Customer.ShowCoWorkersOnExternalPage;
            customerToSave.ShowHelpOnExternalPage = vmodel.Customer.ShowHelpOnExternalPage;           

            if (customerToSave == null)
                throw new Exception("No customer found...");

            IDictionary<string, string> errors = new Dictionary<string, string>();
            
            this._customerService.SaveEditCustomer(customerToSave, out errors);

            var allCategories = _documentService.GetDocumentCategories(id);

            foreach (var cat in allCategories)
            {
                if (SelectedCategories != null && SelectedCategories.Contains(cat.Id))
                    cat.ShowOnExternalPage = true;
                else
                    cat.ShowOnExternalPage = false;

                _documentService.SaveDocumentCategory(cat, out errors);
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
                SelectedCategories = selectedCats
            };

            return this.View(model);
        }
                                
    }
}
