
namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using DHDomain=DH.Helpdesk.Domain;
    using System;
    using System.Linq;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
    using System.Collections.Generic;
    using System.Web.Configuration;
    
    

    public class CustomerSettingsController : BaseAdminController
    {
         private readonly ICustomerService _customerService;
         private readonly ISettingService _settingService;
         private readonly ILanguageService _languageService;
        private readonly ICaseSolutionService _caseSolutionService;

        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IWorkContext workContext;

        public CustomerSettingsController(
            ICustomerService customerService,
            ISettingService settingService,
            ILanguageService languageService,
            IMasterDataService masterDataService, IWorkContext workContext,
            ICaseSolutionService caseSolutionService)
            : base(masterDataService)
        {
            this._customerService = customerService;
            this._settingService = settingService;
            this._languageService = languageService;
            this.workContext = workContext;
            this._caseSolutionService = caseSolutionService;
        }

        [CustomAuthorize(Roles = "3,4")]
        public ActionResult Edit(int id)
        {
            var customer = this._customerService.GetCustomer(id);

            if (customer == null)
                return new HttpNotFoundResult("No customer found...");

            var setting = this._settingService.GetCustomerSetting(id);
            
            if (setting == null)
            {
                setting = new DHDomain.Setting { Customer_Id = id };
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
        public ActionResult Edit(int id, DHDomain.Customer customer, FormCollection coll, CustomerInputViewModel vmodel, List<DHDomain.ReportCustomer> ReportCustomers, int[] UsSelected)
        {
            var customerToSave = this._customerService.GetCustomer(id);
            customerToSave.NDSPath = vmodel.Customer.NDSPath;

            var b = this.TryUpdateModel(customerToSave, "customer");
            var setting = this._settingService.GetCustomerSetting(id);
            
            if (setting != null && vmodel.Setting != null)
            {
                //setting.CaseFiles = 6;
                //setting.ComputerUserInfoListLocation = 1;
                //setting.ModuleCase = 1;
                vmodel.Setting.DefaultAdministrator = setting.DefaultAdministrator;
                vmodel.Setting.DefaultAdministratorExternal = setting.DefaultAdministratorExternal;
                vmodel.Setting.CreateCaseFromOrder = setting.CreateCaseFromOrder;
                vmodel.Setting.CreateComputerFromOrder = setting.CreateComputerFromOrder;
                vmodel.Setting.IsUserFirstLastNameRepresentation = setting.IsUserFirstLastNameRepresentation;
            }

            vmodel.Setting.Id = setting.Id;
            vmodel.Setting.CaseFiles = setting.CaseFiles;
            vmodel.Setting.Customer_Id = setting.Customer_Id;
            vmodel.Setting.LDAPPassword = setting.LDAPPassword.ToString();
            vmodel.Setting.ComputerUserInfoListLocation = setting.ComputerUserInfoListLocation;
            vmodel.Setting.ModuleCase = setting.ModuleCase;

            //Now these settings exists in the UI, therefore disable these resetters
            //vmodel.Setting.POP3Server = setting.POP3Server.ToString();
            //vmodel.Setting.POP3Port = setting.POP3Port;
            //vmodel.Setting.POP3UserName = setting.POP3UserName.ToString();
            //vmodel.Setting.POP3Password = setting.POP3Password.ToString();
            //vmodel.Setting.POP3DebugLevel = setting.POP3DebugLevel;
            //vmodel.Setting.PhysicalFilePath = setting.PhysicalFilePath;
            //vmodel.Setting.MailServerProtocol = setting.MailServerProtocol;



            vmodel.Setting.EMailAnswerSeparator = setting.EMailAnswerSeparator.ToString();
            vmodel.Setting.EMailSubjectPattern = setting.EMailSubjectPattern.ToString();
            vmodel.Setting.LDAPSyncType = setting.LDAPSyncType;
            vmodel.Setting.LDAPCreateOrganization = setting.LDAPCreateOrganization;
           // vmodel.Setting.IntegrationType = setting.IntegrationType;
            
            vmodel.Setting.VirtualFilePath = setting.VirtualFilePath;
            vmodel.Setting.CaseComplaintDays = setting.CaseComplaintDays;
            vmodel.Setting.FileIndexingServerName = setting.FileIndexingServerName;
            vmodel.Setting.FileIndexingCatalogName = setting.FileIndexingCatalogName;
            vmodel.Setting.SMSEMailDomain = setting.SMSEMailDomain;
            vmodel.Setting.SMSEMailDomainPassword = setting.SMSEMailDomainPassword;
            vmodel.Setting.SMSEMailDomainUserId = setting.SMSEMailDomainUserId;
            vmodel.Setting.SMSEMailDomainUserName = setting.SMSEMailDomainUserName;
            vmodel.Setting.EMailRegistrationMailID = setting.EMailRegistrationMailID;
            vmodel.Setting.DefaultEmailLogDestination = setting.DefaultEmailLogDestination;
            vmodel.Setting.TimeZone_offset = setting.TimeZone_offset;
            vmodel.Setting.CalcSolvedInTimeByLatestSLADate = setting.CalcSolvedInTimeByLatestSLADate;
            vmodel.Setting.SMTPPassWord = setting.SMTPPassWord;
            vmodel.Setting.SMTPPort = setting.SMTPPort;
            vmodel.Setting.SMTPServer = setting.SMTPServer;
            vmodel.Setting.SMTPUserName = setting.SMTPUserName;
            vmodel.Setting.IsSMTPSecured = setting.IsSMTPSecured;
            vmodel.Setting.BatchEmail = setting.BatchEmail;
            vmodel.Setting.InvoiceType = setting.InvoiceType;
            vmodel.Setting.CaseSMS = setting.CaseSMS;

            //keep original value
            if (vmodel.Setting.ComplexPassword > 0)
            {
                vmodel.Setting.MinPasswordLength = setting.MinPasswordLength;
            }

            if (customerToSave == null)
                throw new Exception("No customer found...");

            IDictionary<string, string> errors = new Dictionary<string, string>();

            this._customerService.SaveCustomerSettings(customerToSave, vmodel.Setting, ReportCustomers, customer.Language_Id, out errors);

            if (errors.Count == 0)
            {
                this.workContext.Customer.Refresh();
                return this.RedirectToAction("edit", "customersettings");
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

        private CustomerInputViewModel CustomerInputViewModel(DHDomain.Customer customer)
        {
            if (customer.Id == 0)
            {
                customer.Language_Id = SessionFacade.CurrentLanguageId;
            }

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
             #region Reports

            /* Hide unfinished report options Redmine #13433 */
            //var reports = this._customerService.GetAllReports();
            //var customerReports = this._customerService.GetCustomerReportList(customer.Id);

            var reports = this._customerService.GetAllReports().Where(r => r.Id == (int)ReportType.ReportGenerator || r.Id == (int)ReportType.ReportGeneratorExtendedCase);
            var customerReports = this._customerService.GetCustomerReportList(customer.Id).Where(r => r.ReportId == (int)ReportType.ReportGenerator || r.ReportId == (int)ReportType.ReportGeneratorExtendedCase);
            var reportList = new List<CustomerReportList>();

            
            
            foreach (var rep in reports)
            {
                var o = new CustomerReportList
                {
                    ReportId = rep.Id,
                };

                if (rep.Id == 2)
                    o.ReportName = Translation.Get("Rapport - Ledtid (avslutade ärenden)", Enums.TranslationSource.TextTranslation);
                else if (rep.Id == 3)
                    o.ReportName = Translation.Get("Rapport - Ledtid (aktiva ärenden)", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 4)
                    o.ReportName = Translation.Get("Rapport - Avslutsorsak per avdelning", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 19)
                    o.ReportName = Translation.Get("Rapport - Avslutskategori per avdelning", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 5)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Rapport - Avslutade ärenden per dag", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 6)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
				else if (rep.Id == 21)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("timme", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 7)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
				else if (rep.Id == 8)
                    o.ReportName = Translation.Get("Rapport - Servicerapport", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 15)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("driftgrupp", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 17)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("ärendetyp", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 9)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Frågeregistrering", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 14)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärendetyp", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("produktområde", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 16)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärendetyp", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("leverantör", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 13)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Genomsnittlig lösningstid", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 20)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Källa registrering", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == 22)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Svarstid", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == (int)ReportType.ReportGenerator)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Rapportgenerator", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == (int)ReportType.ReportGeneratorExtendedCase)
					o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Rapportgenerator", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Utökat ärende", Enums.TranslationSource.TextTranslation);
				else if (rep.Id == (int)ReportType.CaseSatisfaction)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Case satisfaction", Enums.TranslationSource.TextTranslation);

                reportList.Add(o);
            }

            foreach (var report in customerReports)
            {
                int index = reportList.FindIndex(x => x.ReportId == report.ReportId);
                //if (index > 0)
                    reportList[index].ActiveOnPage = report.ActiveOnPage;
            }

            #endregion

            #region Model

            var model = new CustomerInputViewModel
            {
                //CustomerCaseSummaryViewModel = new CustomerCaseSummaryViewModel(),
                Customer = customer,
                ListCustomerReports = reportList,
                MinimumPasswordLength = sl,
                PasswordHistory = sli,
                Setting = _settingService.GetCustomerSetting(customer.Id) ?? new DHDomain.Setting(),
                CaseSolutionList = _caseSolutionService.GetCustomerCaseSolutionsOverview(customer.Id).Where(x => x.Status == 1).Select(x => new SelectListItem
                {
                    Text = Translation.GetCoreTextTranslation(x.Name),
                    Value = x.CaseSolutionId.ToString(),
                }).ToList(),

            };

            model.Setting.LDAPPassword = WebConfigurationManager.AppSettings["dh_maskedpassword"].ToString();
            #endregion
            return model;
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

        [CustomAuthorize(Roles = "3,4")]
        [HttpPost]
        public void SaveLDAPPassword(int id, string newPassword, string confirmPassword)
        {
            var setting = this._settingService.GetCustomerSetting(id);

            if (setting == null)
            {
                setting = new DHDomain.Setting() { Customer_Id = id };
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


    }
}