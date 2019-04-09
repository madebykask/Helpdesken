using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Customer;
using DH.Helpdesk.BusinessData.Models.Customer.Input;
using DH.Helpdesk.BusinessData.OldComponents;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.Domain;

    public interface ICustomerService
    {
        IList<Customer> GetAllCustomers();
        IList<Customer> GetCustomers(int customerId);
        IList<ItemOverview> GetCustomers(IList<int> ids);
        IList<Customer> SearchAndGenerateCustomers(ICustomerSearch searchCustomers);
        IList<Customer> SearchAndGenerateCustomersConnectedToUser(ICustomerSearch searchCustomers, int userId);
        IList<CustomerReportList> GetCustomerReportList(int id);
        IList<Report> GetAllReports();
        IList<MoveCaseCustomersListItem> GetCustomersForCaseMove(int userId);
        int GetCustomerLanguage(int customerid);

        Customer GetCustomer(int id);
        Task<Customer> GetCustomerAsync(int id);
        ReportCustomer GetReportCustomerById(int customerId, int reportId);

        DeleteMessage DeleteCustomer(int id);

        //void SaveCaseFieldSettingsForCustomer(Customer customer, Setting setting, int[] us, List<CaseFieldSetting> CaseFieldSettings, int LanguageId, out IDictionary<string, string> errors);
        void SaveCaseFieldSettingsForCustomer(int customerId, int languageId, IEnumerable<CaseFieldSettingsWithLanguage> caseFieldSettingWithLanguages, IList<CaseFieldSetting> caseFieldSettings, out IDictionary<string, string> errors);
        void SaveCaseFieldSettingsForCustomer(Customer customer, IEnumerable<CaseFieldSettingsWithLanguage> caseFieldSettingWithLanguages, int[] us, List<CaseFieldSetting> CaseFieldSettings, int LanguageId, out IDictionary<string, string> errors);
        void SaveCaseFieldSettingsForCustomerCopy(int customerId, int languageId, CaseFieldSetting caseFieldSetting, out IDictionary<string, string> errors);
        void SaveCaseFieldSettingsLangForCustomerCopy(CaseFieldSettingLanguage caseFieldSettingLanguage, out IDictionary<string, string> errors);
        void SaveEditCustomer(Customer customer, Setting setting, int[] us, int LanguageId, out IDictionary<string, string> errors);
        void SaveEditCustomer(Customer customer, out IDictionary<string, string> errors);
        void SaveNewCustomerToGetId(Customer customer, out IDictionary<string, string> errors);
        void Commit();

        void SaveCustomerSettings(Customer customerToSave, Setting setting, List<ReportCustomer> ReportCustomers, int LanguageId, out IDictionary<string, string> errors);
        void SaveCaseSettingsForNewCustomer(int customerId, int languageId, CaseSettings caseSettings, out IDictionary<string, string> errors);

        CustomerDetails GetCustomerDetails(int id);
        ItemOverview GetItemOverview(int customerId);
        CaseDefaultsInfo GetCustomerDefaults(int customerId);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICaseFieldSettingRepository _caseFieldSettingRepository;
        private readonly ICaseFieldSettingLanguageRepository _caseFieldSettingLanguageRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IReportCustomerRepository _reportCustomerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICaseSettingRepository _caseSettingRepository;


        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;

        public CustomerService(
            ICaseFieldSettingRepository caseFieldSettingRepository,
            ICaseFieldSettingLanguageRepository caseFieldSettingLanguageRepository,
            ICustomerRepository customerRepository,
            IReportRepository reportRepository,
            IReportCustomerRepository reportCustomerRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICaseSettingRepository caseSettingRepository,

            ICaseFieldSettingService caseFieldSettingService,
            ISettingService settingService,
            IUserService userService)
        {
            _caseFieldSettingRepository = caseFieldSettingRepository;
            _caseFieldSettingLanguageRepository = caseFieldSettingLanguageRepository;
            _customerRepository = customerRepository;
            _reportRepository = reportRepository;
            _reportCustomerRepository = reportCustomerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _caseSettingRepository = caseSettingRepository;

            _caseFieldSettingService = caseFieldSettingService;
            _settingService = settingService;
            _userService = userService;
        }

        public IList<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public IList<ItemOverview> GetCustomers(IList<int> ids)
        {
            return
                _customerRepository.GetMany(x => ids.Contains(x.Id)).AsQueryable()
                    .Select(c => new ItemOverview()
                    {
                        Name = c.Name,
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Name)
                    .ToList();
        }

        public int  GetCustomerLanguage(int customerid)
        {
            return _customerRepository.GetCustomerLanguage(customerid);
        }

        public IList<Customer> GetCustomers(int customerId)
        {
            return _customerRepository.GetMany(x => x.Id == customerId).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// The search and generate customers.
        /// </summary>
        /// <param name="searchCustomers">
        /// The search customers.
        /// </param>
        /// <returns>
        /// The return.
        /// </returns>
        public IList<Customer> SearchAndGenerateCustomers(ICustomerSearch searchCustomers)
        {
            var filter = !string.IsNullOrEmpty(searchCustomers.SearchCs) ? searchCustomers.SearchCs : string.Empty;
            var query = from c in _customerRepository.GetAll() select c;

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Address.ContainsText(filter)
                    || x.CustomerID.ContainsText(filter)
                    || x.CustomerNumber.ContainsText(filter)
                    || x.Name.ContainsText(filter)
                    || x.Phone.ContainsText(filter)
                    || x.PostalAddress.ContainsText(filter)
                    || x.PostalCode.ContainsText(filter));
            }

            return query.OrderBy(x => x.Name).ToList();
        }

        public IList<Customer> SearchAndGenerateCustomersConnectedToUser(ICustomerSearch searchCustomers, int userId)
        {
            var filter = !string.IsNullOrEmpty(searchCustomers.SearchCs) ? searchCustomers.SearchCs : string.Empty;
            var query = from c in _userService.GetCustomersConnectedToUser(userId) select c;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Address.ContainsText(filter)
                    || x.CustomerID.ContainsText(filter)
                    || x.CustomerNumber.ContainsText(filter)
                    || x.Name.ContainsText(filter)
                    || x.Phone.ContainsText(filter)
                    || x.PostalAddress.ContainsText(filter)
                    || x.PostalCode.ContainsText(filter));
            }

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

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
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

            var helpdeskEMail = "";
            if (customer.HelpdeskEmail != null)
                helpdeskEMail = customer.HelpdeskEmail.TrimStart().TrimEnd();

            customer.HelpdeskEmail = helpdeskEMail;

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

            if (customer.Id == 0)
                _customerRepository.Add(customer);
            else
                _customerRepository.Update(customer);

            if (errors.Count == 0)
                this.Commit();

            if (setting != null)
                _settingService.SaveSettingForCustomerEdit(setting, out errors);



        }

        public void SaveEditCustomer(Customer customer, out IDictionary<string, string> errors)
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

            var helpdeskEMail = "";
            if (customer.HelpdeskEmail != null)
                helpdeskEMail = customer.HelpdeskEmail.TrimStart().TrimEnd();

            customer.HelpdeskEmail = helpdeskEMail;

            #endregion

            if (customer.Id != 0)
                _customerRepository.Update(customer);

            if (errors.Count == 0)
                this.Commit();

        }

        public void SaveCaseFieldSettingsForCustomer(int customerId, int languageId, IEnumerable<CaseFieldSettingsWithLanguage> caseFieldSettingWithLanguages, IList<CaseFieldSetting> caseFieldSettings, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
         
            if (caseFieldSettings != null)
            {
                var caseFieldSettingsOld = _caseFieldSettingRepository.GetMany(x => x.Customer_Id == customerId).ToList();

                foreach (var caseFieldSetting in caseFieldSettings)
                {
                    CaseFieldSetting cs = null;
                    if (caseFieldSetting.Id != 0)
                    {
                        cs = caseFieldSettingsOld.FirstOrDefault(x => x.Id == caseFieldSetting.Id);
                    }

                    if (cs == null)
                    {
                        cs = new CaseFieldSetting
                        {
                            Customer_Id =  customerId,
                            CaseFieldSettingsGUID = Guid.NewGuid()
                        };
                    }

                    //change only required fields. There may be other pages that change other values!
                    cs.Name = caseFieldSetting.Name;
                    cs.ShowOnStartPage = caseFieldSetting.ShowOnStartPage;
                    cs.Hide = caseFieldSetting.Hide;
                    cs.ShowExternal = caseFieldSetting.ShowExternal;
                    cs.EMailIdentifier = caseFieldSetting.EMailIdentifier;
                    cs.Locked = caseFieldSetting.Locked;
                    cs.Required = caseFieldSetting.Required;
                    cs.RequiredIfReopened = caseFieldSetting.RequiredIfReopened;
                    cs.ShowStatusBar = caseFieldSetting.ShowStatusBar;
                    cs.ShowExternalStatusBar = caseFieldSetting.ShowExternalStatusBar;
                    cs.ChangedDate = DateTime.UtcNow;

                    if (cs.Name.Equals(GlobalEnums.TranslationCaseFields.CaseType_Id.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        cs.ShowOnStartPage = 1;
                        cs.Required = 1;
                    }

                    if (cs.Id == 0)
                    {
                        _caseFieldSettingRepository.Add(cs);
                    }
                    else
                    {
                        _caseFieldSettingRepository.Update(cs);
                    }
                }

                _caseFieldSettingRepository.Commit();
            }

            if (caseFieldSettingWithLanguages.Any())
            {
                _caseFieldSettingLanguageRepository.DeleteByLanguageId(languageId, customerId);

                var newCaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

                foreach (var caseFieldSetting in newCaseFieldSettings)
                {
                    var cfsl = caseFieldSettingWithLanguages.FirstOrDefault(x => x.Name == caseFieldSetting.Name);
                    if (cfsl == null)
                        continue;

                    var upd = new CaseFieldSettingLanguage
                    {
                        CaseFieldSettings_Id = caseFieldSetting.Id,
                        Language_Id = cfsl.Language_Id = languageId,
                        Label = cfsl.Label,
                        FieldHelp = cfsl.FieldHelp
                    };

                    _caseFieldSettingLanguageRepository.Add(upd);
                }

                _caseFieldSettingLanguageRepository.Commit();
            }
        }

        public void SaveCaseFieldSettingsForCustomerCopy(int customerId, int languageId, CaseFieldSetting caseFieldSetting, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            if (caseFieldSetting.Id == 0)
                _caseFieldSettingRepository.Add(caseFieldSetting);
            else
                _caseFieldSettingRepository.Update(caseFieldSetting);


            _caseFieldSettingRepository.Commit();

        }


        public void SaveCaseSettingsForNewCustomer(int customerId, int languageId, CaseSettings caseSettings, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            if (caseSettings.Id == 0)
                _caseSettingRepository.Add(caseSettings);
            else
                _caseSettingRepository.Update(caseSettings);


            _caseSettingRepository.Commit();

        }

        public void SaveCaseFieldSettingsLangForCustomerCopy(CaseFieldSettingLanguage caseFieldSettingLanguage, out IDictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            var upd = new CaseFieldSettingLanguage
            {
                CaseFieldSettings_Id = caseFieldSettingLanguage.CaseFieldSettings_Id,
                Language_Id = caseFieldSettingLanguage.Language_Id,
                Label = caseFieldSettingLanguage.Label,
                FieldHelp = caseFieldSettingLanguage.FieldHelp
            };

            _caseFieldSettingLanguageRepository.Add(upd);


            _caseFieldSettingLanguageRepository.Commit();
        }


        public void SaveCaseFieldSettingsForCustomer(Customer customer, IEnumerable<CaseFieldSettingsWithLanguage> caseFieldSettingWithLanguages, int[] us, List<CaseFieldSetting> CaseFieldSettings, int LanguageId, out IDictionary<string, string> errors)
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

                    //var rowCfsl = _caseFieldSettingLanguageRepository.Get(x => x.Language_Id == LanguageId);

                    //TODO ALF: kollar först ovan om det finns översättningar på inställningar på denna kund, men annars skall den skapa nya översättningar till rätt inställningsid
                    //if (rowCfsl == null)
                    //{
                    //    rowCfsl = new CaseFieldSettingLanguage() { CaseFieldSettings_Id = rowCfs.Id };
                    //}

                    //TODO: too many unnessary requests to db, refactor
                    foreach (var label in _caseFieldSettingRepository.GetMany(x => x.Id == rowCfs.Id))
                    {
                        rowCfs.Customer_Id = customer.Id;
                        rowCfs.DefaultValue = change.DefaultValue;
                        rowCfs.FieldSize = change.FieldSize;
                        rowCfs.ListEdit = change.ListEdit;
                        rowCfs.Name = label.Name;
                        //rowCfs.NameOrigin = label.NameOrigin;
                        rowCfs.RelatedField = change.RelatedField;
                        rowCfs.Required = change.Required;
                        rowCfs.ShowExternal = change.ShowExternal;
                        rowCfs.ShowOnStartPage = change.ShowOnStartPage;
                        rowCfs.Locked = change.Locked;
                        //rowCfsl.Language_Id = LanguageId;
                        //rowCfsl.Label = label.NameOrigin;

                        if (rowCfs != null)
                            customer.CaseFieldSettings.Add(rowCfs);

                        //if (rowCfsl != null)
                        //    _caseFieldSettingLanguageRepository.Add(rowCfsl);

                    }
                }
            }

            #endregion

            if (customer.Id == 0)
                _customerRepository.Add(customer);
            else
                _customerRepository.Update(customer);


            // _settingService.SaveSettingForCustomerEdit(setting, out errors);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveCustomerSettings(Customer customer, Setting setting, List<ReportCustomer> ReportCustomers, int LanguageId, out IDictionary<string, string> errors)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            errors = new Dictionary<string, string>();

            #region ReportCustomers

            if (customer.ReportCustomers != null && ReportCustomers != null)
            {
                var currentExistReports = _reportCustomerRepository.GetMany(rc => rc.Customer_Id == customer.Id).ToList();
                foreach (var change in ReportCustomers)
                {
                    var existingReport = currentExistReports.Where(r => r.Report_Id == change.Report_Id).FirstOrDefault();
                    if (existingReport == null)
                    {
                        _reportCustomerRepository.Add(change);
                    }
                    else
                    {
                        if (existingReport.ShowOnPage != change.ShowOnPage)
                        {
                            var reportForUpdate = customer.ReportCustomers.Where(r => r.Customer_Id == change.Customer_Id && r.Report_Id == change.Report_Id).FirstOrDefault();
                            if (reportForUpdate != null)
                            {
                                reportForUpdate.ShowOnPage = change.ShowOnPage;
                                _reportCustomerRepository.Update(reportForUpdate);
                            }
                        }
                    }
                }
            }

            #endregion

            customer.NDSPath = customer.NDSPath ?? string.Empty;
            _settingService.SaveSetting(setting, out errors);

            if (errors.Count == 0)
                this.Commit();
        }

        public ItemOverview GetItemOverview(int customerId)
        {
            return _customerRepository.GetOverview(customerId);
        }

        public CustomerDetails GetCustomerDetails(int id)
        {
            var customer = 
                _customerRepository.GetMany(c => c.Id == id).AsQueryable()
                .Select(c => new CustomerDetails()
                {
                    Id = c.Id,
                    Name = c.Name,
                    CustomerID = c.CustomerID, 
                    CustomerNumber = c.CustomerNumber,
                    CustomerGUID = c.CustomerGUID,
                    HelpdeskEmail = c.HelpdeskEmail,
                    CustomerGroup_Id = c.CustomerGroup_Id,
                    Language_Id = c.Language_Id
                }).SingleOrDefault();
            return customer;
        }

        public CaseDefaultsInfo GetCustomerDefaults(int customerId)
        {
            return _customerRepository.GetCustomerDefaults(customerId);
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
            //customer.CommunicateWithNotifier = customer.CommunicateWithNotifier ?? 1;

            var helpdeskEMail = "";
            if (customer.HelpdeskEmail != null)
                helpdeskEMail = customer.HelpdeskEmail.TrimStart().TrimEnd();

            customer.HelpdeskEmail = helpdeskEMail;

            if (string.IsNullOrEmpty(customer.Name))
                errors.Add("Customer.Name", "Du måste ange ett kundnamn");

            if (customer.Id == 0)
                _customerRepository.Add(customer);

            if (errors.Count == 0)
                this.Commit();
        }

        public IList<MoveCaseCustomersListItem> GetCustomersForCaseMove(int userId)
        {
            List<MoveCaseCustomersListItem> res = new List<MoveCaseCustomersListItem>();
            var userCustomers =
                _customerRepository.CustomersForUser(userId)
                    .Select(x => new MoveCaseCustomersListItem
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsUserCustomer = true
                    }).ToList();

            if (userCustomers.Any())
                res.AddRange(userCustomers);

            var excludeCustomerIds = userCustomers.Select(x => x.Id).ToList();

            var allowCaseMoveCustomers = 
                _customerRepository.GetAllowCaseMoveCustomers()
                    .Where(x => !excludeCustomerIds.Contains(x.Id))
                    .Select(x => new MoveCaseCustomersListItem
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsUserCustomer = false
                    }).ToList();

            if (allowCaseMoveCustomers.Any())
                res.AddRange(allowCaseMoveCustomers);

            return res.OrderBy(x => x.Name).ToList();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
