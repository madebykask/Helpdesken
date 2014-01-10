
namespace dhHelpdesk_NG.Domain
{
    public interface ICalendarSearch : ISearch
    {
        int CustomerId { get; set; }
        string SearchCs { get; set; }

    }

    public class CalendarSearch : Search, ICalendarSearch
    {
        public int CustomerId { get; set; }
        public string SearchCs { get; set; }
    }
}
