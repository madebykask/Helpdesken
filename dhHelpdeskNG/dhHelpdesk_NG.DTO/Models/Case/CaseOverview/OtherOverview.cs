namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    using System;

    public sealed class OtherOverview
    {
        public OtherOverview(
            string workingGroup, 
            string responsible, 
            string administrator, 
            string priority, 
            string state, 
            string subState, 
            DateTime? plannedActionDate, 
            DateTime? watchDate, 
            bool verified, 
            string verifiedDescription, 
            string solutionRate, 
            string causingPart,
            string problem)
        {
            this.CausingPart = causingPart;
            this.SolutionRate = solutionRate;
            this.VerifiedDescription = verifiedDescription;
            this.Verified = verified;
            this.WatchDate = watchDate;
            this.PlannedActionDate = plannedActionDate;
            this.SubState = subState;
            this.State = state;
            this.Priority = priority;
            this.Administrator = administrator;
            this.Responsible = responsible;
            this.WorkingGroup = workingGroup;
            this.Problem = problem;
        }

        public string WorkingGroup { get; private set; }

        public string Responsible { get; private set; }

        public string Administrator { get; private set; }

        public string Priority { get; private set; }

        public string State { get; private set; }

        public string SubState { get; private set; }

        public DateTime? PlannedActionDate { get; private set; }

        public DateTime? WatchDate { get; private set; }

        public bool Verified { get; private set; }

        public string VerifiedDescription { get; private set; }

        public string SolutionRate { get; private set; }

        public string CausingPart { get; private set; }

        public string Problem { get; private set; }
    }
}