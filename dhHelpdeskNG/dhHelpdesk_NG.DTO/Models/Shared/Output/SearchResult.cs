using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
