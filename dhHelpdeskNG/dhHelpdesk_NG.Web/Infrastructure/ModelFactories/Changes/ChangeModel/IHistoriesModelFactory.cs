namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Models.Changes;

    public interface IHistoriesModelFactory
    {
        HistoriesModel Create(FindChangeResponse response);
    }
}