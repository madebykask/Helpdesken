namespace DH.Helpdesk.Services.Infrastructure.MailTemplateFormatters
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public interface IMailTemplateFormatter<TBusinessModel>
    {
        Mail Format(MailTemplate template, TBusinessModel model, int customerId, int languageId);
    }
}