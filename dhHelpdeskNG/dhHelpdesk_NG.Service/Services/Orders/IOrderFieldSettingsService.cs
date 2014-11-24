namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;

    public interface IOrderFieldSettingsService
    {
        FullFieldSettingsOverview GetOrdersFieldSettingsOverview(
                                int customerId,
                                int? orderTypeId);

        OrderFieldSettingsFilterData GetFilterData(int customerId);

        GetSettingsResponse GetOrderFieldSettings(
                                int customerId,
                                int? orderTypeId);

        void Update(FullFieldSettings settings);
    }
}