namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Tools;

    public sealed class CaseRemainingTimeViewModel
    {
        private readonly List<CaseRemainingTimeItemViewModel> hours = new List<CaseRemainingTimeItemViewModel>();
        private readonly List<CaseRemainingTimeItemViewModel> days = new List<CaseRemainingTimeItemViewModel>();

        public CaseRemainingTimeViewModel(CaseRemainingTimeData data)
        {
            this.Data = data;

            this.Delayed = new CaseRemainingTimeItemViewModel(int.MinValue, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime < 0));

            /*var hs = this.Data.CaseRemainingTimes.Select(t => t.RemainingTime).Where(t => t > 0 && t < 24).Distinct().OrderBy(t => t);
            foreach (var h in hs)
            {
                this.hours.Add(new CaseRemainingTimeItemViewModel(h, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime == h)));
            }*/

            if (this.Data.CaseRemainingTimes.Select(t => t.RemainingTime).Any(t => t > 0 && t < 24))
            {
                this.hours.Add(new CaseRemainingTimeItemViewModel(1, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime == 1)));                
                this.hours.Add(new CaseRemainingTimeItemViewModel(2, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime == 2)));                
                this.hours.Add(new CaseRemainingTimeItemViewModel(3, 4, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime >= 3 && t.RemainingTime <= 4)));
                this.hours.Add(new CaseRemainingTimeItemViewModel(5, 8, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime >= 5 && t.RemainingTime <= 8)));
                this.hours.Add(new CaseRemainingTimeItemViewModel(9, 16, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime >= 9 && t.RemainingTime <= 16)));
                this.hours.Add(new CaseRemainingTimeItemViewModel(17, 23, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime >= 17 && t.RemainingTime <= 23)));                
            }

            var ds = this.Data.CaseRemainingTimes.Select(t => t.RemainingTime).Where(t => t >= 24).ToList();
            this.MaxDays = ds.Any() ? ds.Max() / 24 : 5;
            if (this.MaxDays > 5)
            {
                this.MaxDays = 5;
                this.MoreThenMaxDays = new CaseRemainingTimeItemViewModel(int.MaxValue, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursGreaterDays(this.MaxDays)));
            }

            for (var i = 1; i <= this.MaxDays; i++)
            {
                this.days.Add(new CaseRemainingTimeItemViewModel(i, null, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursEqualDays(i))));
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
    }
}