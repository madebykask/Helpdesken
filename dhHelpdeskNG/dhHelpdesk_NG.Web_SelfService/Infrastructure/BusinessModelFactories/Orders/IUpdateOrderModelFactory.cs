using System;
using DH.Helpdesk.BusinessData.Models.Orders.Order;
using DH.Helpdesk.SelfService.Models.Orders.OrderEdit;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders
{
    public interface IUpdateOrderModelFactory
    {
        UpdateOrderRequest Create(
                    FullOrderEditModel model, 
                    int customerId, 
                    DateTime dateAndTime, 
                    IEmailService emailService, 
                    int userId,
                    int languageId);
    }
}