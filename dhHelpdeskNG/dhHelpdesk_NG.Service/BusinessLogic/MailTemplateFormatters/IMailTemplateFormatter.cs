namespace DH.Helpdesk.Services.Infrastructure.MailTemplateFormatters
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public interface IMailTemplateFormatter<TBusinessModel>
    {
        Mail Format(MailTemplate template, TBusinessModel businessModel, int customerId, int languageId);
    }
}