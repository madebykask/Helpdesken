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
    public class CustomerCaseOverviewController : BaseController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;

        public CustomerCaseOverviewController(
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSettingsService caseSettingsService,
            ICustomerService customerService,
            ILanguageService languageService,
            IUserService userService,
            ISettingService settingService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseSettingsService = caseSettingsService;
            _customerService = customerService;
            _languageService = languageService;
            _userService = userService;
            _settingService = settingService;
        }

        [HttpGet]
        public ActionResult Edit(int id, int usergroupId)
        {
            var customer = _customerService.GetCustomer(id);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

           
            //var casefieldsetting = _caseFieldSettingService.GetCaseFieldSettings(id);
            //var caseSettings = _caseSettingsService.GenerateCSFromUGChoice(id, 1);
            //if (caseSettings == null)
            //

            var model = CustomerInputViewModel(customer);
            model.CustomerCaseSummaryViewModel = CustomerCaseSummaryViewModel(null, customer, usergroupId);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Customer customer, FormCollection coll, CustomerInputViewModel vmodel, int[] UsSelected)
        {
           
                return RedirectToAction("edit", "customer", new { Id = customer.Id });

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
                customer.Language_Id = SessionFacade.CurrentLanguageId;
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
                UserGroups = _userService.GetUserGroups().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList(),
            };

            #endregion

            return model;
        }

        private CustomerCaseSummaryViewModel CustomerCaseSummaryViewModel(CaseSettings caseSetting, Customer customer, int usergroupId)
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
                CaseSettings = _caseSettingsService.GenerateCSFromUGChoice(customer.Id, usergroupId),
                CSetting = caseSetting,
                CaseFieldSettingLanguages = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customer.Id, customer.Language_Id),
                ListSummaryForLabel = _caseFieldSettingService.ListToShowOnCustomerSettingSummaryPage(customer.Id, customer.Language_Id, usergroupId),
                LineList = li,
                UserGroupId = usergroupId,
                CaseFieldSetting = _caseFieldSettingService.GetCaseFieldSettings(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }

        public string AddRowToCaseSettings(int usergroupId, int customerId, string labellist, int linelist, int minWidthValue, int colOrderValue)
        {
            var caseSetting = new CaseSettings();
            var customer = _customerService.GetCustomer(customerId);

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = CustomerCaseSummaryViewModel(caseSetting, customer, usergroupId);

            if (ModelState.IsValid)
            {
                caseSetting.UserGroup = usergroupId;
                caseSetting.Customer_Id = customerId;
                caseSetting.Line = linelist;
                caseSetting.MinWidth = minWidthValue;
                caseSetting.ColOrder = colOrderValue;
                caseSetting.Name = labellist;
            }

            model.CSetting = caseSetting;

            _caseSettingsService.SaveCaseSetting(model.CSetting, out errors);

            return UpdateUserGroupList(usergroupId, customerId);
        }

        public string ChangeLabel(int id, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);

            if (customer == null)
            {
                customer = new Customer() { };
            }

            var ListToReturn = _caseFieldSettingService.ListToShowOnCasePage(customer.Id, id);

            var model = CustomerInputViewModel(customer);
            model.ListCaseForLabel = ListToReturn;
            model.Customer.Language_Id = id;
            model.Languages.Where(x => x.Value == id.ToString()).FirstOrDefault().Selected = true;

            return RenderRazorViewToString("_Case", model);
        }

        [HttpPost]
        public string DeleteRowFromCaseSettings(int id, int usergroupId, int customerId)
        {
            var caseSetting = _caseSettingsService.GetCaseSetting(id);
            var customer = _customerService.GetCustomer(customerId);
            var model = CustomerCaseSummaryViewModel(caseSetting, customer, usergroupId);

            if (_caseSettingsService.DeleteCaseSetting(id) == DeleteMessage.Success)
                return UpdateUserGroupList(usergroupId, customerId);
            else
            {
                TempData.Add("Error", "");
                return UpdateUserGroupList(usergroupId, customerId);
            }
        }

        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")] //TODO: Is duration time (10 seconds) too short? well, 60 seconds is too much anyway.. 
        public string UpdateUserGroupList(int id, int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            var ugListToUpdate = _caseSettingsService.GenerateCSFromUGChoice(customer.Id, id);

            if (ugListToUpdate == null)
                ugListToUpdate = _caseSettingsService.GetCaseSettings(customer.Id).ToList();

            var labelForDDL = _caseFieldSettingService.ListToShowOnCustomerSettingSummaryPage(customer.Id, customer.Language_Id, id);
            var caseSettings = new CaseSettings() { };
          
            var model = CustomerCaseSummaryViewModel(caseSettings, customer, id);

            model.CaseSettings = ugListToUpdate;
            model.ListSummaryForLabel = labelForDDL;
            model.UserGroupId = id;
            model.Customer = customer;

            UpdateModel(model, "caseSettings");

            //return View(model);
            var view = "~/areas/admin/views/CustomerCaseOverview/_CaseSummaryPartialView.cshtml";
            return RenderRazorViewToString(view, model);
        }
    }
}
