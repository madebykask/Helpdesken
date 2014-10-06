namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IGeneralModelFactory
    {
        #region Public Methods and Operators

        GeneralModel Create(FindChangeResponse response);

        #endregion
    }
}