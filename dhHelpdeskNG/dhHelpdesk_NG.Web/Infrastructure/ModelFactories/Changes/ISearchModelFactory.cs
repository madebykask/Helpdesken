namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.Web.Infrastructure.Filters.Changes;
    using DH.Helpdesk.Web.Models.Changes;

    public interface ISearchModelFactory
    {
        #region Public Methods and Operators

        SearchModel Create(ChangesFilter filter, SearchData searchData, SearchSettings settings);

        #endregion
    }
}