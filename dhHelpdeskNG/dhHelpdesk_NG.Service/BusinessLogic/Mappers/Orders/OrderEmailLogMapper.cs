namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;

    using DH.Helpdesk.Domain;

    public static class OrderEmailLogMapper
    {
        public static OrderEMailLog MapToEntity(BusinessData.Models.Orders.Order.OrderEditFields.EmailLog log)
        {
            return new OrderEMailLog
                       {
                           OrderEMailLogGUID = Guid.NewGuid(),
                           OrderHistoryId = log.HistoryId,
                           CreatedDate = log.CreatedDateAndTime,
                           EMailAddress = string.Join(";", log.Emails),
                           MailID = log.MailId,
                           MessageId = log.MessageId
                       };
        }
    }
}