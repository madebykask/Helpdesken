namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders;

    public interface IOrdersService
    {
        OrderFieldSettingsOverview[] GetFieldSettingsOverviews(
                                int customerId,
                                int? orderTypeId);
    }
}