namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;

    public interface IUpdateOrderRequestValidator
    {
        void Validate(FullOrderEditFields updatedOrder, FullOrderEditFields existingOrder, FullOrderEditSettings settings); 
    }
}