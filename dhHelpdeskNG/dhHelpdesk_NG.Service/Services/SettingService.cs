// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ISettingService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using DH.Helpdesk.Dal.Mappers;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The SettingService interface.
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// The get customer setting.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Setting"/>.
        /// </returns>
        Setting GetCustomerSetting(int id);
        
        Task<CustomerSettings> GetCustomerSettingsAsync(int customerId);

        /// <summary>
        /// The save setting.
        /// </summary>
        /// <param name="setting">
        /// The setting.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        void SaveSetting(Setting setting, out IDictionary<string, string> errors);

        /// <summary>
        /// The save setting for customer edit.
        /// </summary>
        /// <param name="setting">
        /// The setting.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        void SaveSettingForCustomerEdit(Setting setting, out IDictionary<string, string> errors);

        /// <summary>
        /// The commit.
        /// </summary>
        void Commit();

        /// <summary>
        /// The get customer settings.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="CustomerSettings"/>.
        /// </returns>
        CustomerSettings GetCustomerSettings(int customerId);

        /// <summary>
        /// The get customer ids that included in advanced extended search
        /// </summary>
        /// <returns></returns>
        List<int> GetExtendedSearchIncludedCustomers();
    }

    /// <summary>
    /// The setting service.
    /// </summary>
    public class SettingService : ISettingService
    {
        /// <summary>
        /// The setting repository.
        /// </summary>
        private readonly ISettingRepository _settingRepository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        /// 
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

        private readonly IEntityToBusinessModelMapper<Setting, CustomerSettings> _toBusinessModelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingService"/> class.
        /// </summary>
        /// <param name="settingRepository">
        /// The setting repository.
        /// </param>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        /// 
#pragma warning disable 0618
        public SettingService(ISettingRepository settingRepository, 
                              IUnitOfWork unitOfWork,
                              IEntityToBusinessModelMapper<Setting, CustomerSettings> businessModelMapper)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
            _toBusinessModelMapper = businessModelMapper;
        }
#pragma warning restore 0618

        /// <summary>
        /// The get customer setting.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Setting"/>.
        /// </returns>
        public Setting GetCustomerSetting(int id)
        {
            //TODO: add cache
            var res = this._settingRepository.GetCustomerSetting(id);

            //TODO default values. move to mapper
            if (res != null && res.MinRegWorkingTime == 0)
                res.MinRegWorkingTime = 30;
            return res;
        }

        /// <summary>
        /// The save setting.
        /// </summary>
        /// <param name="setting">
        /// The setting.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        public void SaveSetting(Setting setting, out IDictionary<string, string> errors)
        {
            //From Customersettingscontroller
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            errors = new Dictionary<string, string>();

            setting.ADSyncURL = setting.ADSyncURL ?? string.Empty;
            setting.CaseOverviewInfo = setting.CaseOverviewInfo ?? string.Empty;
            setting.DSN_Sync = setting.DSN_Sync ?? string.Empty;
            setting.LDAPBase = setting.LDAPBase ?? string.Empty;
            setting.LDAPFilter = setting.LDAPFilter ?? string.Empty;
            setting.LDAPPassword = setting.LDAPPassword ?? string.Empty;
            setting.LDAPUserName = setting.LDAPUserName ?? string.Empty;
            setting.POP3EMailPrefix = setting.POP3EMailPrefix ?? string.Empty;
            setting.POP3Password = setting.POP3Password ?? string.Empty;
            setting.POP3Server = setting.POP3Server ?? string.Empty;
            setting.POP3Port = setting.POP3Port;
            setting.POP3UserName = setting.POP3UserName ?? string.Empty;
            setting.SMSEMailDomain = setting.SMSEMailDomain ?? string.Empty;
            setting.SMSEMailDomainPassword = setting.SMSEMailDomainPassword ?? string.Empty;
            setting.SMSEMailDomainUserId = setting.SMSEMailDomainUserId ?? string.Empty;
            setting.SMSEMailDomainUserName = setting.SMSEMailDomainUserName ?? string.Empty;
            setting.XMLFileFolder = setting.XMLFileFolder ?? string.Empty;
            setting.ErrorMailTo = setting.ErrorMailTo ?? string.Empty;
            setting.EMailFolder = setting.EMailFolder ?? string.Empty;
            setting.EMailFolderArchive = setting.EMailFolderArchive ?? string.Empty;
            setting.EMailAnswerSeparator = setting.EMailAnswerSeparator ?? string.Empty;
            setting.EMailSubjectPattern = setting.EMailSubjectPattern ?? string.Empty;



            if (setting.QuickNewCaseLinkText == null)
                setting.QuickNewCaseLinkText = "+";
            else
                setting.QuickNewCaseLinkText = setting.QuickNewCaseLinkText;

            if (setting.QuickNewCaseLinkUrl == null)
                setting.QuickNewCaseLinkUrl = "/cases/new";
            else
                setting.QuickNewCaseLinkUrl = setting.QuickNewCaseLinkUrl;


            if (setting.Id == 0)
            {
                this._settingRepository.Add(setting);
            }
            else
            {
                this._settingRepository.Update(setting);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        /// <summary>
        /// The save setting for customer edit.
        /// </summary>
        /// <param name="setting">
        /// The setting.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        public void SaveSettingForCustomerEdit(Setting setting, out IDictionary<string, string> errors)
        {
            //From Customercontroller
            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }
            
            errors = new Dictionary<string, string>();

            setting.ADSyncURL = setting.ADSyncURL ?? string.Empty;
            setting.CaseOverviewInfo = setting.CaseOverviewInfo ?? string.Empty;
            setting.DSN_Sync = setting.DSN_Sync ?? string.Empty;
            setting.EMailAnswerSeparator = setting.EMailAnswerSeparator ?? string.Empty;
            setting.EMailSubjectPattern = setting.EMailSubjectPattern ?? string.Empty;
            setting.BlockedEmailRecipients = setting.BlockedEmailRecipients ?? string.Empty;
            setting.LDAPBase = setting.LDAPBase ?? string.Empty;
            setting.LDAPFilter = setting.LDAPFilter ?? string.Empty;
            setting.LDAPPassword = setting.LDAPPassword ?? string.Empty;
            setting.LDAPUserName = setting.LDAPUserName ?? string.Empty;
            setting.POP3EMailPrefix = setting.POP3EMailPrefix ?? string.Empty;
            setting.POP3Password = setting.POP3Password ?? string.Empty;
            setting.POP3Server = setting.POP3Server ?? string.Empty;
            setting.POP3UserName = setting.POP3UserName ?? string.Empty;
            setting.SMSEMailDomain = setting.SMSEMailDomain ?? string.Empty;
            setting.SMSEMailDomainPassword = setting.SMSEMailDomainPassword ?? string.Empty;
            setting.SMSEMailDomainUserId = setting.SMSEMailDomainUserId ?? string.Empty;
            setting.SMSEMailDomainUserName = setting.SMSEMailDomainUserName ?? string.Empty;
            setting.XMLFileFolder = setting.XMLFileFolder ?? string.Empty;

            if (setting.Id == 0)
            {
                this._settingRepository.Add(setting);
            }
            else
            {
                this._settingRepository.Update(setting);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        /// <summary>
        /// The get customer settings.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="CustomerSettings"/>.
        /// </returns>
        public CustomerSettings GetCustomerSettings(int customerId)
        {
            var setting = _settingRepository.GetCustomerSetting(customerId);
            return _toBusinessModelMapper.Map(setting);
        }

        public async Task<CustomerSettings> GetCustomerSettingsAsync(int customerId)
        {
            var setting = await _settingRepository.GetCustomerSettingAsync(customerId);
            return _toBusinessModelMapper.Map(setting);
        }

        public List<int> GetExtendedSearchIncludedCustomers()
        {
            return _settingRepository.GetExtendedSearchIncludedCustomers();
        }
    }
}