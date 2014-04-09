namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Changes
{
    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;
    using DH.Helpdesk.Services.Requests.Changes;
    using DH.Helpdesk.Services.Services;

    public sealed class OwnerChangedAuditor : IBusinessModelAuditor<UpdateChangeRequest, ChangeAuditData>
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

            var templateId = this.mailTemplateRepository.GetTemplateId(
                ChangeTemplate.AssignedToUser,
                businessModel.Context.CustomerId);

            var template = this.mailTemplateLanguageRepository.GetTemplate(templateId, businessModel.Context.LanguageId);

            var mail = this.mailTemplateFormatter.Format(
                template,
                businessModel.Change,
                businessModel.Context.CustomerId,
                businessModel.Context.LanguageId);

            var from = this.customerRepository.GetCustomerEmail(businessModel.Context.CustomerId);
            this.emailService.SendEmail(from, newOwnerEmails, mail);
        }

        #endregion
    }
}