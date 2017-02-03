namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Domain;

    public static class OrderEmailLogMapper
    {
        public static OrderEMailLog MapToEntity(BusinessData.Models.Orders.Order.OrderEditFields.EmailLog log)
        {
            return new OrderEMailLog
            {
                Order_Id = log.OrderId,
                OrderEMailLogGUID = Guid.NewGuid(),
                OrderHistoryId = log.HistoryId,
                CreatedDate = log.CreatedDateAndTime,
                EMailAddress = string.Join(";", log.Emails),
                MailID = log.MailId,
                MessageId = log.MessageId
            };
        }

        public static List<EmailLogOverview> MapToOverviews(this IQueryable<OrderEMailLog> query)
        {
            var entities = query.Select(l => new { l.OrderHistoryId, l.EMailAddress })
                            .ToList();

            return entities.Where(l => l.OrderHistoryId.HasValue).Select(l => new EmailLogOverview(l.OrderHistoryId.Value, l.EMailAddress)).ToList();
        }
    }
}