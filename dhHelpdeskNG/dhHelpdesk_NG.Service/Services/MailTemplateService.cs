namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IMailTemplateService
    {        
        IDictionary<string, string> Validate(MailTemplate mailTemplateToValidate);

        IList<MailTemplateList> GetMailTemplates(int customerId, int langaugeId);
        IList<MailTemplateIdentifier> GetMailTemplateIdentifiers();

        MailTemplate GetMailTemplate(int id, int customerId);
        MailTemplateLanguage GetMailTemplateLanguage(int id, int languageId);
        MailTemplateLanguage GetMailTemplateForCustomerAndLanguage(int customerId, int languageId, int mailTemplateId);

        void SaveMailTemplateLanguage(MailTemplateLanguage mailtemplatelanguage,  bool update, out IDictionary<string, string> errors);
        void DeleteMailTemplateLanguage(MailTemplateLanguage mailtemplatelanguage, out IDictionary<string, string> errors);
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

        public IDictionary<string, string> Validate(MailTemplate mailTemplateToValidate)
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

        public IList<MailTemplateIdentifier> GetMailTemplateIdentifiers()
        {
            return this._mailTemplateIdentifierRepository.GetAll().ToList();
        }

        public MailTemplate GetMailTemplate(int id, int customerId)
        {
            return this._mailTemplateRepository.GetMany(x => x.MailID == id && x.Customer_Id == customerId).FirstOrDefault();
        }

        public MailTemplateLanguage GetMailTemplateForCustomerAndLanguage(int customerId, int languageId, int mailTemplateId)
        {
            return this._mailTemplateLanguageRepository.GetMailTemplateForCustomerAndLanguage(customerId, languageId, mailTemplateId);    
        }

        public MailTemplateLanguage GetMailTemplateLanguage(int id, int languageId)
        {
            return this._mailTemplateLanguageRepository.Get(x => x.MailTemplate_Id == id && x.Language_Id == languageId);
        }

        public void SaveMailTemplateLanguage(MailTemplateLanguage mailtemplatelanguage, bool update, out IDictionary<string, string> errors)
        {
            if (mailtemplatelanguage == null)
                throw new ArgumentNullException("mailtemplatelanguage");

            errors = new Dictionary<string, string>();

            mailtemplatelanguage.Subject = mailtemplatelanguage.Subject ?? "";
            mailtemplatelanguage.Body = mailtemplatelanguage.Body ?? "";
            mailtemplatelanguage.Name = mailtemplatelanguage.Name ?? "";

            if (!update)
                this._mailTemplateLanguageRepository.Add(mailtemplatelanguage);
            else
                this._mailTemplateLanguageRepository.Update(mailtemplatelanguage);

            if (errors.Count == 0)
                this.Commit();
        }

        public void DeleteMailTemplateLanguage(MailTemplateLanguage mailtemplatelanguage, out IDictionary<string, string> errors)
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
