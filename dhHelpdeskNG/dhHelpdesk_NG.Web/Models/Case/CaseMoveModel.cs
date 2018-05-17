using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Customer.Input;

namespace DH.Helpdesk.Web.Models.Case
{
    public class CaseMoveModel
    {
        public int CaseCustomerId { get; set; }
        public IList<MoveCaseCustomersListItem> Customers { get; set; }
        public bool HasExtendedCase { get; set; }

        public IList<MoveCaseCustomersListItem> NonUserCustomers
        {
            get { return Customers.Where(x => !x.IsUserCustomer).OrderBy(x => x.Name).ToList(); }
        }

        public IList<MoveCaseCustomersListItem> UserCustomers
        {
            get { return Customers.Where(x => x.IsUserCustomer).OrderBy(x => x.Name).ToList(); }
        }
    }
}