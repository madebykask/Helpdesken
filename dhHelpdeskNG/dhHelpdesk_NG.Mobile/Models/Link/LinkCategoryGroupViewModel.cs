using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Link.Output;

namespace DH.Helpdesk.Mobile.Models.Link
{
    public sealed class LinkCategoryGroupViewModel
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        
        private readonly List<LinkOverview> _links = new List<LinkOverview>();
        public List<LinkOverview> Links
        {
            get { return _links; }
        }
    }
}