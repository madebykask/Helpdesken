namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;

    public class HomeIndexViewModel
    {
        public IEnumerable<CustomerUser> CustomerUsers { get; set; }
        public IEnumerable<CustomerUserList> ForStartCaseCustomerUsers { get; set; }

        public IList<BulletinBoard> BulletinBoards { get; set; }
        public IList<Calendar> Calendars { get; set; }
        public IList<Helpdesk.Domain.Case> Cases { get; set; }
        public IList<CustomerUser> CustomerUsersForStart { get; set; }
        public IList<WorkingGroupEntity> WorkingGroups { get; set; }

        public IList<SelectListItem> Customers { get; set; }
    }
}
