namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit.ExistingChange
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Responses.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IRegistrationModelFactory
    {
        #region Public Methods and Operators

        RegistrationViewModel Create(
            FindChangeResponse response, ChangeEditData editData, RegistrationEditSettings settings);

        #endregion
    }
}