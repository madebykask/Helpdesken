namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Mobile.Models.Changes.ChangeEdit;

    public interface IChangeModelFactory
    {
        #region Public Methods and Operators

        InputModel Create(
            FindChangeResponse response,
            OperationContext context);

        #endregion
    }
}