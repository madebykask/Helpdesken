namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Services.Response.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IImplementationModelFactory
    {
        #region Public Methods and Operators

        ImplementationViewModel Create(
            FindChangeResponse response, ChangeEditData editData, ImplementationEditSettings settings);

        #endregion
    }
}