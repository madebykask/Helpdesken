namespace DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OtherSettings
    {
        public OtherSettings(
            FieldOverviewSetting workingGroup, 
            FieldOverviewSetting responsible, 
            FieldOverviewSetting administrator, 
            FieldOverviewSetting priority, 
            FieldOverviewSetting state, 
            FieldOverviewSetting subState, 
            FieldOverviewSetting plannedActionDate, 
            FieldOverviewSetting watchDate, 
            FieldOverviewSetting verified, 
            FieldOverviewSetting verifiedDescription, 
            FieldOverviewSetting solutionRate, 
            FieldOverviewSetting causingPart,
            FieldOverviewSetting problem,
            FieldOverviewSetting project)
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
            this.Project = project;
        }

        [NotNull]
        public FieldOverviewSetting WorkingGroup { get; private set; }

        [NotNull]
        public FieldOverviewSetting Responsible { get; private set; }

        [NotNull]
        public FieldOverviewSetting Administrator { get; private set; }

        [NotNull]
        public FieldOverviewSetting Priority { get; private set; }

        [NotNull]
        public FieldOverviewSetting State { get; private set; }

        [NotNull]
        public FieldOverviewSetting SubState { get; private set; }

        [NotNull]
        public FieldOverviewSetting PlannedActionDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting WatchDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting Verified { get; private set; }

        [NotNull]
        public FieldOverviewSetting VerifiedDescription { get; private set; }

        [NotNull]
        public FieldOverviewSetting SolutionRate { get; private set; }

        [NotNull]
        public FieldOverviewSetting CausingPart { get; private set; }

        [NotNull]
        public FieldOverviewSetting Problem { get; private set; }

        [NotNull]
        public FieldOverviewSetting Project { get; private set; }
    }
}