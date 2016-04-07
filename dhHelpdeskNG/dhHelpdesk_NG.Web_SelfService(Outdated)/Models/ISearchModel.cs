namespace DH.Helpdesk.SelfService.Models
{
    public interface ISearchModel<TFilter>
    {
        TFilter ExtractFilters();
    }
}