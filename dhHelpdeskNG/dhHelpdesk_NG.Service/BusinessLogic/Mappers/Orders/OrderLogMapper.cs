namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Domain;

    using Log = DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields.Log;

    public static class OrderLogMapper
    {
        public static List<Log> MapToLogs(this IQueryable<OrderLog> query, IQueryable<User> users)
        {
            var entities =
                    query.Join(
                        users,
                        l => l.User_Id,
                        u => u.Id,
                        (l, u) =>
                            new
                            {
                                l.Id,
                                DateAndTime = l.CreatedDate,
                                RegisteredByFirstName = u.FirstName,
                                RegisteredByLastName = u.SurName,
                                Text = l.LogNote
                            }).ToList();

            return
                entities.Select(
                    l =>
                        new Log(
                            l.Id,
                            l.DateAndTime,
                            new UserName(l.RegisteredByFirstName, l.RegisteredByLastName),
                            l.Text)).ToList();       
        }

        public static OrderLog MapToEntity(ManualLog model)
        {
            return new OrderLog
                    {
                        OrderHistoryId = model.OrderHistoryId,
                        Order_Id = model.OrderId,
                        User_Id = model.CreatedByUserId,
                        CreatedDate = model.CreatedDateAndTime,
                        ChangedDate = model.CreatedDateAndTime,
                        LogNote = model.Text
                    };
        }
    }
}