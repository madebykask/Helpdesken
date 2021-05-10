using DH.Helpdesk.BusinessData.Models.User;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.BusinessData.Models.Case.CaseLock;
    using DH.Helpdesk.Web.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public class ChangeUserPasswordModel
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public User User { get; set; }
    }

    /// <summary>
    /// The user index view model.
    /// </summary>
    public class UserIndexViewModel
    {
        /// <summary>
        /// The filter.
        /// </summary>
        private UserSearch filter = new UserSearch();

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        public UserSearch Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                this.filter = value;
            }
        }

        public User User { get; set; }
        
        public Customer Customer { get; set; }

        public UserList UserOverviewList { get; set; }

        public IList<LoggedInUsers> ListLoggedInUsers { get; set; }

        public LockedCaseOverviewModel LockedCaseModel { get; set; }

        public IList<SelectListItem> CsSelected { get; set; }

        public IList<SelectListItem> StatusUsers { get; set; }

        public IList<SelectListItem> Customers { get; set; }        


        public int OnlineUsersTabSelectedCustomerId { get; set; }
    }

    public class UserList
    {
        public UserList()
        {
        }
        public IEnumerable<User> Users { get; set; }

        public UserSort Sorting { get; set; }
    }

    public class UserSort
    {
        public UserSort()
        {
        }

        public string FieldName { get; set; }

        public bool IsAsc { get; set; }
    }

    

    public class UserInputViewModel
    {
        public int OrderP1 { get; set; }
        public int OrderP2 { get; set; }
        public int ContractPermission { get; set; }
        public int SendMailYesNo { get; set; }
        public int? SendMailYesNoWhen { get; set; }
        public int StateStatusCase { get; set; }
        public int? UserRights { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }
        public string[] MenuSetting { get; set; }

        public User User { get; set; }
        public UsersUserRole UsersUserRole { get; set; }
        public IList<CustomerUserForEdit> CustomerUsers { get; set; }
        public IList<Department> Departments { get; set; }
        public IList<CustomerWorkingGroupForUser> ListWorkingGroupsForUser { get; set; }
        public IList<SelectListItem> AAsAvailable { get; set; }
        public IList<SelectListItem> AAsSelected { get; set; }

        /// <summary>
        /// Customers available  for this user
        /// </summary>
        public IList<StateSelectListItem> CsAvailable { get; set; }

        /// <summary>
        /// Customers selected for this user
        /// </summary>
        public IList<StateSelectListItem> CsSelected { get; set; }
        public IList<StateSelectListItem> Customers { get; set; }
        public IList<SelectListItem> Domains { get; set; }
        public IList<SelectListItem> Languages { get; set; }
        public IList<SelectListItem> OTsAvailable { get; set; }
        public IList<SelectListItem> OTsSelected { get; set; }

        public IList<SelectListItem> CCsAvailable { get; set; }
        public IList<SelectListItem> CCsSelected { get; set; }
        public IList<SelectListItem> RefreshInterval { get; set; }
        public IList<SelectListItem> StartPageShowList { get; set; }

        /// <summary>
        /// Available groups to select
        /// </summary>
        public IList<SelectListItem> UserGroups { get; set; }
        public IList<SelectListItem> UserRoles { get; set; }
        public IList<SelectListItem> WorkingGroups { get; set; }

        //public IList<CaseSettings> UserColumns { get; set; }
        public int CopyUserid { get; set; }
        public List<SelectListItem> CaseInfoMailList { get; set; }

        public string SelectedTimeZone { get; set; }

        public IEnumerable<SelectListItem> AvailvableTimeZones { get; set; }

        public ChangePasswordModel ChangePasswordModel { get; set; }

        public bool IsUserCopy
        {
            get { return CopyUserid > 0; }
        }
    }

    public class UserSaveInputModel
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

        public int CopyUserId { get; set; }
        public string SelectedTimeZone { get; set; }

        public int[] AAsSelected { get; set; }
        public int[] CsSelected { get; set; }
        public int[] OTsSelected { get; set; }
        public int[] CCsSelected { get; set; }
        public int[] Departments { get; set; }
        public List<UserWorkingGroup> UserWorkingGroups { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public List<CustomerUserForEdit> CustomerUsers { get; set; }
    }
}