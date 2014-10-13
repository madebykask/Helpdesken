namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.Mobile.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Mobile.Models.Changes;

    public interface ISearchModelFactory
    {
        #region Public Methods and Operators

        SearchModel Create(ChangesFilter filter, GetSearchDataResponse response);

        #endregion
    }
}