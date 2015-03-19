namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Tools;

    public sealed class CaseRemainingTimeViewModel
    {
        private readonly List<CaseRemainingTimeItemViewModel> days = new List<CaseRemainingTimeItemViewModel>();

        public CaseRemainingTimeViewModel(CaseRemainingTimeData data, int maxDays)
        {
            this.MaxDays = maxDays;
            this.Data = data;

            this.Delayed = new CaseRemainingTimeItemViewModel(int.MinValue, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime < 0));
            this.LessThenOneDay = new CaseRemainingTimeItemViewModel(0, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursEqualDays(0)));

            for (var i = 1; i <= this.MaxDays; i++)
            {
                this.days.Add(new CaseRemainingTimeItemViewModel(i, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursEqualDays(i))));
            }

            this.MoreThenMaxDays = new CaseRemainingTimeItemViewModel(int.MaxValue, this.Data.CaseRemainingTimes.Count(t => t.RemainingTime.IsHoursGreaterDays(this.MaxDays)));
        }

        public int MaxDays { get; private set; }

        public CaseRemainingTimeItemViewModel Delayed { get; private set; }

        public CaseRemainingTimeItemViewModel LessThenOneDay { get; private set; }

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