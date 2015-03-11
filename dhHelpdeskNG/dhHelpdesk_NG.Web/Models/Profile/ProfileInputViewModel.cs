namespace DH.Helpdesk.Web.Models.Profile
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Users;
    using DH.Helpdesk.BusinessData.Models.Users.Input;
    using DH.Helpdesk.Domain;

    public class ChangeUserPasswordModel
    {
        public int UserId { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public User User { get; set; }
    }
    
    public class ProfileInputViewModel
    {
        private UserModulesViewModel modules = new UserModulesViewModel();

        private UserCustomersSettingsViewModel customersSettings = new UserCustomersSettingsViewModel();

        public User User { get; set; }

        public string ConfirmPassword { get; set; }

        public string NewPassword { get; set; }

        public IList<SelectListItem> RefreshInterval { get; set; }

        public UserModulesViewModel Modules
        {
            get { return this.modules; }
            set { this.modules = value; }
        }

        public UserCustomersSettingsViewModel CustomersSettings
        {
            get
            {
                return this.customersSettings;
            }

            set
            {
                this.customersSettings = value;
            }
        }
    }

    public class ProfileSaveViewModel
    {
        private IList<UserModule> modules = new List<UserModule>();

        private List<UserProfileCustomerSettings> customersSettings = new List<UserProfileCustomerSettings>();

        public User User { get; set; }

        public int OrderP1 { get; set; }

        public int OrderP2 { get; set; }

        public int UserOrderPermission { get; set; }

        public int SendMailYesNo { get; set; }

        public int? SendMailYesNoWhen { get; set; }

        public int StateStatusCase { get; set; }

        public int? UserRights { get; set; }

        public string[] MenuSetting { get; set; }

        public IList<UserModule> Modules
        {
            get { return this.modules; }
            set { this.modules = value; }
        }

        public List<UserProfileCustomerSettings> CustomersSettings
        {
            get
            {
                return this.customersSettings;
            }

            set
            {
                this.customersSettings = value;
            }
        }
    }
}