namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IImplementationModelFactory
    {
        #region Public Methods and Operators

        ImplementationModel Create(
            string temporaryId,
            ImplementationFieldEditSettings editSettings,
            ChangeEditData editData);

        ImplementationModel Create(Change change, ImplementationFieldEditSettings editSettings, ChangeEditData editData);

        #endregion
    }
}