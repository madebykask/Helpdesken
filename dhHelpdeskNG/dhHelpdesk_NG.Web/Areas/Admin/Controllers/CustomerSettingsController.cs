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
    public class CustomerSettingsController : BaseController
    {
         private readonly ICustomerService _customerService;
         private readonly ISettingService _settingService;
         private readonly ILanguageService _languageService;

        public CustomerSettingsController(
            ICustomerService customerService,
            ISettingService settingService,
            ILanguageService languageService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _customerService = customerService;
            _settingService = settingService;
            _languageService = languageService;
        }

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
        public ActionResult Edit(int id, Customer customer, FormCollection coll, CustomerInputViewModel vmodel, List<ReportCustomer> ReportCustomers, int[] UsSelected)
        {
            var customerToSave = _customerService.GetCustomer(id);
            customerToSave.NDSPath = vmodel.Customer.NDSPath;

            var b = TryUpdateModel(customerToSave, "customer");
            var setting = _settingService.GetCustomerSetting(id);

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

            vmodel.Setting.ComputerUserInfoListLocation = setting.ComputerUserInfoListLocation;
            vmodel.Setting.ModuleCase = setting.ModuleCase;
            //vmodel.Setting.PasswordHistory = returnPasswordHistoryForSave(vmodel);
          

            if (customerToSave == null)
                throw new Exception("No customer found...");

            IDictionary<string, string> errors = new Dictionary<string, string>();

            _customerService.SaveCustomerSettings(customerToSave, vmodel.Setting, ReportCustomers, customer.Language_Id, out errors);

            if (errors.Count == 0)
                return RedirectToAction("edit", "customersettings");

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

            var reports = _customerService.GetAllReports();
            var customerReports = _customerService.GetCustomerReportList(customer.Id);
            var reportList = new List<CustomerReportList>();

            foreach (var rep in reports)
            {
                var o = new CustomerReportList
                {
                    ReportId = rep.Id,
                };

                if (rep.Id == 2)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ledtid (avslutade ärenden)", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 3)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Ledtid (aktiva ärenden)", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 4)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Avslutsorsak per avdelning", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 19)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Avslutskategori per avdelning", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 5)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Avslutade ärenden per dag", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 6)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
                if (rep.Id == 21)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Registrerade ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("timme", Enums.TranslationSource.TextTranslation);
                if (rep.Id == 7)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) + "/" + Translation.Get("dag", Enums.TranslationSource.TextTranslation).ToLower();
                if (rep.Id == 8)
                    o.ReportName = Translation.Get("Rapport", Enums.TranslationSource.TextTranslation) + " - " + Translation.Get("Servicerapport", Enums.TranslationSource.TextTranslation);
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
                Setting = _settingService.GetCustomerSetting(customer.Id) ?? new Setting(),
                
            };

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