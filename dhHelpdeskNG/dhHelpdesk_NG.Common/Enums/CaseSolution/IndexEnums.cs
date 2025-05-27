namespace DH.Helpdesk.Common.Enums.CaseSolution
{
    public static class CaseSolutionIndexColumns
    {
        public const string Name = "Name";

        public const string Category = "Category";

        public const string Caption = "Caption";

        public const string Administrator = "PerformerUser";

        public const string Priority = "Priority";

        public const string Status = "Status";

        public const string ConnectedToButton = "ConnectedToButton";

        public const string SortOrder = "SortOrder";

    }


    public enum CaseRelationType : int
    {
        None = 0,
        ParentAndChildren,
        OnlyDescendants,
        SelfAndDescendandts,
    }
}
