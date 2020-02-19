// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerContext.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CustomerContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    using System.Web;

    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;

    /// <summary>
    /// The customer context.
    /// </summary>
    internal sealed class CustomerContext : ICustomerContext
    {
        /// <summary>
        /// The customer settings.
        /// </summary>
        private const string CustomerSettings = "CUSTOMER_CONTEXT_SETTINGS";

        /// <summary>
        /// The customer settings service.
        /// </summary>
        private readonly ISettingService customerSettingsService;

        private readonly ICustomerService customerService;

        private Customer customer;

        /// <summary>
        /// The customer id.
        /// </summary>
        private int? customerId;

        private string customerName;

        /// <summary>
        /// The working day start.
        /// </summary>
        private int? workingDayStart;

        /// <summary>
        /// The working day end.
        /// </summary>
        private int? workingDayEnd;

        /// <summary>
        /// The settings.
        /// </summary>
        private CustomerSettings settings;
       
        public CustomerContext(
                ISettingService customerSettingsService, 
                ICustomerService customerService)
        {
            this.customerSettingsService = customerSettingsService;
            this.customerService = customerService;
        }

        /// <summary>
        /// Gets the customer id.
        /// </summary>
        public int CustomerId
        {
            get
            {
                if (!this.customerId.HasValue)
                {
                    this.customerId = this.Customer.Id;    
                }

                return this.customerId.Value;
            }
        }

        public string CustomerName
        {
            get
            {
                if (string.IsNullOrEmpty(this.customerName))
                {
                    this.customerName = this.Customer.Name;
                }

                return this.customerName;
            }
        }

        /// <summary>
        /// Gets the working day start.
        /// </summary>
        public int WorkingDayStart
        {
            get
            {
                if (!this.workingDayStart.HasValue)
                {
                    this.workingDayStart = this.Customer.WorkingDayStart;
                }

                return this.workingDayStart.Value;
            }
        }

        /// <summary>
        /// Gets the working day end.
        /// </summary>
        public int WorkingDayEnd
        {
            get
            {
                if (!this.workingDayEnd.HasValue)
                {
                    this.workingDayEnd = this.Customer.WorkingDayEnd;
                }

                return this.workingDayEnd.Value;
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public CustomerSettings Settings
        {
            get
            {
                if (this.settings == null)
                {
                    var settingsKey = this.GetSettingsKey();
                    this.settings = (CustomerSettings)HttpContext.Current.Session[settingsKey];
                    if (this.settings == null)
                    {
                        HttpContext.Current.Session[settingsKey] =
                            this.settings = this.customerSettingsService.GetCustomerSettings(this.CustomerId);
                    }
                }

                return this.settings;
            }
        }

        private Customer Customer
        {
            get
            {
                if (this.customer == null)
                {
                    this.customer = this.GetCurrentCustomer();
                }

                return this.customer;
            }
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            var settingsKey = this.GetSettingsKey();
            HttpContext.Current.Session[settingsKey] = this.settings = null;
            this.customerId = null;
            this.workingDayStart = null;
            this.workingDayEnd = null;
        }

        public void SetCustomer(int cusId)
        {
            var currentCustomer = this.GetCurrentCustomer();
            if (currentCustomer != null && currentCustomer.Id == cusId)
            {
                return;
            }

            var customer = this.customerService.GetCustomer(cusId);
            if (customer == null)
            {
                return;
            }

            SessionFacade.CurrentCustomer = customer;
			SessionFacade.ClearSearches();
            this.Refresh();
        }

        public bool IsCutomerEmpty()
        {
            return this.Customer == null;
        }

        private Customer GetCurrentCustomer()
        {
            return SessionFacade.CurrentCustomer;
        }

        private string GetSettingsKey()
        {
            return string.Format("{0}{1}", CustomerSettings, this.CustomerId);
        }
    }
}