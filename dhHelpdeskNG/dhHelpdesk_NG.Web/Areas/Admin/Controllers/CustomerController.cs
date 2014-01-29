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
    public class CustomerController : BaseController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly IRegionService _regionService;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;

        public CustomerController(
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSettingsService caseSettingsService,
            ICustomerService customerService,
            ILanguageService languageService,
            IRegionService regionService,
            ISettingService settingService,
            IUserService userService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseSettingsService = caseSettingsService;
            _customerService = customerService;
            _languageService = languageService;
            _regionService = regionService;
            _settingService = settingService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var model = IndexViewModel();

            model.Customers = _customerService.GetAllCustomers().ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CustomerSearch SearchCustomers)
        {
            var c = _customerService.SearchAndGenerateCustomers(SearchCustomers);
            var model = IndexViewModel();

            model.Customers = c;

            return View(model);
        }

        public ActionResult New()
        {
            var model = CustomerInputViewModel(new Customer() { });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Customer customer)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _customerService.SaveNewCustomerToGetId(customer, out errors);

            return RedirectToAction("edit", "customer", new { customer.Id });
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomer(id);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

            var setting = _settingService.GetCustomerSetting(id);

            if (setting == null)
            {
                setting = new Setting { Customer_Id = id };
                setting.CaseFiles = 6;
                setting.ComputerUserInfoListLocation = 1;
                setting.ModuleCase = 1;
            }

            var model = CustomerInputViewModel(customer);

            model.PasswordHis = model.Setting.PasswordHistory;

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Customer customer, FormCollection coll, CustomerInputViewModel vmodel, int[] UsSelected)
        {
            var customerToSave = _customerService.GetCustomer(id);
            customerToSave.OrderPermission = returnOrderPermissionForSave(id, vmodel);
            customerToSave.OverwriteFromMasterDirectory = vmodel.Customer.OverwriteFromMasterDirectory;
            customerToSave.PasswordRequiredOnExternalPage = vmodel.Customer.PasswordRequiredOnExternalPage;
            customerToSave.ShowBulletinBoardOnExtPage = vmodel.Customer.ShowBulletinBoardOnExtPage;
            customerToSave.ShowDashboardOnExternalPage = vmodel.Customer.ShowDashboardOnExternalPage;
            customerToSave.ShowFAQOnExternalPage = vmodel.Customer.ShowFAQOnExternalPage;

            var b = TryUpdateModel(customerToSave, "customer");
            var setting = _settingService.GetCustomerSetting(id);

            if (setting == null)
            {
                setting = new Setting { Customer_Id = id };
                setting.CaseFiles = 6;
                setting.ComputerUserInfoListLocation = 1;
                setting.ModuleCase = 1;
            }

            if (setting != null && vmodel.Setting != null)
            {
                setting.DefaultAdministrator = vmodel.Setting.DefaultAdministrator;
                setting.DefaultAdministratorExternal = vmodel.Setting.DefaultAdministratorExternal;
                setting.CreateCaseFromOrder = vmodel.Setting.CreateCaseFromOrder;
                setting.ComplexPassword = vmodel.Setting.ComplexPassword;
            }

            var CaseFieldSettingLanguages = _caseFieldSettingService.GetCaseFieldSettingLanguages();

            if (customerToSave == null)
                throw new Exception("No customer found...");

            IDictionary<string, string> errors = new Dictionary<string, string>();

            _customerService.SaveEditCustomer(customerToSave, setting, UsSelected, customer.Language_Id, out errors);

            if (errors.Count == 0)
                return RedirectToAction("edit", "customer");

            var model = CustomerInputViewModel(customerToSave);

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

            #region Generals

            var usSelected = customer.Users ?? new List<User>();
            var usAvailable = new List<User>();

            if (customer.Id != 0)
            {
                foreach (var us in _userService.GetUsers())
                {
                    if (!usSelected.Contains(us))
                        usAvailable.Add(us);
                }
            }

            #endregion

            #region SelectListItems

            List<SelectListItem> sl = new List<SelectListItem>();
            for (int i = 0; i < 13; i++)
            {
                sl.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            List<SelectListItem> sli = new List<SelectListItem>();
            for (int i = 0; i < 11; i++)
            {
                sli.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            #endregion

            //#region Reports

            //var reports = _customerService.GetAllReports();
            //var customerReports = _customerService.GetCustomerReportList(customer.Id);
            //var reportList = new List<CustomerReportList>();

            //foreach (var rep in reports)
            //{
            //    var o = new CustomerReportList
            //    {
            //        ReportId = rep.Id,
            //    };

            //    if (rep.Id == 2)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ledtid (avslutade ärenden)", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 3)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ledtid (aktiva ärenden)", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 4)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Avslutsorsak per avdelning", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 19)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Avslutskategori per avdelning", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 5)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Avslutade ärenden per dag", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 6)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
            //    if (rep.Id == 21)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("timme", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 7)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
            //    if (rep.Id == 8)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Servicerapport", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 15)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("driftgrupp", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 17)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("ärendetyp", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 9)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Frågeregistrering", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 14)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärendetyp", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("produktområde", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 16)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärendetyp", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("leverantör", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 13)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Genomsnittlig lösningstid", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 20)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Källa registrering", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 22)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Svarstid", Enums.TranslationSource.TextTranslation);
            //    if (rep.Id == 18)
            //        o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Rapportgenerator", Enums.TranslationSource.TextTranslation);

            //    reportList.Add(o);
            //}

            //foreach (var report in customerReports)
            //{
            //    int index = reportList.FindIndex(x => x.ReportId == report.ReportId);
            //    if (index > 0)
            //        reportList[index].ActiveOnPage = report.ActiveOnPage;
            //}

            //#endregion

            #region Model

            var model = new CustomerInputViewModel
            {
                CustomerCaseSummaryViewModel = new CustomerCaseSummaryViewModel(),
                CaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customer.Id),
                Customer = customer,
                ListCaseForLabel = _caseFieldSettingService.ListToShowOnCasePage(customer.Id, customer.Language_Id),
                //ListCustomerReports = reportList,
                MinimumPasswordLength = sl,
                PasswordHistory = sli,
                Regions = _regionService.GetRegions(customer.Id),
                Setting = _settingService.GetCustomerSetting(customer.Id) ?? new Setting(),
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

                UsAvailable = usAvailable.Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList(),
                UsSelected = usSelected.Select(x => new SelectListItem
                {
                    Text = x.SurName + " " + x.FirstName,
                    Value = x.Id.ToString()
                }).ToList(),
                UserGroups = _userService.GetUserGroups().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList(),
            };

            #endregion

            #region Ints

            if (customer.OrderPermission == 0)
            {
                model.OrderPermission = 0;
            }
            else
            {
                model.OrderPermission = 1;
            }

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

        private int returnOrderPermissionForSave(int id, CustomerInputViewModel vmodel)
        {
            var customer = _customerService.GetCustomer(id);

            if (vmodel.OrderPermission == 0)
            {
                customer.OrderPermission = 0;
            }
            else
            {
                customer.OrderPermission = 1;
            }

            return customer.OrderPermission;
        }

        private int returnPasswordHistoryForSave(CustomerInputViewModel model)
        {
            var pw = model.Setting.PasswordHistory;

            if (model.PasswordHis == 0)
            {
                pw = 0;
            }
            else
            {
                pw = model.PasswordHis;
            }

            return pw;
        }

        private int returnCreateCaseFromOrderForSave(CustomerInputViewModel model)
        {
            var ccf = model.Setting.CreateCaseFromOrder;

            if (model.CreateCaseFromOrder == 0)
            {
                ccf = 0;
            }
            else
            {
                ccf = model.CreateCaseFromOrder;
            }

            return ccf;
        }

        public string AddRowToCaseSettings(int usergroupId, int customerId, int labellist, int linelist, int minWidthValue, int colOrderValue)
        {
            var caseSetting = new CaseSettings();

            IDictionary<string, string> errors = new Dictionary<string, string>();

            var model = CustomerCaseSummaryViewModel(caseSetting);

            if (ModelState.IsValid)
            {
                caseSetting.UserGroup = usergroupId;
                caseSetting.Customer_Id = customerId;
                caseSetting.Line = linelist;
                caseSetting.MinWidth = minWidthValue;
                caseSetting.ColOrder = colOrderValue;
                caseSetting.Name = _caseSettingsService.SetListCaseName(labellist);
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
            var model = CustomerCaseSummaryViewModel(caseSetting);

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

            var labelForDDL = _caseFieldSettingService.ListToShowOnCaseSummaryPage(customer.Id, SessionFacade.CurrentLanguage, id);

            var caseSetting = new CaseSettings() { };

            var model = CustomerCaseSummaryViewModel(caseSetting);

            model.CaseSettings = ugListToUpdate;
            model.ListSummaryForLabel = labelForDDL;
            model.UserGroupId = id;

            UpdateModel(model, "caseSetting");

            return RenderRazorViewToString("_CaseSummaryPartialView", model);
        }

        [HttpPost]
        public void SaveLDAPPassword(int id, string newPassword, string confirmPassword)
        {
            var setting = _settingService.GetCustomerSetting(id);

            if (setting == null)
            {
                setting = new Setting() { Customer_Id = id };
                setting.CaseFiles = 6;
                setting.ComputerUserInfoListLocation = 1;
                setting.ModuleCase = 1;
            }

            IDictionary<string, string> errors = new Dictionary<string, string>();

            if (newPassword == confirmPassword)
            {
                setting.LDAPPassword = newPassword;
                _settingService.SaveSetting(setting, out errors);
            }
            else
                errors.Add("Setting.LDAPPassword", @Translation.Get("Angivna ord stämmer ej överens", Enums.TranslationSource.TextTranslation));
        }
    }
}