namespace DH.Helpdesk.Services.Services.Orders
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
using DH.Helpdesk.BusinessData.Models.Case;

    public interface IOrdersService
    {
        OrdersFilterData GetOrdersFilterData(int customerId, out int[] filters);

        SearchResponse Search(SearchParameters parameters);

        NewOrderEditData GetNewOrderEditData(int customerId, int orderTypeId, int? lowestchildordertypeid);

        FindOrderResponse FindOrder(int orderId, int customerId);

        int AddOrUpdate(UpdateOrderRequest request, string userId, CaseMailSetting caseMailSetting, int languageId);

        void Delete(int id);

        List<BusinessData.Models.Orders.Order.OrderEditFields.Log> FindLogsExcludeSpecified(int orderId, List<int> excludeLogIds);
    }
}