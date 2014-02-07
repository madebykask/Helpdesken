namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.Edit;

    public interface IImplementationModelFactory
    {
        ImplementationModel Create(
            string temporaryId,
            ImplementationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);

        ImplementationModel Create(
            ChangeAggregate change,
            ImplementationFieldEditSettings editSettings,
            ChangeEditOptions optionalData);
    }
}