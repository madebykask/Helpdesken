using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Shared.Output
{
    public class SearchResult<T>
    {
        public SearchResult()
        {
            Items = new List<T>();
        }
        public IList<T> Items { get; set; }

        public int Count { get; set; }
    }
}
