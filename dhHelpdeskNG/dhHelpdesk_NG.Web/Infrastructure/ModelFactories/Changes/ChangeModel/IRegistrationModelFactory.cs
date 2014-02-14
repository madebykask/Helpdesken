namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IRegistrationModelFactory
    {
        #region Public Methods and Operators

        RegistrationModel Create(
            string temporaryId,
            RegistrationFieldEditSettings editSettings,
            ChangeEditData editData);

        RegistrationModel Create(Change change, RegistrationFieldEditSettings editSettings, ChangeEditData editData);

        #endregion
    }
}