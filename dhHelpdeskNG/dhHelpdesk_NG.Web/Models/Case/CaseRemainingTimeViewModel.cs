namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class CaseRemainingTimeViewModel
    {
        private const int DefaultMaxDays = 5;

        private readonly List<CaseRemainingTimeItemViewModel> hours = new List<CaseRemainingTimeItemViewModel>();

        private readonly List<CaseRemainingTimeItemViewModel> days = new List<CaseRemainingTimeItemViewModel>();

        public CaseRemainingTimeViewModel(CaseRemainingTimeData data, IWorkContext workContext)
        {
            const int maxHoursId = 6;
            this.Data = data;
            this.Delayed = new CaseRemainingTimeItemViewModel(0, int.MinValue, null, 
                                                             this.Data.CaseRemainingTimes.Count(t => t.RemainingTime < 0),
                                                             Translation.Get("Akut"));
            int workingHoursCount = 24;
            if (workContext.Customer.WorkingDayEnd > workContext.Customer.WorkingDayStart)
            {
                workingHoursCount = workContext.Customer.WorkingDayEnd - workContext.Customer.WorkingDayStart;
            }
            else if (workContext.Customer.WorkingDayEnd < workContext.Customer.WorkingDayStart)
            {
                workingHoursCount = workContext.Customer.WorkingDayStart - workContext.Customer.WorkingDayEnd;
            }

            AddHoursItem(1, 0, 0, workingHoursCount, data, this.hours);
            AddHoursItem(2, 1, 1, workingHoursCount, data, this.hours);
            AddHoursItem(3, 2, 3, workingHoursCount, data, this.hours);
            AddHoursItem(4, 4, 7, workingHoursCount, data, this.hours);
            AddHoursItem(5, 8, 15, workingHoursCount, data, this.hours);
            AddHoursItem(6, 16, 23, workingHoursCount, data, this.hours);

            var ds = this.Data.CaseRemainingTimes.Select(t => t.RemainingTime).Where(t => t >= workingHoursCount).ToList();
            this.MaxDays = ds.Any() ? ds.Max() / workingHoursCount : 1;
            if (this.MaxDays > DefaultMaxDays)
            {
                this.MaxDays = DefaultMaxDays;
            }

            var curId = maxHoursId; 
            for (var i = 1; i < this.MaxDays; i++)
            {
                curId = maxHoursId + i;
                this.days.Add(new CaseRemainingTimeItemViewModel(curId, i + 1, null, 
                                                                 this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursEqualDays(i, workingHoursCount)),
                                                                 string.Format("<{0} {1}", i + 1, Translation.Get("d"))
                                                                 ));

            }

            if (this.MaxDays > 0)
            {
                this.MoreThenMaxDays = new CaseRemainingTimeItemViewModel(++curId, int.MaxValue, null, 
                                                                          this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursGreaterEqualDays(this.MaxDays, workingHoursCount)),
                                                                          string.Format(">={0} {1}", int.MaxValue, Translation.Get("d")));

            }
        }

        public int MaxDays { get; private set; }

        public CaseRemainingTimeItemViewModel Delayed { get; private set; }

        public List<CaseRemainingTimeItemViewModel> Hours
        {
            get
            {
                return this.hours;
            }
        }

        public List<CaseRemainingTimeItemViewModel> Days
        {
            get
            {
                return this.days;
            }
        }

        public CaseRemainingTimeItemViewModel MoreThenMaxDays { get; private set; }

        private CaseRemainingTimeData Data { get; set; }

        private static void AddHoursItem(
                        int id,
                        int start,
                        int end,
                        int workingHours,
                        CaseRemainingTimeData data,
                        List<CaseRemainingTimeItemViewModel> hours)
        {
            if (start >= workingHours)
            {
                return;
            }

            var count = 0;
            var s = start;
            if (data != null)
            {
                while (workingHours > s && s <= end)
                {
                    count += data.CaseRemainingTimes.Count(t => t.RemainingTime == s);
                    s++;
                }
            }
            else
            {
                while (workingHours > s && s <= end)
                {
                    s++;
                }
            }

            var curTime = start <= 1 ? start + 1 : start;
            var curUntil = start != end ? s : (int?)null;
            var caption = string.Empty; 

            if (!curUntil.HasValue)                
            {
                caption = string.Format("<{0} {1}", curTime, Translation.Get("h"));
            }
            else
            {
                if (curUntil.Value < 23)
                    caption = string.Format("<{0} {1}", curUntil, Translation.Get("h"));
                else
                    caption = string.Format("<24 {0}", Translation.Get("h"));                
            }

            hours.Add(new CaseRemainingTimeItemViewModel(
                id,
                curTime,
                curUntil,
                count,
                string.Empty,
                caption));
        }
    }
}