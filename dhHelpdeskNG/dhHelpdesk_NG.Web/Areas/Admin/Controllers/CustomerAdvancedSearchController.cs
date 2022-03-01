using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Web.Areas.Admin.Models;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    public class CustomerAdvancedSearchController : BaseAdminController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly IUserService _userService;
        private const char Separator = ',';

        // GET: Admin/CustomerAdvancedSearch
        public CustomerAdvancedSearchController(IMasterDataService masterDataService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSettingsService caseSettingsService,
            ICustomerService customerService,
            ILanguageService languageService,
            IUserService userService) : base(masterDataService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseSettingsService = caseSettingsService;
            _customerService = customerService;
            _languageService = languageService;
            _userService = userService;
        }


        [CustomAuthorize(Roles = "3,4")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomer(id);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customer.Id);
            var model = CustomerInputViewModel(customer, caseFieldSettings);
            model.CustomerCaseSummaryViewModel = CustomerCaseSummaryViewModel(null, customer, caseFieldSettings);

            return View(model);
        }


        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Customer customer, FormCollection coll, CustomerInputViewModel vmodel,
            int[] usSelected)
        {
            var caseFieldStreem = coll.ReturnFormValue("SortedFields");
            if (!string.IsNullOrEmpty(caseFieldStreem))
            {
                var lstCaseFieldIds = caseFieldStreem.Split(Separator).ToList();
                _caseSettingsService.ReOrderCaseSetting(lstCaseFieldIds);
            }

            return RedirectToAction("Edit", new {id = customer.Id});
        }

        [HttpPost]
        public string AddRowToCaseSettings(int usergroupId, int customerId, string labellist, int linelist, int minWidthValue, int colOrderValue, string clientOrder)
        {
            var existFields = _caseSettingsService.GetCaseSettingsByUserGroup(customerId, usergroupId, CaseSettingTypes.AdvancedSearch)
                .Select(f => f.Name)
                .ToList();
            if (existFields.Contains(labellist))
                return "Repeated";

            IDictionary<string, string> errors = new Dictionary<string, string>();
            var caseSetting = new CaseSettings();

            if (ModelState.IsValid)
            {
                caseSetting.UserGroup = usergroupId;
                caseSetting.Customer_Id = customerId;
                caseSetting.Line = linelist;
                caseSetting.MinWidth = minWidthValue;
                caseSetting.ColOrder = colOrderValue;
                caseSetting.Name = labellist;
                caseSetting.Type = CaseSettingTypes.AdvancedSearch;
            }
            _caseSettingsService.SaveCaseSetting(caseSetting, out errors);

            return UpdateUserGroupList(usergroupId, customerId, clientOrder);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public string DeleteRowFromCaseSettings(int id, int usergroupId, int customerId, string clientOrder)
        {
            if (_caseSettingsService.DeleteCaseSetting(id) != DeleteMessage.Success)
                TempData.Add("Error", "");
            return UpdateUserGroupList(usergroupId, customerId, clientOrder);
        }

        private string UpdateUserGroupList(int id, int customerId, string clientOrder)
        {
            var customer = _customerService.GetCustomer(customerId);
            var ugListToUpdate = _caseSettingsService.GenerateCSFromUGChoice(customer.Id, id, CaseSettingTypes.AdvancedSearch) ??
                                 _caseSettingsService.GetCaseSettings(customer.Id, null, CaseSettingTypes.AdvancedSearch).ToList();

            if (!string.IsNullOrEmpty(clientOrder))
            {
                var orderForList = clientOrder.Split(',').Select(int.Parse).ToList();
                ugListToUpdate = ugListToUpdate.OrderBy(d => orderForList.Contains(d.Id) ? orderForList.IndexOf(d.Id) : 99).ToList();
            }

            var labelForDDL = _caseFieldSettingService.ListToShowOnCustomerSettingSummaryPage(customer.Id, SessionFacade.CurrentLanguageId, id); //customer.Language_Id
            var caseSettings = new CaseSettings() { };
          
            var caseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customer.Id);
            var model = CustomerCaseSummaryViewModel(caseSettings, customer, caseFieldSettings);

            model.CaseSettings = ugListToUpdate;
            model.ListSummaryForLabel = labelForDDL;
            model.UserGroupId = id;
            model.Customer = customer;

            model.SortedFields = string.Empty;
            foreach (var cs in model.CaseSettings)
                model.SortedFields += Separator.ToString() + cs.Id.ToString();

            model.Seperator = Separator.ToString();
            this.UpdateModel(model, "caseSettings");
                        
            var view = "~/areas/admin/views/CustomerAdvancedSearch/_CaseSummaryPartialView.cshtml";
            return RenderRazorViewToString(view, model);
        }

        private CustomerInputViewModel CustomerInputViewModel(Customer customer, IList<CaseFieldSetting> caseFieldSettings)
        {
            if (customer.Id == 0)
            {
                customer.Language_Id = SessionFacade.CurrentLanguageId;
            }

            #region Model

            var model = new CustomerInputViewModel
            {
                CustomerCaseSummaryViewModel = new CustomerCaseSummaryViewModel(),
                CaseFieldSettings = caseFieldSettings,
                Customer = customer,
                ListCaseForLabel = _caseFieldSettingService.ListToShowOnCasePage(customer.Id, SessionFacade.CurrentLanguageId), // customer.Language_Id
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
                    Text = Translation.Get(x.Name),
                    Value = x.Id.ToString(),
                }).ToList(),
            };

            #endregion

            return model;
        }

        private CustomerCaseSummaryViewModel CustomerCaseSummaryViewModel(CaseSettings caseSetting, Customer customer, IList<CaseFieldSetting> caseFieldSettings)
        {
            var li = new List<SelectListItem>();
            var defaultUserGroupId = 0;
            li.Add(new SelectListItem()
            {
                Text = Translation.Get("Info"),
                Value = "1",
                Selected = false
            });
            //li.Add(new SelectListItem()
            //{
            //    Text = Translation.Get("Utökad info", Enums.TranslationSource.TextTranslation),
            //    Value = "2",
            //    Selected = false
            //});

            var model = new CustomerCaseSummaryViewModel
            {
                CaseSettings = _caseSettingsService.GenerateCSFromUGChoice(customer.Id, defaultUserGroupId, CaseSettingTypes.AdvancedSearch),
                CSetting = caseSetting,
                CaseFieldSettingLanguages = _caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customer.Id, SessionFacade.CurrentLanguageId), //customer.Language_Id
                ListSummaryForLabel = _caseFieldSettingService.ListToShowOnCustomerSettingSummaryPage(customer.Id, SessionFacade.CurrentLanguageId, defaultUserGroupId), //customer.Language_Id
                LineList = li,
                UserGroupId = defaultUserGroupId,
                CaseFieldSetting = caseFieldSettings.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            model.Seperator = Separator.ToString();
            if(model.CaseSettings.Count > 1)
            {
                foreach (var cs in model.CaseSettings)
                    model.SortedFields += Separator.ToString() + cs.Id.ToString();
            }

            
            return model;
        }
    }
}