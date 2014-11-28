// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICustomerContext.cs" company="">
//   
// </copyright>
// <summary>
//   The CustomerContext interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Infrastructure.Context
{
    using DH.Helpdesk.BusinessData.Models.Customer;

    /// <summary>
    /// The CustomerContext interface.
    /// </summary>
    public interface ICustomerContext
    {
        /// <summary>
        /// Gets the customer id.
        /// </summary>
        int CustomerId { get; }

        string CustomerName { get; }

        /// <summary>
        /// Gets the working day end.
        /// </summary>
        int WorkingDayEnd { get; }

        /// <summary>
        /// Gets the working day start.
        /// </summary>
        int WorkingDayStart { get; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        CustomerSettings Settings { get; }

        /// <summary>
        /// The refresh.
        /// </summary>
        void Refresh();

        void SetCustomer(int customerId);

        bool IsCutomerEmpty();
    }
}