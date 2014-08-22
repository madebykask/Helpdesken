namespace DH.Helpdesk.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models;
    using DH.Helpdesk.Web.Infrastructure;

    public class CustomerSettingsController : BaseAdminController
    {
         private readonly ICustomerService _customerService;
         private readonly ISettingService _settingService;
         private readonly ILanguageService _languageService;

        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IWorkContext workContext;

        public CustomerSettingsController(
            ICustomerService customerService,
            ISettingService settingService,
            ILanguageService languageService,
            IMasterDataService masterDataService, IWorkContext workContext)
            : base(masterDataService)
        {
            this._customerService = customerService;
            this._settingService = settingService;
            this._languageService = languageService;
            this.workContext = workContext;
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
        public ActionResult Edit(int id, Customer customer, FormCollection coll, CustomerInputViewModel vmodel, List<ReportCustomer> ReportCustomers, int[] UsSelected)
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
            }

            vmodel.Setting.Id = setting.Id;
            vmodel.Setting.CaseFiles = setting.CaseFiles;
            vmodel.Setting.Customer_Id = setting.Customer_Id;
            vmodel.Setting.LDAPPassword = setting.LDAPPassword.ToString();

            vmodel.Setting.ComputerUserInfoListLocation = setting.ComputerUserInfoListLocation;
            vmodel.Setting.ModuleCase = setting.ModuleCase;
            //vmodel.Setting.PasswordHistory = returnPasswordHistoryForSave(vmodel);
          

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

        private CustomerInputViewModel CustomerInputViewModel(Customer customer)
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

            var reports = this._customerService.GetAllReports();
            var customerReports = this._customerService.GetCustomerReportList(customer.Id);
            var reportList = new List<CustomerReportList>();

            foreach (var rep in reports)
            {
                var o = new CustomerReportList
                {
                    ReportId = rep.Id,
                };

                if (rep.Id == 2)
                    o.ReportName = Translation.Get("Rapport - Ledtid (avslutade ärenden)", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 3)
                    o.ReportName = Translation.Get("Rapport - Ledtid (aktiva ärenden)", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 4)
                    o.ReportName = Translation.Get("Rapport - Avslutsorsak per avdelning", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 19)
                    o.ReportName = Translation.Get("Rapport - Avslutskategori per avdelning", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 5)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Rapport - Avslutade ärenden per dag", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 6)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
                if (rep.Id == 21)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("timme", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 7)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
                if (rep.Id == 8)
                    o.ReportName = Translation.Get("Rapport - Servicerapport", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 15)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("driftgrupp", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 17)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("ärendetyp", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 9)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Frågeregistrering", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 14)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärendetyp", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("produktområde", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 16)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ärendetyp", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("leverantör", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 13)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Genomsnittlig lösningstid", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 20)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Källa registrering", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 22)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Svarstid", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 18)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Rapportgenerator", Enums.TranslationSource.TextTranslation);

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
                Setting = this._settingService.GetCustomerSetting(customer.Id) ?? new Setting(),
                
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


    }
}