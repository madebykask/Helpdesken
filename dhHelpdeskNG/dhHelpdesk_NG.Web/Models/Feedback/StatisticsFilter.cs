using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.Web.Models.Shared;

namespace DH.Helpdesk.Web.Models.Feedback
{
    public class StatisticsFilter
    {
        public StatisticsFilter()
        {
            this.CircularCreatedDate = new DateRange();
        }

        [NotNull]
        public DateRange CircularCreatedDate { get; set; }

        public int EmailsCount { get; set; }
    }
}