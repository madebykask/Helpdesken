using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Web.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<CustomerUser> CustomerUsers { get; set; }
        public IEnumerable<CustomerUserList> ForStartCaseCustomerUsers { get; set; }

        public IList<BulletinBoard> BulletinBoards { get; set; }
        public IList<Calendar> Calendars { get; set; }
        public IList<Case> Cases { get; set; }
        public IList<CustomerUser> CustomerUsersForStart { get; set; }
        public IList<WorkingGroup> WorkingGroups { get; set; }

        public IList<SelectListItem> Customers { get; set; }
    }
}
