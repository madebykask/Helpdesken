// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkContext.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the WorkContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    using DH.Helpdesk.Dal.Infrastructure.Context;

    /// <summary>
    /// The work context.
    /// </summary>
    internal sealed class WorkContext : IWorkContext
    {
        /// <summary>
        /// The user.
        /// </summary>
        private readonly IUserContext user;

        /// <summary>
        /// The customer.
        /// </summary>
        private readonly ICustomerContext customer;

        /// <summary>
        /// The cache.
        /// </summary>
        private readonly ICacheContext cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkContext"/> class.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        /// <param name="customer">
        /// The customer.
        /// </param>
        public WorkContext(IUserContext userContext, ICacheContext cache, ICustomerContext customer)
        {
            this.user = userContext;
            this.cache = cache;
            this.customer = customer;
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        public IUserContext User
        {
            get { return this.user; }
        }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        public ICustomerContext Customer
        {
            get
            {
                return this.customer;
            }
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        public ICacheContext Cache
        {
            get
            {
                return this.cache;
            }
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            this.User.Refresh();
            this.Customer.Refresh();
            this.Cache.Refresh();
        }
    }
}