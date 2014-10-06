namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface ILogModelFactory
    {
        #region Public Methods and Operators

        LogModel Create(FindChangeResponse response);

        #endregion
    }
}