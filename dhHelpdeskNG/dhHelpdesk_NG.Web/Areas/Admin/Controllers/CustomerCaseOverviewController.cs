
namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions;

    public class CustomerCaseOverviewController : BaseAdminController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;

        private readonly char _seperator;

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
            this._caseFieldSettingService = caseFieldSettingService;
            this._caseSettingsService = caseSettingsService;
            this._customerService = customerService;
            this._languageService = languageService;
            this._userService = userService;
            this._settingService = settingService;
            this._seperator = ',';
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpGet]
        public ActionResult Edit(int id, int usergroupId)
        {
            var customer = this._customerService.GetCustomer(id);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

            var model = this.CustomerInputViewModel(customer);
            model.CustomerCaseSummaryViewModel = this.CustomerCaseSummaryViewModel(null, customer, usergroupId);

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Customer customer, FormCollection coll, CustomerInputViewModel vmodel, int[] UsSelected)
        {
            var caseFieldStreem = coll.ReturnFormValue("SortedFields");
            var userGroupIds = coll.ReturnFormValue("UserGroupId");
            var userGroupId = string.Empty;
            if (!string.IsNullOrEmpty(userGroupIds))
            {
                userGroupId = userGroupIds.Split(_seperator).First();
            }
            if (!string.IsNullOrEmpty(caseFieldStreem))
            {
                var lstCaseFieldIds = caseFieldStreem.Split(_seperator).ToList();
                _caseSettingsService.ReOrderCaseSetting(lstCaseFieldIds);
            }

            return RedirectToAction("Edit", new {id = customer.Id, usergroupId = userGroupId});

        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._customerService.DeleteCustomer(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "customer");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "customer", new { id = id });
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
                CaseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customer.Id),
                Customer = customer,
                ListCaseForLabel = this._caseFieldSettingService.ListToShowOnCasePage(customer.Id, SessionFacade.CurrentLanguageId), // customer.Language_Id
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList(),
                UserGroups = this._userService.GetUserGroups().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name, Enums.TranslationSource.TextTranslation),
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
            //li.Add(new SelectListItem()
            //{
            //    Text = Translation.Get("Utökad info", Enums.TranslationSource.TextTranslation),
            //    Value = "2",
            //    Selected = false
            //});

            var model = new CustomerCaseSummaryViewModel
            {
                CaseSettings = this._caseSettingsService.GenerateCSFromUGChoice(customer.Id, usergroupId),
                CSetting = caseSetting,
                CaseFieldSettingLanguages = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customer.Id, SessionFacade.CurrentLanguageId), //customer.Language_Id
                ListSummaryForLabel = this._caseFieldSettingService.ListToShowOnCustomerSettingSummaryPage(customer.Id, SessionFacade.CurrentLanguageId, usergroupId), //customer.Language_Id
                LineList = li,
                UserGroupId = usergroupId,
                CaseFieldSetting = this._caseFieldSettingService.GetCaseFieldSettings(customer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            model.Seperator = _seperator.ToString();
            foreach (var cs in model.CaseSettings)
                model.SortedFields +=  _seperator.ToString() + cs.Id.ToString();
            
            return model;
        }

        public string AddRowToCaseSettings(int usergroupId, int customerId, string labellist, int linelist, int minWidthValue, int colOrderValue, string clientOrder)
        {
            var caseSetting = new CaseSettings();
            var customer = this._customerService.GetCustomer(customerId);

            var existFields = this._caseSettingsService.GetCaseSettingsByUserGroup(customerId, usergroupId)
                                                       .Select(f => f.Name)
                                                       .ToList();
            if (existFields.Contains(labellist))
                return "Repeated";

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = this.CustomerCaseSummaryViewModel(caseSetting, customer, usergroupId);

            if (this.ModelState.IsValid)
            {
                caseSetting.UserGroup = usergroupId;
                caseSetting.Customer_Id = customerId;
                caseSetting.Line = linelist;
                caseSetting.MinWidth = minWidthValue;
                caseSetting.ColOrder = colOrderValue;
                caseSetting.Name = labellist;
            }
                        
            model.CSetting = caseSetting;
            this._caseSettingsService.SaveCaseSetting(model.CSetting, out errors);

            return this.UpdateUserGroupList(usergroupId, customerId, clientOrder);
        }

        public string ChangeLabel(int id, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);

            if (customer == null)
            {
                customer = new Customer() { };
            }

            var ListToReturn = this._caseFieldSettingService.ListToShowOnCasePage(customer.Id, id);

            var model = this.CustomerInputViewModel(customer);
            model.ListCaseForLabel = ListToReturn;
            model.Customer.Language_Id = id;
            model.Languages.Where(x => x.Value == id.ToString()).FirstOrDefault().Selected = true;

            return this.RenderRazorViewToString("_Case", model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public string DeleteRowFromCaseSettings(int id, int usergroupId, int customerId, string clientOrder)
        {
            var caseSetting = this._caseSettingsService.GetCaseSetting(id);
            var customer = this._customerService.GetCustomer(customerId);
            var model = this.CustomerCaseSummaryViewModel(caseSetting, customer, usergroupId);

            if (this._caseSettingsService.DeleteCaseSetting(id) == DeleteMessage.Success)
                return this.UpdateUserGroupList(usergroupId, customerId, clientOrder);
            else
            {
                this.TempData.Add("Error", "");
                return this.UpdateUserGroupList(usergroupId, customerId, clientOrder);
            }
        }

        [CustomAuthorize(Roles = "3,4")]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")] //TODO: Is duration time (10 seconds) too short? well, 60 seconds is too much anyway.. 
        public string UpdateUserGroupList(int id, int customerId, string clientOrder)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var ugListToUpdate = this._caseSettingsService.GenerateCSFromUGChoice(customer.Id, id);

            if (ugListToUpdate == null)
                ugListToUpdate = this._caseSettingsService.GetCaseSettings(customer.Id).ToList();

            if (!string.IsNullOrEmpty(clientOrder))
            {
                var orderForList = clientOrder.Split(',').Select(int.Parse).ToList();
                ugListToUpdate = ugListToUpdate.OrderBy(d => orderForList.Contains(d.Id) ? orderForList.IndexOf(d.Id) : 99).ToList();
            }

            var labelForDDL = this._caseFieldSettingService.ListToShowOnCustomerSettingSummaryPage(customer.Id, SessionFacade.CurrentLanguageId, id); //customer.Language_Id
            var caseSettings = new CaseSettings() { };
          
            var model = this.CustomerCaseSummaryViewModel(caseSettings, customer, id);

            model.CaseSettings = ugListToUpdate;
            model.ListSummaryForLabel = labelForDDL;
            model.UserGroupId = id;
            model.Customer = customer;

            model.SortedFields = string.Empty;
            foreach (var cs in model.CaseSettings)
                model.SortedFields += _seperator.ToString() + cs.Id.ToString();

            model.Seperator = _seperator.ToString();
            this.UpdateModel(model, "caseSettings");
                        
            var view = "~/areas/admin/views/CustomerCaseOverview/_CaseSummaryPartialView.cshtml";
            return this.RenderRazorViewToString(view, model);
        }
    }
}
