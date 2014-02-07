namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes;

    public interface INewChangeModelFactory
    {
        NewChangeModel Create(string temporatyId, ChangeEditOptions optionalData, ChangeEditSettings editSettings);
    }
}