// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkContext.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the WorkContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.WebApi.Infrastructure.Contexts
{
    /// <summary>
    /// The work context.
    /// </summary>
    internal sealed class WorkContext : IWorkContext
    {
        public WorkContext(IUserContext userContext) //ICacheContext cache, ICustomerContext customer
        {
            User = userContext;
            //Cache = cache;
            //Customer = customer;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public IUserContext User { get; }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        public ICustomerContext Customer { get; }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        public ICacheContext Cache { get; }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            User?.Refresh();
            Customer?.Refresh();
            Cache?.Refresh();
        }
    }
}