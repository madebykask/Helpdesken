using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChangeUserPasswordModel
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public User User { get; set; }
    }

    public class UserIndexViewModel
    {
        public string SearchUs { get; set; }

        public User User { get; set; }
        public Customer Customer { get; set; }

        public IEnumerable<User> Users { get; set; }

        public IList<LoggedOnUsersOnIndexPage> ListLoggedOnUsers { get; set; }

        public IList<SelectListItem> CsSelected { get; set; }
        public IList<SelectListItem> StatusUsers { get; set; }
        public IList<SelectListItem> Customers { get; set; }
    }

    public class UserInputViewModel
    {
        public int OrderP1 { get; set; }
        public int OrderP2 { get; set; }
        public int SendMailYesNo { get; set; }
        public int? SendMailYesNoWhen { get; set; }
        public int StateStatusCase { get; set; }
        public int? UserRights { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }
        public string[] MenuSetting { get; set; }

        public User User { get; set; }
        public UsersUserRole UsersUserRole { get; set; }

        public IEnumerable<CustomerUser> CustomerUsers { get; set; }

        public IList<Department> Departments { get; set; }
        public IList<CustomerWorkingGroupForUser> ListWorkingGroupsForUser { get; set; }

        public IList<SelectListItem> AAsAvailable { get; set; }
        public IList<SelectListItem> AAsSelected { get; set; }
        public IList<SelectListItem> CsAvailable { get; set; }
        public IList<SelectListItem> CsSelected { get; set; }
        public IList<SelectListItem> Customers { get; set; }
        public IList<SelectListItem> Domains { get; set; }
        public IList<SelectListItem> Languages { get; set; }
        public IList<SelectListItem> OTsAvailable { get; set; }
        public IList<SelectListItem> OTsSelected { get; set; }
        public IList<SelectListItem> RefreshInterval { get; set; }
        public IList<SelectListItem> StartPageShowList { get; set; }
        public IList<SelectListItem> UserGroups { get; set; }
        public IList<SelectListItem> UserRoles { get; set; }
        public IList<SelectListItem> WorkingGroups { get; set; }

        public List<SelectListItem> CaseInfoMailList { get; set; }

        public UserInputViewModel() { }
    }

    public class UserSaveViewModel
    {
        public User User { get; set; }

        public int OrderP1 { get; set; }
        public int OrderP2 { get; set; }
        public int UserOrderPermission { get; set; }
        public int SendMailYesNo { get; set; }
        public int? SendMailYesNoWhen { get; set; }
        public int StateStatusCase { get; set; }
        public int? UserRights { get; set; }
        //public string CaseStateSecondaryColor { get; set; }
        public string[] MenuSetting { get; set; }
    }
}