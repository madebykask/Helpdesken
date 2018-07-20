using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Holiday.Output;
using DH.Helpdesk.Common;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.WebApi.Infrastructure.Contexts
{
    //TODO: Created for copability with legacy code. Replace it with ICacheService directly
    internal sealed class CacheContext : ICacheContext
    {
        private const string CacheDefaultCalendar = "CACHE_CONTEXT_DEFAULT_HOLIDAYS";
        private readonly IHolidayService _holidayService;
        private readonly ICacheService _cacheService;

        public CacheContext(IHolidayService holidayService, ICacheService cacheService)
        {
            _holidayService = holidayService;
            _cacheService = cacheService;
        }

        public IEnumerable<HolidayOverview> DefaultCalendarHolidays
        {
            get
            {
                return _cacheService.Get(CacheDefaultCalendar, () => _holidayService.GetDefaultCalendar());
            }
        }

        public void Refresh()
        {
            _cacheService.Delete(CacheDefaultCalendar);
        }
    }
}