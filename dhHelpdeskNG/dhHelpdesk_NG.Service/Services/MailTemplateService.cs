namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.MailTemplates;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.MailTemplates;

    public interface IMailTemplateService
    {        
        IDictionary<string, string> Validate(MailTemplateEntity mailTemplateToValidate);

        IList<MailTemplateList> GetMailTemplates(int customerId, int langaugeId);
        IList<MailTemplateIdentifierEntity> GetMailTemplateIdentifiers();
        MailTemplateEntity GetMailTemplate(int id, int customerId);
        MailTemplate GetMailTemplateForCustomer(int id, int customerId, int languageId);
        MailTemplateLanguageEntity GetMailTemplateLanguage(int id, int languageId);
        MailTemplateLanguageEntity GetMailTemplateForCustomerAndLanguage(int customerId, int languageId, int mailTemplateId);
        MailTemplateLanguage GetMailTemplateLanguageForCustomer(int id, int customerId, int languageId);

        void SaveMailTemplateLanguage(MailTemplateLanguageEntity mailtemplatelanguage,  bool update, out IDictionary<string, string> errors);
        void DeleteMailTemplateLanguage(MailTemplateLanguageEntity mailtemplatelanguage, out IDictionary<string, string> errors);
        void Commit();

        //void GetMailTemplateId();
    }

    public class MailTemplateService : IMailTemplateService
    {
        private readonly IMailTemplateRepository _mailTemplateRepository;
        private readonly IMailTemplateLanguageRepository _mailTemplateLanguageRepository;
        private readonly IMailTemplateIdentifierRepository _mailTemplateIdentifierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MailTemplateService(
            IMailTemplateRepository mailTemplateRepository,
            IMailTemplateLanguageRepository mailTemplateLanguageRepository,
            IMailTemplateIdentifierRepository mailTemplateIdentifierRepository,
            IUnitOfWork unitOfWork)
        {
            this._mailTemplateRepository = mailTemplateRepository;
            this._mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            this._mailTemplateIdentifierRepository = mailTemplateIdentifierRepository;
            this._unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(MailTemplateEntity mailTemplateToValidate)
        {
            if (mailTemplateToValidate == null)
                throw new ArgumentNullException("mailtemplatetovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<MailTemplateList> GetMailTemplates(int customerId, int langaugeId)
        {
            return this._mailTemplateRepository.GetMailTemplate(customerId, langaugeId).ToList();
        }

        public IList<MailTemplateIdentifierEntity> GetMailTemplateIdentifiers()
        {
            return this._mailTemplateIdentifierRepository.GetAll().ToList();
        }

        public MailTemplateEntity GetMailTemplate(int id, int customerId)
        {
            return this._mailTemplateRepository.GetMany(x => x.MailID == id && x.Customer_Id == customerId).FirstOrDefault();
        }

        public MailTemplate GetMailTemplateForCustomer(int id, int customerId, int languageId)
        {
            return this._mailTemplateRepository.GetMailTemplateForCustomer(id, customerId, languageId);
        }

        public MailTemplateLanguage GetMailTemplateLanguageForCustomer(int id, int customerId, int languageId)
        {
            return this._mailTemplateLanguageRepository.GetMailTemplateLanguageForCustomer(id, customerId, languageId);
        }

        public MailTemplateLanguageEntity GetMailTemplateForCustomerAndLanguage(int customerId, int languageId, int mailTemplateId)
        {
            return this._mailTemplateLanguageRepository.GetMailTemplateForCustomerAndLanguage(customerId, languageId, mailTemplateId);    
        }

        public MailTemplateLanguageEntity GetMailTemplateLanguage(int id, int languageId)
        {
            return this._mailTemplateLanguageRepository.Get(x => x.MailTemplate_Id == id && x.Language_Id == languageId);
        }

        public void SaveMailTemplateLanguage(MailTemplateLanguageEntity mailtemplatelanguage, bool update, out IDictionary<string, string> errors)
        {
            if (mailtemplatelanguage == null)
                throw new ArgumentNullException("mailtemplatelanguage");

            errors = new Dictionary<string, string>();

            mailtemplatelanguage.Subject = mailtemplatelanguage.Subject ?? "";
            mailtemplatelanguage.Body = mailtemplatelanguage.Body ?? "";
            mailtemplatelanguage.MailTemplateName = mailtemplatelanguage.MailTemplateName ?? "";

            if (!update)
                this._mailTemplateLanguageRepository.Add(mailtemplatelanguage);
            else
                this._mailTemplateLanguageRepository.Update(mailtemplatelanguage);

            if (errors.Count == 0)
                this.Commit();
        }

        public void DeleteMailTemplateLanguage(MailTemplateLanguageEntity mailtemplatelanguage, out IDictionary<string, string> errors)
        {
           
            errors = new Dictionary<string, string>();

            if (mailtemplatelanguage.Language_Id != 0 && mailtemplatelanguage.MailTemplate_Id != 0)
                this._mailTemplateLanguageRepository.Delete(mailtemplatelanguage);
            
            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
