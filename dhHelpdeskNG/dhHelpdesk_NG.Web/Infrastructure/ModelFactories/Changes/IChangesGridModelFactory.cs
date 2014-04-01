namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangesGrid;

    public interface IChangesGridModelFactory
    {
        #region Public Methods and Operators

        ChangesGridModel Create(SearchResult result, ChangeOverviewSettings settings);

        #endregion
    }
}