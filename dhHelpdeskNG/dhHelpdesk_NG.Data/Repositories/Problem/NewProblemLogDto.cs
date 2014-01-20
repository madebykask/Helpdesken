namespace dhHelpdesk_NG.Data.Repositories.Problem
{
    using System;

    public class NewProblemLogDto
    {
        public int Id { get; set; }

        public int ChangedByUserId { get; set; }

        public string LogText { get; set; }

        public int ShowOnCase { get; set; }

        public int FinishingCauseId { get; set; }

        public DateTime FinishingDate { get; set; }

        public int FinishConnectedCases { get; set; }
    }
}