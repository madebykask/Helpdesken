namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure.Context;

    public sealed class CaseRemainingTimeViewModel
    {
        private const int DefaultMaxDays = 5;

        private readonly List<CaseRemainingTimeItemViewModel> hours = new List<CaseRemainingTimeItemViewModel>();

        private readonly List<CaseRemainingTimeItemViewModel> days = new List<CaseRemainingTimeItemViewModel>();

        public CaseRemainingTimeViewModel(CaseRemainingTimeData data, IWorkContext workContext)
        {
            this.Data = data;
            this.Delayed = new CaseRemainingTimeItemViewModel(int.MinValue, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime < 0));
            int workingHoursCount = 24;
            if (workContext.Customer.WorkingDayEnd > workContext.Customer.WorkingDayStart)
            {
                workingHoursCount = workContext.Customer.WorkingDayEnd - workContext.Customer.WorkingDayStart;
            }
            else if (workContext.Customer.WorkingDayEnd < workContext.Customer.WorkingDayStart)
            {
                workingHoursCount = workContext.Customer.WorkingDayStart - workContext.Customer.WorkingDayEnd;
            }

            AddHoursItem(0, 0, workingHoursCount, data, this.hours);
            AddHoursItem(1, 1, workingHoursCount, data, this.hours);
            AddHoursItem(2, 3, workingHoursCount, data, this.hours);
            AddHoursItem(4, 7, workingHoursCount, data, this.hours);
            AddHoursItem(8, 15, workingHoursCount, data, this.hours);
            AddHoursItem(16, 23, workingHoursCount, data, this.hours);

            var ds = this.Data.CaseRemainingTimes.Select(t => t.RemainingTime).Where(t => t >= workingHoursCount).ToList();
            this.MaxDays = ds.Any() ? ds.Max() / workingHoursCount : 1;
            if (this.MaxDays > DefaultMaxDays)
            {
                this.MaxDays = DefaultMaxDays;
            }

            for (var i = 1; i < this.MaxDays; i++)
            {
                this.days.Add(new CaseRemainingTimeItemViewModel(i + 1, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursEqualDays(i, workingHoursCount))));
            }

            if (this.MaxDays > 0)
            {
                this.MoreThenMaxDays = new CaseRemainingTimeItemViewModel(int.MaxValue, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursGreaterEqualDays(this.MaxDays, workingHoursCount)));
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
            while (workingHours > s && s <= end)
            {
                count += data.CaseRemainingTimes.Count(t => t.RemainingTime == s);
                s++;
            }

            hours.Add(new CaseRemainingTimeItemViewModel(
                start <= 1 ? start + 1 : start,
                start != end ? s : (int?)null,
                count,
                string.Empty));
        }
    }
}