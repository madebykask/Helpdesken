namespace DH.Helpdesk.Dal.Repositories.MailTemplates.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;

    #region MAILTEMPLATE

    public sealed class MailTemplateRepository : RepositoryBase<MailTemplateEntity>, IMailTemplateRepository
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
                orderby l.MailTemplateName
                select
                    new MailTemplateList
                    {
                        Id = m.Id,
                        Customer_Id = m.Customer_Id,
                        OrderType_Id = m.OrderType_Id,
                        AccountActivity_Id = m.AccountActivity_Id,
                        MailID = m.MailID,
                        IsStandard = m.IsStandard,
                        Language_Id = l.Language_Id,
                        Name = l.MailTemplateName,
                        Subject = l.Subject,
                        Body = l.Body,
                        Footer = l.MailFooter,
                    };

            return q;
        }

        public MailTemplateEntity GetMailTemplateForCustomer(int id, int customerId, int languageId)
        {
            return (from m in this.DataContext.MailTemplates
                join l in this.DataContext.MailTemplateLanguages on m.Id equals l.MailTemplate_Id
                where
                    m.MailID == id && l.Language_Id == languageId
                    && (m.Customer_Id == customerId || m.Customer_Id == null)
                select m).FirstOrDefault();
        }

        public int GetTemplateId(ChangeTemplate template, int customerId)
        {
            var customerTemplates = this.DataContext.MailTemplates.Where(t => t.Customer_Id == customerId);

            switch (template)
            {
                case ChangeTemplate.AssignedToUser:
                    return customerTemplates.Single(t => t.MailID == (int)ChangeTemplate.AssignedToUser).Id;
                case ChangeTemplate.SendLogNoteTo:
                    return customerTemplates.Single(t => t.MailID == (int)ChangeTemplate.SendLogNoteTo).Id;
                case ChangeTemplate.Cab:
                    return customerTemplates.Single(t => t.MailID == (int)ChangeTemplate.Cab).Id;
                case ChangeTemplate.Pir:
                    return customerTemplates.Single(t => t.MailID == (int)ChangeTemplate.Pir).Id;
                case ChangeTemplate.StatusChanged:
                    return customerTemplates.Single(t => t.MailID == (int)ChangeTemplate.StatusChanged).Id;
                case ChangeTemplate.Change:
                    return customerTemplates.Single(t => t.MailID == (int)ChangeTemplate.Change).Id;
                default:
                    throw new ArgumentOutOfRangeException("template");
            }
        }
    }

    #endregion
}