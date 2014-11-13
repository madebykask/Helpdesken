namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.OrderSettings;

    public interface IOrderFieldSettingsService
    {
        OrderFieldSettingsOverview[] GetOrderFieldSettingsOverviews(
                                int customerId,
                                int? orderTypeId); 
    }
}