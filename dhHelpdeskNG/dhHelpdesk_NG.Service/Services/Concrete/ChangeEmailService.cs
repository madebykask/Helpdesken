namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Services.MailTemplateFormatters;

    public sealed class ChangeEmailService : IChangeEmailService
    {
        private readonly IMailTemplateRepository mailTemplateRepository;

        private readonly IMailTemplateLanguageRepository mailTemplateLanguageRepository;

        private readonly ICustomerRepository customerRepository;

        private readonly IMailTemplateFormatter<Change> mailTemplateFormatter; 

        private readonly IEmailService emailService;

        public ChangeEmailService(
            IEmailService emailService,
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            ICustomerRepository customerRepository,
            IMailTemplateFormatter<Change> mailTemplateFormatter)
        {
            this.emailService = emailService;
            this.mailTemplateRepository = mailTemplateRepository;
            this.mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this.customerRepository = customerRepository;
            this.mailTemplateFormatter = mailTemplateFormatter;
        }

        public void SendInternalLogNoteTo(Change change, string text, List<string> emails, int customerId, int languageId)
        {
            var templateId = this.mailTemplateRepository.GetTemplateId(ChangeTemplate.SendLogNoteTo, customerId);
            var template = this.mailTemplateLanguageRepository.GetTemplate(templateId, languageId);

            var mail = this.mailTemplateFormatter.Format(template, change, customerId, languageId);
            var from = this.customerRepository.GetCustomerEmail(customerId);
            
            this.emailService.SendEmail(from, emails, mail.Subject, mail.Body);
        }
    }
}