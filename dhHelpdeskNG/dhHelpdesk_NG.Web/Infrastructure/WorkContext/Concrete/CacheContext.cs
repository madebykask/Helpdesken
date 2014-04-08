// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheContext.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CacheContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    using System.Collections.Generic;
    using System.Web;

    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;

    /// <summary>
    /// The cache context.
    /// </summary>
    public class CacheContext : ICacheContext
    {
        /// <summary>
        /// The cache holidays.
        /// </summary>
        private const string CacheHolidays = "CACHE_HOLIDAYS";

        /// <summary>
        /// The holiday service.
        /// </summary>
        private readonly IHolidayService holidayService;

        /// <summary>
        /// The holidays.
        /// </summary>
        private IEnumerable<HolidayOverview> holidays;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheContext"/> class.
        /// </summary>
        /// <param name="holidayService">
        /// The holiday service.
        /// </param>
        public CacheContext(IHolidayService holidayService)
        {
            this.holidayService = holidayService;
        }

        /// <summary>
        /// Gets the holidays.
        /// </summary>
        public IEnumerable<HolidayOverview> Holidays
        {
            get
            {
                if (this.holidays == null)
                {
                    this.holidays = (IEnumerable<HolidayOverview>)HttpContext.Current.Cache[CacheHolidays];
                    if (this.holidays == null)
                    {
                        HttpContext.Current.Cache[CacheHolidays] = this.holidays = this.holidayService.GetHolidays();
                    }
                }
                return this.holidays;
            }
        }
    }
}