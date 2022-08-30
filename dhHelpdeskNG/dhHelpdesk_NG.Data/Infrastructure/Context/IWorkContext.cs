// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorkContext.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IWorkContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Infrastructure.Context
{
    /// <summary>
    /// The WorkContext interface.
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets the user.
        /// </summary>
        IUserContext User { get; }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        ICustomerContext Customer { get; }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// 
#pragma warning disable 0618
        ICacheContext Cache { get; }
#pragma warning restore 0618

        /// <summary>
        /// The refresh.
        /// </summary>
        void Refresh();
    }
}