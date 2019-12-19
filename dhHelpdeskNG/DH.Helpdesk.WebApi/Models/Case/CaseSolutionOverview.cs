using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.WebApi.Models.Case
{
    public class CustomerCaseSolution
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<CustomerCaseSolutionOverviewItem> Items { get; set; }
    }

    public class CustomerCaseSolutionOverviewItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}