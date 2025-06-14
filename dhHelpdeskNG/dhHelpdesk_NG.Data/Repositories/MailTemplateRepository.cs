namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region MAILTEMPLATE

    public interface IMailTemplateRepository : IRepository<MailTemplate>
    {
        IEnumerable<MailTemplateList> GetMailTemplate(int customerId, int languageId);
        MailTemplate GetMailTemplateForCustomer(int id, int customerId, int languageId);
        int GetNewMailTemplateMailId();
    }

    public class MailTemplateRepository : RepositoryBase<MailTemplate>, IMailTemplateRepository
    {
        public MailTemplateRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<MailTemplateList> GetMailTemplate(int customerId, int languageId)
        {
            var q = from m in this.DataContext.MailTemplates
                    join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                    where m.MailID >= 100 && l.Language_Id == languageId && m.Customer_Id == customerId
                    orderby l.Name
                    select new MailTemplateList
                    {
                        Id = m.Id,
                        Customer_Id = m.Customer_Id,
                        OrderType_Id = m.OrderType_Id,
                        AccountActivity_Id = m.AccountActivity_Id,
                        MailID = m.MailID,
                        IsStandard = m.IsStandard,
                        Language_Id = l.Language_Id,
                        Name = l.Name,
                        Subject = l.Subject,
                        Body = l.Body,
                        Footer = l.MailFooter,
                    };

            return q;
        }

        public MailTemplate GetMailTemplateForCustomer(int id, int customerId, int languageId)
        {
            return (from m in this.DataContext.MailTemplates
                    join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                    where m.MailID == id && l.Language_Id == languageId && (m.Customer_Id == customerId || m.Customer_Id == null)
                    select m).FirstOrDefault();


            
        }

        public int GetNewMailTemplateMailId()
        {
            
            var max_MailId =
                 (from m in this.DataContext.MailTemplates
                  select m.MailID).Max();

            return max_MailId;

        }
        
    }

    #endregion

    #region MAILTEMPLATELANGUAGE

    public interface IMailTemplateLanguageRepository : IRepository<MailTemplateLanguage>
    {
        MailTemplateLanguage GetMailTemplateForCustomerAndLanguage(int customerId, int languageId, int mailTemplateId);
        MailTemplateLanguage GetMailTemplateLanguageForCustomer(int Id, int customerId, int languageId);
        MailTemplateLanguage GetMailTemplateLanguageForCustomerToSave(int Id, int customerId, int languageId);
    }

    public class MailTemplateLanguageRepository : RepositoryBase<MailTemplateLanguage>, IMailTemplateLanguageRepository
    {
        public MailTemplateLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public MailTemplateLanguage GetMailTemplateForCustomerAndLanguage(int customerId, int languageId, int mailTemplateId)
        {
            return (from m in this.DataContext.MailTemplates
                    join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                    where m.MailID == mailTemplateId && l.Language_Id == languageId && m.Customer_Id == customerId
                    select l).FirstOrDefault();

        }

        public MailTemplateLanguage GetMailTemplateLanguageForCustomer(int Id, int customerId, int languageId)
        {
            return (from m in this.DataContext.MailTemplates
                    join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                    where m.MailID == Id && l.Language_Id == languageId && (m.Customer_Id == customerId || m.Customer_Id == null)
                    select l).FirstOrDefault();
        }


        public MailTemplateLanguage GetMailTemplateLanguageForCustomerToSave(int Id, int customerId, int languageId)
        {
            return (from m in this.DataContext.MailTemplates
                    join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                    where m.MailID == Id && l.Language_Id == languageId && m.Customer_Id == customerId
                    select l).FirstOrDefault();
        }
    }

   
    #endregion

    #region MAILTEMPLATEIDENTIFIER

    public interface IMailTemplateIdentifierRepository : IRepository<MailTemplateIdentifier>
    {
        // expandable ....
    }

    public class MailTemplateIdentifierRepository : RepositoryBase<MailTemplateIdentifier>, IMailTemplateIdentifierRepository
    {
        public MailTemplateIdentifierRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
