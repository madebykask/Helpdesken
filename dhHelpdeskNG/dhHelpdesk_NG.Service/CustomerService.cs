using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
    public interface ICustomerService
    {
        IList<Customer> GetAllCustomers();
        IList<Customer> GetCustomers(int customerId);
        IList<Customer> SearchAndGenerateCustomers(ICustomerSearch SearchCustomers);
        IList<CustomerReportList> GetCustomerReportList(int id);
        IList<Report> GetAllReports();

        Customer GetCustomer(int id);
        ReportCustomer GetReportCustomerById(int customerId, int reportId);

        DeleteMessage DeleteCustomer(int id);

        void SaveCaseFieldSettingsForCustomer(Customer customer, Setting setting, int[] us, List<CaseFieldSetting> CaseFieldSettings, int LanguageId, out IDictionary<string, string> errors);
        void SaveEditCustomer(Customer customer, Setting setting, int[] us, int LanguageId, out IDictionary<string, string> errors);
        void SaveNewCustomerToGetId(Customer customer, out IDictionary<string, string> errors);
        void Commit();

        void SaveCustomerSettings(Customer customerToSave, Setting setting, List<ReportCustomer> ReportCustomers, int LanguageId, out IDictionary<string, string> errors);
    }                            

    public class CustomerService : ICustomerService
    {
        private readonly ICaseFieldSettingRepository _caseFieldSettingRepository;
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IReportCustomerRepository _reportCustomerRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ISettingService _settingService;

        public CustomerService(
            ICaseFieldSettingRepository caseFieldSettingRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            ICustomerRepository customerRepository,
            IReportRepository reportRepository,
            IReportCustomerRepository reportCustomerRepository,
            ISettingRepository settingRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,

            ICaseFieldSettingService caseFieldSettingService,
            ISettingService settingService)
        {
            _caseFieldSettingRepository = caseFieldSettingRepository;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository;
            _customerRepository = customerRepository;
            _reportRepository = reportRepository;
            _reportCustomerRepository = reportCustomerRepository;
            _settingRepository = settingRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;

            _caseFieldSettingService = caseFieldSettingService;
            _settingService = settingService;
        }

        public IList<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll().ToList();
        }

        public IList<Customer> GetCustomers(int customerId)
        {
            return _customerRepository.GetMany(x => x.Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<Customer> SearchAndGenerateCustomers(ICustomerSearch SearchCustomers)
        {
            string s = SearchCustomers.SearchCs.ToLower();
            var query = (from c in _customerRepository.GetAll()
                         select c);

            if (!string.IsNullOrEmpty(s))
                query = query.Where(x => x.Address.ToLower().Contains(s)
                    || x.CustomerID.ToLower().Contains(s)
                    || x.CustomerNumber.ToLower().Contains(s)
                    || x.Name.ToLower().Contains(s)
                    || x.Phone.ToLower().Contains(s)
                    || x.PostalAddress.ToLower().Contains(s)
                    || x.PostalCode.ToLower().Contains(s));

            return query.OrderBy(x => x.Name).ToList();
        }

        public IList<CustomerReportList> GetCustomerReportList(int id)
        {
            return _reportCustomerRepository.GetCustomerReportListForCustomer(id).ToList();
        }

        public IList<Report> GetAllReports()
        {
            return _reportRepository.GetAll().ToList();
        }

        public Customer GetCustomer(int id)
        {
            return _customerRepository.GetById(id);
        }

        public ReportCustomer GetReportCustomerById(int customerId, int reportId)
        {
            return _reportCustomerRepository.Get(x => x.Customer_Id == customerId && x.Report_Id == reportId);
        }

        public DeleteMessage DeleteCustomer(int id)
        {
            var customer = _customerRepository.GetById(id);

            if (customer != null)
            {
                try
                {
                    _customerRepository.Delete(customer);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }
       // public void SaveEditCustomer(Customer customer, Setting setting, int[] us, List<ReportCustomer> ReportCustomers, List<CaseFieldSetting> CaseFieldSettings, int LanguageId, out IDictionary<string, string> errors)
        public void SaveEditCustomer(Customer customer, Setting setting, int[] us, int LanguageId, out IDictionary<string, string> errors)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            errors = new Dictionary<string, string>();

            #region Check if fields are filled out or should be left empty

            customer.Address = customer.Address ?? string.Empty;
            customer.CaseStatisticsEmailList = customer.CaseStatisticsEmailList ?? string.Empty;
            customer.CustomerID = customer.CustomerID ?? string.Empty;
            customer.CustomerNumber = customer.CustomerNumber ?? string.Empty;
            customer.DailyReportEmail = customer.DailyReportEmail ?? string.Empty;
            customer.DirectoryPathExclude = customer.DirectoryPathExclude ?? string.Empty;
            customer.HelpdeskEmail = customer.HelpdeskEmail ?? string.Empty;
            customer.Logo = customer.Logo ?? string.Empty;
            customer.LogoBackColor = customer.LogoBackColor ?? string.Empty;
            customer.NDSPath = customer.NDSPath ?? string.Empty;
            customer.NewCaseEmailList = customer.NewCaseEmailList ?? string.Empty;
            customer.CloseCaseEmailList = customer.CloseCaseEmailList ?? string.Empty;
            customer.OrderEMailList = customer.OrderEMailList ?? string.Empty;
            customer.PostalAddress = customer.PostalAddress ?? string.Empty;
            customer.PostalCode = customer.PostalCode ?? string.Empty;
            customer.Phone = customer.Phone ?? string.Empty;
            customer.RegistrationMessage = customer.RegistrationMessage ?? string.Empty;
            customer.ResponsibleReminderEmailList = customer.ResponsibleReminderEmailList ?? string.Empty;
            customer.CaseStatisticsEmailList = customer.CaseStatisticsEmailList ?? string.Empty;
            customer.DailyReportEmail = customer.DailyReportEmail ?? string.Empty;
            customer.NewCaseEmailList = customer.NewCaseEmailList ?? string.Empty;

            #endregion

            if (string.IsNullOrEmpty(customer.Name))
                errors.Add("Customer.Name", "Du måste ange ett kundnamn");

            #region Users

            if (customer.Users != null)
                foreach (var delete in customer.Users.ToList())
                    customer.Users.Remove(delete);
            else
                customer.Users = new List<User>();

            if (us != null)
            {
                foreach (int id in us)
                {
                    var u = _userRepository.GetById(id);

                    if (u != null)
                        customer.Users.Add(u);
                }
            }

            #endregion

            //#region CaseFieldSettings

            //if (customer.CaseFieldSettings != null)
            //    foreach (var delete in customer.CaseFieldSettings.ToList())
            //        customer.CaseFieldSettings.Remove(delete);
            //else
            //    customer.CaseFieldSettings = new List<CaseFieldSetting>();

            ////TODO ALF: här ska jag spara ner ny och befintlig förändring i Customer, flik Case. I befintlig funkar den, men när en kund inte har några casefieldsettings på sig blir den galen!
            //if (CaseFieldSettings != null)
            //{
            //    foreach (var change in CaseFieldSettings)
            //    {
            //        var rowCfs = _caseFieldSettingRepository.Get(x => x.Customer_Id == customer.Id);
            //        //TODO ALF: kollar först ovan om det finns inställningar på denna kund, men annars skall den skapa nya inställningar
            //        if (rowCfs == null)
            //        {
            //            rowCfs = new CaseFieldSetting() { Customer_Id = customer.Id };
            //        }

            //        var rowCfsl = _caseFieldSettingLanguageRepository.Get(x => x.Language_Id == LanguageId);
            //        //TODO ALF: kollar först ovan om det finns översättningar på inställningar på denna kund, men annars skall den skapa nya översättningar till rätt inställningsid
            //        if (rowCfsl == null)
            //        {
            //            rowCfsl = new CaseFieldSettingLanguage() { CaseFieldSettings_Id = rowCfs.Id };
            //        }

            //        foreach (var label in _caseFieldSettingRepository.GetAll().Where(x => x.Id == rowCfs.Id))
            //        {
            //            rowCfs.Customer_Id = customer.Id;
            //            rowCfs.DefaultValue = change.DefaultValue;
            //            rowCfs.FieldSize = change.FieldSize;
            //            rowCfs.ListEdit = change.ListEdit;
            //            rowCfs.Name = label.Name;
            //            rowCfs.NameOrigin = label.NameOrigin;
            //            rowCfs.RelatedField = change.RelatedField;
            //            rowCfs.Required = change.Required;
            //            rowCfs.ShowExternal = change.ShowExternal;
            //            rowCfs.ShowOnStartPage = change.ShowOnStartPage;
            //            rowCfsl.Language_Id = LanguageId;
            //            rowCfsl.Label = label.NameOrigin;

            //            if (rowCfs != null)
            //                customer.CaseFieldSettings.Add(rowCfs);

            //            if (rowCfsl != null)
            //                _caseFieldSettingLanguageRepository.Add(rowCfsl);
            //        }
            //    }
            //}

            //#endregion

            if (customer.Id == 0)
                _customerRepository.Add(customer);
            else
                _customerRepository.Update(customer);

            
            _settingService.SaveSettingForCustomerEdit(setting, out errors);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveCaseFieldSettingsForCustomer(Customer customer, Setting setting, int[] us, List<CaseFieldSetting> CaseFieldSettings, int LanguageId, out IDictionary<string, string> errors)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            errors = new Dictionary<string, string>();

            #region Check if fields are filled out or should be left empty

            customer.Address = customer.Address ?? string.Empty;
            customer.CaseStatisticsEmailList = customer.CaseStatisticsEmailList ?? string.Empty;
            customer.CustomerID = customer.CustomerID ?? string.Empty;
            customer.CustomerNumber = customer.CustomerNumber ?? string.Empty;
            customer.DailyReportEmail = customer.DailyReportEmail ?? string.Empty;
            customer.DirectoryPathExclude = customer.DirectoryPathExclude ?? string.Empty;
            customer.HelpdeskEmail = customer.HelpdeskEmail ?? string.Empty;
            customer.Logo = customer.Logo ?? string.Empty;
            customer.LogoBackColor = customer.LogoBackColor ?? string.Empty;
            customer.NDSPath = customer.NDSPath ?? string.Empty;
            customer.NewCaseEmailList = customer.NewCaseEmailList ?? string.Empty;
            customer.CloseCaseEmailList = customer.CloseCaseEmailList ?? string.Empty;
            customer.OrderEMailList = customer.OrderEMailList ?? string.Empty;
            customer.PostalAddress = customer.PostalAddress ?? string.Empty;
            customer.PostalCode = customer.PostalCode ?? string.Empty;
            customer.Phone = customer.Phone ?? string.Empty;
            customer.RegistrationMessage = customer.RegistrationMessage ?? string.Empty;
            customer.ResponsibleReminderEmailList = customer.ResponsibleReminderEmailList ?? string.Empty;
            customer.CaseStatisticsEmailList = customer.CaseStatisticsEmailList ?? string.Empty;
            customer.DailyReportEmail = customer.DailyReportEmail ?? string.Empty;
            customer.NewCaseEmailList = customer.NewCaseEmailList ?? string.Empty;

            #endregion

            if (string.IsNullOrEmpty(customer.Name))
                errors.Add("Customer.Name", "Du måste ange ett kundnamn");

            #region Users

            if (customer.Users != null)
                foreach (var delete in customer.Users.ToList())
                    customer.Users.Remove(delete);
            else
                customer.Users = new List<User>();

            if (us != null)
            {
                foreach (int id in us)
                {
                    var u = _userRepository.GetById(id);

                    if (u != null)
                        customer.Users.Add(u);
                }
            }

            #endregion

            #region CaseFieldSettings

            if (customer.CaseFieldSettings != null)
                foreach (var delete in customer.CaseFieldSettings.ToList())
                    customer.CaseFieldSettings.Remove(delete);
            else
                customer.CaseFieldSettings = new List<CaseFieldSetting>();

            //TODO ALF: här ska jag spara ner ny och befintlig förändring i Customer, flik Case. I befintlig funkar den, men när en kund inte har några casefieldsettings på sig blir den galen!
            if (CaseFieldSettings != null)
            {
                foreach (var change in CaseFieldSettings)
                {
                    var rowCfs = _caseFieldSettingRepository.Get(x => x.Customer_Id == customer.Id);
                    //TODO ALF: kollar först ovan om det finns inställningar på denna kund, men annars skall den skapa nya inställningar
                    if (rowCfs == null)
                    {
                        rowCfs = new CaseFieldSetting() { Customer_Id = customer.Id };
                    }

                    var rowCfsl = _caseFieldSettingLanguageRepository.Get(x => x.Language_Id == LanguageId);
                    //var rowCfsl = _caseFieldSettingLanguageRepository.GetMany(x => x.Language_Id == LanguageId).FirstOrDefault();
                    //TODO ALF: kollar först ovan om det finns översättningar på inställningar på denna kund, men annars skall den skapa nya översättningar till rätt inställningsid
                    if (rowCfsl == null)
                    {
                        rowCfsl = new CaseFieldSettingLanguage() { CaseFieldSettings_Id = rowCfs.Id };
                    }

                    foreach (var label in _caseFieldSettingRepository.GetAll().Where(x => x.Id == rowCfs.Id))
                    {
                        rowCfs.Customer_Id = customer.Id;
                        rowCfs.DefaultValue = change.DefaultValue;
                        rowCfs.FieldSize = change.FieldSize;
                        rowCfs.ListEdit = change.ListEdit;
                        rowCfs.Name = label.Name;
                        rowCfs.NameOrigin = label.NameOrigin;
                        rowCfs.RelatedField = change.RelatedField;
                        rowCfs.Required = change.Required;
                        rowCfs.ShowExternal = change.ShowExternal;
                        rowCfs.ShowOnStartPage = change.ShowOnStartPage;
                        rowCfsl.Language_Id = LanguageId;
                        rowCfsl.Label = label.NameOrigin;

                        if (rowCfs != null)
                            customer.CaseFieldSettings.Add(rowCfs);

                        if (rowCfsl != null)
                            _caseFieldSettingLanguageRepository.Add(rowCfsl);
                       
                    }
                }
            }

            #endregion

            if (customer.Id == 0)
                _customerRepository.Add(customer);
            else
                _customerRepository.Update(customer);


            _settingService.SaveSettingForCustomerEdit(setting, out errors);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveCustomerSettings(Customer customer, Setting setting, List<ReportCustomer> ReportCustomers, int LanguageId, out IDictionary<string, string> errors)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            errors = new Dictionary<string, string>();

            #region ReportCustomers

            //TODO ALF: precis samma problem som finns på casefieldsettings finns här! fixar du det?
            if (customer.ReportCustomers.Count == 0)
            {
                foreach (var r in ReportCustomers)
                {
                    if (r.ShowOnPage == 1)
                    {
                        _reportCustomerRepository.Add(r);
                    }
                }
            }

            foreach (var rc in customer.ReportCustomers)
            {
                foreach (var change in ReportCustomers.Where(x => x.Customer_Id == rc.Customer_Id && x.Report_Id == rc.Report_Id))
                {
                    if (change.ShowOnPage != rc.ShowOnPage)
                    {
                        rc.ShowOnPage = change.ShowOnPage;
                        _reportCustomerRepository.Update(rc);
                    }
                }
            }

            #endregion

            _settingService.SaveSetting(setting, out errors);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveNewCustomerToGetId(Customer customer, out IDictionary<string, string> errors)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            errors = new Dictionary<string, string>();

            customer.Address = customer.Address ?? string.Empty;
            customer.CaseStatisticsEmailList = customer.CaseStatisticsEmailList ?? string.Empty;
            customer.CustomerID = customer.CustomerID ?? string.Empty;
            customer.CustomerNumber = customer.CustomerNumber ?? string.Empty;
            customer.DailyReportEmail = customer.DailyReportEmail ?? string.Empty;
            customer.DirectoryPathExclude = customer.DirectoryPathExclude ?? string.Empty;
            customer.CustomerGUID = Guid.NewGuid();
            customer.HelpdeskEmail = customer.HelpdeskEmail ?? string.Empty;
            customer.Logo = customer.Logo ?? string.Empty;
            customer.LogoBackColor = customer.LogoBackColor ?? string.Empty;
            customer.NDSPath = customer.NDSPath ?? string.Empty;
            customer.NewCaseEmailList = customer.NewCaseEmailList ?? string.Empty;
            customer.OrderEMailList = customer.OrderEMailList ?? string.Empty;
            customer.PostalAddress = customer.PostalAddress ?? string.Empty;
            customer.PostalCode = customer.PostalCode ?? string.Empty;
            customer.Phone = customer.Phone ?? string.Empty;
            customer.RegistrationMessage = customer.RegistrationMessage ?? string.Empty;
            customer.ResponsibleReminderEmailList = customer.ResponsibleReminderEmailList ?? string.Empty;

            if (string.IsNullOrEmpty(customer.Name))
                errors.Add("Customer.Name", "Du måste ange ett kundnamn");

            if (customer.Id == 0)
                _customerRepository.Add(customer);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
