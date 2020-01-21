using DH.Helpdesk.Web.Models.Shared;

namespace DH.Helpdesk.Web.Models.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Users;
    using DH.Helpdesk.BusinessData.Models.Users.Input;
    using DH.Helpdesk.Domain;


    public class ProfileInputViewModel
    {
        public User User { get; set; }

        public ChangePasswordModel ChangePasswordModel { get; set; }

        public string ConfirmPassword { get; set; }

        public string NewPassword { get; set; }

        public IList<SelectListItem> RefreshInterval { get; set; }

        public UserModulesViewModel Modules { get; set; } = new UserModulesViewModel();

        public UserCustomersSettingsViewModel CustomersSettings { get; set; } = new UserCustomersSettingsViewModel();

        public string SelectedTimeZone { get; set; }

        public IEnumerable<SelectListItem> AvailvableTimeZones { get; set; }
    }

    public class ProfileSaveViewModel
    {

        public User User { get; set; }

        public int OrderP1 { get; set; }

        public int OrderP2 { get; set; }

        public int UserOrderPermission { get; set; }

        public int SendMailYesNo { get; set; }

        public int? SendMailYesNoWhen { get; set; }

        public int StateStatusCase { get; set; }

        public int? UserRights { get; set; }

        public string[] MenuSetting { get; set; }

        public IList<UserModule> Modules { get; set; } = new List<UserModule>();

        public IList<UserProfileCustomerSettings> CustomersSettings { get; set; } = new List<UserProfileCustomerSettings>();
        
        public string SelectedTimeZone { get; set; }
    }
}