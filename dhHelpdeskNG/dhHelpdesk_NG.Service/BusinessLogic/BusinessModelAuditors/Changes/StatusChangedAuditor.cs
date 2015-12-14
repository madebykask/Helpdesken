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

    public sealed class StatusChangedAuditor : IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>
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

        #endregion

        #region Constructors and Destructors

        public StatusChangedAuditor(
            IUserEmailRepository userEmailRepository,
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            ICustomerRepository customerRepository,
            IEmailService emailService, 
            IMailUniqueIdentifierProvider mailUniqueIdentifierProvider, 
            IChangeEmailLogRepository changeEmailLogRepository)
        {
            this.userEmailRepository = userEmailRepository;
            this.mailTemplateRepository = mailTemplateRepository;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this.customerRepository = customerRepository;
            this.emailService = emailService;
            this.mailUniqueIdentifierProvider = mailUniqueIdentifierProvider;
            this.changeEmailLogRepository = changeEmailLogRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void Audit(UpdateChangeRequest businessModel, ChangeAuditData optionalData)
        {
            if (businessModel.Change.General.AdministratorId == null)
            {
                return;
            }

            if (businessModel.Change.General.StatusId == optionalData.ExistingChange.General.StatusId)
            {
                return;
            }

            var ownerEmails = this.userEmailRepository.FindUserEmails(
                businessModel.Change.General.AdministratorId.Value);
            if (ownerEmails == null || !ownerEmails.Any())
            {
                return;
            }

            var templateId = this.mailTemplateRepository.GetTemplateId(
                ChangeTemplate.StatusChanged,
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

            this.emailService.SendEmail(from, ownerEmails, mail, EmailResponse.GetEmptyEmailResponse());

            var mailUniqueIdentifier = this.mailUniqueIdentifierProvider.Provide(
                businessModel.Context.DateAndTime,
                from);

            if (string.IsNullOrEmpty(mailUniqueIdentifier))
            {
                return;
            }

            var emailLog = EmailLog.CreateNew(
                optionalData.HistoryId,
                ownerEmails,
                (int)ChangeTemplate.StatusChanged,
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