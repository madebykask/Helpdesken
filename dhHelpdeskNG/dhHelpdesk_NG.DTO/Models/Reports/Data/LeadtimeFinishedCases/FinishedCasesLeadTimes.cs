namespace DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FinishedCasesLeadTimes
    {
        public FinishedCasesLeadTimes()
        {
            this.NumberOfCasesLeadTime = new List<int>();
        }

        public FinishedCasesLeadTimes(
            DateTime finishDate, 
            int numberOfCases, 
            List<int> numberOfCasesLeadTime)
        {
            this.NumberOfCasesLeadTime = numberOfCasesLeadTime;
            this.NumberOfCases = numberOfCases;
            this.FinishDate = finishDate;
        }

        public DateTime FinishDate { get; private set; }

        [MinValue(0)]
        public int NumberOfCases { get; private set; }

        [NotNull]
        public List<int> NumberOfCasesLeadTime { get; private set; }

        public double GetPercents(int leadTime, int digits = 2)
        {
            if (this.NumberOfCases == 0)
            {
                return 0;
            }

            return Math.Round(((double)leadTime / this.NumberOfCases) * 100, digits);
        }
    }
}