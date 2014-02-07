namespace DH.Helpdesk.BusinessData
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Model;

    public class Pageable<T> : IPageable<T>
    {
        public int PageCount { get; set; }

        public IEnumerable<T> Page { get; set; }

        public int PageIndex { get; set; }

        public int RowTotal { get; set; }

        public bool HasPreviousPage
        {
            get { return (this.PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (this.PageIndex < (this.PageCount - 1)); }
        }

        public bool IsFirstPage
        {
            get { return (this.PageIndex <= 0); }
        }

        public bool IsLastPage
        {
            get { return (this.PageIndex >= (this.PageCount - 1)); }
        }
    }
}
