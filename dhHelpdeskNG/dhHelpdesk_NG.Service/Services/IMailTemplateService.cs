namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;

    public interface IMailTemplateServiceNew
    {
        MailTemplate GetTemplate(int mailId, OperationContext operationContext);
    }
}