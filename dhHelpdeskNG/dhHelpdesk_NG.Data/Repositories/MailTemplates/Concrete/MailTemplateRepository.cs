namespace DH.Helpdesk.Dal.Repositories.MailTemplates.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.MailTemplates;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Language.Output;

    #region MAILTEMPLATE

    public sealed class MailTemplateRepository : RepositoryBase<MailTemplateEntity>, IMailTemplateRepository
    {
        public MailTemplateRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<MailTemplateList> GetMailTemplates(int customerId, int languageId)
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
                        IncludeLogText_External = m.IncludeLogText_External,
                        Language_Id = l.Language_Id,
                        Name = l.MailTemplateName,
                        Subject = l.Subject,
                        Body = l.Body,
                        Footer = l.MailFooter,
                    };

            return q;
        }
        public IEnumerable<MailTemplateList> GetAllMailTemplatesForCustomer(int customerId)
        {
            var q = from m in this.DataContext.MailTemplates
                    where m.Customer_Id == customerId
                    orderby m.MailID
                    select
                        new MailTemplateList
                        {
                            Id = m.Id,
                            Customer_Id = m.Customer_Id,
                            OrderType_Id = m.OrderType_Id,
                            AccountActivity_Id = m.AccountActivity_Id,
                            MailID = m.MailID,
                            IsStandard = m.IsStandard,
                            IncludeLogText_External = m.IncludeLogText_External,
                        };

            return q;
        }

        public CustomMailTemplate GetCustomMailTemplate(int mailTemplateId)
        {
            var res = new CustomMailTemplate();

            var mailTemplateEntities = this.DataContext.MailTemplates.Where(t => t.Id == mailTemplateId).FirstOrDefault();

            if (mailTemplateEntities == null)
                return null;

            var mailTemplateLanguageEntities = this.DataContext.MailTemplateLanguages
                                                               .Where(tl => tl.Language.IsActive != 0).ToList();

            
            var templateLanguages = new List<CustomMailTemplateLanguage>();
            foreach (var templateLang in mailTemplateLanguageEntities.Where(ml => ml.MailTemplate_Id == mailTemplateEntities.Id))
            {
                var templateLanguage = new CustomMailTemplateLanguage
                {
                    Subject = templateLang.Subject,
                    Body = templateLang.Body,
                    LanguageId = templateLang.Language_Id,
                    TemplateName = templateLang.MailTemplateName,
                    Language = new LanguageOverview
                    {
                        Id = templateLang.Language.Id,
                        LanguageId = templateLang.Language.LanguageID,
                        Name = templateLang.Language.Name,
                        IsActive = templateLang.Language.IsActive
                    }
                };

                templateLanguages.Add(templateLanguage);
            }

            var mailTemplate = new CustomMailTemplate
            {
                MailTemplateId = mailTemplateEntities.Id,
                CustomerId = mailTemplateEntities.Customer_Id.Value,
                IsStandard = mailTemplateEntities.IsStandard,
                MailId = mailTemplateEntities.MailID,
                OrderTypeId = mailTemplateEntities.OrderType_Id,
                TemplateLanguages = templateLanguages,
                IncludeLogText_External = mailTemplateEntities.IncludeLogText_External,
            };

            res = mailTemplate;
            

            return res;
        }

        public List<CustomMailTemplate> GetCustomMailTemplatesList(int customerId)
        {
            var res = new List<CustomMailTemplate>();

            var items = (from mt in Table
                         from mt_l in mt.MailTemplateLanguages
                         let lang = mt_l.Language
                         where mt.Customer_Id == customerId &&
                         lang.IsActive > 0
                         select new 
                         {
                             MailTemplateId = mt.Id,
                             CustomerId = mt.Customer_Id.Value,
                             IsStandard = mt.IsStandard,
                             MailId = mt.MailID,
                             OrderTypeId = mt.OrderType_Id,
                             IncludeLogText_External = mt.IncludeLogText_External,
                             TemplateLanguage = new CustomMailTemplateLanguage
                             {
                                 Subject = mt_l.Subject,
                                 //Body = mt_l.Body,
                                 LanguageId = lang.Id,
                                 TemplateName = mt_l.MailTemplateName,
                                 Language = new LanguageOverview
                                 {
                                     Id = lang.Id,
                                     LanguageId = lang.LanguageID,
                                     Name = lang.Name,
                                     IsActive = lang.IsActive
                                 }
                             }
                         }).ToList();

            res = items.GroupBy(k => 
                            new 
                            {
                                k.MailTemplateId,
                                k.CustomerId,
                                k.IsStandard,
                                k.MailId,
                                k.OrderTypeId
                            })
                        .Select(gr => 
                            new CustomMailTemplate
                            {
                                MailTemplateId = gr.Key.MailTemplateId,
                                CustomerId = gr.Key.CustomerId,
                                IsStandard = gr.Key.IsStandard,
                                MailId = gr.Key.MailId,
                                OrderTypeId = gr.Key.OrderTypeId,
                                TemplateLanguages = gr.Select(o => o.TemplateLanguage).ToList()
                            }).ToList();

            return res;
        }

        public List<CustomMailTemplate> GetCustomMailTemplatesFull(int customerId)
        {
            var items = (from mt in Table
                         from mt_l in mt.MailTemplateLanguages
                         let lang = mt_l.Language
                         where mt.Customer_Id == customerId &&
                         lang.IsActive > 0
                         select new
                         {
                             MailTemplate = mt,
                             MailTemplateLanguage = mt_l,
                             Language = lang
                         }).ToList();

            var res = items.GroupBy(k => k.MailTemplate)
                        .Select(gr =>
                            new CustomMailTemplate
                            {
                                MailTemplateId = gr.Key.Id,
                                CustomerId = gr.Key.Customer_Id ?? 0,
                                IsStandard = gr.Key.IsStandard,
                                MailId = gr.Key.MailID,
                                OrderTypeId = gr.Key.OrderType_Id,
                                IncludeLogText_External = gr.Key.IncludeLogText_External,
                                TemplateLanguages = gr.Select(o => 
                                    new CustomMailTemplateLanguage
                                    {
                                        Subject = o.MailTemplateLanguage.Subject,
                                        Body = o.MailTemplateLanguage.Body,
                                        LanguageId = o.MailTemplateLanguage.Language_Id,
                                        TemplateName = o.MailTemplateLanguage.MailTemplateName,
                                        Language = new LanguageOverview
                                        {
                                            Id = o.Language.Id,
                                            LanguageId = o.Language.LanguageID,
                                            Name = o.Language.Name,
                                            IsActive = o.Language.IsActive
                                        }
                                    }).ToList()
                            }).ToList();
            return res;
        }

        public int GetNewMailTemplateMailId()
        {
            var mailIds = (from m in this.DataContext.MailTemplates select m.MailID).ToList();

            if (mailIds.Any())
                return mailIds.Max();
            else
                return 0;
        }

        public int GetMailTemlpateMailId(int templateId)
        {
            var mailId = DataContext.MailTemplates.Where(t => t.Id == templateId).Select(x => x.MailID).FirstOrDefault();
            return mailId;
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
                    entity = customerTemplates.FirstOrDefault(t => t.MailID == (int)ChangeTemplate.AssignedToUser);
                    break;
                case ChangeTemplate.SendLogNoteTo:
                    entity = customerTemplates.FirstOrDefault(t => t.MailID == (int)ChangeTemplate.SendLogNoteTo);
                    break;
                case ChangeTemplate.Cab:
                    entity = customerTemplates.FirstOrDefault(t => t.MailID == (int)ChangeTemplate.Cab);
                    break;
                case ChangeTemplate.Pir:
                    entity = customerTemplates.FirstOrDefault(t => t.MailID == (int)ChangeTemplate.Pir);
                    break;
                case ChangeTemplate.StatusChanged:
                    entity = customerTemplates.FirstOrDefault(t => t.MailID == (int)ChangeTemplate.StatusChanged);
                    break;
                case ChangeTemplate.Change:
                    entity = customerTemplates.FirstOrDefault(t => t.MailID == (int)ChangeTemplate.Change);
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