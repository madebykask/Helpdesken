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

        public int GetNewMailTemplateMailId()
        {
            var max_MailId = (from m in this.DataContext.MailTemplates select m.MailID).Max();

            return max_MailId;
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

        public int? GetTemplateId(ChangeTemplate template, int customerId)
        {
            var customerTemplates = this.DataContext.MailTemplates.Where(t => t.Customer_Id == customerId);

            MailTemplateEntity entity;
            switch (template)
            {
                case ChangeTemplate.AssignedToUser:
                    entity = customerTemplates.SingleOrDefault(t => t.MailID == (int)ChangeTemplate.AssignedToUser);
                    break;
                case ChangeTemplate.SendLogNoteTo:
                    entity = customerTemplates.SingleOrDefault(t => t.MailID == (int)ChangeTemplate.SendLogNoteTo);
                    break;
                case ChangeTemplate.Cab:
                    entity = customerTemplates.SingleOrDefault(t => t.MailID == (int)ChangeTemplate.Cab);
                    break;
                case ChangeTemplate.Pir:
                    entity = customerTemplates.SingleOrDefault(t => t.MailID == (int)ChangeTemplate.Pir);
                    break;
                case ChangeTemplate.StatusChanged:
                    entity = customerTemplates.SingleOrDefault(t => t.MailID == (int)ChangeTemplate.StatusChanged);
                    break;
                case ChangeTemplate.Change:
                    entity = customerTemplates.SingleOrDefault(t => t.MailID == (int)ChangeTemplate.Change);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("template");
            }

            if (entity == null)
            {
                return null;
            }

            return entity.Id;
        }
    }

    #endregion
}