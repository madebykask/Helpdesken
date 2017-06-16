namespace DH.Helpdesk.Services.Services.Orders
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Index;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
using DH.Helpdesk.BusinessData.Models.Case;

    public interface IOrdersService
    {
        OrdersFilterData GetOrdersFilterData(int customerId, int userId, out int[] filters);

        SearchResponse Search(SearchParameters parameters, int userId, bool isSelfService = false);

        NewOrderEditData GetNewOrderEditData(int customerId, int orderTypeId, int? lowestchildordertypeid, bool useExternal);

        FindOrderResponse FindOrder(int orderId, int customerId, bool useExternal);

        int AddOrUpdate(UpdateOrderRequest request, string userId, CaseMailSetting caseMailSetting, int languageId, bool useExternal);

        void Delete(int id);

        List<BusinessData.Models.Orders.Order.OrderEditFields.Log> FindLogsExcludeSpecified(int orderId, List<int> excludeLogIds);
    }
}