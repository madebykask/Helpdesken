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
        private readonly List<CaseRemainingTimeItemViewModel> hours = new List<CaseRemainingTimeItemViewModel>();
        private readonly List<CaseRemainingTimeItemViewModel> days = new List<CaseRemainingTimeItemViewModel>();

        public CaseRemainingTimeViewModel(CaseRemainingTimeData data, IWorkContext workContext)
        {
            this.Data = data;

            this.Delayed = new CaseRemainingTimeItemViewModel(int.MinValue, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime < 0));

            var workingHours = workContext.Customer.WorkingDayEnd - workContext.Customer.WorkingDayStart;

            if (this.Data.CaseRemainingTimes.Select(t => t.RemainingTime).Any(t => t > 0 && t < workingHours))
            {
                AddHoursItem(1, 1, workingHours, data, this.hours);
                AddHoursItem(2, 2, workingHours, data, this.hours);
                AddHoursItem(3, 4, workingHours, data, this.hours);
                AddHoursItem(5, 8, workingHours, data, this.hours);
                AddHoursItem(9, 16, workingHours, data, this.hours);
                AddHoursItem(17, 23, workingHours, data, this.hours);
            }

            var ds = this.Data.CaseRemainingTimes.Select(t => t.RemainingTime).Where(t => t >= workingHours).ToList();
            this.MaxDays = ds.Any() ? ds.Max() / workingHours : 5;
            if (this.MaxDays > 5)
            {
                this.MaxDays = 5;
                this.MoreThenMaxDays = new CaseRemainingTimeItemViewModel(int.MaxValue, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursGreaterEqualDays(this.MaxDays + 1, workingHours)));
            }

            for (var i = 1; i <= this.MaxDays; i++)
            {
                this.days.Add(new CaseRemainingTimeItemViewModel(i, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursEqualDays(i, workingHours))));
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

            s--;

            hours.Add(new CaseRemainingTimeItemViewModel(
                start,
                start != s ? s : (int?)null,
                count,
                start != s ? string.Format("{0}-{1} {2}", start, s, Translation.Get("h")) : string.Empty));
        }
    }
}