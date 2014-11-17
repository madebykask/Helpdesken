namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index;

    public interface IOrderFieldSettingsService
    {
        OrdersFieldSettingsOverview GetOrdersFieldSettingsOverview(
                                int customerId,
                                int? orderTypeId); 
    }
}