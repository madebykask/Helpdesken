namespace DH.Helpdesk.Services.BusinessLogic.MailTools.TemplateFormatters
{
    using DH.Helpdesk.BusinessData.Models.MailTemplates;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;

    public sealed class OrderMailTemplateFormatter : MailTemplateFormatter<UpdateOrderRequest>
    {
        protected override EmailMarkValues GetMarkValues(MailTemplate template, UpdateOrderRequest businessModel, int customerId, int languageId)
        {
            var markValues = new EmailMarkValues();

            return markValues;
        }
    }
}