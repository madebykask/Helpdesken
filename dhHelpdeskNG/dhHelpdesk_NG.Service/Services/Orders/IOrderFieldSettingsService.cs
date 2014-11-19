namespace DH.Helpdesk.Services.Services.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview;

    public interface IOrderFieldSettingsService
    {
        FullFieldSettingsOverview GetOrdersFieldSettingsOverview(
                                int customerId,
                                int? orderTypeId); 
    }
}