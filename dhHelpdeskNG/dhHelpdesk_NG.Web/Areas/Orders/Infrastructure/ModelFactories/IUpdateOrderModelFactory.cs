namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public interface IUpdateOrderModelFactory
    {
        UpdateOrderRequest Create(FullOrderEditModel model, int customerId, DateTime dateAndTime);
    }
}