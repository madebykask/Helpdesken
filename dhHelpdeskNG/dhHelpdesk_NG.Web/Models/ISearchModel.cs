namespace DH.Helpdesk.Web.Models
{
    public interface ISearchModel<TFilter>
    {
        TFilter ExtractFilters();
    }
}