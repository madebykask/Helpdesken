namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.BusinessData.Models.Shared;

    public class CustomerController : BaseController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSettingsService _caseSettingsService;
        private readonly ICustomerService _customerService;
        private readonly ILanguageService _languageService;
        private readonly IRegionService _regionService;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICategoryService _categoryService;
        private readonly IProductAreaService _productAreaService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IStatusService _statusService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICustomerUserService _customerUserService;
        private readonly IPriorityService _priorityService;
        private readonly IComputerService _computerService;
        private readonly IMailTemplateService _mailTemplateService;

        public CustomerController(
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSettingsService caseSettingsService,
            ICustomerService customerService,
            ILanguageService languageService,
            IRegionService regionService,
            ISettingService settingService,
            IUserService userService,
            ICaseTypeService caseTypeService,
            ICategoryService categoryService,
            IProductAreaService productAreaService,
            IStateSecondaryService stateSecondaryService,
            IWorkingGroupService workingGroupService,
            IStatusService statusService,
            IFinishingCauseService finishingCauseService,
            ICaseSolutionService caseSolutionsService,
            ICustomerUserService customerUserSerivce,
            IPriorityService priorityService,
            IComputerService computerService,
            IMailTemplateService mailTemplateService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            this._caseFieldSettingService = caseFieldSettingService;
            this._caseSettingsService = caseSettingsService;
            this._customerService = customerService;
            this._languageService = languageService;
            this._regionService = regionService;
            this._settingService = settingService;
            this._userService = userService;
            this._caseTypeService = caseTypeService;
            this._categoryService = categoryService;
            this._productAreaService = productAreaService;
            this._stateSecondaryService = stateSecondaryService;
            this._workingGroupService = workingGroupService;
            this._statusService = statusService;
            this._finishingCauseService = finishingCauseService;
            this._caseSolutionService = caseSolutionsService;
            this._customerUserService = customerUserSerivce;
            this._priorityService = priorityService;
            this._computerService = computerService;
            this._mailTemplateService = mailTemplateService;
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult Index()
        {
            var model = this.IndexViewModel();

            //If administrator. return all customers, else: return only customers that user is assigned to.
            if (SessionFacade.CurrentUser.UserGroupId == 4)
            {
                model.Customers = this._customerService.GetAllCustomers().ToList();
            }
            else
            {
                model.Customers = this._userService.GetCustomersConnectedToUser(SessionFacade.CurrentUser.Id);
                //model.Customers = this._userService.GetCustomersForUser(SessionFacade.CurrentUser.Id);
            }

            



            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult Index(CustomerSearch SearchCustomers)
        {
            var model = this.IndexViewModel();

            //If administrator. return all customers, else: return only customers that user is assigned to.
            if (SessionFacade.CurrentUser.UserGroupId == 4)
            {
                model.Customers = this._customerService.SearchAndGenerateCustomers(SearchCustomers);
            }
            else
            {
                model.Customers = this._customerService.SearchAndGenerateCustomersConnectedToUser(SearchCustomers, SessionFacade.CurrentUser.Id);
            }


            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult New()
        {
            var model = this.CustomerInputViewModel(new Customer() { });

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public ActionResult New(Customer customer)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._customerService.SaveNewCustomerToGetId(customer, out errors);

            var newCustomerSetting = new Setting()
            {
                Customer_Id = customer.Id,
                ModuleCase = 1,
                CaseComplaintDays = 14             
            };

            this._customerService.SaveCustomerSettings(customer, newCustomerSetting, null, customer.Language_Id, out errors);


            // Get CaseSettings from "default" customer
            var caseSettingsToCopy = this._caseSettingsService.GetCaseSettingsForDefaultCust();

            foreach (var cs in caseSettingsToCopy)
            {
                var newCustomerCaseSettings = new CaseSettings() { };

                newCustomerCaseSettings.Customer_Id = customer.Id;
                newCustomerCaseSettings.User_Id = cs.User_Id;
                newCustomerCaseSettings.Name = cs.Name;
                newCustomerCaseSettings.Line = cs.Line;
                newCustomerCaseSettings.MinWidth = cs.MinWidth;
                newCustomerCaseSettings.ColOrder = cs.ColOrder;
                newCustomerCaseSettings.RegTime = DateTime.Now;
                newCustomerCaseSettings.ChangeTime = DateTime.Now;
                newCustomerCaseSettings.UserGroup = cs.UserGroup;

                this._customerService.SaveCaseSettingsForNewCustomer(customer.Id, customer.Language_Id, newCustomerCaseSettings, out errors);

            }


            //Get values from "default" customer
            var caseFieldSettingsToCopy = this._caseFieldSettingService.GetCaseFieldSettingsForDefaultCust();


            foreach (var cfs in caseFieldSettingsToCopy)
            {
                var newCustomerCaseFieldSettings = new CaseFieldSetting() { };

                newCustomerCaseFieldSettings.Customer_Id = customer.Id;
                newCustomerCaseFieldSettings.Name = cfs.Name;
                newCustomerCaseFieldSettings.ShowOnStartPage = cfs.ShowOnStartPage;
                newCustomerCaseFieldSettings.Required = cfs.Required;
                newCustomerCaseFieldSettings.RequiredIfReopened = cfs.RequiredIfReopened;
                newCustomerCaseFieldSettings.ShowExternal = cfs.ShowExternal;
                newCustomerCaseFieldSettings.EMailIdentifier = cfs.EMailIdentifier;
                newCustomerCaseFieldSettings.ChangedDate = DateTime.UtcNow;
                newCustomerCaseFieldSettings.CreatedDate = DateTime.UtcNow;
                newCustomerCaseFieldSettings.CaseFieldSettingsGUID = Guid.NewGuid();
              

                this._customerService.SaveCaseFieldSettingsForCustomerCopy(customer.Id, customer.Language_Id, newCustomerCaseFieldSettings, out errors);
            }

            //Get CaseFieldSettingLang for "Default customer"
            var language = this._languageService.GetLanguages();

            foreach (var l in language)
            {
                //var caseFieldSettingsLangToCopy = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(null, l.Id);
                var caseFieldSettingsLangToCopy = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguagesForDefaultCust(l.Id);
                var caseFieldSettingsForNewCustomer = this._caseFieldSettingService.GetCaseFieldSettings(customer.Id);

                foreach (var cfsl in caseFieldSettingsLangToCopy)
                {

                    foreach (var cfs in caseFieldSettingsForNewCustomer)
                    {
                        if (cfsl.Name == cfs.Name)
                        {
                            var newCustomerCaseFieldSettingsLang = new CaseFieldSettingLanguage() { };

                            newCustomerCaseFieldSettingsLang.CaseFieldSettings_Id = cfs.Id;
                            newCustomerCaseFieldSettingsLang.Label = cfsl.Label;
                            newCustomerCaseFieldSettingsLang.Language_Id = cfsl.Language_Id;
                            newCustomerCaseFieldSettingsLang.FieldHelp = cfsl.FieldHelp;


                            this._customerService.SaveCaseFieldSettingsLangForCustomerCopy(newCustomerCaseFieldSettingsLang, out errors);

                            break;
                        }

                    }

                }
            }

            // Get ComputerUserFieldSettings
            var computerUserFieldSettingsToCopy = this._computerService.GetComputerUserFieldSettingsForDefaultCust();

            foreach (var cufs in computerUserFieldSettingsToCopy)
            {
                var newCustomerComputerUserFS = new ComputerUserFieldSettings() { };

                newCustomerComputerUserFS.Customer_Id = customer.Id;
                newCustomerComputerUserFS.ComputerUserField = cufs.ComputerUserField;
                newCustomerComputerUserFS.Show = cufs.Show;
                newCustomerComputerUserFS.Required = cufs.Required;
                newCustomerComputerUserFS.MinLength = cufs.MinLength;
                newCustomerComputerUserFS.ShowInList = cufs.ShowInList;
                newCustomerComputerUserFS.LDAPAttribute = cufs.LDAPAttribute;

                this._computerService.SaveComputerUserFieldSettingForCustomerCopy(newCustomerComputerUserFS, out errors);
            }

            //ComputerUserFieldSettingsLanguage

            foreach (var l in language)
            {
                //var computerUserFieldSettingsLangToCopy = this._computerService.GetComputerUserFieldSettingsWithLanguages(customerToCopy.Id, l.Id);
                var computerUserFieldSettingsLangToCopy = this._computerService.GetComputerUserFieldSettingsWithLanguagesForDefaultCust(l.Id);
                var computerUserFieldSettingsForNewCustomer = this._computerService.GetComputerUserFieldSettings(customer.Id);

                if (computerUserFieldSettingsLangToCopy != null)
                {

                    foreach (var cfsl in computerUserFieldSettingsLangToCopy)
                    {

                        foreach (var cfs in computerUserFieldSettingsForNewCustomer)
                        {
                            if (cfsl.Name == cfs.ComputerUserField)
                            {
                                var newComputerUserFSL = new ComputerUserFieldSettingsLanguage
                                {
                                    ComputerUserFieldSettings_Id = cfs.Id,
                                    Language_Id = cfsl.Language_Id,
                                    Label = cfsl.Label,
                                    FieldHelp = cfsl.FieldHelp,
                                };

                                this._computerService.SaveComputerUserFieldSettingLangForCustomerCopy(newComputerUserFSL, out errors);

                                break;
                            }

                        }

                    }

                }

            }

            return this.RedirectToAction("edit", "customer", new { customer.Id });
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = this._customerService.GetCustomer(id);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

            var setting = this._settingService.GetCustomerSetting(id);

            if (setting == null)
            {
                setting = new Setting { Customer_Id = id };
                setting.CaseFiles = 6;
                setting.ComputerUserInfoListLocation = 1;
                setting.ModuleCase = 1;
            }

            var model = this.CustomerInputViewModel(customer);

            model.PasswordHis = model.Setting.PasswordHistory;

            return this.View(model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(
            int id, 
            Customer customer, 
            FormCollection coll, 
            CustomerInputViewModel vmodel, 
            int[] UsSelected)
        {
            var customerToSave = this._customerService.GetCustomer(id);
            if (customerToSave == null)
            {
                throw new Exception("No customer found...");
            }

            customerToSave.OrderPermission = this.returnOrderPermissionForSave(id, vmodel);
            customerToSave.OverwriteFromMasterDirectory = vmodel.Customer.OverwriteFromMasterDirectory;            
            customerToSave.CommunicateWithNotifier = vmodel.Customer.CommunicateWithNotifier;

            var b = this.TryUpdateModel(customerToSave, "customer");
            var setting = this._settingService.GetCustomerSetting(id);

            if (setting == null)
            {
                setting = new Setting { Customer_Id = id };
                setting.CaseFiles = 6;
                setting.ComputerUserInfoListLocation = 1;
                setting.ModuleCase = 1;
                setting.CaseComplaintDays = 14;
            }

            if (vmodel.Setting != null)
            {
                setting.DefaultAdministrator = vmodel.Setting.DefaultAdministrator;
                setting.DefaultAdministratorExternal = vmodel.Setting.DefaultAdministratorExternal;
                setting.CreateCaseFromOrder = vmodel.Setting.CreateCaseFromOrder;
                setting.ComplexPassword = vmodel.Setting.ComplexPassword;
                setting.IsUserFirstLastNameRepresentation = vmodel.UserFirstLastNameRepresentationId == UserFirstLastNameModes.LastFirstNameMode ? 0 : 1;
                setting.CalcSolvedInTimeByLatestSLADate = vmodel.Setting.CalcSolvedInTimeByLatestSLADate;
	            setting.BatchEmail = vmodel.Setting.BatchEmail;
            }

            IDictionary<string, string> errors;
            this._customerService.SaveEditCustomer(customerToSave, setting, UsSelected, customer.Language_Id, out errors);
            if (errors.Count == 0)
            {
                return this.RedirectToAction("edit", "customer");
            }

            var model = this.CustomerInputViewModel(customerToSave);

            return this.View(model);
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
                customer.CommunicateWithNotifier = 1;
            }

            #region Generals

            var usSelected = customer.Id != 0 ? this._userService.GetCustomerActiveUsers(customer.Id) : new List<User>();
            var usAvailable = new List<User>();

            if (customer.Id != 0)
            {
                foreach (var us in this._userService.GetAllUsers())
                {
                    if (!usSelected.Select(u => u.Id).Contains(us.Id))
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

            List<SelectListItem> cn = new List<SelectListItem>();
            cn.Add(new SelectListItem()
            {
                Text = Translation.Get("Nej", Enums.TranslationSource.TextTranslation),
                Value = "0",
                Selected = false
            });
            cn.Add(new SelectListItem()
            {
                Text = Translation.Get("Ja", Enums.TranslationSource.TextTranslation),
                Value = "1",
                Selected = false
            });

            var userFirstLastNameSelectList =
                new SelectList(
                    new[]
                        {
                            new
                                {
                                    Id = UserFirstLastNameModes.LastFirstNameMode,
                                    Name = Translation.Get("Enligt efternamn")
                                },
                            new
                                {
                                    Id = UserFirstLastNameModes.FirstLastNameMode,
                                    Name = Translation.Get("Enligt förnamn")
                                }
                        },
                    "Id",
                    "Name");

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

            var settings = this._settingService.GetCustomerSetting(customer.Id) ?? new Setting();
            var availableUsers = usAvailable.Select(x => new ListItem(x.Id.ToString(), x.SurName + " " + x.FirstName, Convert.ToBoolean(x.IsActive)))
                                          .ToList();
            var availableUsersModel = new CustomSelectList();
            availableUsersModel.Items.AddItems(availableUsers);
            var selectedUsers = usSelected.Select(x => new ListItem(x.Id.ToString(), x.SurName + " " + x.FirstName, Convert.ToBoolean(x.IsActive)))
                                          .ToList();
            var selectedUsersModel = new CustomSelectList();
            selectedUsersModel.Items.AddItems(selectedUsers);

            var model = new CustomerInputViewModel
            {
                CustomerCaseSummaryViewModel = new CustomerCaseSummaryViewModel(),
                CaseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customer.Id),
                Customer = customer,
                ListCaseForLabel = this._caseFieldSettingService.ListToShowOnCasePage(customer.Id, customer.Language_Id),
                //ListCustomerReports = reportList,
                MinimumPasswordLength = sl,
                PasswordHistory = sli,
                CWNSelect = cn,
                Regions = this._regionService.GetRegions(customer.Id),
                Setting = settings,
                Customers = this._customerService.GetAllCustomers().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Languages = this._languageService.GetLanguages().Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.Name),
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
                    Value = x.Id.ToString(),
                }).ToList(),

                AvUsMultiSelect = availableUsersModel,
                UsMultiSelect = selectedUsersModel,

                UserGroups = this._userService.GetUserGroups().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList(),
                UserFirstLastNameRepresentationList = userFirstLastNameSelectList,
                UserFirstLastNameRepresentationId = settings.IsUserFirstLastNameRepresentation.AsUserFirstLastNameMode()
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
                CaseSettings = this._caseSettingsService.GetCaseSettings(SessionFacade.CurrentCustomer.Id),
                CSetting = caseSetting,
                CaseFieldSettingLanguages = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguagesForDefaultCust(SessionFacade.CurrentLanguageId),
                //CaseFieldSettingLanguages = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId),
                LineList = li,
            };

            return model;
        }

        private int returnOrderPermissionForSave(int id, CustomerInputViewModel vmodel)
        {
            var customer = this._customerService.GetCustomer(id);

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

            var model = this.CustomerCaseSummaryViewModel(caseSetting);

            if (this.ModelState.IsValid)
            {
                caseSetting.UserGroup = usergroupId;
                caseSetting.Customer_Id = customerId;
                caseSetting.Line = linelist;
                caseSetting.MinWidth = minWidthValue;
                caseSetting.ColOrder = colOrderValue;
                caseSetting.Name = this._caseSettingsService.SetListCaseName(labellist);
            }

            model.CSetting = caseSetting;

            this._caseSettingsService.SaveCaseSetting(model.CSetting, out errors);

            return this.UpdateUserGroupList(usergroupId, customerId);
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

        [HttpPost]
        public string DeleteRowFromCaseSettings(int id, int usergroupId, int customerId)
        {
            var caseSetting = this._caseSettingsService.GetCaseSetting(id);
            var model = this.CustomerCaseSummaryViewModel(caseSetting);

            if (this._caseSettingsService.DeleteCaseSetting(id) == DeleteMessage.Success)
                return this.UpdateUserGroupList(usergroupId, customerId);
            else
            {
                this.TempData.Add("Error", "");
                return this.UpdateUserGroupList(usergroupId, customerId);
            }
        }

        [OutputCache(Location = OutputCacheLocation.Client, Duration = 10, VaryByParam = "none")] //TODO: Is duration time (10 seconds) too short? well, 60 seconds is too much anyway.. 
        public string UpdateUserGroupList(int id, int customerId)
        {
            var customer = this._customerService.GetCustomer(customerId);
            var ugListToUpdate = this._caseSettingsService.GenerateCSFromUGChoice(customer.Id, id);

            if (ugListToUpdate == null)
                ugListToUpdate = this._caseSettingsService.GetCaseSettings(customer.Id).ToList();

            var labelForDDL = this._caseFieldSettingService.ListToShowOnCaseSummaryPage(customer.Id, SessionFacade.CurrentLanguageId, id);

            var caseSetting = new CaseSettings() { };

            var model = this.CustomerCaseSummaryViewModel(caseSetting);

            model.CaseSettings = ugListToUpdate;
            model.ListSummaryForLabel = labelForDDL;
            model.UserGroupId = id;

            this.UpdateModel(model, "caseSetting");

            return this.RenderRazorViewToString("_CaseSummaryPartialView", model);
        }

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public void SaveLDAPPassword(int id, string newPassword, string confirmPassword)
        {
            var setting = this._settingService.GetCustomerSetting(id);

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
                this._settingService.SaveSetting(setting, out errors);
            }
            else
                errors.Add("Setting.LDAPPassword", @Translation.Get("Angivna ord stämmer ej överens", Enums.TranslationSource.TextTranslation));
        }

        [HttpPost]
        public int CopyCustomer(int id, string customerNumber, string customerName, string customerEmail)
        {
            var customerToCopy = this._customerService.GetCustomer(id);
            var customerToCopySettings = this._settingService.GetCustomerSetting(customerToCopy.Id);

            var newCustomerToSave = new Customer() 
            {
               CustomerID = customerNumber,
               Name = customerName,
               HelpdeskEmail = customerEmail,
               Language_Id = customerToCopy.Language_Id
            };

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._customerService.SaveNewCustomerToGetId(newCustomerToSave, out errors);

            var newCustomerSetting = new Setting()
            {
                Customer_Id = newCustomerToSave.Id,
                ModuleAccount = customerToCopySettings.ModuleAccount,
                ModuleADSync = customerToCopySettings.ModuleADSync,
                ModuleAsset = customerToCopySettings.ModuleAsset,
                ModuleBulletinBoard = customerToCopySettings.ModuleBulletinBoard,
                ModuleCalendar = customerToCopySettings.ModuleCalendar,
                ModuleCase = customerToCopySettings.ModuleCase,
                ModuleChangeManagement = customerToCopySettings.ModuleChangeManagement,
                ModuleChecklist = customerToCopySettings.ModuleChecklist,
                ModuleComputerUser = customerToCopySettings.ModuleComputerUser,
                ModuleContract = customerToCopySettings.ModuleContract,
                ModuleDailyReport = customerToCopySettings.ModuleDailyReport,
                ModuleDocument = customerToCopySettings.ModuleDocument,
                ModuleFAQ = customerToCopySettings.ModuleFAQ,
                ModuleInventory = customerToCopySettings.ModuleInventory,
                ModuleInventoryImport = customerToCopySettings.ModuleInventoryImport,
                ModuleInvoice = customerToCopySettings.ModuleInvoice,
                ModuleLicense = customerToCopySettings.ModuleLicense,
                ModuleOperationLog = customerToCopySettings.ModuleOperationLog,
                ModuleOrder = customerToCopySettings.ModuleOrder,
                ModulePlanning = customerToCopySettings.ModulePlanning,
                ModuleProblem = customerToCopySettings.ModuleProblem,
                ModuleProject = customerToCopySettings.ModuleProject,
                ModuleQuestion = customerToCopySettings.ModuleQuestion,
                ModuleQuestionnaire = customerToCopySettings.ModuleQuestionnaire,
                ModuleTimeRegistration = customerToCopySettings.ModuleTimeRegistration,
                ModuleWatch = customerToCopySettings.ModuleWatch,
                DepartmentFilterFormat = customerToCopySettings.DepartmentFilterFormat,
                DepartmentFormat = customerToCopySettings.DepartmentFormat,
                ProductAreaFilterFormat = customerToCopySettings.ProductAreaFilterFormat,
                CaseDateFormat = customerToCopySettings.CaseDateFormat,
                PriorityFormat = customerToCopySettings.PriorityFormat,
                PlanDateFormat = customerToCopySettings.PlanDateFormat,
                CategoryFilterFormat = customerToCopySettings.CategoryFilterFormat,
                DisableCaseEndDate = customerToCopySettings.DisableCaseEndDate,
                SetFirstUserToOwner = customerToCopySettings.SetFirstUserToOwner,
                CaseWorkingGroupSource = customerToCopySettings.CaseWorkingGroupSource,
                SearchCaseOnExternalPage = customerToCopySettings.SearchCaseOnExternalPage,
                NoMailToNotifierChecked = customerToCopySettings.NoMailToNotifierChecked,
                DontConnectUserToWorkingGroup = customerToCopySettings.DontConnectUserToWorkingGroup,
                LogLevel = customerToCopySettings.LogLevel,
                StateSecondaryReminder = customerToCopySettings.StateSecondaryReminder,
                StateSecondaryFormat =customerToCopySettings.StateSecondaryFormat,
                MinPasswordLength = customerToCopySettings.MinPasswordLength,
                MinRegWorkingTime = customerToCopySettings.MinRegWorkingTime,
                InvoiceType = customerToCopySettings.InvoiceType,
                ComplexPassword = customerToCopySettings.ComplexPassword,
                DBType = customerToCopySettings.DBType,
                ComputerUserInfoListLocation = customerToCopySettings.ComputerUserInfoListLocation,
                ComputerUserLog = customerToCopySettings.ComputerUserLog,
                StateSecondaryFilterFormat = customerToCopySettings.StateSecondaryFilterFormat,
                CreateCaseFromOrder = customerToCopySettings.CreateCaseFromOrder,
                LeadTimeFromProductAreaSetDate = customerToCopySettings.LeadTimeFromProductAreaSetDate,
                CaseSMS = customerToCopySettings.CaseSMS,
                MaxPasswordAge = customerToCopySettings.MaxPasswordAge,
                PasswordHistory = customerToCopySettings.PasswordHistory,
                CaseFiles = customerToCopySettings.CaseFiles,
                LogNoteFormat = customerToCopySettings.LogNoteFormat,
                CaseArchiveDays = customerToCopySettings.CaseArchiveDays,
                CaseComplaintDays = customerToCopySettings.CaseComplaintDays,
                ModuleCaseInvoice = customerToCopySettings.ModuleCaseInvoice,
                PreventToSaveCaseWithInactiveValue = customerToCopySettings.PreventToSaveCaseWithInactiveValue,
                ShowOUsOnDepartmentFilter = customerToCopySettings.ShowOUsOnDepartmentFilter,
                FileIndexingServerName = customerToCopySettings.FileIndexingServerName,
                FileIndexingCatalogName = customerToCopySettings.FileIndexingCatalogName,
				BatchEmail = customerToCopySettings.BatchEmail,
                BulletinBoardWGRestriction = customerToCopySettings.BulletinBoardWGRestriction,
                CalendarWGRestriction = customerToCopySettings.CalendarWGRestriction,
                ModuleExtendedCase = customerToCopySettings.ModuleExtendedCase,
                AttachmentPlacement = customerToCopySettings.AttachmentPlacement,
            };

            //Get CaseSettings to copy
            var caseSettingsToCopy = this._caseSettingsService.GetCaseSettings(customerToCopy.Id);

            foreach (var cs in caseSettingsToCopy)
            {
                var newCustomerCaseSetting = new CaseSettings() { };

                newCustomerCaseSetting.Customer_Id = newCustomerToSave.Id;
                newCustomerCaseSetting.Name = cs.Name;
                newCustomerCaseSetting.Line = cs.Line;
                newCustomerCaseSetting.MinWidth = cs.MinWidth;
                newCustomerCaseSetting.UserGroup = cs.UserGroup;
                newCustomerCaseSetting.ColOrder = cs.ColOrder;

                this._caseSettingsService.SaveCaseSetting(newCustomerCaseSetting, out errors);
            }

            //Get CaseFieldSettings to copy
            var caseFieldSettingsToCopy = this._caseFieldSettingService.GetCaseFieldSettings(customerToCopy.Id);

            var curTime = DateTime.Now;
            foreach (var cfs in caseFieldSettingsToCopy)
            {
                var newCustomerCaseFieldSettings = new CaseFieldSetting() { };

                newCustomerCaseFieldSettings.Customer_Id = newCustomerToSave.Id;
                newCustomerCaseFieldSettings.Name = cfs.Name;
                newCustomerCaseFieldSettings.ShowOnStartPage = cfs.ShowOnStartPage;
                newCustomerCaseFieldSettings.Required = cfs.Required;
                newCustomerCaseFieldSettings.RequiredIfReopened = cfs.RequiredIfReopened;
                newCustomerCaseFieldSettings.ShowExternal = cfs.ShowExternal;
                newCustomerCaseFieldSettings.FieldSize = cfs.FieldSize;
                newCustomerCaseFieldSettings.ListEdit = cfs.ListEdit;
                newCustomerCaseFieldSettings.EMailIdentifier = cfs.EMailIdentifier;
                newCustomerCaseFieldSettings.ChangedDate = curTime;
                newCustomerCaseFieldSettings.CaseFieldSettingsGUID = Guid.NewGuid();
                
                this._customerService.SaveCaseFieldSettingsForCustomerCopy(newCustomerToSave.Id, newCustomerToSave.Language_Id, newCustomerCaseFieldSettings, out errors);
            }

            //Get CaseFieldSettingLang
            var language = this._languageService.GetLanguages();

            foreach (var l in language)
            {
                var caseFieldSettingsLangToCopy = this._caseFieldSettingService.GetCaseFieldSettingsWithLanguages(customerToCopy.Id, l.Id);
                var caseFieldSettingsForNewCustomer = this._caseFieldSettingService.GetCaseFieldSettings(newCustomerToSave.Id);

                foreach (var cfsl in caseFieldSettingsLangToCopy)
                {

                    foreach (var cfs in caseFieldSettingsForNewCustomer)
                    {
                        if (cfsl.Name == cfs.Name)
                        {
                            var newCustomerCaseFieldSettingsLang = new CaseFieldSettingLanguage() { };

                            newCustomerCaseFieldSettingsLang.CaseFieldSettings_Id = cfs.Id;
                            newCustomerCaseFieldSettingsLang.Label = cfsl.Label;
                            newCustomerCaseFieldSettingsLang.Language_Id = cfsl.Language_Id;
                            newCustomerCaseFieldSettingsLang.FieldHelp = cfsl.FieldHelp;


                            this._customerService.SaveCaseFieldSettingsLangForCustomerCopy(newCustomerCaseFieldSettingsLang, out errors);

                            break;
                        }

                    }
                }                
            }


            // Get ComputerUserFieldSettings
            var computerUserFieldSettingsToCopy = this._computerService.GetComputerUserFieldSettings(customerToCopy.Id);

            foreach (var cufs in computerUserFieldSettingsToCopy)
            {
                var newCustomerComputerUserFS = new ComputerUserFieldSettings() { };

                newCustomerComputerUserFS.Customer_Id = newCustomerToSave.Id;
                newCustomerComputerUserFS.ComputerUserField = cufs.ComputerUserField;
                newCustomerComputerUserFS.Show = cufs.Show;
                newCustomerComputerUserFS.Required = cufs.Required;
                newCustomerComputerUserFS.MinLength = cufs.MinLength;
                newCustomerComputerUserFS.ShowInList = cufs.ShowInList;
                newCustomerComputerUserFS.LDAPAttribute = cufs.LDAPAttribute;

                this._computerService.SaveComputerUserFieldSettingForCustomerCopy(newCustomerComputerUserFS, out errors);
            }

            //ComputerUserFieldSettingsLanguage

            foreach (var l in language)
            {
                //var computerUserFieldSettingsLangToCopy = this._computerService.GetComputerUserFieldSettingsWithLanguages(customerToCopy.Id, l.Id);
                var computerUserFieldSettingsLangToCopy = this._computerService.GetComputerUserFieldSettingsWithLanguagesForDefaultCust(l.Id);
                var computerUserFieldSettingsForNewCustomer = this._computerService.GetComputerUserFieldSettings(newCustomerToSave.Id);

                if (computerUserFieldSettingsLangToCopy != null)
                {

                    foreach (var cfsl in computerUserFieldSettingsLangToCopy)
                    {

                        foreach (var cfs in computerUserFieldSettingsForNewCustomer)
                        {
                            if (cfsl.Name == cfs.ComputerUserField)
                            {
                                var newComputerUserFSL = new ComputerUserFieldSettingsLanguage
                                {
                                    ComputerUserFieldSettings_Id = cfs.Id,
                                    Language_Id = cfsl.Language_Id,
                                    Label = cfsl.Label,
                                    FieldHelp = cfsl.FieldHelp,
                                };

                                this._computerService.SaveComputerUserFieldSettingLangForCustomerCopy(newComputerUserFSL, out errors);

                                break;
                            }

                        }

                    }

                }

            }

            

            //Get CustomerUser to copy
            var customerUserToCopy = this._customerUserService.GetCustomerUsersForCustomer(customerToCopy.Id);

            foreach (var cu in customerUserToCopy)
            {
                var newCustomerCustomerUser = new CustomerUser() { };

                newCustomerCustomerUser.Customer_Id = newCustomerToSave.Id;
                newCustomerCustomerUser.User_Id = cu.User_Id;
                newCustomerCustomerUser.CasePerformerFilter = string.Empty;
                newCustomerCustomerUser.ShowOnStartPage = cu.ShowOnStartPage;
                newCustomerCustomerUser.UserInfoPermission = cu.UserInfoPermission;

                this._customerUserService.SaveCustomerUserForCopy(newCustomerCustomerUser, out errors);
            }



            //Get Casetype to copy
            var caseTypesToCopy = this._caseTypeService.GetCaseTypes(customerToCopy.Id).Where(x=> x.Parent_CaseType_Id == null);
            

            foreach (var ct in caseTypesToCopy)
            {
                var newCustomerCaseType = new CaseType() { };

                newCustomerCaseType.Customer_Id = newCustomerToSave.Id;
                newCustomerCaseType.Name = ct.Name;
                newCustomerCaseType.IsDefault = ct.IsDefault;
                newCustomerCaseType.RequireApproving = ct.RequireApproving;
                newCustomerCaseType.ShowOnExternalPage = ct.ShowOnExternalPage;
                newCustomerCaseType.IsEMailDefault = ct.IsEMailDefault;
                newCustomerCaseType.AutomaticApproveTime = ct.AutomaticApproveTime;
                newCustomerCaseType.User_Id = ct.User_Id;
                newCustomerCaseType.IsActive = ct.IsActive;
                newCustomerCaseType.Selectable = ct.Selectable;
                newCustomerCaseType.ITILProcess = 0;
                newCustomerCaseType.RelatedField = String.Empty;
                newCustomerCaseType.Parent_CaseType_Id = null;

                
                this._caseTypeService.SaveCaseType(newCustomerCaseType, out errors);

                // Save sub case types
                CopyCaseTypeChildren(ct.SubCaseTypes.ToList(), newCustomerToSave.Id, newCustomerCaseType.Id);

            };

            //Get Category to copy
            var categoriesToCopy = this._categoryService.GetAllCategories(customerToCopy.Id);

            foreach (var c in categoriesToCopy)
            {
                var newCustomerCategory = new Category() { };

                newCustomerCategory.Customer_Id = newCustomerToSave.Id;
                newCustomerCategory.Name = c.Name;
                newCustomerCategory.Description = c.Description;
                newCustomerCategory.IsActive = c.IsActive;
                newCustomerCategory.Parent_Category_Id = c.Parent_Category_Id;
                newCustomerCategory.CreatedDate = DateTime.UtcNow;
                newCustomerCategory.ChangedDate = DateTime.UtcNow;

                this._categoryService.SaveCategory(newCustomerCategory, out errors);
            }

            //Get Priority to copy
            var prioritiesToCopy = this._priorityService.GetPriorities(customerToCopy.Id);

            foreach (var pr in prioritiesToCopy)
            {
                var newCustomerPriority = new Priority() { };

                newCustomerPriority.Customer_Id = newCustomerToSave.Id;
                newCustomerPriority.Name = pr.Name;
                newCustomerPriority.Description = pr.Description;
                newCustomerPriority.SolutionTime = pr.SolutionTime;
                newCustomerPriority.IsDefault = pr.IsDefault;
                newCustomerPriority.IsEmailDefault = pr.IsEmailDefault;
                newCustomerPriority.IsActive = pr.IsActive;
                newCustomerPriority.Code = pr.Code;

                this._priorityService.SavePriority(newCustomerPriority, out errors);

            }


            //Get Product area to copy
            var productAreasToCopy = this._productAreaService.GetAllProductAreas(customerToCopy.Id);

            foreach (var p in productAreasToCopy)
            {
                var newCustomerProductArea = new ProductArea() { };

                newCustomerProductArea.Customer_Id = newCustomerToSave.Id;
                newCustomerProductArea.Name = p.Name;
                newCustomerProductArea.Parent_ProductArea_Id = p.Parent_ProductArea_Id;
                newCustomerProductArea.IsActive = p.IsActive;

                this._productAreaService.SaveProductArea(newCustomerProductArea, null, out errors);
            }
            
            //Get StateSecondary to copy
            var stateSecondariesToCopy = this._stateSecondaryService.GetStateSecondaries(customerToCopy.Id).ToList();

            foreach (var ss in stateSecondariesToCopy)
            {
                var newCustomerStateSecondaries = new StateSecondary() { };

                newCustomerStateSecondaries.Customer_Id = newCustomerToSave.Id;
                newCustomerStateSecondaries.Name = ss.Name;
                newCustomerStateSecondaries.IncludeInCaseStatistics = ss.IncludeInCaseStatistics;
                newCustomerStateSecondaries.NoMailToNotifier = ss.NoMailToNotifier;
                newCustomerStateSecondaries.ResetOnExternalUpdate = ss.ResetOnExternalUpdate;
                newCustomerStateSecondaries.IsActive = ss.IsActive;

                this._stateSecondaryService.SaveStateSecondary(newCustomerStateSecondaries, out errors);
            }

            //Get WorkingGroups to copy
            var workingGroupsToCopy = this._workingGroupService.GetWorkingGroupsForIndexPage(customerToCopy.Id);

            foreach (var wg in workingGroupsToCopy)
            {
                var newCustomerWorkingGroup = new WorkingGroupEntity() { };

                newCustomerWorkingGroup.Customer_Id = newCustomerToSave.Id;
                newCustomerWorkingGroup.WorkingGroupName = wg.WorkingGroupName;
                newCustomerWorkingGroup.Code = wg.Code;
                newCustomerWorkingGroup.EMail = wg.EMail;
                newCustomerWorkingGroup.AllocateCaseMail = wg.AllocateCaseMail;
                newCustomerWorkingGroup.IsDefault = wg.IsDefault;
                newCustomerWorkingGroup.IsDefaultCalendar = wg.IsDefaultCalendar;
                newCustomerWorkingGroup.IsDefaultBulletinBoard = wg.IsDefaultBulletinBoard;
                newCustomerWorkingGroup.IsDefaultOperationLog = wg.IsDefaultOperationLog;
                newCustomerWorkingGroup.IsActive = wg.IsActive;

                this._workingGroupService.SaveWorkingGroup(newCustomerWorkingGroup, out errors);
            }

            //Get Status to copy
            var statusesToCopy = this._statusService.GetStatuses(customerToCopy.Id).ToList();

            foreach (var s in statusesToCopy)
            {
                var newCustomerStatuses = new Status() { };

                newCustomerStatuses.Customer_Id = newCustomerToSave.Id;
                newCustomerStatuses.Name = s.Name;
                newCustomerStatuses.IsDefault = s.IsDefault;

                this._statusService.SaveStatus(newCustomerStatuses, out errors);
            }

            //Get FinishingCause to copy
            var finishingCausesToCopy = this._finishingCauseService.GetFinishingCauses(customerToCopy.Id);

            foreach (var fc in finishingCausesToCopy)
            {
                var newCustomerFinishingCause = new FinishingCause() { };

                newCustomerFinishingCause.Customer_Id = newCustomerToSave.Id;
                newCustomerFinishingCause.Name = fc.Name;
                newCustomerFinishingCause.Parent_FinishingCause_Id = fc.Parent_FinishingCause_Id;
                newCustomerFinishingCause.FinishingCauseCategory_Id = fc.FinishingCauseCategory_Id;
                newCustomerFinishingCause.PromptUser = fc.PromptUser;

                this._finishingCauseService.SaveFinishingCause(newCustomerFinishingCause, out errors);
            }

            //Get CaseSolutionCategory to copy
            var caseSolutionCategoriesToCopy = this._caseSolutionService.GetCaseSolutionCategories(customerToCopy.Id);

            foreach (var csc in caseSolutionCategoriesToCopy)
            {
                var newCustomerCaseSolutionCategories = new CaseSolutionCategory() { };

                newCustomerCaseSolutionCategories.Customer_Id = newCustomerToSave.Id;
                newCustomerCaseSolutionCategories.Name = csc.Name;
                newCustomerCaseSolutionCategories.IsDefault = csc.IsDefault;

                this._caseSolutionService.SaveCaseSolutionCategory(newCustomerCaseSolutionCategories, out errors);
            }

            // Get CaseSolution to copy
            var caseSolutionToCopy = this._caseSolutionService.GetCaseSolutions(customerToCopy.Id);

            foreach (var cs in caseSolutionToCopy)
            {
                var newCustomerCaseSolution = new CaseSolution() { };

                newCustomerCaseSolution.Customer_Id = newCustomerToSave.Id;
                newCustomerCaseSolution.WorkingGroup_Id = cs.WorkingGroup_Id;
                newCustomerCaseSolution.CaseSolutionCategory_Id = cs.CaseSolutionCategory_Id;
                newCustomerCaseSolution.Name = cs.Name;
                newCustomerCaseSolution.CaseType_Id = cs.CaseType_Id;
                newCustomerCaseSolution.ProductArea_Id = cs.ProductArea_Id;
                newCustomerCaseSolution.Caption = cs.Caption;
                newCustomerCaseSolution.Description = cs.Description;
                newCustomerCaseSolution.Miscellaneous = cs.Miscellaneous;
                newCustomerCaseSolution.Priority_Id = cs.Priority_Id;
                newCustomerCaseSolution.Text_External = cs.Text_External;
                newCustomerCaseSolution.Text_Internal = cs.Text_Internal;
                newCustomerCaseSolution.FinishingCause_Id = cs.FinishingCause_Id;
                newCustomerCaseSolution.CaseWorkingGroup_Id = cs.CaseWorkingGroup_Id;
                newCustomerCaseSolution.Category_Id = cs.Category_Id;

                this._caseSolutionService.SaveCaseSolution(newCustomerCaseSolution, null, null, out errors);
            }

            //Get Mailtemplate
            for (int i = 1; i < 100; i++)
            {
                var mailTemplateToCopy = this._mailTemplateService.GetMailTemplateForCopyCustomer(i, customerToCopy.Id);

                if (mailTemplateToCopy != null)
                {
                    var mailTemplateToSave = new MailTemplateEntity
                    {
                        //Id = id,
                        MailID = mailTemplateToCopy.MailID,
                        Customer_Id = newCustomerToSave.Id

                    };

                    this._mailTemplateService.SaveMailTemplate(mailTemplateToSave, out errors);

                    //Get MailTemplateLanguage
                    foreach (var l in language)
                    {
                        var mailTemplateLanguageToCopy = this._mailTemplateService.GetMailTemplateLanguageForCustomer(mailTemplateToCopy.Id, customerToCopy.Id, l.Id);



                        if (mailTemplateLanguageToCopy != null)
                        {
                            var newMailTemplateLanguage = new MailTemplateLanguageEntity
                            {
                                MailTemplate_Id = mailTemplateToSave.Id,
                                Language_Id = mailTemplateLanguageToCopy.Language_Id,
                                MailTemplate = mailTemplateToSave,
                                Subject = mailTemplateLanguageToCopy.Subject,
                                Body = mailTemplateLanguageToCopy.Body,
                                MailTemplateName = mailTemplateLanguageToCopy.MailTemplateName,
                            };

                            this._mailTemplateService.SaveMailTemplateLanguage(newMailTemplateLanguage, false, out errors);
                        }

                    }
                }

               
            }

            this._customerService.SaveEditCustomer(newCustomerToSave, newCustomerSetting, null, newCustomerToSave.Language_Id, out errors);


            return newCustomerToSave.Id;
        }

        private void CopyCaseTypeChildren(List<CaseType> caseTypeChildren, int newCustomerId, int parentId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();

            foreach (var ct in caseTypeChildren)
            {
                var newCustomerCaseType = new CaseType() { };

                newCustomerCaseType.Customer_Id = newCustomerId;
                newCustomerCaseType.Name = ct.Name;
                newCustomerCaseType.IsDefault = ct.IsDefault;
                newCustomerCaseType.RequireApproving = ct.RequireApproving;
                newCustomerCaseType.ShowOnExternalPage = ct.ShowOnExternalPage;
                newCustomerCaseType.IsEMailDefault = ct.IsEMailDefault;
                newCustomerCaseType.AutomaticApproveTime = ct.AutomaticApproveTime;
                newCustomerCaseType.User_Id = ct.User_Id;
                newCustomerCaseType.IsActive = ct.IsActive;
                newCustomerCaseType.Selectable = ct.Selectable;
                newCustomerCaseType.ITILProcess = 0;
                newCustomerCaseType.RelatedField = String.Empty;
                newCustomerCaseType.Parent_CaseType_Id = parentId;                

                this._caseTypeService.SaveCaseType(newCustomerCaseType, out errors);

                // Save sub case types
                CopyCaseTypeChildren(ct.SubCaseTypes.ToList(), newCustomerId, newCustomerCaseType.Id);
            };
        }
    }
}