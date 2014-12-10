namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;

    public interface IOrderRestorer
    {
         void Restore(FullOrderEditFields updatedOrder, FullOrderEditFields existingOrder, FullOrderEditSettings settings);
    }
}