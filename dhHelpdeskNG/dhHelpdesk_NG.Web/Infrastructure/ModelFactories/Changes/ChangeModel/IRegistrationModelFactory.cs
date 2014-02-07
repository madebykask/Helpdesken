namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.Edit;

    public interface IRegistrationModelFactory
    {
        RegistrationModel Create(
            string temporaryId,
            RegistrationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);

        RegistrationModel Create(
            ChangeAggregate change,
            RegistrationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);
    }
}