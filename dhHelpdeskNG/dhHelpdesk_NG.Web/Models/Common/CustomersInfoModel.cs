using System.Collections.Generic;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Models.Common
{
    public class CustomersInfoModel
    {
        public IList<Domain.Case> Cases { get; set; }
        public IList<CustomerUser> CustomerUsersForStart { get; set; }
    }
}