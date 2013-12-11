using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region MAILTEMPLATE

    public interface IMailTemplateRepository : IRepository<MailTemplate>
    {
        IEnumerable<MailTemplateList> GetMailTemplate(int customerId, int languageId);
       
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

    }

    #endregion

    #region MAILTEMPLATELANGUAGE

    public interface IMailTemplateLanguageRepository : IRepository<MailTemplateLanguage>
    {
       
    }

    public class MailTemplateLanguageRepository : RepositoryBase<MailTemplateLanguage>, IMailTemplateLanguageRepository
    {
        public MailTemplateLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
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
