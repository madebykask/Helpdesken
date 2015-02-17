namespace DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LeadtimeFinishedCasesData
    {
        public LeadtimeFinishedCasesData()
        {
            this.CasesByLeadTimes = new List<FinishedCasesLeadTimes>();
            this.CasesByLeadTime = new List<FinishedCasesLeadTime>();
        }

        public LeadtimeFinishedCasesData(
            int leadTime, 
            int numberOfCases, 
            int numberOfCasesShorterEqual, 
            int numberOfCasesLonger, 
            List<FinishedCasesLeadTime> casesByLeadTime, 
            List<FinishedCasesLeadTimes> casesByLeadTimes)
        {
            this.CasesByLeadTimes = casesByLeadTimes;
            this.CasesByLeadTime = casesByLeadTime;
            this.NumberOfCasesLonger = numberOfCasesLonger;
            this.NumberOfCasesShorterEqual = numberOfCasesShorterEqual;
            this.NumberOfCases = numberOfCases;
            this.LeadTime = leadTime;
        }

        [MinValue(1)]
        [MaxValue(10)]
        public int LeadTime { get; private set; }

        [MinValue(0)]
        public int NumberOfCases { get; private set; }

        [MinValue(0)]
        public int NumberOfCasesShorterEqual { get; private set; }

        [MinValue(0)]
        public int NumberOfCasesLonger { get; private set; }

        [NotNull]
        public List<FinishedCasesLeadTime> CasesByLeadTime { get; private set; } 

        [NotNull]
        public List<FinishedCasesLeadTimes> CasesByLeadTimes { get; private set; }

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