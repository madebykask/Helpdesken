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
    using DH.Helpdesk.Dal.Infrastructure.Context;

    /// <summary>
    /// The customer context.
    /// </summary>
    internal sealed class CustomerContext : ICustomerContext
    {
        /// <summary>
        /// Gets the customer id.
        /// </summary>
        public int CustomerId
        {
            get
            {
                return SessionFacade.CurrentCustomer.Id;
            }
        }

        /// <summary>
        /// Gets the working day start.
        /// </summary>
        public int WorkingDayStart
        {
            get
            {
                return SessionFacade.CurrentCustomer.WorkingDayStart;
            }
        }

        /// <summary>
        /// Gets the working day end.
        /// </summary>
        public int WorkingDayEnd
        {
            get
            {
                return SessionFacade.CurrentCustomer.WorkingDayEnd;
            }
        }
    }
}