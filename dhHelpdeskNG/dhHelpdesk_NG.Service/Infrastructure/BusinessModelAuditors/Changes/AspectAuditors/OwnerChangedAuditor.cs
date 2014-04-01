namespace DH.Helpdesk.Services.Infrastructure.BusinessModelAuditors.Changes.AspectAuditors
{
    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Services.Infrastructure.MailTemplateFormatters;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;

    public sealed class OwnerChangedAuditor : IChangeAspectAuditor
    {
        #region Fields

        private readonly ICustomerRepository customerRepository;

        private readonly IEmailService emailService;

        private readonly IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter;

        private readonly IMailTemplateLanguageRepository mailTemplateLanguageRepository;

        private readonly IMailTemplateRepository mailTemplateRepository;

        private readonly IUserEmailRepository userEmailRepository;

        #endregion

        #region Constructors and Destructors

        public OwnerChangedAuditor(
            IUserEmailRepository userEmailRepository,
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter,
            ICustomerRepository customerRepository,
            IEmailService emailService)
        {
            this.userEmailRepository = userEmailRepository;
            this.mailTemplateRepository = mailTemplateRepository;
            this.mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this.mailTemplateFormatter = mailTemplateFormatter;
            this.customerRepository = customerRepository;
            this.emailService = emailService;
        }

        #endregion

        #region Public Methods and Operators

        public void Audit(UpdateChangeRequest updated, Change existing, int historyId)
        {
            if (updated.Change.General.AdministratorId == null)
            {
                return;
            }

            if (updated.Change.General.AdministratorId == existing.General.AdministratorId)
            {
                return;
            }

            var newOwnerEmails = this.userEmailRepository.FindUserEmails(updated.Change.General.AdministratorId.Value);

            var templateId = this.mailTemplateRepository.GetTemplateId(
                ChangeTemplate.AssignedToUser,
                updated.Context.CustomerId);

            var template = this.mailTemplateLanguageRepository.GetTemplate(templateId, updated.Context.LanguageId);

            var mail = this.mailTemplateFormatter.Format(
                template,
                updated.Change,
                updated.Context.CustomerId,
                updated.Context.LanguageId);

            var from = this.customerRepository.GetCustomerEmail(updated.Context.CustomerId);
            this.emailService.SendEmail(from, newOwnerEmails, mail);
        }

        #endregion
    }
}