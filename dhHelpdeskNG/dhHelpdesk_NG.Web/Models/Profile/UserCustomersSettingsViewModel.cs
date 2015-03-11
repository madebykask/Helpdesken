namespace DH.Helpdesk.Web.Models.Profile
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Users;

    public sealed class UserCustomersSettingsViewModel
    {
        public UserCustomersSettingsViewModel()
        {
            this.CustomersSettings = new List<UserProfileCustomerSettings>();
        }

        public UserCustomersSettingsViewModel(List<UserProfileCustomerSettings> customersSettings)
        {
            this.CustomersSettings = customersSettings;
        }

        public List<UserProfileCustomerSettings> CustomersSettings { get; private set; }
    }
}