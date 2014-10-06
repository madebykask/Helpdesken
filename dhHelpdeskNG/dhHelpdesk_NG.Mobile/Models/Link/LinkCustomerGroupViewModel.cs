using System.Collections.Generic;

namespace DH.Helpdesk.Web.Models.Link
{
    public sealed class LinkCustomerGroupViewModel
    {
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }

        private readonly List<LinkCategoryGroupViewModel> _categories = new List<LinkCategoryGroupViewModel>();
        public List<LinkCategoryGroupViewModel> Categories
        {
            get { return _categories; }
        }
    }
}