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
    /// <summary>
    /// The CustomerContext interface.
    /// </summary>
    public interface ICustomerContext
    {
        /// <summary>
        /// Gets the customer id.
        /// </summary>
        int CustomerId { get; }

        /// <summary>
        /// Gets the working day end.
        /// </summary>
        int WorkingDayEnd { get; }

        /// <summary>
        /// Gets the working day start.
        /// </summary>
        int WorkingDayStart { get; }
    }
}