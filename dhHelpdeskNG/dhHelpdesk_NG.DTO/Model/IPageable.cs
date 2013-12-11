using System.Collections.Generic;

namespace  dhHelpdesk_NG.DTO.Model
{
    public interface IPageableInfo
    {
        int PageCount { get; set; }
        int PageIndex { get; set; }
        int RowTotal { get; set; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
    }

    public interface IPageable<T> : IPageableInfo
    {
        IEnumerable<T> Page { get; set; }
    }
}
