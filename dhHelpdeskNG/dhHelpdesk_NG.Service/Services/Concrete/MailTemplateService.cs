using System.Linq;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.EmailTemplate;

    public class MailTemplateServiceNew : IMailTemplateServiceNew
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public MailTemplateServiceNew(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public MailTemplate GetTemplate(int mailId, OperationContext operationContext)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var templateLangaugeRepository = uof.GetRepository<MailTemplateLanguageEntity>();
                var templateRepository = uof.GetRepository<MailTemplateEntity>();

                MailTemplate mailTemplate =
                    templateLangaugeRepository.GetAll()
                        .ExtractMailTemplate(
                            templateRepository.GetAll(),
                            operationContext.CustomerId,
                            operationContext.LanguageId,
                            mailId);

                return mailTemplate;
            }
        }

        public MailTemplate GetTemplate(int mailId, int customerId, int languageId, IUnitOfWork uow)
        {
            var templateLangaugeRepository = uow.GetRepository<MailTemplateLanguageEntity>();
            var templateRepository = uow.GetRepository<MailTemplateEntity>();

            MailTemplate mailTemplate = 
                templateLangaugeRepository.GetAll()
                    .ExtractMailTemplate(
                        templateRepository.GetAll(),
                        customerId,
                        languageId,
                        mailId);

            return mailTemplate;            
        }

        public MailTemplate GetTemplateById(int id, OperationContext operationContext)
        {
            using (IUnitOfWork uof = this.unitOfWorkFactory.Create())
            {
                var templateLangaugeRepository = uof.GetRepository<MailTemplateLanguageEntity>();
                var templateRepository = uof.GetRepository<MailTemplateEntity>();

                MailTemplate mailTemplate =
                    templateLangaugeRepository.GetAll()
                        .ExtractMailTemplateById(
                            templateRepository.GetAll(),
                            operationContext.CustomerId,
                            operationContext.LanguageId,
                            id);

                return mailTemplate;
            }
        }
    }
}