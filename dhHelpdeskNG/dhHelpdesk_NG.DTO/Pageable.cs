using System.Collections.Generic;
using dhHelpdesk_NG.DTO.Model;

namespace dhHelpdesk_NG.DTO
{
    public class Pageable<T> : IPageable<T>
    {
        public int PageCount { get; set; }

        public IEnumerable<T> Page { get; set; }

        public int PageIndex { get; set; }

        public int RowTotal { get; set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < (PageCount - 1)); }
        }

        public bool IsFirstPage
        {
            get { return (PageIndex <= 0); }
        }

        public bool IsLastPage
        {
            get { return (PageIndex >= (PageCount - 1)); }
        }
    }
}
