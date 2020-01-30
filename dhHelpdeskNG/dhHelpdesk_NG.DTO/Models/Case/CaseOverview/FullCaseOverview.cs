namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FullCaseOverview
    {
        public FullCaseOverview(
                int id, 
                UserOverview user, 
                ComputerOverview computer, 
                CaseInfoOverview caseInfo, 
                OtherOverview other, 
				LogsOverview log,
				ExtendedCaseOverview extendedCaseOverview = null)
        {
            this.Log = log;
            this.Other = other;
            this.CaseInfo = caseInfo;
            this.Computer = computer;
            this.User = user;
            this.Id = id;
			this.ExtendedCase = extendedCaseOverview;
        }

        [IsId]
        public int Id { get; private set; }
    
        [NotNull]
        public UserOverview User { get; private set; }

        [NotNull]
        public ComputerOverview Computer { get; private set; }

        [NotNull]
        public CaseInfoOverview CaseInfo { get; private set; }

        [NotNull]
        public OtherOverview Other { get; private set; }

        [NotNull]
        public LogsOverview Log { get; private set; }
		public ExtendedCaseOverview ExtendedCase { get; private set; }
	}
}