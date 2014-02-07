namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.ChangeAggregate;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes;

    public interface IChangeModelFactory
    {
        ChangeModel Create(ChangeAggregate change, ChangeEditOptions optionalData, ChangeEditSettings editSettings);
    }
}