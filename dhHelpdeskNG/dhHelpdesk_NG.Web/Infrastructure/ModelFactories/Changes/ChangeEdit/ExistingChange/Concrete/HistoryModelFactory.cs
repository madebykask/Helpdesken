namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class HistoryModelFactory : IHistoryModelFactory
    {
        #region Public Methods and Operators

        public HistoryModel Create(FindChangeResponse response)
        {
            var historyModels = new List<HistoryRecordModel>(response.EditData.Histories.Count);

            foreach (var history in response.EditData.Histories)
            {
                var differences =
                    history.History.Select(h => new FieldDifferencesModel(h.FieldName, h.OldValue, h.NewValue)).ToList();

                var historyModel = new HistoryRecordModel(
                    history.DateAndTime,
                    history.RegisteredBy,
                    history.Log,
                    differences,
                    history.Emails);

                historyModels.Add(historyModel);
            }

            return new HistoryModel(historyModels);
        }

        #endregion
    }
}