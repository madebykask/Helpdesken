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

    public sealed class ManualLogsAudit : IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>
    {
        #region Fields

        private readonly IChangeEmailLogRepository changeEmailLogRepository;

        private readonly IChangeLogRepository changeLogRepository;

        private readonly ICustomerRepository customerRepository;

        private readonly IEmailService emailService;

        private readonly IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter;

        private readonly IMailTemplateLanguageRepository mailTemplateLanguageRepository;

        private readonly IMailTemplateRepository mailTemplateRepository;

        private readonly IMailUniqueIdentifierProvider mailUniqueIdentifierProvider;

        #endregion

        #region Constructors and Destructors

        public ManualLogsAudit(
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            ICustomerRepository customerRepository,
            IMailUniqueIdentifierProvider mailUniqueIdentifierProvider,
            IEmailService emailService,
            IChangeEmailLogRepository changeEmailLogRepository,
            IChangeLogRepository changeLogRepository)
        {
            this.mailTemplateRepository = mailTemplateRepository;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this.customerRepository = customerRepository;
            this.mailUniqueIdentifierProvider = mailUniqueIdentifierProvider;
            this.emailService = emailService;
            this.changeEmailLogRepository = changeEmailLogRepository;
            this.changeLogRepository = changeLogRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void Audit(UpdateChangeRequest businessModel, ChangeAuditData optionalData)
        {
            foreach (var log in businessModel.NewLogs)
            {
                if (!log.Emails.Any())
                {
                    continue;
                }

                var templateId = this.mailTemplateRepository.GetTemplateId(
                    ChangeTemplate.SendLogNoteTo,
                    businessModel.Context.CustomerId);
                if (!templateId.HasValue)
                {
                    continue;
                }

                var template = this.mailTemplateLanguageRepository.GetTemplate(
                    templateId.Value,
                    businessModel.Context.LanguageId);
                if (template == null)
                {
                    continue;
                }

                var mail = this.mailTemplateFormatter.Format(
                    template,
                    businessModel.Change,
                    businessModel.Context.CustomerId,
                    businessModel.Context.LanguageId);
                if (mail == null)
                {
                    continue;
                }

                var from = this.customerRepository.GetCustomerEmail(businessModel.Context.CustomerId);
                if (from == null)
                {
                    continue;
                }

                var mailUniqueIdentifier = this.mailUniqueIdentifierProvider.Provide(
                    businessModel.Context.DateAndTime,
                    from);

                this.emailService.SendEmail(from, log.Emails, mail);

                var emailLog = EmailLog.CreateNew(
                    optionalData.HistoryId,
                    log.Emails,
                    (int)ChangeTemplate.SendLogNoteTo,
                    mailUniqueIdentifier,
                    businessModel.Context.DateAndTime);

                this.changeEmailLogRepository.AddEmailLog(emailLog);
                this.changeEmailLogRepository.Commit();

                log.ChangeEmailLogId = emailLog.Id;

                this.changeLogRepository.UpdateLogEmailLogId(log.Id, emailLog.Id);
                this.changeLogRepository.Commit();
            }
        }

        #endregion
    }
}