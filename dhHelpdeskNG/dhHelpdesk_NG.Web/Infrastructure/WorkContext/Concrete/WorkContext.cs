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
        /// 
#pragma warning disable 0618
        private readonly ICacheContext cache;
#pragma warning restore 0618

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
        /// 
#pragma warning disable 0618
        public WorkContext(IUserContext userContext, ICacheContext cache, ICustomerContext customer)
        {
            this.user = userContext;
            this.cache = cache;
            this.customer = customer;
        }
#pragma warning restore 0618

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
        /// 
#pragma warning disable 0618
        public ICacheContext Cache
        {
            get
            {
                return this.cache;
            }
        }
#pragma warning restore 0618

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