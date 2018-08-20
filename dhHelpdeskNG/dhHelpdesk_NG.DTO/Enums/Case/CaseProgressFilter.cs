namespace DH.Helpdesk.BusinessData.Enums.Case
{
    public static class CaseProgressFilter
    {
        public const string None = "-1";

        public const string ClosedCases = "1";

        public const string CasesInProgress = "2";

        public const string CasesInRest = "3";

        public const string UnreadCases = "4";

        public const string FinishedNotApproved = "5";

        public const string InProgressStatusGreater1 = "6";

        public const string CasesWithWatchDate = "7";

        public const string FollowUp = "8";
    }

    public enum CaseProgressFilterEnum
    {
        None = -1,
        ClosedCases = 1,
        CasesInProgress = 2,
        CasesInRest = 3,
        UnreadCases = 4,
        FinishedNotApproved = 5,
        InProgressStatusGreater1 = 6,
        CasesWithWatchDate = 7,
        FollowUp = 8
    }
}