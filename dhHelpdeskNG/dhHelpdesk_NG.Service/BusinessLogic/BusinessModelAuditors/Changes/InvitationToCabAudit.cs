namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Changes
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
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

    public sealed class InvitationToCabAudit : IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>
    {
        private readonly IChangeEmailLogRepository changeEmailLogRepository;

        private readonly IChangeLogRepository changeLogRepository;

        private readonly ICustomerRepository customerRepository;

        private readonly IEmailService emailService;

        private readonly IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter;

        private readonly IMailTemplateLanguageRepository mailTemplateLanguageRepository;

        private readonly IMailTemplateRepository mailTemplateRepository;

        private readonly IMailUniqueIdentifierProvider mailUniqueIdentifierProvider;

        private readonly ISettingService settingService;

        private readonly IEmailSendingSettingsProvider _emailSendingSettingsProvider;

        public InvitationToCabAudit(
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            ICustomerRepository customerRepository,
            IMailUniqueIdentifierProvider mailUniqueIdentifierProvider,
            IEmailService emailService,
            IChangeEmailLogRepository changeEmailLogRepository,
            IChangeLogRepository changeLogRepository,
            ISettingService settingService,
            IEmailSendingSettingsProvider emailSendingSettingsProvider)
        {
            this.mailTemplateRepository = mailTemplateRepository;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this.customerRepository = customerRepository;
            this.mailUniqueIdentifierProvider = mailUniqueIdentifierProvider;
            this.emailService = emailService;
            this.changeEmailLogRepository = changeEmailLogRepository;
            this.changeLogRepository = changeLogRepository;
            this.settingService = settingService;
            this._emailSendingSettingsProvider = emailSendingSettingsProvider;
        }

        public void Audit(UpdateChangeRequest businessModel, ChangeAuditData optionalData)
        {
            var log = businessModel.NewLogs.FirstOrDefault(l => l.Subtopic == Subtopic.InviteToCab);

            if (log == null)
            {
                return;
            }

            if (!log.Emails.Any())
            {
                return;
            }

            var templateId = this.mailTemplateRepository.GetTemplateId(
                ChangeTemplate.Cab,
                businessModel.Context.CustomerId);
            if (!templateId.HasValue)
            {
                return;
            }

            var template = this.mailTemplateLanguageRepository.GetTemplate(
                templateId.Value,
                businessModel.Context.LanguageId);
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

            var mailUniqueIdentifier = this.mailUniqueIdentifierProvider.Provide(
                businessModel.Context.DateAndTime,
                from);

            var customerSetting = settingService.GetCustomerSetting(businessModel.Context.CustomerId);
            var smtpInfo = new MailSMTPSetting(customerSetting.SMTPServer, customerSetting.SMTPPort, customerSetting.SMTPUserName, customerSetting.SMTPPassWord, customerSetting.IsSMTPSecured);

            if (string.IsNullOrEmpty(smtpInfo.Server) || smtpInfo.Port <= 0)
            {
                var info = _emailSendingSettingsProvider.GetSettings();
                smtpInfo = new MailSMTPSetting(info.SmtpServer, info.SmtpPort);
            }
            var mailResponse = EmailResponse.GetEmptyEmailResponse();
            var mailSetting = new EmailSettings(mailResponse, smtpInfo);
            this.emailService.SendEmail(from, log.Emails, mail, mailSetting);

            var emailLog = EmailLog.CreateNew(
                optionalData.HistoryId,
                log.Emails,
                (int)ChangeTemplate.Cab,
                mailUniqueIdentifier,
                businessModel.Context.DateAndTime);

            this.changeEmailLogRepository.AddEmailLog(emailLog);
            this.changeEmailLogRepository.Commit();

            log.ChangeEmailLogId = emailLog.Id;

            this.changeLogRepository.UpdateLogEmailLogId(log.Id, emailLog.Id);
            this.changeLogRepository.Commit();
        }
    }
}
