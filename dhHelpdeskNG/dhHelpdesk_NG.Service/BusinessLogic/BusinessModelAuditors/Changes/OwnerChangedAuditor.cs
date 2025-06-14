namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Changes
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Services.BusinessLogic.MailTools;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.BusinessData.Models.Email;
    using Infrastructure;

    public sealed class OwnerChangedAuditor : IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>
    {
        #region Fields

        private readonly ICustomerRepository customerRepository;

        private readonly IEmailService emailService;

        private readonly IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter;

        private readonly IMailTemplateLanguageRepository mailTemplateLanguageRepository;

        private readonly IMailTemplateRepository mailTemplateRepository;

        private readonly IUserEmailRepository userEmailRepository;

        private readonly IMailUniqueIdentifierProvider mailUniqueIdentifierProvider;

        private readonly IChangeEmailLogRepository changeEmailLogRepository;

        private readonly ISettingService settingService;

        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;

        #endregion

        #region Constructors and Destructors

        public OwnerChangedAuditor(
            IUserEmailRepository userEmailRepository,
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter,
            ICustomerRepository customerRepository,
            IEmailService emailService, 
            IMailUniqueIdentifierProvider mailUniqueIdentifierProvider, 
            IChangeEmailLogRepository changeEmailLogRepository,
            ISettingService settingService,
            IEmailSendingSettingsProvider emailSendingSettingsProvider)
        {
            this.userEmailRepository = userEmailRepository;
            this.mailTemplateRepository = mailTemplateRepository;
            this.mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.customerRepository = customerRepository;
            this.emailService = emailService;
            this.mailUniqueIdentifierProvider = mailUniqueIdentifierProvider;
            this.changeEmailLogRepository = changeEmailLogRepository;
            this.settingService = settingService;
            this._emailSendingSettingsProvider = emailSendingSettingsProvider;
        }

        #endregion

        #region Public Methods and Operators

        public void Audit(UpdateChangeRequest businessModel, ChangeAuditData optionalData)
        {
            if (businessModel.Change.General.AdministratorId == null)
            {
                return;
            }

            if (businessModel.Change.General.AdministratorId == optionalData.ExistingChange.General.AdministratorId)
            {
                return;
            }

            var newOwnerEmails =
                this.userEmailRepository.FindUserEmails(businessModel.Change.General.AdministratorId.Value);
            if (newOwnerEmails == null || !newOwnerEmails.Any())
            {
                return;
            }

            var templateId = this.mailTemplateRepository.GetTemplateId(
                ChangeTemplate.AssignedToUser,
                businessModel.Context.CustomerId);
            if (!templateId.HasValue)
            {
                return;
            }

            var template = this.mailTemplateLanguageRepository.GetTemplate(templateId.Value, businessModel.Context.LanguageId);
            if (template == null)
            {
                return;
            }

            var mail = this.mailTemplateFormatter.Format(
                template,
                businessModel.Change,
                businessModel.Context.CustomerId,
                businessModel.Context.LanguageId);
            if (mail == null)
            {
                return;
            }

            var from = this.customerRepository.GetCustomerEmail(businessModel.Context.CustomerId);
            if (from == null)
            {
                return;
            }

            var customerSetting = settingService.GetCustomerSetting(businessModel.Context.CustomerId);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }
            var mailResponse = EmailResponse.GetEmptyEmailResponse();
            var mailSetting = new EmailSettings(mailResponse, smtpInfo, customerSetting.BatchEmail);
            emailService.SendEmail(from, newOwnerEmails, mail, mailSetting);

            var mailUniqueIdentifier = this.mailUniqueIdentifierProvider.Provide(
                businessModel.Context.DateAndTime,
                from);

            if (string.IsNullOrEmpty(mailUniqueIdentifier))
            {
                return;
            }

            var emailLog = EmailLog.CreateNew(
                optionalData.HistoryId,
                newOwnerEmails,
                (int)ChangeTemplate.AssignedToUser,
                mailUniqueIdentifier,
                businessModel.Context.DateAndTime);

            if (emailLog == null)
            {
                return;
            }

            this.changeEmailLogRepository.AddEmailLog(emailLog);
            this.changeEmailLogRepository.Commit();
        }

        #endregion
    }
}