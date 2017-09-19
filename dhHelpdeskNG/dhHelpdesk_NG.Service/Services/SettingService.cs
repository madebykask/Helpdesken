// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ISettingService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
    }

    /// <summary>
    /// The setting service.
    /// </summary>
    public class SettingService : ISettingService
    {
        /// <summary>
        /// The setting repository.
        /// </summary>
        private readonly ISettingRepository settingRepository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingService"/> class.
        /// </summary>
        /// <param name="settingRepository">
        /// The setting repository.
        /// </param>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        public SettingService(
            ISettingRepository settingRepository,
            IUnitOfWork unitOfWork)
        {
            this.settingRepository = settingRepository;
            this.unitOfWork = unitOfWork;
        }

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
            var res = this.settingRepository.GetCustomerSetting(id);

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
                this.settingRepository.Add(setting);
            }
            else
            {
                this.settingRepository.Update(setting);
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
                this.settingRepository.Add(setting);
            }
            else
            {
                this.settingRepository.Update(setting);
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
            this.unitOfWork.Commit();
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
            return this.settingRepository.GetCustomerSettings(customerId);
        }
    }
}