// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheContext.cs" company="">
//   
// </copyright>
// <summary>
//   The CacheContext interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Infrastructure.Context
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;

    /// <summary>
    /// The CacheContext interface.
    /// </summary>
    public interface ICacheContext
    {
        /// <summary>
        /// Gets the holidays.
        /// </summary>
        IEnumerable<HolidayOverview> Holidays { get; }

        /// <summary>
        /// The refresh.
        /// </summary>
        void Refresh();
    }
}