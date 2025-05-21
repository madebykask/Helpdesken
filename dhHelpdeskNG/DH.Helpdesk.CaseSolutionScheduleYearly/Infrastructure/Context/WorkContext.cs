using DH.Helpdesk.Dal.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Infrastructure.Context
{
    internal sealed class WorkContext : IWorkContext
    {
        public WorkContext(IUserContext userContext) //ICacheContext cache, ICustomerContext customer
        {
            //User = userContext;
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
        /// 
#pragma warning disable 0618
        public ICacheContext Cache { get; }
#pragma warning restore 0618

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
