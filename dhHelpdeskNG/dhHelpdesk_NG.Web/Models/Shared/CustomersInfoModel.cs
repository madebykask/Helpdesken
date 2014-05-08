namespace DH.Helpdesk.Web.Models.Shared
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class CustomersInfoModel
    {
        public IList<Case> Cases { get; set; }
        public IList<CustomerUser> CustomerUsersForStart { get; set; }
    }
}