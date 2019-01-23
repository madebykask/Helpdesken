// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICacheContext.cs" company="">
//   
// </copyright>
// <summary>
//   The CacheContext interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DH.Helpdesk.Dal.Infrastructure.Context
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;

    /// <summary>
    /// The CacheContext interface.
    /// </summary>
    //TODO: move it to ICacheService/IHelpdeskCache
    [Obsolete("Dont add any new methods here. Use ICacheService instead")]
    public interface ICacheContext
    {
        IEnumerable<HolidayOverview> DefaultCalendarHolidays { get; }

        /// <summary>
        /// The refresh.
        /// </summary>
        void Refresh();
    }
}