using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Service
{
    public interface IMailTemplateService
    {        
        IDictionary<string, string> Validate(MailTemplate mailTemplateToValidate);

        IList<MailTemplateList> GetMailTemplates(int customerId, int langaugeId);
        IList<MailTemplateIdentifier> GetMailTemplateIdentifiers();

        MailTemplate GetMailTemplate(int id, int customerId);
        MailTemplateLanguage GetMailTemplateLanguage(int id, int languageId);

        void SaveMailTemplateLanguage(MailTemplateLanguage mailtemplatelanguage, out IDictionary<string, string> errors);
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
            _mailTemplateRepository = mailTemplateRepository;
            _mailTemplateLanguageRepository = mailTemplateLanguageRepository;
            _mailTemplateIdentifierRepository = mailTemplateIdentifierRepository;
            _unitOfWork = unitOfWork;
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
            return _mailTemplateRepository.GetMailTemplate(customerId, langaugeId).ToList();
        }

        public IList<MailTemplateIdentifier> GetMailTemplateIdentifiers()
        {
            return _mailTemplateIdentifierRepository.GetAll().ToList();
        }

        public MailTemplate GetMailTemplate(int id, int customerId)
        {
           // return _mailTemplateRepository.GetById(id)
            return _mailTemplateRepository.Get(x => x.MailID == id && x.Customer_Id == customerId);
        }

        //public MailTemplate GetMailTemplateId()
        //{
        //    return _mailTemplateRepository.GetMailTemplateId();
        //}

        public MailTemplateLanguage GetMailTemplateLanguage(int id, int languageId)
        {
            return _mailTemplateLanguageRepository.Get(x=>x.MailTemplate_Id == id && x.Language_Id == languageId);
        }

        public void SaveMailTemplateLanguage(MailTemplateLanguage mailtemplatelanguage, out IDictionary<string, string> errors)
        {
            if (mailtemplatelanguage == null)
                throw new ArgumentNullException("mailtemplatelanguage");

            errors = new Dictionary<string, string>();

            mailtemplatelanguage.Subject = mailtemplatelanguage.Subject ?? "";
            mailtemplatelanguage.Body = mailtemplatelanguage.Body ?? "";

            if (mailtemplatelanguage.MailTemplate_Id == 0)
                _mailTemplateLanguageRepository.Add(mailtemplatelanguage);
            else
                _mailTemplateLanguageRepository.Update(mailtemplatelanguage);

            if (errors.Count == 0)
                this.Commit();
        }

        public void DeleteMailTemplateLanguage(MailTemplateLanguage mailtemplatelanguage, out IDictionary<string, string> errors)
        {
           
            errors = new Dictionary<string, string>();

            if (mailtemplatelanguage.Language_Id != 0 && mailtemplatelanguage.MailTemplate_Id != 0)
                _mailTemplateLanguageRepository.Delete(mailtemplatelanguage);
            
            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
