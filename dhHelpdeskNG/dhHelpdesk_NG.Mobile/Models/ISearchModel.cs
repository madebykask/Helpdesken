namespace DH.Helpdesk.Mobile.Models
{
    public interface ISearchModel<TFilter>
    {
        TFilter ExtractFilters();
    }
}