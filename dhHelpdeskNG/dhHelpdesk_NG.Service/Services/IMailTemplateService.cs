namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.Dal.NewInfrastructure;

    public interface IMailTemplateServiceNew
    {
        MailTemplate GetTemplate(int mailId, OperationContext operationContext);

        MailTemplate GetTemplate(int mailId, int customerId, int languageId, IUnitOfWork uow);
        MailTemplate GetTemplateById(int id, OperationContext operationContext);
    }
}