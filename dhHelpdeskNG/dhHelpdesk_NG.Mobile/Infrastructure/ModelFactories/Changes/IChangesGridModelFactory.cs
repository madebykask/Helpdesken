namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Mobile.Models.Changes.ChangesGrid;

    public interface IChangesGridModelFactory
    {
        #region Public Methods and Operators

        ChangesGridModel Create(SearchResponse response, SortField sortField);

        #endregion
    }
}