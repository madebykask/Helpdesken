namespace DH.Helpdesk.Services.Infrastructure.BusinessModelAuditors.Changes.AspectAuditors
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Services.Infrastructure.MailTemplateFormatters;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;

    public sealed class ManualAddedLogsAuditor : IChangeAspectAuditor
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

        public ManualAddedLogsAuditor(
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

        public void Audit(UpdateChangeRequest updated, Change existing, int historyId)
        {
            foreach (var newLog in updated.NewLogs)
            {
                int? emailLogId = null;

                if (newLog.Emails.Any())
                {
                    var templateId = this.mailTemplateRepository.GetTemplateId(
                        ChangeTemplate.SendLogNoteTo,
                        updated.Context.CustomerId);

                    var template = this.mailTemplateLanguageRepository.GetTemplate(
                        templateId,
                        updated.Context.LanguageId);

                    var mail = this.mailTemplateFormatter.Format(
                        template,
                        updated.Change,
                        updated.Context.CustomerId,
                        updated.Context.LanguageId);

                    var from = this.customerRepository.GetCustomerEmail(updated.Context.CustomerId);

                    var mailUniqueIdentifier = this.mailUniqueIdentifierProvider.Provide(
                        updated.Context.DateAndTime,
                        from);

                    this.emailService.SendEmail(from, newLog.Emails, mail);

                    var emailLog = EmailLog.CreateNew(
                        historyId,
                        newLog.Emails,
                        (int)ChangeTemplate.SendLogNoteTo,
                        mailUniqueIdentifier,
                        updated.Context.DateAndTime);

                    this.changeEmailLogRepository.AddEmailLog(emailLog);
                    this.changeEmailLogRepository.Commit();

                    emailLogId = emailLog.Id;
                }

                newLog.ChangeId = updated.Change.Id;
                newLog.ChangeHistoryId = historyId;
                newLog.ChangeEmailLogId = emailLogId;
                newLog.CreatedDateAndTime = updated.Context.DateAndTime;
                newLog.CreatedByUserId = updated.Context.UserId;

                this.changeLogRepository.AddManualLog(newLog);
            }

            this.changeLogRepository.Commit();
        }

        #endregion
    }
}