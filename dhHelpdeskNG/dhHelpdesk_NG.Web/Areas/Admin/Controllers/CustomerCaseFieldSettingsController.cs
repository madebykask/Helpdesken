using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Areas.Admin.Models;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class CustomerCaseFieldSettingsController : BaseController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;

        public CustomerCaseFieldSettingsController(
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSettingsService caseSettingsService,
            ICustomerService customerService,
            ILanguageService languageService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseSettingsService = caseSettingsService;
            _customerService = customerService;
            _languageService = languageService;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomer(id);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

            var casefieldsetting = _caseFieldSettingService.GetCaseFieldSettings(id);

            if (casefieldsetting == null)
            {
               
            }

            var model = CustomerInputViewModel(customer);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Customer customer, FormCollection coll, CustomerInputViewModel vmodel, /*List<ReportCustomer> ReportCustomers, List<CaseFieldSetting> CaseFieldSettings,*/ int[] UsSelected)
        {
            var customerToSave = _customerService.GetCustomer(id);

            var b = TryUpdateModel(customerToSave, "customer");
            var casefieldsetting = _caseFieldSettingService.GetCaseFieldSettings(id);

            if (casefieldsetting != null)
            {
                
            }

            var CaseFieldSettingLanguages = _caseFieldSettingService.GetCaseFieldSettingLanguages();

            if (customerToSave == null)
                throw new Exception("No customer found...");

            IDictionary<string, string> errors = new Dictionary<string, string>();

            
            //_customerService.SaveEditCustomer(customerToSave, setting, UsSelected, customer.Language_Id, out errors);

            if (errors.Count == 0)
                return RedirectToAction("edit", "customer");

            var model = CustomerInputViewModel(customerToSave);
            //model.Tabposition = coll["activeTab"];

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_customerService.DeleteCustomer(id) == DeleteMessage.Success)
                return RedirectToAction("index", "customer");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "customer", new { id = id });
            }
        }

        private CustomerIndexViewModel IndexViewModel()
        {
            var model = new CustomerIndexViewModel { };

            return model;
        }

        private CustomerInputViewModel CustomerInputViewModel(Customer customer)
        {
            if (customer.Id == 0)
            {
                customer.Language_Id = SessionFacade.CurrentLanguage;
            }
            
            #region Model

            var model = new CustomerInputViewModel
            {
                CustomerCaseSummaryViewModel = new CustomerCaseSummaryViewModel(),
                CaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customer.Id),
                Customer = customer,
                ListCaseForLabel = _caseFieldSettingService.ListToShowOnCasePage(customer.Id, customer.Language_Id),
                Customers = _customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = _languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList(),
            };

            #endregion

            return model;
        }

        private CustomerCaseSummaryViewModel CustomerCaseSummaryViewModel(CaseSettings caseSetting)
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Info", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Utökad info", Enums.TranslationSource.TextTranslation),
                Value = "2",
                Selected = false
            });

            var model = new CustomerCaseSummaryViewModel
            {
                CaseSettings = _caseSettingsService.GetCaseSettings(SessionFacade.CurrentCustomer.Id),
                CSetting = caseSetting,
                CaseFieldSettingLanguages = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage),
                LineList = li,
            };

            return model;
        }
    }
}
