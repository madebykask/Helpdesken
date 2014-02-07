namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface INotifiersGridModelFactory
    {
        NotifiersGridModel Create(SearchResultDto searchResult, FieldSettingsDto displaySettings);
    }
}