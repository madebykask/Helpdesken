namespace DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public interface IMailTemplateFormatter<TBusinessModel>
    {
        Mail Format(MailTemplate template, TBusinessModel businessModel, int customerId, int languageId);
    }

    public interface IMailTemplateFormatterNew
    {
        Mail Format(MailTemplate template, EmailMarkValues markValues);
    }
}