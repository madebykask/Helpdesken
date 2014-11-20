namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;

    public interface IOrderFieldSettingsService
    {
        FullFieldSettingsOverview GetOrdersFieldSettingsOverview(
                                int customerId,
                                int? orderTypeId);

        OrderFieldSettingsFilterData GetFilterData(int customerId);
    }
}