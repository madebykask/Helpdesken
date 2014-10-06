namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Mobile.Models.Notifiers;
    using DH.Helpdesk.Mobile.Models.Shared;

    public interface INotifiersGridModelFactory
    {
        NotifiersGridModel Create(SearchResult searchResult, FieldSettings settings, SortFieldModel sortField);
    }
}