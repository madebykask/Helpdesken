namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IHistoryModelFactory
    {
        #region Public Methods and Operators

        HistoryModel Create(FindChangeResponse response);

        #endregion
    }
}