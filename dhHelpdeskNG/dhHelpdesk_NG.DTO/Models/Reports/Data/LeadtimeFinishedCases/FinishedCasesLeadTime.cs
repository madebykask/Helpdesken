namespace DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FinishedCasesLeadTime
    {
        public FinishedCasesLeadTime(
            DateTime finishDate, 
            int numberOfCases, 
            int numberOfCasesShorterEqual, 
            int numberOfCasesLonger)
        {
            this.NumberOfCasesLonger = numberOfCasesLonger;
            this.NumberOfCasesShorterEqual = numberOfCasesShorterEqual;
            this.NumberOfCases = numberOfCases;
            this.FinishDate = finishDate;
        }

        public DateTime FinishDate { get; private set; }

        [MinValue(0)]
        public int NumberOfCases { get; private set; }

        [MinValue(0)]
        public int NumberOfCasesShorterEqual { get; private set; }

        [MinValue(0)]
        public int NumberOfCasesLonger { get; private set; }

        public double GetShorterEqualPercents(int digits = 2)
        {
            if (this.NumberOfCases == 0)
            {
                return 0;
            }

            return Math.Round(((double)this.NumberOfCasesShorterEqual / this.NumberOfCases) * 100, digits);
        }

        public double GetLongerPercents(int digits = 2)
        {
            if (this.NumberOfCases == 0)
            {
                return 0;
            }

            return Math.Round(((double)this.NumberOfCasesLonger / this.NumberOfCases) * 100, digits);
        }
    }
}