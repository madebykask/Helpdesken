namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Models.Changes;

    public sealed class HistoriesModelFactory : IHistoriesModelFactory
    {
        public HistoriesModel Create(FindChangeResponse response)
        {
            var historyModels = new List<HistoryModel>(response.Histories.Count);

            foreach (var history in response.Histories)
            {
                var differences =
                    history.History.Select(h => new FieldDifferencesModel(h.FieldName, h.OldValue, h.NewValue)).ToList();

                var historyModel = new HistoryModel(
                    history.DateAndTime, history.RegisteredBy, history.Log, differences, history.Emails);

                historyModels.Add(historyModel);
            }

            return new HistoriesModel(historyModels);
        }
    }
}