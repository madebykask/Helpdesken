using System.Collections.Generic;

namespace DH.Helpdesk.Mobile.Models.Link
{
    public sealed class LinksInfoViewModel
    {
        private readonly List<LinkCustomerGroupViewModel> _customerGroups = new List<LinkCustomerGroupViewModel>();
        public List<LinkCustomerGroupViewModel> CustomerGroups
        {
            get { return _customerGroups; }
        }
    }
}