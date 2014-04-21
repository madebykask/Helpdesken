namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.NewChange
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface INewChangeModelFactory
    {
        #region Public Methods and Operators

        InputModel Create(string temporatyId, GetNewChangeEditDataResponse response);

        #endregion
    }
}