namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public interface IHistoryModelFactory
    {
        #region Public Methods and Operators

        HistoryModel Create(FindChangeResponse response);

        #endregion
    }
}