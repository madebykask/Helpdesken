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

        /// <summary>
        /// The customer id.
        /// </summary>
        private int? customerId;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerContext"/> class.
        /// </summary>
        /// <param name="customerSettingsService">
        /// The customer settings service.
        /// </param>
        public CustomerContext(ISettingService customerSettingsService)
        {
            this.customerSettingsService = customerSettingsService;
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
                    this.customerId = SessionFacade.CurrentCustomer.Id;    
                }

                return this.customerId.Value;
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
                    this.workingDayStart = SessionFacade.CurrentCustomer.WorkingDayStart;
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
                    this.workingDayEnd = SessionFacade.CurrentCustomer.WorkingDayEnd;
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
                    this.settings = (CustomerSettings)HttpContext.Current.Session[CustomerSettings];
                    if (this.settings == null)
                    {
                        HttpContext.Current.Session[CustomerSettings] =
                            this.settings = this.customerSettingsService.GetCustomerSettings(this.CustomerId);
                    }
                }

                return this.settings;
            }
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            this.customerId = null;
            this.workingDayStart = null;
            this.workingDayEnd = null;
            HttpContext.Current.Session[CustomerSettings] = this.settings = null;
        }
    }
}