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
    internal sealed class CacheContext : ICacheContext
    {
        /// <summary>
        /// The cache holidays.
        /// </summary>
        private const string CacheHolidays = "CACHE_CONTEXT_HOLIDAYS";

        /// <summary>
        /// Cache for default calendar
        /// </summary>
        private const string CacheDefaultCalendar = "CACHE_CONTEXT_DEFAULT_HOLIDAYS";

        /// <summary>
        /// The holiday service.
        /// </summary>
        private readonly IHolidayService holidayService;

        /// <summary>
        /// The holidays.
        /// </summary>
        private IEnumerable<HolidayOverview> holidays;  
        
        /// <summary>
        /// The default calendar holidays.
        /// </summary>
        private IEnumerable<HolidayOverview> defaultHolidays;

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
                        HttpContext.Current.Cache[CacheHolidays] = this.holidays = this.holidayService.GetHolidayOverviews();
                    }
                }

                return this.holidays;
            }
        }

        public IEnumerable<HolidayOverview> DefaultCalendarHolidays
        {
            get
            {
                if (this.defaultHolidays == null)
                {
                    this.defaultHolidays = (IEnumerable<HolidayOverview>)HttpContext.Current.Cache[CacheDefaultCalendar];
                    if (this.defaultHolidays == null)
                    {
                        HttpContext.Current.Cache[CacheDefaultCalendar] =
                            this.defaultHolidays = this.holidayService.GetDefaultCalendar();
                    }
                }

                return this.defaultHolidays;
            }
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        public void Refresh()
        {
            this.holidays = null;
            HttpContext.Current.Cache.Remove(CacheDefaultCalendar);
            HttpContext.Current.Cache.Remove(CacheHolidays);
        }
    }
}