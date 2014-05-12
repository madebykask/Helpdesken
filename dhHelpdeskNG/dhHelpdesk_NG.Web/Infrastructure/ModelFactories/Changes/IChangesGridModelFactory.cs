namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangesGrid;

    public interface IChangesGridModelFactory
    {
        #region Public Methods and Operators

        ChangesGridModel Create(SearchResponse response, SortField sortField);

        #endregion
    }
}