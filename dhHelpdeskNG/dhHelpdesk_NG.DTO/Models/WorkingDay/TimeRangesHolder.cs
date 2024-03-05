namespace DH.Helpdesk.BusinessData.Models.WorkingDay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.Tools;

    public class TimeRange
    {
        public DateTime begin;

        public DateTime end;

        public Boolean IsValidSingleDay(bool isThrowException = false)
        {
            var res = this.begin > this.end || (this.end - this.begin).TotalHours > 24;
            if (isThrowException && res)
            {
                throw new ArgumentException(
                    string.Format(
                        "working hours Item1 can not be more Item2 and diff can not be more than 24 hours ({0} - {1} was supplyed)",
                        this.begin,
                        this.end));
            }

            return res;
        }

        /// <summary>
        /// Returns difference in minutes between begin and end
        /// </summary>
        /// <returns></returns>
        public int Sum()
        {
            return (int)Math.Round((this.end - this.begin).TotalMinutes);
        }

        public int SelectSum(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentException("'from' can not be more that 'to'");
            }

            var res = (int)Math.Round((DatesHelper.Min(this.end, to) - DatesHelper.Max(this.begin, from)).TotalMinutes);
            return res;
        }
    }

    /// <summary>
    /// Holds all working hours ranges for specific date
    /// </summary>
    public class TimeRangesHolder
    {
        private readonly List<TimeRange> workTimeRanges;

        private int summaryTime;

        public TimeRangesHolder()
        {
            this.workTimeRanges = new List<TimeRange>();
            this.summaryTime = 0;
        }

        /// <summary>
        /// Summary working time in minutes value for date
        /// </summary>
        public int SummaryTime
        {
            get
            {
                return this.summaryTime;
            }
        }

        /// <summary>
        /// List of time ranges (begin,end in hours) for specific date. Can not intersect with each other. Order of ranges is not mandatory
        /// </summary>
        public TimeRange[] WorkingHours
        {
            get
            {
                return this.workTimeRanges.ToArray();
            }

            set
            {
                this.summaryTime = value.Sum(
                    it =>
                        {
                            it.IsValidSingleDay(true);
                            return it.Sum();
                        });
                this.workTimeRanges.Clear();
                this.workTimeRanges.AddRange(value);
            }
        }

        public void Add(TimeRange rangeToAdd)
        {
            rangeToAdd.IsValidSingleDay(true);
            this.summaryTime += rangeToAdd.Sum();
            this.workTimeRanges.Add(rangeToAdd);
        }

        public void AddRange(TimeRange[] rangesToAdd)
        {
            foreach (var timeRange in rangesToAdd)
            {
                timeRange.IsValidSingleDay(true);
                this.summaryTime += timeRange.Sum();
            }

            this.workTimeRanges.AddRange(rangesToAdd);
        }

        /// <summary>
        /// Calculates summary work time in munutes for specified range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public int Sum(DateTime from, DateTime to)
        {
            if (from > to)
            {
                return 0;
                //throw new ArgumentException("'from' can not be more that 'to'");
            }

            var res =
                this.workTimeRanges.Where(
                    it =>
                    (it.begin < from && it.end > to) || (it.begin >= from && it.begin <= to)
                    || (it.end >= from && it.end <= to))
                    .Sum(it => (int)Math.Round((DatesHelper.Min(it.end, to) - DatesHelper.Max(it.begin, from)).TotalMinutes));
            return res;

        }
    }
}