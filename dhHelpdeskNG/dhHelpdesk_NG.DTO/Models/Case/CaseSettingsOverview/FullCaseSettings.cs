namespace DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FullCaseSettings
    {
        public FullCaseSettings(
            UserSettings user, 
            ComputerSettings computer, 
            CaseInfoSettings caseInfo, 
            OtherSettings other, 
            LogSettings log)
        {
            this.Log = log;
            this.Other = other;
            this.CaseInfo = caseInfo;
            this.Computer = computer;
            this.User = user;
        }

        [NotNull]
        public UserSettings User { get; private set; }

        [NotNull]
        public ComputerSettings Computer { get; private set; }

        [NotNull]
        public CaseInfoSettings CaseInfo { get; private set; }

        [NotNull]
        public OtherSettings Other { get; private set; }

        [NotNull]
        public LogSettings Log { get; private set; }
		public ExtendedCaseSettings ExtendedCase { get; internal set; }
	}
}