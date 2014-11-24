namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings;
    using DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings;

    public interface IOrderFieldSettingsModelFactory
    {
        OrderFieldSettingsIndexModel GetIndexModel(OrderFieldSettingsFilterData data, OrderFieldSettingsFilterModel filter);

        FullFieldSettingsModel Create(                    
                    GetSettingsResponse response,
                    int? orderTypeId);

        FullFieldSettings CreateForUpdate(
                    FullFieldSettingsModel model, 
                    int customerId,
                    int? orderTypeId,
                    DateTime changedDate);
    }
}