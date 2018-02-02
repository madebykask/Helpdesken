using DH.Helpdesk.SelfService.Infrastructure;

namespace DH.Helpdesk.SelfService.Models
{
    public class CaseSearchInputModel
    {
        public int CustomerId { get; set; }
        public string ProgressId { get; set; }
        public string PharasSearch { get; set; }
        public string SortBy { get; set; }
        public Enums.SortOrder SortOrder { get; set; }
    }
}