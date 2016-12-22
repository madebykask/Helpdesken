using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECT.Web
{
    public class Utils
    {
        public static bool DateOver(DateTime? startDate, DateTime? endDate, int months)
        {
            if (!startDate.HasValue || !endDate.HasValue) return false;
            DateTime a = startDate.Value.AddMonths(months);
            int result = DateTime.Compare(endDate.Value, a);
            return result > 0;
        } 
    }
}