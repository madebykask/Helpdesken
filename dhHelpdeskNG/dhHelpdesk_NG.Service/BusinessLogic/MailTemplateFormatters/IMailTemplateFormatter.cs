namespace DH.Helpdesk.Services.BusinessLogic.MailTemplateFormatters
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public interface IMailTemplateFormatter<TBusinessModel>
    {
        Mail Format(MailTemplate template, TBusinessModel businessModel, int customerId, int languageId);
    }
}