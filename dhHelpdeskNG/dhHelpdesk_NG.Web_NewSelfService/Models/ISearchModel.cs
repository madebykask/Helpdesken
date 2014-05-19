namespace DH.Helpdesk.NewSelfService.Models
{
    public interface ISearchModel<TFilter>
    {
        TFilter ExtractFilters();
    }
}