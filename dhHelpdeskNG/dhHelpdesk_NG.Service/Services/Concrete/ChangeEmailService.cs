namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters;

    public sealed class ChangeEmailService : IChangeEmailService
    {
        private readonly IMailTemplateRepository mailTemplateRepository;

        private readonly IMailTemplateLanguageRepository mailTemplateLanguageRepository;

        private readonly ICustomerRepository customerRepository;

        private readonly IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter;

        private readonly IEmailService emailService;

        public ChangeEmailService(
            IEmailService emailService,
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            ICustomerRepository customerRepository,
            IMailTemplateFormatter<UpdatedChange> mailTemplateFormatter)
        {
            this.emailService = emailService;
            this.mailTemplateRepository = mailTemplateRepository;
            this.mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this.customerRepository = customerRepository;
            this.mailTemplateFormatter = mailTemplateFormatter;
        }

        public void SendInternalLogNoteTo(
            UpdatedChange change,
            string text,
            List<string> emails,
            int customerId,
            int languageId)
        {
            var templateId = this.mailTemplateRepository.GetTemplateId(ChangeTemplate.SendLogNoteTo, customerId);
            if (!templateId.HasValue)
            {
                return;
            }

            var template = this.mailTemplateLanguageRepository.GetTemplate(templateId.Value, languageId);

            var mail = this.mailTemplateFormatter.Format(template, change, customerId, languageId);
            var from = this.customerRepository.GetCustomerEmail(customerId);

//            this.emailService.SendEmail(from, emails, mail.Subject, mail.Body);
        }

        public void SendAssignedToUser(UpdatedChange change, List<string> emails, int customerId, int languageId)
        {
            var templateId = this.mailTemplateRepository.GetTemplateId(ChangeTemplate.AssignedToUser, customerId);
            if (!templateId.HasValue)
            {
                return;
            }

            var template = this.mailTemplateLanguageRepository.GetTemplate(templateId.Value, languageId);

            var mail = this.mailTemplateFormatter.Format(template, change, customerId, languageId);
            var from = this.customerRepository.GetCustomerEmail(customerId);

//            this.emailService.SendEmail(from, emails, mail.Subject, mail.Body);
        }

        public void SendStatusChanged(UpdatedChange change, List<string> ownerEmails, int customerId, int languageId)
        {
            var templateId = this.mailTemplateRepository.GetTemplateId(ChangeTemplate.StatusChanged, customerId);
            if (!templateId.HasValue)
            {
                return;
            }

            var template = this.mailTemplateLanguageRepository.GetTemplate(templateId.Value, languageId);

            var mail = this.mailTemplateFormatter.Format(template, change, customerId, languageId);
            var from = this.customerRepository.GetCustomerEmail(customerId);

//            this.emailService.SendEmail(from, ownerEmails, mail.Subject, mail.Body);
        }
    }
}