namespace DH.Helpdesk.BusinessData.Models.Users
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UserProfileCustomerSettings
    {
        public UserProfileCustomerSettings()
        {            
        }

        public UserProfileCustomerSettings(
            int customerId, 
            string customerName, 
            bool showOnStartPage,
			bool active)
        {
            this.CustomerName = customerName;
            this.ShowOnStartPage = showOnStartPage;
            this.CustomerId = customerId;
			this.Active = active;
        }

        [IsId]
        public int CustomerId { get; set; }

        [NotNull]
        public string CustomerName { get; set; }

        public bool ShowOnStartPage { get; set; }

		public bool Active { get; set; }
	}
}