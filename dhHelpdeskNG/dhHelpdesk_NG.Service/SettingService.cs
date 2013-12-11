using System;
using System.Collections.Generic;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface ISettingService
    {
        Setting GetCustomerSetting(int id);

        void SaveSetting(Setting setting, out IDictionary<string, string> errors);
        void SaveSettingForCustomerEdit(Setting setting, out IDictionary<string, string> errors);
        void Commit();
    }

    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SettingService(
            ISettingRepository settingRepository,
            IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }

        public Setting GetCustomerSetting(int id)
        {
            return _settingRepository.Get(x => x.Customer_Id == id);
        }

        public void SaveSetting(Setting setting, out IDictionary<string, string> errors)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

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
                _settingRepository.Add(setting);
            else
            {
                _settingRepository.Update(setting);
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveSettingForCustomerEdit(Setting setting, out IDictionary<string, string> errors)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");
            

            errors = new Dictionary<string, string>();

            if (setting.Id == 0)
                _settingRepository.Add(setting);
            else
            {
                _settingRepository.Update(setting);
            }

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}