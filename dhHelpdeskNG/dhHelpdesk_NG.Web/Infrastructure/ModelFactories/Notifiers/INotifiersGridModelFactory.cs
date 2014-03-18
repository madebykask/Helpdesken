namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.Web.Models.Common;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface INotifiersGridModelFactory
    {
        NotifiersGridModel Create(SearchResult searchResult, FieldSettings settings, SortFieldModel sortField);
    }
}