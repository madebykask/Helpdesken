namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class HistoryModelFactory : IHistoryModelFactory
    {
        public HistoryModel Create(FindOrderResponse response)
        {
            var historyModels = new List<HistoryRecordModel>(response.EditData.Histories.Count);

            foreach (var history in response.EditData.Histories)
            {
                var differences =
                    history.History.Select(h => new FieldDifferencesModel(h.FieldName, h.OldValue, h.NewValue)).ToList();

                var historyModel = new HistoryRecordModel(
                    history.DateAndTime,
                    history.RegisteredBy,
                    //history.Log,
                    differences,
                    history.Emails);

                historyModels.Add(historyModel);
            }

            return new HistoryModel(historyModels);
        }
    }
}